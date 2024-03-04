using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBP_backend.Cache
{
    public interface ICacheProvider
    {
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options) where T : class;

        Task<T> GetAsync<T>(string key) where T : class;

        Task DeleteAsync(string key);

        void SetInHashSet(string hashSetKey, string key, string value);

        T GetFromHashSet<T>(string hashSetKey, string key);

        List<T> GetAllFromHashSet<T>(string hashSetKey) where T : class;

        //void SetInHashSet(string v, int commentId, object jsonSerialize);
    }
}
