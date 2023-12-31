﻿using System.Collections;

namespace EUtility.ConsoleEx.Message;

public interface IMessageOutputer : ICollection<IMessageUnit>, IEnumerable<IMessageUnit>, IEnumerator<IMessageUnit>, IEnumerable, IEnumerator
{
    /// <summary>
    /// Write message string with a message string formater.
    /// </summary>
    /// <param name="messageFormater">Will use message string formater.</param>
    public void Write(IMessageFormatter messageFormater);

    /// <summary>
    /// Write message string.
    /// </summary>
    public void Write();
}
