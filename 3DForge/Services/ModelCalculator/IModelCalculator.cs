namespace Backend3DForge.Services.ModelCalculator
{
    public interface IModelCalculator
    {
        ModelCalculatorResult CalculateSurfaceArea(Stream modelStream, string format);
    }
}
