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

        public Vector3(Vector3 v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public static Vector3 Read(BinaryReader reader)
        {
            const int floatSize = sizeof(float);
            const int vertexSize = (floatSize * 3);

            if (reader == null) 
                throw new ArgumentNullException(nameof(reader));

            byte[] data = new byte[vertexSize];
            reader.Read(data, 0, data.Length);

            return new Vector3()
            {
                X = BitConverter.ToSingle(data, 0),
                Y = BitConverter.ToSingle(data, floatSize),
                Z = BitConverter.ToSingle(data, (floatSize * 2))
            };

        }
    }
}
