using ReLogic.Content;
using Terraria.ModLoader;

namespace BlockVanity.Common.Utilities;

public class SlowAsset<T> where T : class
{
    public Asset<T> asset;
    private string _path;

    public SlowAsset(string path)
    {
        this._path = path;
        asset = ModContent.Request<T>(_path);
    }

    public T Value
    {
        get
        {
            Request();
            return asset.Value;
        }
    }

    public Asset<T> Request()
    {
        if (!asset.IsLoaded)
        {
            asset = ModContent.Request<T>(_path);
        }

        return asset;
    }
}
