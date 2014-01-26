using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Confluxx.StringFlection
{
    public static class ObjectExtensions
    {
        public static string ToPropertyString(this object obj, string format)
        {
            return ToPropertyString(obj, format, null);
        }

        public static string ToPropertyString(this object obj, string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            
            Regex reg = new Regex(@"{([^}]+)}", RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(format);

            int startIndex = 0;
            foreach (Match match in matches)
            {
                Group group = match.Groups[1];
                
                // Append everything before the "{"
                int length = group.Index - startIndex - 1;
                sb.Append(format.Substring(startIndex, length));

                string propertyChain = null;
                string formatString = null;
                int formatIndex = group.Value.IndexOf(":");
                
                if (formatIndex == -1) 
                {
                    propertyChain = group.Value;
                }
                else 
                {
                    propertyChain = group.Value.Substring(0, formatIndex);
                    formatString = group.Value.Substring(formatIndex + 1);
                }

                object value = ObjectStringFlector.GetValue(obj, propertyChain);

                string result = null;
                object[] invokeArgs = null;


                if (formatString != null)
                    invokeArgs = new object[] { formatString, formatProvider };

                result = (string)value.GetType().InvokeMember(
                        "ToString",
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                        null,
                        value,
                        invokeArgs);
                
                sb.Append(result);
                                
                startIndex = group.Index + group.Length + 1;
            }

            // Include the rest of the string.
            if (startIndex < format.Length)
                sb.Append(format.Substring(startIndex));            

            return sb.ToString();
        }
    }
}
