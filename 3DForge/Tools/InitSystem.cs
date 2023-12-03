using Backend3DForge.Models;
using Backend3DForge.Models.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
            InitializeTableWithData<User, User>(
                dbContext: db,
                dataKey: "users",
                getEntityDbSet: (db) => db.Users,
                createNewEntity: (record, index) =>
                {
                    record.RegistrationDate = DateTime.UtcNow;
                    record.PasswordHash = PasswordTool.Hash(record.PasswordHash);
                    return record;
                },
                validate: (existingEntity, entity) => existingEntity.Login == entity.Login || existingEntity.Email == entity.Email
            );

            InitializeTableWithData<ModelCategory, string>(
                dbContext: db,
                dataKey: "categories",
                getEntityDbSet: (db) => db.ModelCategories,
                createNewEntity: (record, index) => new(record),
                validate: (existingEntity, entity) => existingEntity.ModelCategoryName == entity
            );

            InitializeTableWithData<PrintExtension, string>(
                dbContext: db,
                dataKey: "printExtensions",
                getEntityDbSet: (db) => db.PrintExtensions,
                createNewEntity: (record, index) => new(record),
                validate: (existingEntity, entity) => existingEntity.Name == entity
            );

            InitializeTableWithData<ModelExtension, string>(
                dbContext: db,
                dataKey: "previewExtensions",
                getEntityDbSet: (db) => db.ModelExtensions,
                createNewEntity: (record, index) => new(record),
                validate: (existingEntity, entity) => existingEntity.Name == entity
            );

            InitializeTableWithData<PrintType, string>(
                dbContext: db,
                dataKey: "printTypes",
                getEntityDbSet: (db) => db.PrintTypes,
                createNewEntity: (record, index) => new(record),
                validate: (existingEntity, entity) => existingEntity.Name == entity
            );

            InitializeTableWithData<PrintMaterial, PrintMaterial>(
                dbContext: db,
                dataKey: "printMaterials",
                getEntityDbSet: (db) => db.PrintMaterials,
                createNewEntity: (record, index) => record,
                validate: (existingEntity, entity) => existingEntity.Name == entity.Name
            );

            var forPrintingPath = Path.Combine(Directory.GetCurrentDirectory(), "src", "3dModels", "forPrinting");
            var filesForPrinting = Directory.GetFiles(forPrintingPath, "*.*", SearchOption.AllDirectories);

            var forPreviewPath = Path.Combine(Directory.GetCurrentDirectory(), "src", "3dModels", "forPreview");
            var filesForPreview = Directory.GetFiles(forPreviewPath, "*.*", SearchOption.AllDirectories);

            var iconsPath = Path.Combine(Directory.GetCurrentDirectory(), "src", "3dModels", "icons");
            var filesIcons = Directory.GetFiles(iconsPath, "*.*", SearchOption.AllDirectories);


            InitializeTableWithData<CatalogModel, CatalogModelJson>(
                dbContext: db,
                dataKey: "catalogModels",
                getEntityDbSet: (db) => db.CatalogModels,
                createNewEntity: (record, index) =>
                {
                    var keywords = new HashSet<Keyword>();
                    var categories = new HashSet<ModelCategory>();

                    foreach (var keyword in record.Keywords)
                    {
                        Keyword? keywordObj = db.Keywords.SingleOrDefault(k => k.Name == keyword);
                        keywords.Add(
                            keywordObj == null ?
                            db.Keywords.Add(new Keyword(keyword)).Entity :
                            keywordObj
                            );
                    }

					foreach (var category in record.Categories)
                    {
                        ModelCategory? categoryObj = db.ModelCategories.SingleOrDefault(c => c.Id == category);
                        if (categoryObj is null)
                        {
                            throw new KeyNotFoundException($"'{category}' not found");
                        }
                        else
                        {
                            categories.Add(categoryObj);
                        }
                    }

                    var printFile = filesForPrinting.FirstOrDefault(f => Path.GetFileName(f).StartsWith($"{index}."));
                    if (printFile is null)
                        throw new FileNotFoundException();
                    var printFileInfo = new FileInfo(printFile);
                    var printFileEx = Path.GetExtension(printFile).Remove(0, 1);
                    var printFileDirPath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "FilesToPrint");

                    CopyFile(printFile, Path.Combine(printFileDirPath, Path.GetFileName(printFile)), true);

                    var previewFile = filesForPreview.FirstOrDefault(f => Path.GetFileName(f).StartsWith($"{index}."));
                    if (previewFile is null)
                        throw new FileNotFoundException();
                    var previewFileInfo = new FileInfo(previewFile);
                    var previewFileEx = Path.GetExtension(previewFile).Remove(0, 1);
                    var previewFileDirPath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "PreviewFiles");
                    CopyFile(printFile, Path.Combine(previewFileDirPath, Path.GetFileName(previewFile)), true);

                    var user = db.Users.Single(p => p.Login == record.UserLogin);
                    var printExtension = db.PrintExtensions.Single(p => p.Name == printFileEx);
                    var modelExtension = db.ModelExtensions.Single(p => p.Name == previewFileEx);

                    var model = new CatalogModel
                    {
                        Name = record.Name,
                        Description = record.Description,
                        Uploaded = DateTime.UtcNow,
                        Publicized = DateTime.UtcNow,
                        XSize = record.XSize,
                        YSize = record.YSize,
                        ZSize = record.ZSize,
                        Volume = record.Volume,
                        Depth = record.Depth,
                        Rating = record.Rating,
                        MinPrice = record.MinPrice,
                        PrintFileSize = printFileInfo.Length,
                        PrintExtension = printExtension,
                        ModelFileSize = previewFileInfo.Length,
                        ModelExtension = modelExtension,
                        UserId = user.Id,
                        User = user,
                    };

                    model.Keywords.AddRange(keywords);
                    model.ModelCategoryes.AddRange(categories);

                    return model;
                },
                validate: (existingEntity, entity) => false,
                entityAdded: (entity) =>
                {
                    var iconFile = filesIcons.FirstOrDefault(f => Path.GetFileName(f) == $"{entity.Id}.png");
                    if (iconFile is null)
                        throw new FileNotFoundException();
                    var iconFileInfo = new FileInfo(iconFile);
                    var iconFileEx = Path.GetExtension(iconFile).Remove(0, 1);
                    var iconFileDirPath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "3DModelsPictures");
                    CopyFile(iconFile, Path.Combine(iconFileDirPath, Path.GetFileName(iconFile)), true);

                    db.ModelPictures.Add(new ModelPicture
                    {
                        CatalogModelId = entity.Id,
                        CatalogModel = entity,
                        Size = iconFileInfo.Length,
                        Uploaded = DateTime.UtcNow
                    });

                    db.SaveChanges();
                }
            );

            InitializeTableWithData<OrderStatus, string>(
                dbContext: db,
                dataKey: "orderedStatuses",
                getEntityDbSet: (db) => db.OrderStatuses,
                createNewEntity: (record, index) => new(record),
				validate: (existingEntity, entity) => false
				);

		}

        /// <summary>
        /// Initializes a database table with data based on provided configuration.
        /// </summary>
        /// <typeparam name="TableEntity">The type of the entity representing the database table.</typeparam>
        /// <typeparam name="EntityType">The type of the entity from the provided data.</typeparam>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="dataKey">The key to retrieve data from the configuration.</param>
        /// <param name="getEntityDbSet">A function to get the DbSet for the specified entity.</param>
        /// <param name="createNewEntity">A function that creates a new entity based on the provided data.</param>
        /// <param name="validate">A function to validate data against existing entities.</param>
        /// <exception cref="InvalidDataException">Thrown when invalid or duplicate data is encountered.</exception>
        private static void InitializeTableWithData<TableEntity, EntityType>(
            DbApp dbContext,
            string dataKey,
            Func<DbApp, DbSet<TableEntity>> getEntityDbSet,
            Func<EntityType, int, TableEntity> createNewEntity,
            Func<TableEntity, EntityType, bool> validate,
            Action<TableEntity>? entityAdded = default
        ) where TableEntity : class
          where EntityType : class
        {
            var table = getEntityDbSet(dbContext);
            if (table.Any())
            {
                return;
            }

            var fileContent = File.ReadAllText(Path.Combine("src", "dbDefaultData", $"{dataKey}.json"));

            JArray dataConfig = JArray.Parse(fileContent);

            int i = 0;

            foreach (var dataItem in dataConfig)
            {
                var entity = dataItem.ToObject<EntityType>() ?? throw new InvalidDataException($"Invalid data found: {dataItem}");
                if (table.AsEnumerable().Any(existingEntity => validate.Invoke(existingEntity, entity)))
                {
                    Console.WriteLine($"Invalid data found in 'dbDefaultData{Path.DirectorySeparatorChar}{dataKey}.json'. Value: '{entity}' is duplicated.");
                    continue;
                }

                var dbEntity = table.Add(createNewEntity.Invoke(entity, ++i)).Entity;

                if (entityAdded is not null)
                {
                    dbContext.SaveChanges();
                    entityAdded?.Invoke(dbEntity);
                }
            }

            dbContext.SaveChanges();
        }

        private static void CopyFile(string src, string dist, bool overwrite = false)
        {
            string[] parts = dist.Split(Path.DirectorySeparatorChar);

            string path = parts[0];

            for(int i = 1; i < parts.Length - 1; i++)
            {
                var part = Path.Combine(path, parts[i]);
                if (!Directory.Exists(part))
                {
                    Directory.CreateDirectory(part);
                }
                path = part;
            }
            File.Copy(src, Path.Combine(path, parts.Last()), overwrite);
        }
    }
}
