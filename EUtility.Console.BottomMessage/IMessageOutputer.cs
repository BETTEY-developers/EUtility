namespace EUtility.Console.BottomMessage;

public interface IMessageOutputer : ICollection<IMessageUnit>, IEnumerable<IMessageUnit>, IEnumerator<IMessageUnit>
{
    /// <summary>
    /// Write message string with a message string formater.
    /// </summary>
    /// <param name="messageFormater">Will use message string formater.</param>
    void Write(IMessageFormater messageFormater);

    /// <summary>
    /// Write message string.
    /// </summary>
    void Write();
}
