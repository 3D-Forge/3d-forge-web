namespace Backend3DForge.Models.Json
{
    public class CatalogModelJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserLogin { get; set; }
        public float XSize { get; set; }
        public float YSize { get; set; }
        public float ZSize { get; set; }
        public double Volume { get; set; } = 0;
        public float Depth { get; set; }
        public float Rating { get; set; } = 0;
        public double MinPrice { get; set; } = 0;
        public string[] Keywords { get; set; }
        public int[] Categories { get; set; }
    }
}
