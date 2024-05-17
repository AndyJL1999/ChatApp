using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.API.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string userId, string cacheKey);
        void Set(string userId, string cacheKey, object value);
        Task<T> GetOrSetAsync<T>(string userId, string cacheKey, Func<Task<T>> func) where T : class;
    }
}
