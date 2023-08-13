using System.Collections;
using System.Text;
using SConsole = System.Console;

namespace EUtility.Console.BottomMessage;

public class MessageOutputer : IMessageOutputer
{
    /// <summary>
    /// Default message string formater.
    /// </summary>
    public class MessageFormater : IMessageFormater
    {
        /// <summary>
        /// Format message unit.
        /// </summary>
        /// <param name="messageunits">Need format message unit collection.</param>
        /// <returns>
        /// Formated message string. 
        /// One unit be like: <code>unit: $"{title} {description}"</code>. 
        /// A complete string be like:<code>$"{unit}(4 white space){next unit}"</code>
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

    // ** private message unit collection **
    ICollection<IMessageUnit> messageUnits;

    // ** private enumer current item **
    IMessageUnit? _current;

    // ** private enumer current item index **
    int _currentindex = -1;

    /// <summary>
    /// Message units collection count.
    /// </summary>
    int ICollection<IMessageUnit>.Count => messageUnits.Count;

    /// <summary>
    /// Is read-only (this property is clearful, you need read this doc?)
    /// </summary>
    bool ICollection<IMessageUnit>.IsReadOnly => messageUnits.IsReadOnly;

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
        this.messageUnits = messageUnits;
    }

    /// <summary>
    /// Init this instance.
    /// </summary>
    public MessageOutputer()
    {
        messageUnits = new List<IMessageUnit>();
    }

    /// <summary>
    /// Add a <see cref="IMessageUnit"/> object to the collection.
    /// </summary>
    /// <param name="item">The item to add to the message unit collection.</param>
    /// <exception cref="NotSupportedException"> The collection is read-only.</exception>
    void ICollection<IMessageUnit>.Add(IMessageUnit item) => messageUnits.Add(item);

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    /// <exception cref="NotSupportedException"> The collection is read-only.</exception>
    void ICollection<IMessageUnit>.Clear() => messageUnits.Clear();

    /// <summary>
    /// Determines whether the collection contains a specific value.
    /// </summary>
    /// <param name="item">The <see cref="IMessageUnit"/> object to locate in the collection</param>
    /// <returns><see cref="true"/> if item is found in the collection; otherwise, <see cref="false"/>.</returns>
    bool ICollection<IMessageUnit>.Contains(IMessageUnit item) => messageUnits.Contains(item);

    /// <summary>
    /// Copies the elements of the collection to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from collection. The <see cref="Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see cref="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
    /// <exception cref="ArgumentException">The number of elements in the source collection is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
    void ICollection<IMessageUnit>.CopyTo(IMessageUnit[] array, int arrayIndex) => messageUnits.CopyTo(array, arrayIndex);

    /// <summary>
    /// Removes the first occurrence of a <see cref="IMessageUnit"/> object from the collection.
    /// </summary>
    /// <param name="item"></param>
    /// <returns><see cref="true"/>if <paramref name="item"/> was successfully removed from the collection; otherwise, <see cref="false"/>. This method also returns <see cref="false"/> if <paramref name="item"/> is not found in the original collection.</returns>
    /// <exception cref="NotSupportedException"> The collection is read-only.</exception>
    bool ICollection<IMessageUnit>.Remove(IMessageUnit item) => messageUnits.Remove(item);

    IEnumerator<IMessageUnit> IEnumerable<IMessageUnit>.GetEnumerator()
    {
        foreach(var v in messageUnits)
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
        foreach (var v in messageUnits)
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

    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
    }

    void IMessageOutputer.Write()
    {
        (int currentCurPosX, int currentCurPosY) = SConsole.GetCursorPosition();
        SConsole.SetCursorPosition(0, SConsole.BufferHeight - 1);
        SConsole.Write(new MessageFormater().FormatMessage(messageUnits));
        SConsole.SetCursorPosition(currentCurPosX, currentCurPosY);
    }


    public void Write(IMessageFormater messageFormater)
    {
        (int currentCurPosX, int currentCurPosY) = SConsole.GetCursorPosition();
        SConsole.SetCursorPosition(0, SConsole.BufferHeight - 1);
        SConsole.Write(messageFormater.FormatMessage(messageUnits));
        SConsole.SetCursorPosition(currentCurPosX, currentCurPosY);
    }
}