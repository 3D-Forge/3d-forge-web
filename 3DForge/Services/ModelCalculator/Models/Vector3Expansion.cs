using QuantumConcepts.Formats.StereoLithography;

namespace Backend3DForge.Services.ModelCalculator.Models
{
    public static class Vector3Expansion
    {
        public static IList<Vector3> ToVector3(this ICollection<Vertex> vertices)
        {
            return vertices.Select(p => new Vector3(p.X, p.Y, p.Z)).ToList();
        }

        public static double SignedVolumeOfTriangle(this ICollection<Vector3> vertices)
        {
            Vector3 p1 = vertices.ElementAt(0);
            Vector3 p2 = vertices.ElementAt(1);
            Vector3 p3 = vertices.ElementAt(2);

            double v321 = p3.X * p2.Y * p1.Z;
            double v231 = p2.X * p3.Y * p1.Z;
            double v312 = p3.X * p1.Y * p2.Z;
            double v132 = p1.X * p3.Y * p2.Z;
            double v213 = p2.X * p1.Y * p3.Z;
            double v123 = p1.X * p2.Y * p3.Z;

            return (1.0 / 6.0) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

        public static double CalculateArea(this ICollection<Vector3> vertices)
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

        private static double CalculateTriangleArea(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
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
