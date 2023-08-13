namespace EUtility.Console.BottomMessage;

public interface IMessageUnit
{
    /// <summary>
    /// Message unit's title
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Message unit's description
    /// </summary>
    string Description { get; set; }
}
