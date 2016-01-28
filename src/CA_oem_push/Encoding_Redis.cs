using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CA_oem_push
{
    public class Encoding_Redis
    {
        public static IEnumerable<string> encoding(IList<object> parts)
        {
            foreach (var o in parts)
            {
                if (o.GetType() == typeof(int))
                {
                    yield return string.Format(":{0}\r\n", o);
                }
                else if (o.GetType() == typeof(float) || o.GetType() == typeof(double) || o.GetType() == typeof(string))
                {
                    string _o = o.ToString();
                    yield return string.Format("${0}\r\n{1}\r\n", _o.Length, _o);
                }
                else if (o == null)
                {
                    yield return "$-1\r\n";
                }
            }
        }

        public static string wrap(IList<object> parts)
        {
            if (parts == null || parts.Count == 0)
            {
                return "*-1\r\n";
            }
            return string.Format("*{0}\r\n", parts.Count) + string.Join("", encoding(parts).ToArray());
        }
    }
}
