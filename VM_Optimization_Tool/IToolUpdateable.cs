using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VM_Optimization_Tool
{
    interface IToolUpdateable
    {
        string ApplicationName { get; }
        string ApplicationID { get; }
        Assembly ApplicationAssembly { get; }
        Icon ApplicationIcon { get; }
        Uri UpdateXmlLocation { get; }
    }
}
