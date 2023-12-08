namespace Backend3DForge.Models.Json
{
    public class PrintMaterialJson
    {
        public string Name { get; set; }
        public string PrintTypeName { get; set; }
        public float Density { get; set; }
        public float Cost { get; set; }
        public ICollection<PrintMaterialColorJson> Colors { get; set; } = new List<PrintMaterialColorJson>();
    }
}
