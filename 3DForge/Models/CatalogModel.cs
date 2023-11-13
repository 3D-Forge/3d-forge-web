using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class CatalogModel
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string PrintExtensionName { get; set; }
		public PrintExtension PrintExtension { get; set; }
		[Required]
		public long PrintFileSize { get; set; }
		[Required]
		public string ModelExtensionName { get; set; }
		public ModelExtension ModelExtension { get; set; }
		[Required]
		public long ModelFileSize { get; set; }
		[Required]
		public int UserId { get; set; }
		public User User { get; set; }
		[Required]
		public DateTime Uploaded { get; set; }
		public DateTime? Publicized { get; set; } = null;
		[Required]
		public float XSize { get; set; }
		[Required]
		public float YSize { get; set; }
		[Required] 
		public float ZSize { get; set;}
        [Required]
        public double Volume { get; set; } = 0;
        [Required]
		public float Depth { get; set; }
		public float Rating { get; set; } = 0;
		public double MinPrice { get; set; } = 0;

        public bool IsModelPublicized { get => Publicized is not null; }

		public ICollection<ModelCategory> ModelCategoryes { get; set; } = new List<ModelCategory>();
		public ICollection<Keyword> Keywords { get; set; } = new List<Keyword>();
		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
	}
}
