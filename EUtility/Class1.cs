using System.Collections;

namespace EUtility.Console.Menu
{
    public class MenuItem : IMenuItem
    {
        string name;
        string description;
        ConsoleKey? acceleratorkey;

        string IMenuItem.Description { set => description = value; get => description; }
        string IMenuItem.Name { get => name; set => name = value; }
        ConsoleKey? IMenuItem.AcceleratorKey { get => acceleratorkey; set => acceleratorkey = value; }

        public MenuItem()
        {
            name = string.Empty;
            description = string.Empty;
            acceleratorkey = null;
        }

        public MenuItem(string name, string description, ConsoleKey? acceleratorkey) 
        {
            this.name = name;
            this.description = description;
            this.acceleratorkey = acceleratorkey;
        }
    }

    public class MenuItemCollection : IMenuItemCollection
    {
        List<IMenuItem>? item;

        IMenuItem IList<IMenuItem>.this[int index] { get => item[index]; set => item[index] = value; }

        int ICollection<IMenuItem>.Count => item.Count;

        bool ICollection<IMenuItem>.IsReadOnly => false;

        void ICollection<IMenuItem>.Add(IMenuItem item) => this.item.Add(item);

        void ICollection<IMenuItem>.Clear() => item.Clear();

        bool ICollection<IMenuItem>.Contains(IMenuItem item) => this.item.Contains(item);

        void ICollection<IMenuItem>.CopyTo(IMenuItem[] array, int arrayIndex) => this.item.CopyTo(array, arrayIndex);

        IEnumerator<IMenuItem> IEnumerable<IMenuItem>.GetEnumerator() => this.item.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.item.GetEnumerator();

        int IList<IMenuItem>.IndexOf(IMenuItem item) => this.item.IndexOf(item);

        void IList<IMenuItem>.Insert(int index, IMenuItem item) => this.item.Insert(index, item);

        bool ICollection<IMenuItem>.Remove(IMenuItem item) => this.item.Remove(item);

        void IList<IMenuItem>.RemoveAt(int index) => this.item.RemoveAt(index);
    }

    public class Menu : IMenu
    {
        private IMenuItemCollection? items;
        private IMenuItem 

        IMenuItemCollection? IMenu.Item => throw new NotImplementedException();

        IMenuItem? IMenu.SelectedItem => throw new NotImplementedException();

        void IMenu.Select(int index)
        {
            throw new NotImplementedException();
        }

        void IMenu.UserInputSelect()
        {
            throw new NotImplementedException();
        }
    }
}