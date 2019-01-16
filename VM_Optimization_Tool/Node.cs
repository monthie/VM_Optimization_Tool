using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM_Optimization_Tool
{
    public class Node
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public List<Node> Children { get; } = new List<Node>();
    }
}
