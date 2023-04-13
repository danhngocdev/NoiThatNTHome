using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Utilities.Databases
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
