using QuantumConcepts.Formats.StereoLithography;

namespace Backend3DForge.Services.ModelCalculator.Models
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator Vertex(Vector3 v)
        {
            return new Vertex(v.X, v.Y, v.Z);
        }

        public static implicit operator Vector3(Vertex v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X,
                               v1.Y + v2.Y,
                               v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X,
                               v1.Y - v2.Y,
                               v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v1, float scalar)
        {
            return new Vector3(v1.X * scalar,
                               v1.Y * scalar,
                               v1.Z * scalar);
        }

        public static Vector3 operator /(Vector3 v1, float scalar)
        {
            return new Vector3(v1.X / scalar,
                               v1.Y / scalar,
                               v1.Z / scalar);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 Normalize()
        {
            return this / Length();
        }
    }
}
