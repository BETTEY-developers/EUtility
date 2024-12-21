using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.ValueEx;

public class VariableSwitch<T>
{
    public Dictionary<Union<T, Void>, Func<T, object>> Cases { get; private set; }

    public static VariableSwitch<T> Create()
    {
        return new VariableSwitch<T>();
    }

    public VariableSwitch<T> CreateCase(T triggerValue, Func<T, object> action)
    {
        Cases ??= new();

        Cases.Add(triggerValue, action);
        return this;
    }

    public VariableSwitch<T> SetDefault(Func<T, object> action)
    {
        Cases ??= new();

        Cases.Add(Void.MakeVoid(), action);

        return this;
    }

    public object Switch(T Value)
    {
        if(Cases.TryGetValue(Value, out var result))
        {
            return result(Value);
        }
        else
        {
            return Cases[Void.MakeVoid()](Value);
        }
    }
}
