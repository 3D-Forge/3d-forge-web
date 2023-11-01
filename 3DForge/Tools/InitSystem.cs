using Backend3DForge.Models;

namespace Backend3DForge.Tools
{
    public static class InitSystem
    {
		private static bool isFirstStart = true;

		public static void Init(DbApp db)
		{
			if (isFirstStart)
			{
				isFirstStart = !db.Users.Any();
				if (isFirstStart)
				{
					InitDatabase(db);
					isFirstStart = false;
				}
			}
		}

		private static void InitDatabase(DbApp db)
		{
			if (!db.Users.Any())
			{
				db.Users.AddRange(new List<User>
				{
					new User() {
						Login = "admin",
						PasswordHash = PasswordTool.Hash("somePassword"),
						Email = "admin@3df.com",
						Firstname = "Артем",
						Lastname = "Лаврінєнко",
						Midname = "Леонідович",
						Country = "Україна",
						City = "Харків",
						PhoneNumber = "+3456465356",
						House = "58",
						CanAdministrateSystem = true,
						CanAdministrateForum = true,
						CanModerateCatalog = true,
						CanRetrieveDelivery = true,
                        IsActivated = true,
                        RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "developer",
						PasswordHash = PasswordTool.Hash("somePassword"),
						Email = "developer@3df.com",
						CanAdministrateSystem = true,
						CanAdministrateForum = true,
						CanModerateCatalog = true,
						CanRetrieveDelivery = true,
                        IsActivated = true,
                        RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "manager",
						PasswordHash = PasswordTool.Hash("somePassword"),
						Email = "manager@3df.com",
						CanAdministrateForum = true,
						CanModerateCatalog = true,
						CanRetrieveDelivery = true,
                        IsActivated = true,
                        RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "accountant",
						PasswordHash = PasswordTool.Hash("somePassword"),
						Email = "accountant@3df.com",
						CanAdministrateForum = true,
						CanRetrieveDelivery = true,
                        IsActivated = true,
                        RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "support",
						PasswordHash = PasswordTool.Hash("somePassword"),
						Email = "support@3df.com",
						CanAdministrateForum = true,
						CanRetrieveDelivery = true,
                        IsActivated = true,
                        RegistrationDate = DateTime.Now
					},
				});
				db.SaveChanges();
			}

			db.SaveChanges();
		}
	}
}
