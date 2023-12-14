using Backend3DForge.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge
{
	public class DbApp : DbContext
	{
		public DbApp(DbContextOptions<DbApp> options) : base(options)
		{
            Database.EnsureCreated();
            //Database.EnsureDeleted();
            Tools.InitSystem.Init(this);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<ActivationCode> ActivationCodes { get; set; }
		public DbSet<ModelExtension> ModelExtensions { get; set; }
		public DbSet<PrintExtension> PrintExtensions { get; set; }
		public DbSet<CatalogModel> CatalogModels { get; set; }
		public DbSet<ModelPicture> ModelPictures { get; set; }
		public DbSet<Keyword> Keywords { get; set; }
		public DbSet<ModelCategory> ModelCategories { get; set; }
		public DbSet<PrintMaterial> PrintMaterials { get; set; }
		public DbSet<PrintType> PrintTypes { get; set; }
		public DbSet<PrintMaterialColor> PrintMaterialColors { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderStatusOrder> OrderStatusOrders { get; set; }
		public DbSet<OrderedModel> OrderedModels { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<ForumThread> ForumThreads { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<CatalogModelFeedback> CatalogModelFeedbacks { get; set; }
		public DbSet<BannedWord> BannedWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Post>()
               .HasOne(p => p.User)
               .WithMany()
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CatalogModelFeedback>()
                .HasOne(cmr => cmr.Order)
                .WithMany()
                .HasForeignKey(cmr => cmr.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CatalogModels)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Keyword>()
                .HasKey(p => p.Name)
                .HasName("PK_Keyword");

            modelBuilder.Entity<PrintExtension>()
                .HasKey(p => p.Id)
                .HasName("PK_PrintExtension");

            modelBuilder.Entity<ModelExtension>()
                .HasKey(p => p.Id)
                .HasName("PK_ModelExtension");

            modelBuilder.Entity<OrderedModel>()
                .HasOne(o => o.PrintType)
                .WithMany(p => p.OrderedModels)
                .HasForeignKey(o => o.PrintTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderedModel>()
                .HasOne(o => o.CatalogModel)
                .WithMany()
                .HasForeignKey(o => o.CatalogModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderedModel>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderedModels)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderedModels)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CatalogModel>()
                .HasMany(cm => cm.Pictures)
                .WithOne(mp => mp.CatalogModel)
                .HasForeignKey(mp => mp.CatalogModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrintType>()
                .HasKey(p => p.Id)
                .HasName("PK_PrintType");

            modelBuilder.Entity<PrintMaterial>()
                .HasKey(p => p.Id)
                .HasName("PK_PrintMaterial");

            modelBuilder.Entity<PrintMaterial>()
                .HasMany(p => p.PrintMaterialColors)
                .WithOne(p => p.PrintMaterial)
                .HasForeignKey(p => p.PrintMaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderedModel>()
                .HasOne(o => o.PrintMaterialColor)
                .WithMany(p => p.OrderedModels)
                .HasForeignKey(o => o.PrintMaterialColorId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
		}
	}
}
