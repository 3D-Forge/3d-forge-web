namespace Backend3DForge.Services.ModelCalculator
{
    public class ModelCalculator : IModelCalculator
    {
        public ModelCalculatorResult CalculateSurfaceArea(Stream modelStream, string format)
        {
            if (modelStream is null)
                throw new ArgumentNullException(nameof(modelStream));

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            ModelCalculatorResult result;

            switch (format)
            {
                case "stl":
                    result = CalculateStlSurfaceArea(modelStream);
                    break;
                case "obj":
                    result = CalculateObjSurfaceArea(modelStream);
                    break;
                default:
                    throw new ArgumentException("Unsupported format", nameof(format));
            }

            return result;
        }

        private ModelCalculatorResult CalculateObjSurfaceArea(Stream modelStream)
        {
            ObjTool objReader = new ObjTool();
            return objReader.CalculateFrom(modelStream);
        }

        public ModelCalculatorResult CalculateStlSurfaceArea(Stream modelStream)
        {
            StlTool stlTool = new StlTool();
            return stlTool.CalculateFrom(modelStream);
        }
    }
}
