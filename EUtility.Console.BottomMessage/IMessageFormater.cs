using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Console.BottomMessage
{
    public interface IMessageFormater
    {
        /// <summary>
        /// Custom format message units.
        /// </summary>
        /// <param name="messageunits">Need format message unit collection.</param>
        /// <returns>Formated message string.</returns>
        string FormatMessage(ICollection<IMessageUnit> messageunits);
    }
}
