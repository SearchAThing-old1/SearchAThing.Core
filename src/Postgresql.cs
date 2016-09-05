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

using SearchAThing.Core;
using System;
using System.Globalization;
using System.Text;
using static System.Math;

namespace SearchAThing
{

    public static partial class Extensions
    {

        /// <summary>
        /// return the object cast to type T
        /// or specified defaultValue if the obj is a DBNull.Value
        /// </summary>
        public static T FromPsql<T>(this object obj, T defaultValue)
        {
            if (obj == DBNull.Value) return defaultValue;

            return (T)obj;
        }

        /// <summary>
        /// retrieve psql representation of boolean
        /// "TRUE" or "FALSE" string
        /// </summary>        
        public static string ToPsql(this bool value)
        {
            if (value)
                return "TRUE";
            else
                return "FALSE";
        }

        /// <summary>
        /// retrieve psql representation of boolean
        /// "TRUE" or "FALSE" string
        /// or "null" string if given argument is null
        /// </summary>        
        public static string ToPsql(this bool? value)
        {
            if (value.HasValue)
                return value.Value.ToPsql();
            else
                return "null";
        }

        /// <summary>
        /// retrieve psql representation of integer
        /// (number without quotes)        
        /// </summary>        
        public static string ToPsql(this int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// retrieve psql representation of integer
        /// (number without quotes)
        /// of "null" string if given argument is null
        /// </summary>        
        public static string ToPsql(this int? value)
        {
            if (value.HasValue)
                return value.ToString();
            else
                return "null";
        }

        /// <summary>
        /// retrieve psql repsentation of string
        /// 'string' enquoted and escaped
        /// or "null" stringif given argument is null
        /// </summary>        
        public static string ToPsql(this string str)
        {
            if (str == null)
                return "null";
            else
                return $"'{str.Replace("'", "''")}'";
        }

        /// <summary>
        /// retrieve psql representation of datetime
        /// 'YYYY-MM-DD hh:mm:ss.millis'
        /// </summary>        
        public static string ToPsql(this DateTime dt)
        {
            return string.Format("'{0:0000}-{1:00}-{2:00} {3}'",
                dt.Year, dt.Month, dt.Day,
                string.Format(CultureInfo.InvariantCulture, "{0}", dt.TimeOfDay));
        }

        /// <summary>
        /// retrieve psql representation of datetime
        /// 'YYYY-MM-DD hh:mm:ss.millis'
        /// or "null" string if given argument is null        
        /// </summary>                
        public static string ToPsql(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToPsql();
            else
                return "null";
        }

        /// <summary>
        /// retrieve psql representation of double array
        /// '{val1,val2,...,valn}'
        /// list enquoted, using invariant culture
        /// return "null" string if entire array is a null
        /// </summary>        
        public static string ToPsql(this double[] ary)
        {
            if (ary == null)
                return "null";
            else
            {
                var sb = new StringBuilder();

                sb.Append("'{");

                for (int i = 0; i < ary.Length; ++i)
                {
                    if (i > 0) sb.Append(',');

                    sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}", ary[i]));
                }

                sb.Append("}'");

                return sb.ToString();
            }
        }

        /// <summary>
        /// retrieve psql representation of nullable double array
        /// '{val1,val2,null,...,valn}'
        /// list enquoted, using invariant culture and evaluating null to "null" strings
        /// </summary>        
        public static string ToPsql(this double?[] ary)
        {
            if (ary == null)
                return "null";
            else
            {
                var sb = new StringBuilder();

                sb.Append("'{");

                for (int i = 0; i < ary.Length; ++i)
                {
                    if (i > 0) sb.Append(',');

                    if (ary[i] == null)
                        sb.Append("null");
                    else
                        sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}", ary[i].Value));
                }

                sb.Append("}'");

                return sb.ToString();
            }
        }

    }

}
