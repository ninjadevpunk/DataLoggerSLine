using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DataLoggerTest
{
    public class ENTITYMASTERTests
    {
        protected static EntityMaster GetInMemoryMaster([CallerMemberName] string testName = "")
        {
            var options = new DbContextOptionsBuilder<EntityMaster>()
                .UseInMemoryDatabase(databaseName: $"TestDB_{testName}_{Guid.NewGuid()}")
                .Options;

            return new EntityMaster(options);
        }

        protected string GetTestDbName([CallerMemberName] string testName = "") => $"TestDB_{testName}";

        [Fact]
        private async Task LoginUserTest()
        {
            // Arrange
            //var master = GetInMemoryMaster();
            //var reader = new EntityReader(master);

            //var account = new ACCOUNT { accountID = 1, IsOnline = false };
            //master.Accounts.Add(account);
            //await ((DbContext)master).SaveChangesAsync();

            //// Act
            //var result = await master.SetCurrentUser(account);

            //// Assert
            //Assert.True(result);
            //Assert.True(master.Accounts.Single().IsOnline);
        }

        /// <summary>
        /// Tests the application's integrity during logging in, logging out and logging in again.
        /// </summary>
        [Fact]
        private async Task LoginSignOutLoginTest()
        {
            /*
            // Arrange
            var master = GetInMemoryMaster();
            var reader = new EntityReader(master);
            var writer = new EntityWriter(reader, master);

            var account = new ACCOUNT { accountID = 1, IsOnline = false };
            var account1 = new ACCOUNT { accountID = 2, IsOnline = false };
            var account2 = new ACCOUNT { accountID = 3, IsOnline = false };
            var list = new List<ACCOUNT>
            {
                new ACCOUNT { accountID = 1, IsOnline = false },
                new ACCOUNT { accountID = 2, IsOnline = false },
                new ACCOUNT { accountID = 3, IsOnline = false }
            };
            master.Accounts.AddRange(list);
            await ((DbContext)master).SaveChangesAsync();

            var auth = new AuthService();

            auth.Account = account1;

            // Act
            //var result = await master.SetCurrentUser(auth.Account);

            // Assert

            // After first login
            var onlineCount = master.Accounts.Count(a => a.IsOnline);
            // exactly one user must be online
            Assert.Equal(1, onlineCount); 


            result = await master.UnsetCurrentUser();

            // No users online
            Assert.False(master.Accounts.Any(a => a.IsOnline)); 


            result = await master.SetCurrentUser(account1);

            // After first login
            onlineCount = master.Accounts.Count(a => a.IsOnline);
            // exactly one user must be online
            Assert.Equal(1, onlineCount);
            */

        }

        // Old DataService: holds a snapshot
        public class DataServiceOld
        {
            private ACCOUNT _account;

            public DataServiceOld(AuthService authService)
            {
                _account = authService.Account; // snapshot at construction
            }

            public ACCOUNT GetCurrentAccount() => _account;
        }

        // New DataService: references AuthService directly
        public class DataServiceNew
        {
            private readonly AuthService _authService;

            public DataServiceNew(AuthService authService)
            {
                _authService = authService;
            }

            public ACCOUNT GetCurrentAccount() => _authService.Account;
        }

        [Fact]
        public void StaleAccountReferenceTest()
        {
            var userA = new ACCOUNT { accountID = 1 };
            var userB = new ACCOUNT { accountID = 2 };

            var authService = new AuthService { Account = userA };

            var oldService = new DataServiceOld(authService);
            var newService = new DataServiceNew(authService);

            // Before switching account
            Assert.Equal(1, oldService.GetCurrentAccount().accountID);
            Assert.Equal(1, newService.GetCurrentAccount().accountID);

            // Replace AuthService.Account with userB (simulating login)
            authService.Account = userB;

            // Old DataService still points to User A
            Assert.Equal(1, oldService.GetCurrentAccount().accountID);

            // New DataService sees User B
            Assert.Equal(2, newService.GetCurrentAccount().accountID);
        }
    }
}
