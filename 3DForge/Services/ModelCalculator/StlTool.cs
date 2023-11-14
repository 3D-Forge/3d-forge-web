using Backend3DForge.Services.ModelCalculator.Models;
using System.Text;

namespace Backend3DForge.Services.ModelCalculator
{
    public class StlTool
    {
        private Vector3 maxVector = new Vector3();
        private Vector3 minVector = new Vector3();

        public ModelCalculatorResult CalculateFrom(Stream stream)
        {
            double surfaceArea = 0.0;
            double volume = 0.0;

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                // skip header
                reader.ReadBytes(80);
                reader.ReadBytes(4);
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    Vector3 normal = Vector3.Read(reader);
                    Vector3[] vectors = new Vector3[3];

                    for (int i = 0; i < 3; i++)
                    {
                        vectors[i] = Vector3.Read(reader);

                        if (maxVector.X < vectors[i].X)
                            maxVector.X = vectors[i].X;
                        if (maxVector.Y < vectors[i].Y)
                            maxVector.Y = vectors[i].Y;
                        if (maxVector.Z < vectors[i].Z)
                            maxVector.Z = vectors[i].Z;

                        if (minVector.X > vectors[i].X)
                            minVector.X = vectors[i].X;
                        if (minVector.Y > vectors[i].Y)
                            minVector.Y = vectors[i].Y;
                        if (minVector.Z > vectors[i].Z)
                            minVector.Z = vectors[i].Z;
                    }

                    surfaceArea += vectors.CalculateArea();
                    volume += vectors.SignedVolumeOfTriangle();

                    reader.ReadUInt16();
                }
            }

            return new ModelCalculatorResult
            {
                X = MathF.Round(maxVector.X - minVector.X, 2),
                Y = MathF.Round(maxVector.Y - minVector.Y, 2),
                Z = MathF.Round(maxVector.Z - minVector.Z, 2),
                Volume = (float)Math.Round(volume, 2),
                SurfaceArea = (float)Math.Round(surfaceArea, 2)
            };
        }
    }
}
