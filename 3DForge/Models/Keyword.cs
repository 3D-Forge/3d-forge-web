using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Backend3DForge.Models
{
	public class Keyword
	{
		[Key]
		[MaxLength(30)]
		[RegexStringValidator("^[\\w\\d_]+$")]
		public string Name { get; set; }

		public Keyword() { }

        public Keyword(string name)
        { 
			this.Name = name;
		}

        public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();
	}
}
