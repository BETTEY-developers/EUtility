using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.UnitTestLib.ObjectProperty
{
    public class OutputObjectProperty<T> where T : class
    {

        public static MemberInfo[] GetMembers(BindingFlags bindingFlags)
            => typeof(T).GetMembers(bindingFlags);

        public static string FormatObjectPropertyValue(T value)
        {
            string FormatTypesName<T>(ICollection<T> values, Func<T,string> outputcontent)
            {
                List<string> s = new(); 
                values.ToList()
                      .ForEach(
                    x => s.Add(outputcontent(x))
                                   ); 
                return string.Join(',', s);
            }
            object GetPropertyValue(PropertyInfo pi)
            {
                var v = typeof(T)
                    .GetProperty(pi.Name)
                    .GetValue(value); 
                return v.ToString();
            }
            Type t = typeof(T);
            StringBuilder sb = new();
            sb.AppendLine(new string('-',180));
            sb.AppendLine($"|{"Name",-18}|{"PropertyType",-30}|{"DeclaringType",-59}|{"CustomAttributes",-60}|{"Value",-8}|");
            foreach(PropertyInfo pi in t.GetProperties())
            {
                sb.AppendLine($"|{pi.Name,-18}|{pi.PropertyType,-30}|{pi.DeclaringType,-59}|{FormatTypesName(pi.CustomAttributes.ToArray(),ca => ca.AttributeType.Name),-60}|{GetPropertyValue(pi),-8}|");
            }
            sb.AppendLine(new string('-', 233));
            sb.AppendLine($"|{"Name",-18}|{"ReturnType",-30}|{"DeclaringType",-59}|{"CustomAttributes",-60}|{"Parameters",-60}|");
            foreach (MethodInfo mi in t.GetMethods())
            {
                sb.AppendLine($"|{mi.Name,-18}|{mi.ReturnType,-30}|{mi.DeclaringType,-59}|{FormatTypesName(mi.CustomAttributes.ToArray(),ca => ca.AttributeType.Name),-60}|{FormatTypesName(mi.GetParameters(),p=>$"{p.ParameterType.Name} {p.Name}"),-60}|");
            }
            sb.AppendLine(new string('-', 233));
            return sb.ToString();
        }
    }
}
