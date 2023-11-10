using Backend3DForge.Services.ModelCalculator.Models;

namespace Backend3DForge.Services.ModelCalculator
{
    public class ObjReader
    {
        public static async Task<ObjModel> ReadObjFile(Stream stream)
        {
            ObjModel objModel = new ObjModel();

            try
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    string? line;
                    while ((line = await streamReader.ReadLineAsync()) != null)
                    {
                        string[] parts = line.Split(' ');
                        // remove empty entries
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
                                objModel.Vertices.Add(new Vector3(x, y, z));
                            break;

                            case "vn":
                                float nx = float.Parse(parts[1]);
                                float ny = float.Parse(parts[2]);
                                float nz = float.Parse(parts[3]);
                                objModel.Normals.Add(new Vector3(nx, ny, nz));
                            break;

                            case "f":
                                Face face = new Face();

                                for (int i = 1; i < parts.Length; i++)
                                {
                                    string[] indices = parts[i].Split('/');

                                    if (indices.Length >= 1 && int.TryParse(indices[0], out int vertexIndex))
                                    {
                                        face.Vertices.Add(objModel.Vertices[vertexIndex - 1]);
                                    }

                                    if (indices.Length >= 3 && int.TryParse(indices[2], out int normalIndex))
                                    {
                                        face.Normals.Add(objModel.Normals[normalIndex - 1]);
                                    }
                                }

                                objModel.Faces.Add(face);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading .obj file: " + e.Message);
            }

            return objModel;
        }
    }
}
