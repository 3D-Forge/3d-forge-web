using Backend3DForge.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backend3DForge.Tools
{
    public static class InitSystem
    {
        private static bool isFirstStart = true;
        private static object locker = new object();

        public static void Init(DbApp db)
        {
            lock (locker)
            {
                if (isFirstStart)
                {
                    InitDatabase(db);
                    isFirstStart = false;
                }
            }
        }

        private static void InitDatabase(DbApp db)
        {
            var fileContent = File.ReadAllText(Path.Combine("src", "dbDefaultData.json"));
            JObject? config = JsonConvert.DeserializeObject(fileContent) as JObject;

            if (config is null)
            {
                throw new InvalidDataException();
            }

            InitializeTableWithData<User, User>(
                dbContext: db,
                dataConfig: config,
                dataKey: "users",
                getEntityDbSet: (db) => db.Users,
                createNewEntity: (recodr) =>
                {
                    recodr.RegistrationDate = DateTime.UtcNow;
                    recodr.PasswordHash = PasswordTool.Hash(recodr.PasswordHash);
                    return recodr;
                },
                validate: (existingEntity, entity) => existingEntity.Login == entity.Login || existingEntity.Email == entity.Email
            );

            InitializeTableWithData<ModelCategory, string>(
                dbContext: db,
                dataConfig: config,
                dataKey: "categories",
                getEntityDbSet: (db) => db.ModelCategories,
                createNewEntity: (record) => new (record),
                validate: (existingEntity, entity) => existingEntity.ModelCategoryName == entity
            );

            InitializeTableWithData<PrintExtension, string>(
                dbContext: db,
                dataConfig: config,
                dataKey: "printExtensions",
                getEntityDbSet: (db) => db.PrintExtensions,
                createNewEntity: (record) => new (record),
                validate: (existingEntity, entity) => existingEntity.Name == entity
            );

            InitializeTableWithData<ModelExtension, string>(
                dbContext: db,
                dataConfig: config,
                dataKey: "previewExtensions",
                getEntityDbSet: (db) => db.ModelExtensions,
                createNewEntity: (record) => new (record),
                validate: (existingEntity, entity) => existingEntity.Name == entity
            );
        }

        /// <summary>
        /// Initializes a database table with data based on provided configuration.
        /// </summary>
        /// <typeparam name="TableEntity">The type of the entity representing the database table.</typeparam>
        /// <typeparam name="EntityType">The type of the entity from the provided data.</typeparam>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="dataConfig">The configuration containing data.</param>
        /// <param name="dataKey">The key to retrieve data from the configuration.</param>
        /// <param name="getEntityDbSet">A function to get the DbSet for the specified entity.</param>
        /// <param name="createNewEntity">A function that creates a new entity based on the provided data.</param>
        /// <param name="validate">A function to validate data against existing entities.</param>
        /// <exception cref="InvalidDataException">Thrown when invalid or duplicate data is encountered.</exception>
        private static void InitializeTableWithData<TableEntity, EntityType>(
            DbApp dbContext,
            JObject dataConfig,
            string dataKey,
            Func<DbApp, DbSet<TableEntity>> getEntityDbSet,
            Func<EntityType, TableEntity> createNewEntity,
            Func<TableEntity, EntityType, bool> validate
        ) where TableEntity : class
          where EntityType : class
        {
            var table = getEntityDbSet(dbContext);
            if (table.Any())
            {
                return;
            }

            var dataForKey = dataConfig[dataKey];

            if (dataForKey is null)
            {
                throw new InvalidDataException("No data found for the specified key.");
            }

            foreach (var dataItem in dataForKey)
            {
                var entity = dataItem.ToObject<EntityType>();
                if (entity is null)
                {
                    throw new InvalidDataException($"Invalid data found: {dataItem}");
                }

                if (table.AsEnumerable().Any(existingEntity => validate.Invoke(existingEntity, entity)))
                {
                    Console.WriteLine($"Invalid data found in 'dbDefaultData.json -> {dataKey}'. Value: '{entity}' is duplicated.");
                    continue;
                }

                table.Add(createNewEntity.Invoke(entity));
            }

            dbContext.SaveChanges();
        }
    }
}
