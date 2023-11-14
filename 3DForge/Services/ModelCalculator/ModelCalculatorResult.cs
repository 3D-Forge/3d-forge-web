namespace Backend3DForge.Services.ModelCalculator
{
    /// <summary>
    /// All values are in millimeters
    /// </summary>
    public class ModelCalculatorResult
    {
        public required float X { get; set; }
        public required float Y { get; set; }
        public required float Z { get; set; }
        public required float Volume { get; set; }
        public required float SurfaceArea { get; set; }
    }
}
