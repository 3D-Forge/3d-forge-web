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
				isFirstStart = db.Users.Count() == 0;
				if (isFirstStart)
				{
					InitDatabase(db);
					isFirstStart = false;
				}
			}
		}

		private static void InitDatabase(DbApp db)
		{
			if (db.Users.Count() == 0)
			{
				db.Users.AddRange(new List<User>
				{
					new User() {
						Login = "admin",
						PasswordHash = "someHash",
						Email = "admin@3df.com",
						Birthday = DateTime.MinValue,
						CanAdministrateSystem = true,
						CanAdministrateForum = true,
						CanModeratеCatalog = true,
						CanRetrieveDelivery = true,
						RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "developer",
						PasswordHash = "someHash",
						Email = "developer@3df.com",
						Birthday = DateTime.MinValue,
						CanAdministrateSystem = true,
						CanAdministrateForum = true,
						CanModeratеCatalog = true,
						CanRetrieveDelivery = true,
						RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "manager",
						PasswordHash = "someHash",
						Email = "manager@3df.com",
						Birthday = DateTime.MinValue,
						CanAdministrateForum = true,
						CanModeratеCatalog = true,
						CanRetrieveDelivery = true,
						RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "accountant",
						PasswordHash = "someHash",
						Email = "accountant@3df.com",
						Birthday = DateTime.MinValue,
						CanAdministrateForum = true,
						CanRetrieveDelivery = true,
						RegistrationDate = DateTime.Now
					},
					new User() {
						Login = "support",
						PasswordHash = "someHash",
						Email = "support@3df.com",
						Birthday = DateTime.MinValue,
						CanAdministrateForum = true,
						CanRetrieveDelivery = true,
						RegistrationDate = DateTime.Now
					},
				});
				db.SaveChanges();
			}

			db.SaveChanges();
		}
	}
}
