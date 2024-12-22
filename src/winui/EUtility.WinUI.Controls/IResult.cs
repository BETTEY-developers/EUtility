using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.WinUI.Controls
{
    public interface IResult<out T>
    {
        T Result { get; }
        bool IsSuccess { get; }
    }
}
