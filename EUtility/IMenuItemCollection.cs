using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Console.Menu
{
    public interface IMenuItemCollection : IList<IMenuItem>
    {
        IList<IMenuItem> Items { get; }
    }
}
