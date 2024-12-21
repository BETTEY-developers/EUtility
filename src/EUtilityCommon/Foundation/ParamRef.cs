using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.Foundation;

public static class ParamRef
{
    public static bool ReturnFail<TParam>(out TParam obj)
    {
        obj = default;
        return false;
    }
}
