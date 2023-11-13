using Backend3DForge.Services.ModelCalculator.Models;

namespace Backend3DForge.Services.ModelCalculator
{
    public class ObjTool
    {
        private List<Vector3> vectors = new List<Vector3>();
        private Vector3 maxVector = new Vector3();
        private Vector3 minVector = new Vector3();

        public ModelCalculatorResult CalculateFrom(Stream stream)
        {
            double surfaceArea = 0.0;
            double volume = 0.0;

            try
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    string? line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');
 
                        parts = parts.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        if (parts.Length == 0 || parts[0].StartsWith("#"))
                        {
                            continue;
                        }

                        switch (parts[0])
                        {
                            case "v":
                                float x = float.Parse(parts[1]);
                                float y = float.Parse(parts[2]);
                                float z = float.Parse(parts[3]);
                                vectors.Add(new Vector3(x, y, z));
                                if (x > maxVector.X)
                                {
                                    maxVector.X = x;
                                }
                                if (y > maxVector.Y)
                                {
                                    maxVector.Y = y;
                                }
                                if (z > maxVector.Z)
                                {
                                    maxVector.Z = z;
                                }
                                if (x < minVector.X)
                                {
                                    minVector.X = x;
                                }
                                if (y < minVector.Y)
                                {
                                    minVector.Y = y;
                                }
                                if (z < minVector.Z)
                                {
                                    minVector.Z = z;
                                }
                                break;

                            case "f":
                                Vector3[] vertices = new Vector3[parts.Length - 1];
                                for (int i = 1; i < parts.Length; i++)
                                {
                                    string[] indices = parts[i].Split('/');

                                    if (indices.Length >= 1 && int.TryParse(indices[0], out int vertexIndex))
                                    {
                                        vertices[i - 1] = vectors[vertexIndex - 1];
                                    }
                                }

                                surfaceArea += vertices.CalculateArea();
                                volume += vertices.SignedVolumeOfTriangle();
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading .obj file: " + e.Message);
            }

            return new ModelCalculatorResult
            {
                X = MathF.Round(maxVector.X - minVector.X),
                Y = MathF.Round(maxVector.Y - minVector.Y),
                Z = MathF.Round(maxVector.Z - minVector.Z),
                Volume = (float)Math.Round(volume * 2, 2),
                SurfaceArea = (float)Math.Round(surfaceArea, 2)
            };
        }
    }
}
