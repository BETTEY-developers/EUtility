namespace EUtility.Console.Message;

public class MessageUnit : IMessageUnit
{
    string _title = string.Empty;
    string _description = string.Empty;

    /// <summary>
    /// Message unit's title
    /// </summary>
    public string Title { get => _title; set => _title = value; }

    /// <summary>
    /// Message unit's description
    /// </summary>
    public string Description { get => _description; set => _description = value; }
}
