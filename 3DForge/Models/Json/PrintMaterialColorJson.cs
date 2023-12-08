namespace Backend3DForge.Models.Json
{
    public class PrintMaterialColorJson
    {
        public string Name { get; set; }
        public string RGB { get; set; }
        public float Cost { get; set; }

        public byte R => byte.Parse(RGB.Split(',')[0]);
        public byte G => byte.Parse(RGB.Split(',')[1]);
        public byte B => byte.Parse(RGB.Split(',')[2]);
    }
}
