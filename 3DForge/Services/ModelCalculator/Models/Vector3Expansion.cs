using QuantumConcepts.Formats.StereoLithography;

namespace Backend3DForge.Services.ModelCalculator.Models
{
    public static class Vector3Expansion
    {
        public static IEnumerable<Vector3> ToVector3(this ICollection<Vertex> vertices)
        {
            return vertices.Select(p => new Vector3(p.X, p.Y, p.Z));
        }
    }
}
