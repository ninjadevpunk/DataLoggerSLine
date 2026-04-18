using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Microsoft.EntityFrameworkCore;

namespace DataLoggerTest
{
    public class READERFinderTests : ENTITYMASTERTests
    {

        [Theory]
        [InlineData("Data Logger", true)]
        [InlineData("PyCharm", true)]
        [InlineData("NonExistentApp", false)]
        public async Task FindApplicationByNameTest(string appName, bool shouldExist)
        {
            /*
            var master = GetInMemoryMaster();
            var reader = new EntityReader(master);

            var account1 = new ACCOUNT { accountID = 1, IsOnline = true };
            var account2 = new ACCOUNT { accountID = 329, IsOnline = false };

            var app1 = new ApplicationClass { appID = 1, Name = "Data Logger", User = account1, accountID = 1 };
            var app2 = new ApplicationClass { appID = 2, Name = "PyCharm", User = account2, accountID = 329 };
            master.Applications.AddRange(app1, app2);

            await ((DbContext)master).SaveChangesAsync();

            var accountID = appName == "PyCharm" ? 329 : 1;
            var result = await reader.FindApplication(accountID, appName);


            if (shouldExist)
            {
                Assert.NotNull(result);
                //Assert.Equal(appName, result.Name);
            }
            else
            {
                Assert.Null(result);
            }
            */
        }

        [Fact]
        public async Task FindOnlineUser()
        {
            /*
            // Arrange
            var master = GetInMemoryMaster();
            var reader = new EntityReader(master);

            // Seed accounts
            var account1 = new ACCOUNT { accountID = 1, IsOnline = false };
            var account2 = new ACCOUNT { accountID = 2, IsOnline = true };  // the online user
            var account3 = new ACCOUNT { accountID = 3, IsOnline = false };
            master.Accounts.AddRange(account1, account2, account3);

            await ((DbContext)master).SaveChangesAsync();

            // Only one account should be online
            var onlineCount = master.Accounts.Count(a => a.IsOnline);
            Assert.Equal(1, onlineCount);

            var result = await reader.GetOnlineAccountIDAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result);
            */
        }

        [Fact]
        public async Task FindProjectByNameTest01()
        {
            /*
            // Arrange
            var master = GetInMemoryMaster();
            var reader = new EntityReader(master);

            var account = new ACCOUNT { accountID = 1, IsOnline = true };
            master.Accounts.Add(account);

            var app = new ApplicationClass { appID = 1, Name = "Data Logger", accountID = 1 };
            master.Applications.Add(app);

            var project = new ProjectClass
            {
                projectID = 1,
                Name = "Test Project",
                accountID = 1,
                appID = app.appID
            };
            master.Projects.Add(project);

            await ((DbContext)master).SaveChangesAsync();

            // Only one account should be online
            var onlineCount = master.Accounts.Count(a => a.IsOnline);
            Assert.Equal(1, onlineCount);

            // Act
            var result = await reader.FindProject(account.accountID, "Test Project", app.appID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.projectID, result.projectID);
            Assert.Equal(app.appID, result.appID);
            */
        }

        [Fact]
        public async Task FindProjectByNameTest02()
        {
            /*
            // Arrange
            var master = GetInMemoryMaster();
            var reader = new EntityReader(master);

            // Seed accounts
            var account1 = new ACCOUNT { accountID = 1, IsOnline = false };
            var account2 = new ACCOUNT { accountID = 2, IsOnline = true };  // the online user
            var account3 = new ACCOUNT { accountID = 3, IsOnline = false };
            master.Accounts.AddRange(account1, account2, account3);

            // Seed apps
            var app1 = new ApplicationClass { appID = 1, Name = "App One", accountID = 1 };
            var app2 = new ApplicationClass { appID = 2, Name = "App Two", accountID = 2 };
            var app3 = new ApplicationClass { appID = 3, Name = "App Three", accountID = 3 };
            master.Applications.AddRange(app1, app2, app3);

            // Seed multiple projects
            var project1 = new ProjectClass { projectID = 1, Name = "Project A", accountID = 1, appID = 1 };
            var project2 = new ProjectClass { projectID = 2, Name = "Project B", accountID = 2, appID = 2 }; // online user
            var project3 = new ProjectClass { projectID = 3, Name = "Project C", accountID = 3, appID = 3 };
            var project4 = new ProjectClass { projectID = 4, Name = "Project D", accountID = 2, appID = 2 }; // online user
            master.Projects.AddRange(project1, project2, project3, project4);

            await ((DbContext)master).SaveChangesAsync();

            // Only one account should be online
            var onlineCount = master.Accounts.Count(a => a.IsOnline);
            Assert.Equal(1, onlineCount);

            // Act
            var onlineUserId = await reader.GetOnlineAccountIDAsync();
            var result = await reader.FindProject(onlineUserId, "Project D", 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.projectID);
            Assert.Equal(onlineUserId, result.accountID);
            */
        }





    }
}
