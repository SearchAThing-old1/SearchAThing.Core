#region SearchAThing.Core, Copyright(C) 2015-2016 Lorenzo Delana, License under MIT
/*
* The MIT License(MIT)
* Copyright(c) 2016 Lorenzo Delana, https://searchathing.com
*
* Permission is hereby granted, free of charge, to any person obtaining a
* copy of this software and associated documentation files (the "Software"),
* to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sublicense,
* and/or sell copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;

namespace SearchAThing
{

    public static partial class Extensions
    {

        /// <summary>
        /// Returns the given string stripped from the given part if exists at beginning.
        /// </summary>        
        public static string StripBegin(this string str, char c)
        {
            if (str.Length > 0 && str[0] == c)
                return str.Substring(1, str.Length - 1);
            else
                return str;
        }

        /// <summary>
        /// Returns the given string stripped from the given part if exists at beginning.
        /// </summary>        
        public static string StripBegin(this string str, string partToStrip)
        {
            if (str.StartsWith(partToStrip))
            {
                var ptsl = partToStrip.Length;
                return str.Substring(ptsl, str.Length - ptsl);
            }
            else
                return str;
        }

        /// <summary>
        /// Returns the given string stripped from the given part if exists at end.
        /// </summary>        
        public static string StripEnd(this string str, char c)
        {
            if (str.Length > 0 && str[str.Length - 1] == c)
                return str.Substring(0, str.Length - 1);
            else
                return str;
        }

        /// <summary>
        /// Returns the given string stripped from the given part if exists at end.
        /// </summary>        
        public static string StripEnd(this string str, string partToStrip)
        {
            if (str.EndsWith(partToStrip))
                return str.Substring(0, str.Length - partToStrip.Length);
            else
                return str;
        }

        /// <summary>
        /// Smart line splitter that split a text into lines whatever unix or windows line ending style.
        /// By default its remove empty lines.
        /// </summary>        
        /// <param name="removeEmptyLines">If true remove empty lines.</param>        
        public static IEnumerable<string> Lines(this string txt, bool removeEmptyLines = true)
        {
            var q = txt.Replace("\r\n", "\n").Split('\n');

            if (removeEmptyLines)
                return q.Where(r => r.Trim().Length > 0);
            else
                return q;
        }

        /// <summary>
        /// Returns a human readable bytes length. (eg. 1000, 1K, 1M, 1G, 1T)
        /// </summary>        
        public static string HumanReadable(this long bytes, bool omitByteSuffix = true)
        {
            var k = 1024L;
            var m = k * 1024;
            var g = m * 1024;
            var t = g * 1024;

            if (bytes < k) { if (omitByteSuffix) return $"{bytes}"; else return $"{bytes}b"; }
            else if (bytes >= k && bytes < m) return $"{Round(((double)bytes) / k, 1)}K";
            else if (bytes >= m && bytes < g) return $"{Round(((double)bytes) / m, 1)}M";
            else if (bytes >= g && bytes < t) return $"{Round(((double)bytes) / g, 1)}G";
            else return $"{((double)bytes) / t}T";
        }

        /// <summary>
        /// Repeat given string for cnt by concatenate itself
        /// </summary>        
        public static string Repeat(this string s, int cnt)
        {
            var sb = new StringBuilder();

            while (cnt > 0)
            {
                sb.Append(s);
                --cnt;
            }

            return sb.ToString();
        }

    }

}
