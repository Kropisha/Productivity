using System;
using System.Linq;
using NetProductivity.Models;

namespace NetProductivity.Helpers
{
    public static class SampleData
    {
        public static void Initialize(ProductivityContext context)
        {
            if (!context.User.Any())
            {
                context.User.AddRange(
                    new User
                    {
                        Id = "8010da06-9c55-48ea-9989-41806a115756",
                        UserName = "admin",
                        Role = Roles.Admin.ToString()
                    },
                    new User
                    {
                        Id = "35eee561-7e3a-4b5b-a8b1-d95f1710e90c",
                        UserName = "Garry Smith",
                        Role = Roles.User.ToString()
                    },
                    new User
                    {
                        Id = "6eafcbb5-5a33-4203-8831-c6c4db308adf",
                        UserName = "Kate O'Haur",
                        Role = Roles.User.ToString()
                    },
                    new User
                    {
                        Id = "999381eb-a676-4789-b16f-1118357b8a6e",
                        UserName = "Rick Martin",
                        Role = Roles.User.ToString()
                    }
                    );
            }

            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project
                    {
                        Id = Guid.Parse("1ff525d2-eae9-42e7-ad55-2dfed24a959b"),
                        Name = "House"
                    },
                    new Project
                    {
                        Id = Guid.Parse("dd95dda9-30fc-4e24-95e9-c081d1bdefd1"),
                        Name = "Work"
                    },
                    new Project
                    {
                        Id = Guid.Parse("e3b4bfcc-814f-4001-80b9-190b3dd392d3"),
                        Name = "Family"
                    },
                    new Project
                    {
                        Id = Guid.Parse("0d341e3a-63d3-4043-b54a-0467cb34bce3"),
                        Name = "Travel"
                    }
                    );
            }

