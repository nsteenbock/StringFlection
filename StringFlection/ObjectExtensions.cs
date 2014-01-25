using System;
using System.Collections.Generic;
using System.Linq;
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
            
            Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(format);

            int startIndex = 0;
            foreach (Match m in mc)
            {
                Group group = m.Groups[2];
                
                // Append everything before the "{"
                int length = group.Index - startIndex - 1;
                sb.Append(format.Substring(startIndex, length));

                string propertyChain = String.Empty;
                string formatInfo = String.Empty;
                int formatIndex = group.Value.IndexOf(":");
                if (formatIndex == -1) 
                {
                    propertyChain = group.Value;
                }
                else 
                {
                    propertyChain = group.Value.Substring(0, formatIndex);
                    formatInfo = group.Value.Substring(formatIndex + 1);
                }

                object propertyChainValue = ObjectStringFlector.GetValue(obj, propertyChain);
                
                // TODO: Use formatInfo if available.
                // TODO: What happens if an exception is thrown.
                
                sb.Append(propertyChainValue.ToString());
                                
                startIndex = group.Index + group.Length + 1;
            }

            // Include the rest of the string.
            if (startIndex < format.Length)
                sb.Append(format.Substring(startIndex));            

            return sb.ToString();
        }
    }
}
