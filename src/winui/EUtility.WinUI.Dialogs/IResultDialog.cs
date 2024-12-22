using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.WinUI.Dialogs
{
    public interface IResultDialog<out T>
    {
        T Result { get; }

        bool Complated { get;}
    }
}
