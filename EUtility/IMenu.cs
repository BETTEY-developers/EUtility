using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Console.Menu
{
    public interface IMenu
    {
        void UserInputSelect();
        void Select(int index);

        IMenuItemCollection? Item { get; }
        IMenuItem? SelectedItem { get; }
    }
}
