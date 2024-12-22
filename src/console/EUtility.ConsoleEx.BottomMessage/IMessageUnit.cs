namespace EUtility.ConsoleEx.Message;

public interface IMessageUnit
{
    /// <summary>
    /// Message unit's title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Message unit's description
    /// </summary>
    public string Description { get; set; }
}
