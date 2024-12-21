using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.RegisterEx;

public static class RegisterKeyExtension
{
    public static object? GetDefaultValue(this RegistryKey key) => key.GetValue("");
}
