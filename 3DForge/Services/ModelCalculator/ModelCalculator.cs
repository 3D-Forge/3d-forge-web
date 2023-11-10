using QuantumConcepts.Formats.StereoLithography;
using System.Text;

namespace Backend3DForge.Services.ModelCalculator
{
    public class ModelCalculator : IModelCalculator
    {
        public async Task<ModelCalculatorResult> CalculateSurfaceArea(Stream modelStream, string format)
        {
            if (modelStream is null)
                throw new ArgumentNullException(nameof(modelStream));

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            ModelCalculatorResult result;

            byte[] modelData;

            using (var ms = new MemoryStream())
            {
                await modelStream.CopyToAsync(ms);
                modelData = ms.ToArray();
            }


            switch (format)
            {
                case "stl":
                    result = CalculateStlSurfaceArea(modelData);
                    break;
                case "stp":
                    result = await CalculateStpSurfaceArea(modelData);
                    break;
                case "step":
                    result = await CalculateStpSurfaceArea(modelData);
                    break;
                case "obj":
                    result = await CalculateObjSurfaceArea(modelData);
                    break;
                default:
                    throw new ArgumentException("Unsupported format", nameof(format));
            }

            return result;
        }

        private async Task<ModelCalculatorResult> CalculateStpSurfaceArea(byte[] modelData)
        {
            throw new NotImplementedException();
        }

        private async Task<ModelCalculatorResult> CalculateObjSurfaceArea(byte[] modelData)
        {
            throw new NotImplementedException();
        }

        public ModelCalculatorResult CalculateStlSurfaceArea(byte[] stlData)
        {
            STLDocument stl;

            using (var stream = new MemoryStream(stlData))
            {
                stl = STLDocument.Read(stream);
            }

            var facets = stl.ToArray();

            // size in mm
            double xSize = stl.Max(p => p.Vertices.Max(v => v.X)) - stl.Min(p => p.Vertices.Min(v => v.X));
            double ySize = stl.Max(p => p.Vertices.Max(v => v.Y)) - stl.Min(p => p.Vertices.Min(v => v.Y));
            double zSize = stl.Max(p => p.Vertices.Max(v => v.Z)) - stl.Min(p => p.Vertices.Min(v => v.Z));
            // mm^2
            double surfaceArea = stl.Sum(p => CalculateArea(p.Vertices));
            double volume = CalculateTriangleVolume(stl.Facets, 1);

            return new ModelCalculatorResult
            {
                SurfaceArea = surfaceArea,
                Volume = volume,
                X = xSize,
                Y = ySize,
                Z = zSize
            };
        }

        private double CalculateArea(IList<Vertex> vertices)
        {
            double totalArea = 0;

            for (int i = 1; i < vertices.Count - 1; i++)
            {
                Vertex vertex1 = vertices[0];
                Vertex vertex2 = vertices[i];
                Vertex vertex3 = vertices[i + 1];

                double triangleArea = CalculateTriangleArea(vertex1, vertex2, vertex3);
                totalArea += triangleArea;
            }

            return totalArea;
        }

        private double CalculateTriangleArea(Vertex vertex1, Vertex vertex2, Vertex vertex3)
        {
            double side1 = Math.Sqrt(Math.Pow(vertex2.X - vertex1.X, 2) + Math.Pow(vertex2.Y - vertex1.Y, 2) + Math.Pow(vertex2.Z - vertex1.Z, 2));
            double side2 = Math.Sqrt(Math.Pow(vertex3.X - vertex2.X, 2) + Math.Pow(vertex3.Y - vertex2.Y, 2) + Math.Pow(vertex3.Z - vertex2.Z, 2));
            double side3 = Math.Sqrt(Math.Pow(vertex1.X - vertex3.X, 2) + Math.Pow(vertex1.Y - vertex3.Y, 2) + Math.Pow(vertex1.Z - vertex3.Z, 2));

            double semiPerimeter = (side1 + side2 + side3) / 2;
            double triangleArea = Math.Sqrt(semiPerimeter * (semiPerimeter - side1) * (semiPerimeter - side2) * (semiPerimeter - side3));

            return triangleArea;
        }

        private double CalculateTriangleVolume(ICollection<Facet> facets, double materialMass)
        {
            double totalVolume = facets.Sum(facet => SignedVolumeOfTriangle(facet));
            double totalMass = totalVolume * materialMass;

            return totalVolume;
        }

        private double SignedVolumeOfTriangle(Facet facet)
        {
            Vertex p1 = facet.Vertices[0];
            Vertex p2 = facet.Vertices[1];
            Vertex p3 = facet.Vertices[2];

            double v321 = p3.X * p2.Y * p1.Z;
            double v231 = p2.X * p3.Y * p1.Z;
            double v312 = p3.X * p1.Y * p2.Z;
            double v132 = p1.X * p3.Y * p2.Z;
            double v213 = p2.X * p1.Y * p3.Z;
            double v123 = p1.X * p2.Y * p3.Z;

            return (1.0 / 6.0) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

    }
}
