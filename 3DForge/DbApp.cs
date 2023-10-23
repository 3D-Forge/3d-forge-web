using Backend3DForge.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge
{
	public class DbApp : DbContext
	{
		public DbApp(DbContextOptions<DbApp> options) : base(options)
		{
			Database.EnsureCreated();
			Tools.InitSystem.Init(this);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<ActivationCode> ActivationCodes { get; set; }
		public DbSet<ModelExtension> ModelExtensions { get; set; }
		public DbSet<PrintExtension> PrintExtensions { get; set; }
		public DbSet<CatalogModel> CatalogModels { get; set; }
		public DbSet<Keyword> Keywords { get; set; }
		public DbSet<ModelCategory> ModelCategories { get; set; }
		public DbSet<PrintMaterial> PrintMaterials { get; set; }
		public DbSet<PrintType> PrintTypes { get; set; }
		public DbSet<KeywordCatalogModel> KeywordCatalogModels { get; set; }
		public DbSet<CatalogCategoryModel> CatalogCategoryModels { get; set; }
		public DbSet<OrderStatus> OrderStatuses { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderStatusOrder> OrderStatusOrders { get; set; }
		public DbSet<OrderedModel> OrderedModels { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<ForumThread> ForumThreads { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<CatalogModelResponse> CatalogModelResponses { get; set; }
		public DbSet<BannedWord> BannedWords { get; set; }
	}
}
