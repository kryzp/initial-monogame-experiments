using TiledSharp;

namespace MonogameMirror
{
    public static class Utility
    {
        public static TmxTilesetTile getTilesetTileFromGid(TmxTileset ts, int gid)
        {
            int xid = gid-ts.FirstGid;
            if (ts.Tiles.ContainsKey(xid))
                return ts.Tiles[xid];
            else return null;
        }
    }
}
