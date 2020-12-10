using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo3.Utility
{
    public class MemoryCacheTicketStore : ITicketStore
    {
        private readonly IMemoryCache memoryCache;
        private const string KeyPrefix = "IService-";

        public MemoryCacheTicketStore(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task RemoveAsync(string key)
        {
            memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new MemoryCacheEntryOptions();
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
            {
                options.SetAbsoluteExpiration(expiresUtc.Value);
            }
            options.SetSlidingExpiration(TimeSpan.FromHours(1));
            memoryCache.Set(key, ticket, options);
            return Task.CompletedTask;
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            memoryCache.TryGetValue(key, out AuthenticationTicket ticket);
            return Task.FromResult(ticket);
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = KeyPrefix + Guid.NewGuid().ToString("N");
            await RenewAsync(key, ticket);
            return key;
        }
    }
}
