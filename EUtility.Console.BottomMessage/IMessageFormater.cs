namespace EUtility.Console.Message;

public interface IMessageFormater
{
    /// <summary>
    /// Custom format message units.
    /// </summary>
    /// <param name="messageunits">Need format message unit collection.</param>
    /// <returns>Formated message string.</returns>
    public string FormatMessage(ICollection<IMessageUnit> messageunits);
}
