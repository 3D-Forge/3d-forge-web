using Backend3DForge.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Services
{
	public static class DataCleaner
	{
		private static Thread? cleaner;
		private static DbApp? DB;
		private static bool isRunning = false;

		public static void Start(DbApp db)
		{
			DB = db;
			isRunning = true;

			cleaner = new(CleanData)
			{
				IsBackground = true
			};
			cleaner.Start();
		}

		public static void Stop()
		{
			DB = null;
			isRunning = false;

			if (cleaner?.ThreadState == ThreadState.WaitSleepJoin)
			{
				cleaner.Abort();
			}
		}

		private static void CleanData()
		{
			if (DB == null || !isRunning)
			{
				Stop();
				return;
			}

			while (isRunning)
			{
				Thread.Sleep(3600000);
				CleanActivationCodes();
			}
		}

		private static void CleanActivationCodes()
		{
			if (DB == null || !isRunning)
			{
				Stop();
				return;
			}

			ActivationCode? activationCode = DB.ActivationCodes
					.Include(p => p.User)
					.FirstOrDefault(p => p.Expires < DateTime.Now);

			while (activationCode != null)
			{
				string actionType = activationCode.Action.Split(',')[0];

                switch (actionType)
				{
					case "confirm-registration":
						{
							User user = activationCode.User;
							DB.ActivationCodes.Remove(activationCode);
							DB.Users.Remove(user);
						}
						break;
					case "reset-password-permission":
						{
                            DB.ActivationCodes.Remove(activationCode);
                        }
						break;
                }

				DB.SaveChanges();

				activationCode = DB.ActivationCodes
					.Include(p => p.User)
					.FirstOrDefault(p => p.Expires < DateTime.Now);
			}
		}
	}
}
