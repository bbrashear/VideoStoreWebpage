namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'1633030c-5c0a-43ea-ba7c-7a661d9517d8', N'admin@vidly.com', 0, N'AIY428m3c3kqrGk6F7seBROwPuh4hR3SR6qY0b3kRaJLBoP3c8qTLSzOpYaRoI4ftg==', N'069b6007-ea09-4195-98aa-4cf2a5a35657', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'b3f47b6a-2e2f-4f22-a60f-62ccb4126cb8', N'guest@vidly.com', 0, N'AACqB3P8Sa1TJJl5wtrbosVtLdSDkFREwSKK7/apy2fO0vtDLR4f2qIEPmsx5iw9jQ==', N'35ad1012-4340-441b-a72f-dbacf77b7ecb', NULL, 0, 0, NULL, 1, 0, N'guest@vidly.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'ff406be5-a56e-4282-8a9c-1ca4cc8b6388', N'CanManageMovies')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1633030c-5c0a-43ea-ba7c-7a661d9517d8', N'ff406be5-a56e-4282-8a9c-1ca4cc8b6388')
");
        }
        
        public override void Down()
        {
        }
    }
}
