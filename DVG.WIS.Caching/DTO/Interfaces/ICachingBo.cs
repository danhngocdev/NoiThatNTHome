using DVG.WIS.Caching.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Caching.DTO.Interfaces
{
    public interface ICachingBo
    {
        /// <summary>
        /// Add a new key or edit a exists key.
        /// </summary>
        /// <remarks>
        /// Exceptions: Invalid key
        /// </remarks>
        /// <exception cref="Invalid key" >Update Error</exception>
        /// <param name="model"></param>
        void Update(KeyCacheModel model);

        /// <summary>
        /// Delete a exists key.
        /// </summary>
        /// <remarks>
        /// Exceptions: Invalid key
        /// </remarks>
        /// <exception cref="Invalid key"></exception>
        /// <param name="model"></param>
        void Delete(string key);

        KeyCacheModel GetByKey(string key);

        IEnumerable<KeyCacheModel> GetListByNameSpace(string ns);

        IEnumerable<KeyCacheModel> GetListByDate(DateTime fromDate, DateTime untilDate);
    }
}
