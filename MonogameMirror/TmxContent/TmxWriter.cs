using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TiledSharp;

namespace TmxContent
{
    [ContentTypeWriter]
    public class TmxWriter : ContentTypeWriter<TmxMap>
    {
        protected override void Write(ContentWriter writer, TmxMap value)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, value);

            byte[] data = stream.ToArray();
            writer.Write(data.Length);
            writer.Write(data);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "TmxContent.TmxReader, TmxContent";
        }
    }
}
