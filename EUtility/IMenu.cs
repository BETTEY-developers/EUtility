using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.ConsoleEx.Menu
{
    public interface IMenu : IDictionary<string,KeyValuePair<IMenuItem,Action<IMenu,string>>>, IEnumerator<KeyValuePair<string, KeyValuePair<IMenuItem, Action<IMenu, string>>>>
    {
        void UserInputSelect();
        void Select(int index);
        IMenuItem? SelectedItem { get; }
    }
}
