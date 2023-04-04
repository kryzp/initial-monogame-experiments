using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TiledSharp;

namespace TmxContent
{
    public class TmxReader : ContentTypeReader<TmxMap>
    {
        protected override TmxMap Read(ContentReader reader, TmxMap existingInstance)
        {
            int length = reader.ReadInt32();
            byte[] data = reader.ReadBytes(length);

            MemoryStream stream = new MemoryStream(data);
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object map = formatter.Deserialize(stream);
            return (TmxMap)map;
        }
    }
}
