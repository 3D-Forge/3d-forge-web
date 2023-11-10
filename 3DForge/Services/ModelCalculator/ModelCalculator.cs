﻿using Backend3DForge.Services.ModelCalculator.Models;
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

            switch (format)
            {
                case "stl":
                    result = CalculateStlSurfaceArea(modelStream);
                    break;
                case "stp":
                    result = await CalculateStpSurfaceArea(modelStream);
                    break;
                case "step":
                    result = await CalculateStpSurfaceArea(modelStream);
                    break;
                case "obj":
                    result = await CalculateObjSurfaceArea(modelStream);
                    break;
                default:
                    throw new ArgumentException("Unsupported format", nameof(format));
            }

            return result;
        }

        private async Task<ModelCalculatorResult> CalculateStpSurfaceArea(Stream stream)
        {
            throw new NotImplementedException();
        }

        private async Task<ModelCalculatorResult> CalculateObjSurfaceArea(Stream modelStream)
        {

            byte[] modelData;

            using (var ms = new MemoryStream())
            {
                await modelStream.CopyToAsync(ms);
                modelData = ms.ToArray();
            }

            ObjModel objModel;

            using (var ms = new MemoryStream(modelData))
            {
                objModel = await ObjReader.ReadObjFile(ms);
            }

            var xSize = objModel.Vertices.Max(p => p.X) - objModel.Vertices.Min(p => p.X);
            var ySize = objModel.Vertices.Max(p => p.Y) - objModel.Vertices.Min(p => p.Y);
            var zSize = objModel.Vertices.Max(p => p.Z) - objModel.Vertices.Min(p => p.Z);
            var surfaceArea = objModel.Faces.Sum(p => CalculateArea(p.Vertices));
            double volume = objModel.Faces.Sum(p => SignedVolumeOfTriangle(p.Vertices));

            return new ModelCalculatorResult
            {
                SurfaceArea = surfaceArea,
                Volume = volume,
                X = xSize,
                Y = ySize,
                Z = zSize
            };
        }
        public ModelCalculatorResult CalculateStlSurfaceArea(Stream modelStream)
        {
            STLDocument stl = STLDocument.Read(modelStream);

            var facets = stl.ToArray();

            // size in mm
            double xSize = stl.Max(p => p.Vertices.Max(v => v.X)) - stl.Min(p => p.Vertices.Min(v => v.X));
            double ySize = stl.Max(p => p.Vertices.Max(v => v.Y)) - stl.Min(p => p.Vertices.Min(v => v.Y));
            double zSize = stl.Max(p => p.Vertices.Max(v => v.Z)) - stl.Min(p => p.Vertices.Min(v => v.Z));
            // mm^2
            double surfaceArea = stl.Sum(p => CalculateArea(p.Vertices.ToVector3()));
            double volume = stl.Sum(p => SignedVolumeOfTriangle(p.Vertices.ToVector3()));

            return new ModelCalculatorResult
            {
                SurfaceArea = surfaceArea,
                Volume = volume,
                X = xSize,
                Y = ySize,
                Z = zSize
            };
        }

        private double CalculateArea(IEnumerable<Vector3> vertices)
        {
            double totalArea = 0;

            for (int i = 1; i < vertices.Count() - 1; i++)
            {
                Vector3 vertex1 = vertices.ElementAt(0);
                Vector3 vertex2 = vertices.ElementAt(i);
                Vector3 vertex3 = vertices.ElementAt(i + 1);

                double triangleArea = CalculateTriangleArea(vertex1, vertex2, vertex3);
                totalArea += triangleArea;
            }

            return totalArea;
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

        private double SignedVolumeOfTriangle(IEnumerable<Vector3> vertices)
        {
            Vertex p1 = vertices.ElementAt(0);
            Vertex p2 = vertices.ElementAt(1);
            Vertex p3 = vertices.ElementAt(2);

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