            if (!context.Tasks.Any())
            {
                context.Tasks.AddRange(
                    new TaskP
                    {
                        Id = Guid.Parse("992bc133-9402-425f-845e-c8070cc77f15"),
                        Name = "Wash dishes",
                        ProjectId = Guid.Parse("1ff525d2-eae9-42e7-ad55-2dfed24a959b"),
                        Priority = 3,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019,09,22)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("f2b3f146-fab9-4477-b910-740daa35271d"),
                        Name = "Do laundry",
                        ProjectId = Guid.Parse("1ff525d2-eae9-42e7-ad55-2dfed24a959b"),
                        Priority = 3,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019, 09, 21)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("f73fe2bc-1770-4722-a5d2-a62327ea04ff"),
                        Name = "Sweep the floor",
                        ProjectId = Guid.Parse("1ff525d2-eae9-42e7-ad55-2dfed24a959b"),
                        Priority = 2,
                        Status = Status.Active.ToString(),
                        EndDate = new DateTime(2019, 09, 21)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("fc015a83-6ae1-4095-90ba-8c21711ff03b"),
                        Name = "Buy some food",
                        ProjectId = Guid.Parse("1ff525d2-eae9-42e7-ad55-2dfed24a959b"),
                        Priority = 1,
                        Status = Status.Done.ToString(),
                        EndDate = new DateTime(2019, 09, 20)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("666b338d-aef8-4a80-b25d-2859ad73c527"),
                        Name = "Close task",
                        ProjectId = Guid.Parse("dd95dda9-30fc-4e24-95e9-c081d1bdefd1"),
                        Priority = 1,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019, 09, 20)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("6859f7a9-a6c1-42cd-98ad-3671369e3b12"),
                        Name = "Learn English",
                        ProjectId = Guid.Parse("dd95dda9-30fc-4e24-95e9-c081d1bdefd1"),
                        Priority = 2,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019, 09, 30)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("9069a320-163b-47f7-99dd-3fe731ef7f15"),
                        Name = "Watch tutorial",
                        ProjectId = Guid.Parse("dd95dda9-30fc-4e24-95e9-c081d1bdefd1"),
                        Priority = 3,
                        Status = Status.Active.ToString(),
                        EndDate = new DateTime(2019, 09, 30)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("ec519cdc-3cc1-4b09-91eb-5119e9e930aa"),
                        Name = "Pass quiz",
                        ProjectId = Guid.Parse("dd95dda9-30fc-4e24-95e9-c081d1bdefd1"),
                        Priority = 3,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019, 09, 25)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("4fd951e0-3bdb-419a-8de3-3cd4bec1327f"),
                        Name = "Organize weekend",
                        ProjectId = Guid.Parse("e3b4bfcc-814f-4001-80b9-190b3dd392d3"),
                        Priority = 3,
                        Status = Status.Active.ToString(),
                        EndDate = new DateTime(2019, 09, 25)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("96156a4c-5cb6-4dfa-811b-c67bcafa7f31"),
                        Name = "Check homework",
                        ProjectId = Guid.Parse("e3b4bfcc-814f-4001-80b9-190b3dd392d3"),
                        Priority = 2,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019, 09, 25)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("34237b24-ce24-41df-89d5-c315cfd65c87"),
                        Name = "Choose country",
                        ProjectId = Guid.Parse("0d341e3a-63d3-4043-b54a-0467cb34bce3"),
                        Priority = 1,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019, 09, 25)
                    },
                    new TaskP
                    {
                        Id = Guid.Parse("a6b50f0c-3df9-4a8f-b600-1d42212ad63b"),
                        Name = "Book hotel",
                        ProjectId = Guid.Parse("0d341e3a-63d3-4043-b54a-0467cb34bce3"),
                        Priority = 2,
                        Status = Status.New.ToString(),
                        EndDate = new DateTime(2019, 09, 25)
                    }
                    );
            }

            if (!context.UserProjects.Any())
            {
                context.UserProjects.AddRange(
                    new UserProjects
                    {
                        ProjectId = Guid.Parse("1ff525d2-eae9-42e7-ad55-2dfed24a959b"),
                        Status = Status.Active.ToString(),
                        UserId = "35eee561-7e3a-4b5b-a8b1-d95f1710e90c"
                    },
                    new UserProjects
                    {
                        ProjectId = Guid.Parse("dd95dda9-30fc-4e24-95e9-c081d1bdefd1"),
                        Status = Status.Active.ToString(),
                        UserId = "35eee561-7e3a-4b5b-a8b1-d95f1710e90c"
                    },
                    new UserProjects
                    {
                        ProjectId = Guid.Parse("0d341e3a-63d3-4043-b54a-0467cb34bce3"),
                        Status = Status.Active.ToString(),
                        UserId = "8010da06-9c55-48ea-9989-41806a115756"
                    },
                    new UserProjects
                    {
                        ProjectId = Guid.Parse("e3b4bfcc-814f-4001-80b9-190b3dd392d3"),
                        Status = Status.New.ToString(),
                        UserId = "6eafcbb5-5a33-4203-8831-c6c4db308adf"
                    }
                    );
            }

            if (!context.Login.Any())
            {
                context.Login.AddRange(
                    new Login
                    {
                        Email = "admin@ad.min",
                        Password = "AEzFGztoOfTTjPcXruLZXH2DWQuX1iH5EGj0CXoV+y6MJZHP1K3UE5UURhfCOi4ghw==",
                        UserId = "8010da06-9c55-48ea-9989-41806a115756"
                    },
                    new Login
                    {
                        Email = "smith695@gmail.com",
                        Password = "AF5yxcSOgElz/FXzQDHJmR9n2adKNOjw1+agABq8A2OUNXRiW6aVKOGZf/oUMHfPhQ==",
                        UserId = "35eee561-7e3a-4b5b-a8b1-d95f1710e90c"
                    },
                    new Login
                    {
                        Email = "kate85@mail.co",
                        Password = "ALs01pSKcPrpI/etJ8Pqy/JKHqdR/35cg9UEJC6iuqyMGICnOEyPjDNLD/GAYY7Npg==",
                        UserId = "6eafcbb5-5a33-4203-8831-c6c4db308adf"
                    },
                    new Login
                    {
                        Email = "coolboy22@mail.ru",
                        Password = "APbLgfSL1V5bwZY1uGHZYnCxfXSmeHxqPLn/GnNtWK4XB3J/5MBMBR0NYIumDRcDiA==",
                        UserId = "999381eb-a676-4789-b16f-1118357b8a6e"
                    }
                    );
            }

            context.SaveChanges();
        }
    }
}
