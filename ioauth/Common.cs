using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.AppDomain;

namespace ioauth
{
    internal static class Common
    {
        private static io.Systems.IOSystem _system;

        internal enum DataConnection
        {
            io_auth = 100,
            io_contacts = 101,
            io_tasks = 102,
            io_systemlog = 103
        }

        internal static io.Systems.IOSystem IOSystem
        {
            get
            {
                if (_system == null)
                    _system = new io.Systems.IOSystem();

                return _system;
            }
        }
    }
}