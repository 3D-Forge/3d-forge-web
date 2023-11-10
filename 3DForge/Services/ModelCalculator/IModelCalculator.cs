namespace Backend3DForge.Services.ModelCalculator
{
    public interface IModelCalculator
    {
        Task<ModelCalculatorResult> CalculateSurfaceArea(Stream modelStream, string format);
    }
}
