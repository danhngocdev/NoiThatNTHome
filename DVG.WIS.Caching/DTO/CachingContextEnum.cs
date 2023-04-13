using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.Caching.Cached
{
    public class CachingContextEnum
    {
        public enum DBContextTypes
        {
            /// <summary>
            /// SQL = 0
            /// </summary>
            SQL = 0,

            /// <summary>
            /// PostgreSQL = 1
            /// </summary>
            PostgreSQL = 1,

            /// <summary>
            /// MySQL = 2
            /// </summary>
            MySQL = 2,

            /// <summary>
            /// Oracle = 3
            /// </summary>
            Oracle = 3
        }
    }
}
