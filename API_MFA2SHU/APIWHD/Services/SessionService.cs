using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using APIWHD.Models;

namespace APIWHD.Services
{
    public class SessionService
    {
        private readonly IDistributedCache _cache;

        public SessionService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task StoreSessionDataAsync(string sessionId, MFASecretData data)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            await _cache.SetStringAsync(sessionId, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Set expiration time as needed
            });
        }

        public async Task<MFASecretData> RetrieveSessionDataAsync(string sessionId)
        {
            var data = await _cache.GetStringAsync(sessionId);
            return data != null ? JsonConvert.DeserializeObject<MFASecretData>(data) : null;
        }

        public async Task ClearSessionDataAsync(string sessionId)
        {
            await _cache.RemoveAsync(sessionId);
        }
    }
}
