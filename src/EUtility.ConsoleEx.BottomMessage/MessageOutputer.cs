using EUtility.StringEx.StringExtension;
using System.Collections;
using SConsole = System.Console;

namespace EUtility.ConsoleEx.Message;

public class MessageOutputer : IMessageOutputer
{
    /// <summary>
    /// Default message string formater.
    /// </summary>
    public class MessageFormater : IMessageFormatter
    {
        /// <summary>
        /// Format message unit.
        /// </summary>
        /// <param name="messageunits">Need format message unit collection.</param>
        /// <returns>
        /// Formated message string. 
        /// One unit be like: {title} {description}.
        /// A complete string be like: {unit}(4 white space){next unit}.
        /// </returns>
        public string FormatMessage(ICollection<IMessageUnit> messageunits)
        {
            List<string> messages = new List<string>();
            foreach(var messageunit in messageunits)
            {
                messages.Add($"{messageunit.Title} {messageunit.Description}");
            }
            return string.Join("    ", messages.ToArray());
        }
    }

    // ** protected message unit collection **
    protected ICollection<IMessageUnit> _messageUnits;

    // ** private enumer current item **
    IMessageUnit? _current;

    // ** private enumer current item index **
    int _currentindex = -1;

    // ** protected formatter static instance
    protected MessageOutputer.MessageFormater _messageFormater = new();

    /// <summary>
    /// Message units collection count.
    /// </summary>
    public virtual int Count => _messageUnits.Count;

    /// <summary>
    /// Is read-only (this property is clearful, you need read this doc?)
    /// </summary>
    public virtual bool IsReadOnly => _messageUnits.IsReadOnly;

    /// <summary>
    /// Enumer current element.
    /// </summary>
    IMessageUnit IEnumerator<IMessageUnit>.Current => _current;

    /// <summary>
    /// Enumer current element (object type virsion).
    /// </summary>
    object IEnumerator.Current => _current;

    /// <summary>
    /// Init this instance with a <see cref="IMessageUnit"/> object collection.
    /// </summary>
    /// <param name="messageUnits"><see cref="IMessageUnit"/> object collection.</param>
    public MessageOutputer(ICollection<IMessageUnit> messageUnits)
    {
        this._messageUnits = messageUnits;
    }

    /// <summary>
    /// Init this instance.
    /// </summary>
    public MessageOutputer()
    {
        _messageUnits = new List<IMessageUnit>();
    }

    /// <summary>
    /// Add a <see cref="IMessageUnit"/> object to the collection.
    /// </summary>
    /// <param name="item">The item to add to the message unit collection.</param>
    /// <exception cref="NotSupportedException"> The collection is read-only.</exception>
    public virtual void Add(IMessageUnit item) => _messageUnits.Add(item);

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    /// <exception cref="NotSupportedException"> The collection is read-only.</exception>
    public virtual void Clear() => _messageUnits.Clear();

    /// <summary>
    /// Determines whether the collection contains a specific value.
    /// </summary>
    /// <param name="item">The <see cref="IMessageUnit"/> object to locate in the collection</param>
    /// <returns><see cref="true"/> if item is found in the collection; otherwise, <see cref="false"/>.</returns>


    public virtual bool Contains(IMessageUnit item) => _messageUnits.Contains(item);

    /// <summary>
    /// Copies the elements of the collection to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from collection. The <see cref="Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see cref="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
    /// <exception cref="ArgumentException">The number of elements in the source collection is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
    public virtual void CopyTo(IMessageUnit[] array, int arrayIndex) => _messageUnits.CopyTo(array, arrayIndex);

    /// <summary>
    /// Removes the first occurrence of a <see cref="IMessageUnit"/> object from the collection.
    /// </summary>
    /// <param name="item"></param>
    /// <returns><see cref="true"/>if <paramref name="item"/> was successfully removed from the collection; otherwise, <see cref="false"/>. This method also returns <see cref="false"/> if <paramref name="item"/> is not found in the original collection.</returns>
    /// <exception cref="NotSupportedException"> The collection is read-only.</exception>
    public virtual bool Remove(IMessageUnit item) => _messageUnits.Remove(item);


    IEnumerator<IMessageUnit> IEnumerable<IMessageUnit>.GetEnumerator()
    {
        foreach(var v in _messageUnits)
        {
            _currentindex++;
            _current = v;
            yield return _current;
        }
        _currentindex = -1;
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var v in _messageUnits)
        {
            _currentindex++;
            _current = v;
            yield return _current;
        }
        _currentindex = -1;
        yield break;
    }

    bool IEnumerator.MoveNext()
    {
        return !(_currentindex == -1);
    }

    void IEnumerator.Reset()
    {
        _current = null;
        _currentindex = -1;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Use <see cref="MessageOutputer.MessageFormater"/> formater write message at console bottom(<see cref="System.Console.BufferHeight"/> - 1)
    /// </summary>
    public virtual void Write()
    {
        Write(_messageFormater);
    }

    /// <summary>
    /// Use custom formater write message at console's bottom(<see cref="System.Console.BufferHeight"/> - 1)
    /// </summary>
    /// <param name="messageFormater">Custom message formater</param>
    public virtual void Write(IMessageFormatter messageFormater)
    {
        (int currentCurPosX, int currentCurPosY) = Console.GetCursorPosition();
        SConsole.SetCursorPosition(0, Console.BufferHeight - 1);
        string ms = messageFormater.FormatMessage(_messageUnits);
        SConsole.Write(ms + new string(' ',Console.BufferWidth - ms.GetStringInConsoleGridWidth()));
        SConsole.SetCursorPosition(currentCurPosX, currentCurPosY);
    }
}