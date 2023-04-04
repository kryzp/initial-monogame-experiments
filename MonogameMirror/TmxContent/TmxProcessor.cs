using Microsoft.Xna.Framework.Content.Pipeline;
using TiledSharp;

namespace TmxContent
{
    [ContentProcessor(DisplayName = "Map Processor - TmxContent")]
    public class TmxProcessor : ContentProcessor<TmxMap, TmxMap>
    {
        public override TmxMap Process(TmxMap input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
