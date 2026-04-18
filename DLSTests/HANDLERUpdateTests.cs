using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataLoggerTest
{
    public class HANDLERUpdateTests : ENTITYMASTERTests
    {
        private async Task<CodingLOG> SeedTestDataAsync(EntityMaster master)
        {
            // 1. Author
            var author = new ACCOUNT
            {
                accountID = 1,
                FirstName = "Ninja",
                LastName = "Dev",
                Email = "ninja@test.com",
                Password = "test123",
                ProfilePic = "/Assets/login/user.png"
            };

            // 2. Nav Data
            var app1 = new ApplicationClass(author, "Visual Studio");
            var project1 = new ProjectClass(author, "Project Alpha", app1);
            var output1 = new OutputClass(1, "EXE");
            var type1 = new TypeClass(1, "Bug Report");

            master.Applications.Add(app1);
            master.Projects.Add(project1);
            master.Outputs.Add(output1);
            master.Types.Add(type1);
            await master.SaveChangesAsync();

            // 3. Subject
            var subject = new SubjectClass
            {
                Category = LOG.CATEGORY.CODING,
                accountID = author.accountID,
                Subject = "Existing Subject",
                projectID = project1.projectID,
                appID = app1.appID
            };
            master.Subjects.Add(subject);
            await master.SaveChangesAsync();

            // 4. Post-Its
            var postItToUpdate = new PostIt { Subject = subject, subjectID = subject.subjectID, accountID = author.accountID, Error = "Old Error" };
            var postItToDelete = new PostIt { Subject = subject, subjectID = subject.subjectID, accountID = author.accountID, Error = "Delete Error" };

            // 5. The Log
            var originalLog = new CodingLOG
            {
                ID = 1,
                Author = author,
                accountID = author.accountID,
                Application = app1,
                appID = app1.appID,
                Project = project1,
                projectID = project1.projectID,
                Output = output1,
                outputID = output1.outputID,
                Type = type1,
                typeID = type1.typeID,
                PostItList = new List<PostIt> { postItToUpdate, postItToDelete }
            };

            master.Logs.Add(originalLog);
            await master.SaveChangesAsync();

            // Clear tracker so the "Act" phase fetches fresh data
            master.ChangeTracker.Clear();

            return originalLog;
        }









        #region EXISTING TO EXISTING APP


        /// <summary>
        /// Test the updating of application in the log to an existing application.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_Should_Change_App_To_Existing_App()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();
            using var master = serviceProvider.GetRequiredService<EntityMaster>();

            // Seed parent/nav data
            var author = new ACCOUNT
            {
                accountID = 1,
                ProfilePic = "/Assets/login/user.png",
                FirstName = "Ninja",
                LastName = "Dev",
                Email = "ninja@test.com",
                Password = "test123",
                IsEmployee = false,
                CompanyName = "",
                CompanyAddress = "",
                CompanyLogo = "",
                IsOnline = true
            };

            var app1 = new ApplicationClass(author, "Visual Studio");
            var app2 = new ApplicationClass(author, "VS Code");

            master.Applications.AddRange(app1, app2);
            await master.SaveChangesAsync();

            var project1 = new ProjectClass(author, "Project Alpha", app1);
            var project2 = new ProjectClass(author, "Project Beta", app2);

            master.Projects.AddRange(project1, project2);

            var output1 = new OutputClass(1, "EXE");
            var output2 = new OutputClass(2, "DLL");

            master.Outputs.AddRange(output1, output2);

            var type1 = new TypeClass(1, "Bug Report");
            var type2 = new TypeClass(2, "Feature Update");

            master.Types.AddRange(type1, type2);

            await master.SaveChangesAsync();

            // Seed subjects
            var existingSubject = new SubjectClass
            {
                Category = LOG.CATEGORY.CODING,
                accountID = author.accountID,
                Subject = "Existing Subject",
                projectID = project1.projectID,
                appID = app1.appID
            };

            master.Subjects.Add(existingSubject);
            await master.SaveChangesAsync();

            // Seed original log with two PostIts
            var postItToUpdate = new PostIt
            {
                Subject = existingSubject,
                subjectID = existingSubject.subjectID,
                accountID = author.accountID,
                Error = "Old Error",
                Solution = "Old Solution",
                Suggestion = "Old Suggestion",
                Comment = "Old Comment",
                ERCaptureTime = new DateTime(2026, 1, 1),
                SOCaptureTime = new DateTime(2026, 1, 2)
            };

            var postItToDelete = new PostIt
            {
                Subject = existingSubject,
                subjectID = existingSubject.subjectID,
                accountID = author.accountID,
                Error = "Delete Error",
                Solution = "Delete Solution",
                Suggestion = "Delete Suggestion",
                Comment = "Delete Comment",
                ERCaptureTime = new DateTime(2026, 1, 3),
                SOCaptureTime = new DateTime(2026, 1, 4)
            };

            var originalLog = new CodingLOG
            {
                ID = 1,
                Author = author,
                accountID = author.accountID,
                Application = app1,
                appID = app1.appID,
                Project = project1,
                projectID = project1.projectID,
                Output = output1,
                outputID = output1.outputID,
                Type = type1,
                typeID = type1.typeID,
                Start = new DateTime(2026, 1, 1, 8, 0, 0),
                End = new DateTime(2026, 1, 1, 10, 0, 0),
                Content = "Old content",
                Bugs = 5,
                Success = false,
                PostItList = new List<PostIt> { postItToUpdate, postItToDelete }
            };

            master.Logs.Add(originalLog);
            await master.SaveChangesAsync();

            var updatePostItId = postItToUpdate.postItID;
            var deletePostItId = postItToDelete.postItID;

            master.ChangeTracker.Clear();

            // ----------------------------------------
            // LOAD LOG (simulate it being fetched somewhere else)
            // ----------------------------------------
            var incomingLog = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

            // ----------------------------------------
            // DETACH
            // ----------------------------------------
            master.ChangeTracker.Clear();

            // ----------------------------------------
            // APPLY EDITOR CHANGES (now working on detached object)
            // ----------------------------------------

            // Parent updates
            // APPLICATION

            bool appIsNew = false;
            var editorApplicationText = app2.Name;
            var account = incomingLog.Author;

            if (incomingLog.Application.Name != editorApplicationText)
            {
                // App may be the same or changed. Changed app may be new or existing
                var app = await master.Applications.FirstOrDefaultAsync(a =>
                    a.Name == editorApplicationText);

                if (app == null)
                {
                    app = new ApplicationClass(editorApplicationText);
                    app.accountID = account.accountID;
                }

                appIsNew = app.appID == 0;

                if (appIsNew)
                    incomingLog.Application = app;
                else
                {
                    incomingLog.appID = app.appID;
                    incomingLog.Application = null;
                }
            }



            // Retrieve Dependency
            var handler = serviceProvider.GetRequiredService<EntityHandler>();

            // Update Log
            await handler.UpdateLogAsync(incomingLog);

            // Assert with no Tracking
            master.ChangeTracker.Clear();

            var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

            // Parent scalar/nav assertions
            Assert.Equal(app2.appID, result.appID);
        }




        /// <summary>
        /// #1 + #A1
        /// Test the updating of application in the log to an existing application and changing
        /// project to an existing project.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_Should_Change_App_to_NEW_Existing_App_And_NEW_Existing_Project()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            string editorProjectText = string.Empty;
            int initialAppCount = 0;
            int initialProjectCount = 0;

            ApplicationClass app1, app2;
            ProjectClass project1, project2;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                Assert.NotEqual(0, originalLog.ID);
                app1 = originalLog.Application;
                app2 = new ApplicationClass(originalLog.Author, "VS Code");
                project1 = originalLog.Project;
                project2 = new ProjectClass(originalLog.Author, "Project Beta", app2);
                editorApplicationText = app2.Name;
                editorProjectText = project2.Name;
                await master.Applications.AddAsync(app2);
                await master.Projects.AddAsync(project2);
                await master.SaveChangesAsync();

                initialAppCount = await master.Applications.CountAsync();
                initialProjectCount = await master.Projects.CountAsync();

                Assert.NotNull(await master.Applications.SingleOrDefaultAsync(a => a.Name == "VS Code"));
                Assert.NotNull(await master.Projects.SingleOrDefaultAsync(p => p.Name == "Project Beta"));
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                // APPLICATION

                bool appIsNew = false;
                var account = incomingLog.Author;

                if (incomingLog.Application.Name != editorApplicationText)
                {
                    // App may be the same or changed. Changed app may be new or existing
                    var app = await master.Applications.FirstOrDefaultAsync(a =>
                        a.Name == editorApplicationText);

                    if (app == null)
                    {
                        app = new ApplicationClass(editorApplicationText);
                        app.accountID = account.accountID;
                    }

                    appIsNew = app.appID == 0;

                    if (appIsNew)
                        Assert.Fail("Application should be existing.");
                    else
                    {
                        incomingLog.appID = app.appID;
                        incomingLog.Application = null;
                    }
                }

                // PROJECT

                if (!appIsNew)
                {
                    bool projectBelongsToNewApp = incomingLog.Project.appID == incomingLog.appID;

                    if (string.IsNullOrEmpty(editorProjectText))
                    {
                        // Set to "Unnamed Project" default
                        incomingLog.projectID = 1;
                        incomingLog.Project = null;
                    }
                    else if (incomingLog.Project!.Name != editorProjectText || !projectBelongsToNewApp)
                    {
                        // Find the project by Name AND the NEW App ID
                        var project = await master.Projects.FirstOrDefaultAsync(p =>
                            p.Name == editorProjectText && p.appID == incomingLog.appID);

                        if (project == null)
                        {
                            project = new ProjectClass(editorProjectText);
                            project.accountID = account.accountID;
                            project.appID = incomingLog.appID;
                        }

                        if (project.projectID == 0)
                        {
                            incomingLog.Project = project;
                        }
                        else
                        {
                            incomingLog.projectID = project.projectID;
                            incomingLog.Project = null;
                        }
                    }

                }
                else
                {
                    Assert.Fail("Application should be existing.");
                }



                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }



            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // App changed
                Assert.NotEqual(app1.Name, result.Application.Name);
                Assert.NotEqual(app1.appID, result.appID);
                Assert.Equal(app2.Name, result.Application.Name);

                // App count hasn't changed
                int appCount = await master.Applications.CountAsync();
                Assert.True(initialAppCount == appCount);

                // Project changed
                Assert.Equal(project2.Name, result.Project.Name);

                // Project's app uses the new existing app
                Assert.Equal(result.Project.appID, result.appID);
                Assert.Equal(app2.appID, result.appID);
                Assert.NotEqual(app1.appID, result.appID);

                // Project count hasn't changed
                int projectCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount == projectCount);

                // Project ID changes and should not match project1.
                // project2's ID should match because the app is changed to app2
                Assert.NotEqual(project1.projectID, result.projectID);
                Assert.Equal(project2.projectID, result.projectID);

            }
        }



        /// <summary>
        /// #1 + #A3
        /// Test the updating of application in the log to an existing application with an 
        /// unchanged project.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_Should_Change_App_To_NEW_Existing_App_AND_Project_Unchanged()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            string editorProjectText = string.Empty;
            int initialAppCount = 0;

            ApplicationClass app1, app2;
            ProjectClass project1;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                Assert.NotEqual(0, originalLog.ID);
                app1 = originalLog.Application;
                app2 = new ApplicationClass(originalLog.Author, "VS Code");
                project1 = originalLog.Project;
                editorApplicationText = app2.Name;
                editorProjectText = project1.Name;
                await master.Applications.AddAsync(app2);
                await master.SaveChangesAsync();
                initialAppCount = await master.Applications.CountAsync();

                Assert.NotNull(await master.Applications.SingleOrDefaultAsync(a => a.Name == "VS Code"));
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                // APPLICATION

                bool appIsNew = false;
                var account = incomingLog.Author;

                if (incomingLog.Application.Name != editorApplicationText)
                {
                    // App may be the same or changed. Changed app may be new or existing
                    var app = await master.Applications.FirstOrDefaultAsync(a =>
                        a.Name == editorApplicationText);

                    if (app == null)
                    {
                        app = new ApplicationClass(editorApplicationText);
                        app.accountID = account.accountID;
                    }

                    appIsNew = app.appID == 0;

                    if (appIsNew)
                        Assert.Fail("Application should be existing.");
                    else
                    {
                        incomingLog.appID = app.appID;
                        incomingLog.Application = null;
                    }
                }

                // PROJECT

                if (!appIsNew)
                {
                    bool projectBelongsToNewApp = incomingLog.Project.appID == incomingLog.appID;

                    if (string.IsNullOrEmpty(editorProjectText))
                    {
                        // Set to "Unnamed Project" default
                        incomingLog.projectID = 1;
                        incomingLog.Project = null;
                    }
                    else if (incomingLog.Project!.Name != editorProjectText || !projectBelongsToNewApp)
                    {
                        // Find the project by Name AND the NEW App ID
                        var project = await master.Projects.FirstOrDefaultAsync(p =>
                            p.Name == editorProjectText && p.appID == incomingLog.appID);

                        if (project == null)
                        {
                            project = new ProjectClass(editorProjectText);
                            project.accountID = account.accountID;
                            project.appID = incomingLog.appID;
                        }

                        if (project.projectID == 0)
                        {
                            incomingLog.Project = project;
                        }
                        else
                        {
                            incomingLog.projectID = project.projectID;
                            incomingLog.Project = null;
                        }
                    }

                }
                else
                {
                    Assert.Fail("Application should be existing.");
                }



                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }



            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // App changed
                Assert.NotEqual(app1.Name, result.Application.Name);
                Assert.NotEqual(app1.appID, result.appID);
                Assert.Equal(app2.Name, result.Application.Name);

                // App count hasn't changed
                int appCount = await master.Applications.CountAsync();
                Assert.True(initialAppCount == appCount);

                // Project unchanged
                Assert.Equal(project1.Name, result.Project.Name);

                // Project's app uses the new existing app
                Assert.Equal(app2.appID, result.appID);
                Assert.NotEqual(app1.appID, result.appID);
                Assert.Equal(result.Project.appID, result.appID);

                // Original unchanged project still points to old appID
                Assert.NotEqual(project1.appID, result.appID);

                // Project ID changes because only 1 project in this test
                Assert.NotEqual(project1.projectID, result.Project.projectID);

            }
        }



        #endregion








        #region #2 EXISTING TO NEW APP



        /// <summary>
        /// Test the updating of application in the log to an existing application.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_Should_Change_App_To_NEW_App()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            const string editorApplicationText = "Komodo IDE";
            const string editorProjectText = "Project Delta";

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
            }


            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                // Parent updates
                // APPLICATION

                bool appIsNew = false;
                var account = incomingLog.Author;

                if (incomingLog.Application.Name != editorApplicationText)
                {
                    // App may be the same or changed. Changed app may be new or existing
                    var app = await master.Applications.FirstOrDefaultAsync(a =>
                        a.Name == editorApplicationText);

                    if (app == null)
                    {
                        app = new ApplicationClass(editorApplicationText);
                        app.accountID = account.accountID;
                    }

                    appIsNew = app.appID == 0;

                    if (appIsNew)
                        incomingLog.Application = app;
                    else
                    {
                        incomingLog.appID = app.appID;
                        incomingLog.Application = null;
                    }
                }



                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }


            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // Parent scalar/nav assertions
                Assert.Equal(editorApplicationText, result.Application.Name);
            }
        }




        /// <summary>
        /// #2 + #A2
        /// Test the updating of a project to a NEW project in the log with a NEW application.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_Should_Change_Project_To_NEW_Project_AND_NEW_App()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            const string editorApplicationText = "Komodo IDE";
            const string editorProjectText = "Project Delta";
            int initialAppCount = 0;
            int initialProjectCount = 0;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                initialAppCount = await master.Applications.CountAsync();
                initialProjectCount = await master.Projects.CountAsync();
            }


            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                // Parent updates
                // APPLICATION

                bool appIsNew = false;
                var account = incomingLog.Author;

                if (incomingLog.Application.Name != editorApplicationText)
                {
                    // App may be the same or changed. Changed app may be new or existing
                    var app = await master.Applications.FirstOrDefaultAsync(a =>
                        a.Name == editorApplicationText);

                    if (app == null)
                    {
                        app = new ApplicationClass(editorApplicationText);
                        app.accountID = account.accountID;
                    }

                    appIsNew = app.appID == 0;

                    if (appIsNew)
                        incomingLog.Application = app;
                    else
                    {
                        incomingLog.appID = app.appID;
                        incomingLog.Application = null;
                    }
                }


                // PROJECT

                // App shud be new
                Assert.True(appIsNew);

                incomingLog.Project = new ProjectClass(account, editorProjectText, incomingLog.Application!);

                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }

            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // App name changed
                Assert.Equal(editorApplicationText, result.Application.Name);
                Assert.NotEqual(originalLog.appID, result.appID);

                // App Count is greater than initial Count
                int appCount = await master.Applications.CountAsync();
                Assert.True(initialAppCount < appCount);

                // Project name changed
                Assert.Equal(editorProjectText, result.Project.Name);
                Assert.NotEqual(originalLog.Project.Name, result.Project.Name);

                // Project has the new app
                Assert.Equal(result.appID, result.Project.appID);

                // Project Count is greater than initial count
                int projectCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount < projectCount);
            }
        }





        #endregion








        #region #3 UNCHANGED APP



        /// <summary>
        /// #3 + #A2
        /// Test the updating of a project to a new project in the log with an existing 
        /// application.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_Should_Change_Project_To_NEW_Project_AND_App_Unchanged()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            const string editorProjectText = "Project Delta";
            int initialProjectCount = 0;

            ApplicationClass app1;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                app1 = originalLog.Application;
                initialProjectCount = await master.Projects.CountAsync();
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                // Parent updates
                // APPLICATION

                bool appIsNew = false;
                editorApplicationText = app1.Name;
                var account = incomingLog.Author;

                if (incomingLog.Application.Name != editorApplicationText)
                {
                    // App may be the same or changed. Changed app may be new or existing
                    var app = await master.Applications.FirstOrDefaultAsync(a =>
                        a.Name == editorApplicationText);

                    if (app == null)
                    {
                        app = new ApplicationClass(editorApplicationText);
                        app.accountID = account.accountID;
                    }

                    appIsNew = app.appID == 0;

                    if (appIsNew)
                        incomingLog.Application = app;
                    else
                    {
                        incomingLog.appID = app.appID;
                        incomingLog.Application = null;
                    }
                }

                // PROJECT

                if (!appIsNew)
                {
                    bool projectBelongsToNewApp = incomingLog.Project.appID == incomingLog.appID;

                    if (string.IsNullOrEmpty(editorProjectText))
                    {
                        // Set to "Unnamed Project" default
                        incomingLog.projectID = 1;
                        incomingLog.Project = null;
                    }
                    else if (incomingLog.Project!.Name != editorProjectText || !projectBelongsToNewApp)
                    {
                        // Find the project by Name AND the NEW App ID
                        var project = await master.Projects.FirstOrDefaultAsync(p =>
                            p.Name == editorProjectText && p.appID == incomingLog.appID);

                        if (project == null)
                        {
                            project = new ProjectClass(editorProjectText);
                            project.accountID = account.accountID;
                            project.appID = incomingLog.appID;
                        }

                        if (project.projectID == 0)
                        {
                            incomingLog.Project = project;
                        }
                        else
                        {
                            incomingLog.projectID = project.projectID;
                            incomingLog.Project = null;
                        }
                    }

                }

                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }

            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // Project Changed to New
                Assert.Equal(editorProjectText, result.Project.Name);
                Assert.NotEqual(originalLog.Project.Name, result.Project.Name);

                // Project Count Changed
                int projectsCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount < projectsCount);

                // App Unchanged
                Assert.Equal(app1.Name, result.Application.Name);
                Assert.Equal(originalLog.appID, result.appID);
                Assert.Equal(result.Project.appID, result.appID);
            }
        }



        #endregion










    }



}
