namespace Backend3DForge.Requests.Administrate
{
    public class PrintMaterialColorRequest
    {
        public string PrintMaterialId { get; set; }
        public string Name { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public float Cost { get; set; }
    }
}
