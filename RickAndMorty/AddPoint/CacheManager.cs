using System.Runtime.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Extensions.Caching.Memory;
using MemoryCache = Microsoft.Extensions.Caching.Memory.MemoryCache;

namespace RickAndMorty.AddPoint;

public static class CacheManager
{
    private static MemoryCache cache = MemoryCache.Default;
    private static string cacheFilePath = "cache.dat";

    public static void Add(string key, object value)
    {
        CacheItemPolicy policy = new CacheItemPolicy();
        policy.AbsoluteExpiration = DateTime.Now.AddMinutes(5);
        cache.Set(key, value, policy);
    }

    public static object Get(string key)
    {
        return cache.Get(key);
    }

    public static void Save()
    {
        FileStream fs = null;

        try
        {
            fs = new FileStream(cacheFilePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, cache);
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
        }
    }

    public static void Load()
    {
        if (File.Exists(cacheFilePath))
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(cacheFilePath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                cache = (MemoryCache)bf.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}