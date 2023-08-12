using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Console.Menu
{
    public interface IMenuItem
    {
        string Name { get; set; }
        string Description { get; set; }
        ConsoleKey? AcceleratorKey { get; set; }
    }
}
