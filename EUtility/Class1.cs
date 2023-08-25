using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace EUtility.ConsoleEx.Menu
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

    public class Menu : IMenu
    {
        private Dictionary<string,KeyValuePair<IMenuItem,Action<IMenu,string>>>? _items;
        private IMenuItem? _selecteditem;
        private int _selectedindex = 0;
        private KeyValuePair<string, KeyValuePair<IMenuItem, Action<IMenu, string>>> _current;

        public KeyValuePair<IMenuItem, Action<IMenu, string>> this[string key] { get => _items[key]; set => throw new NotImplementedException(); }

        public IMenuItem? SelectedItem => _selecteditem;

        public ICollection<string> Keys => _items.Keys;

        public ICollection<KeyValuePair<IMenuItem, Action<IMenu, string>>> Values => _items.Values;

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public KeyValuePair<string,KeyValuePair<IMenuItem, Action<IMenu, string>>> Current => _current;

        object IEnumerator.Current => _current;

        public void Add(string key, KeyValuePair<IMenuItem, Action<IMenu, string>> value)
        {
            _items.Add(key, value);
        }

        public void Add(KeyValuePair<string, KeyValuePair<IMenuItem, Action<IMenu, string>>> item)
        {
            _items.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(KeyValuePair<string, KeyValuePair<IMenuItem, Action<IMenu, string>>> item)
        {
            _items.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            _items.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, KeyValuePair<IMenuItem, Action<IMenu, string>>>[] array, int arrayIndex)
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, KeyValuePair<IMenuItem, Action<IMenu, string>>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, KeyValuePair<IMenuItem, Action<IMenu, string>>> item)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Select(int index)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out KeyValuePair<IMenuItem, Action<IMenu, string>> value)
        {
            throw new NotImplementedException();
        }

        public void UserInputSelect()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}