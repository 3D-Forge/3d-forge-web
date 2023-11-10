namespace Backend3DForge.Services.ModelCalculator.Models
{
    public class Face
    {
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        public List<Vector3> Normals { get; } = new List<Vector3>();
    }
}
