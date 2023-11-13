namespace Backend3DForge.Services.ModelCalculator.Models
{
    public class Face : IDisposable
    {
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<Vector3> Normals { get; set; } = new List<Vector3>();

        public void Dispose()
        {
            Vertices.Clear();
            Normals.Clear();

            Vertices = null;
            Normals = null;
        }
    }
}
