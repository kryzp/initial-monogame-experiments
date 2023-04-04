
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using TiledSharp;

namespace TmxContent
{
    [ContentImporter(".tmx", DefaultProcessor = "TmxProcessor", DisplayName = "Map Importer - TmxContent")]
    public class TmxImporter : ContentImporter<TmxMap>
    {
        public override TmxMap Import(string filename, ContentImporterContext context)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);
            return new TmxMap(stream);
        }
    }
}
