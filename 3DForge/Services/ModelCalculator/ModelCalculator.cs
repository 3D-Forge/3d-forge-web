using Backend3DForge.Services.ModelCalculator.Models;
using QuantumConcepts.Formats.StereoLithography;

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
            STLDocument stl = STLDocument.Read(modelStream);

            // size in mm
            double xSize = stl.Max(p => p.Vertices.Max(v => v.X)) - stl.Min(p => p.Vertices.Min(v => v.X));
            double ySize = stl.Max(p => p.Vertices.Max(v => v.Y)) - stl.Min(p => p.Vertices.Min(v => v.Y));
            double zSize = stl.Max(p => p.Vertices.Max(v => v.Z)) - stl.Min(p => p.Vertices.Min(v => v.Z));
            // mm^2
            double surfaceArea = stl.Sum(p => CalculateArea(p.Vertices.ToVector3()));
            // mm^3
            double volume = stl.Sum(p => SignedVolumeOfTriangle(p.Vertices.ToVector3()));

            return new ModelCalculatorResult
            {
                SurfaceArea = Math.Round(surfaceArea, 0),
                Volume = Math.Round(volume, 0),
                X = Math.Round(xSize, 2),
                Y = Math.Round(ySize, 2),
                Z = Math.Round(zSize, 2)
            };
        }

        private double CalculateArea(IList<Vector3> vertices)
        {
            double totalArea = 0;

            for (int i = 1; i < vertices.Count() - 1; i++)
            {
                Vector3 vertex1 = vertices[0];
                Vector3 vertex2 = vertices[i];
                Vector3 vertex3 = vertices[i + 1];

                double triangleArea = CalculateTriangleArea(vertex1, vertex2, vertex3);
                totalArea += triangleArea;
            }

            return totalArea;
        }

        private double SignedVolumeOfTriangle(IList<Vector3> vertices)
        {
            Vector3 p1 = vertices[0];
            Vector3 p2 = vertices[1];
            Vector3 p3 = vertices[2];

            double v321 = p3.X * p2.Y * p1.Z;
            double v231 = p2.X * p3.Y * p1.Z;
            double v312 = p3.X * p1.Y * p2.Z;
            double v132 = p1.X * p3.Y * p2.Z;
            double v213 = p2.X * p1.Y * p3.Z;
            double v123 = p1.X * p2.Y * p3.Z;

            return (1.0 / 6.0) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

        private double CalculateTriangleArea(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            double side1 = Math.Sqrt(Math.Pow(vertex2.X - vertex1.X, 2) + Math.Pow(vertex2.Y - vertex1.Y, 2) + Math.Pow(vertex2.Z - vertex1.Z, 2));
            double side2 = Math.Sqrt(Math.Pow(vertex3.X - vertex2.X, 2) + Math.Pow(vertex3.Y - vertex2.Y, 2) + Math.Pow(vertex3.Z - vertex2.Z, 2));
            double side3 = Math.Sqrt(Math.Pow(vertex1.X - vertex3.X, 2) + Math.Pow(vertex1.Y - vertex3.Y, 2) + Math.Pow(vertex1.Z - vertex3.Z, 2));

            double semiPerimeter = (side1 + side2 + side3) / 2;
            double triangleArea = Math.Sqrt(semiPerimeter * (semiPerimeter - side1) * (semiPerimeter - side2) * (semiPerimeter - side3));

            return triangleArea;
        }

    }
}
