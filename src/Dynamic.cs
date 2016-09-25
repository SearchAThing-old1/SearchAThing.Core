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
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace SearchAThing
{

    public static partial class Dynamic
    {

        public static bool ContainsField(dynamic value, string field)
        {
            return ((IDictionary<string, object>)value).ContainsKey(field);
        }

        /// <summary>
        /// safe retrieve dynamic string type
        /// </summary>        
        public static string GetString(dynamic value, string valueIfNull = "")
        {
            if (value == null) return valueIfNull;

            return (string)value;
        }

        /// <summary>
        /// safe retrieve dynamic bool type
        /// </summary>        
        public static bool GetBool(dynamic value, bool valueIfNull = false)
        {
            if (value == null) return valueIfNull;

            return (bool)value;
        }

        /// <summary>
        /// safe retrieve dynamic double type
        /// </summary>        
        public static double GetDouble(dynamic value, double valueIfNull = 0)
        {
            if (value == null) return valueIfNull;

            return (double)value;
        }

        /// <summary>
        /// safe retrieve dynamic long type
        /// </summary>        
        public static long GetLong(dynamic value, long valueIfNull = 0L)
        {
            if (value == null) return valueIfNull;

            return (long)value;
        }

        /// <summary>
        /// safe retrieve dynamic int type
        /// </summary>        
        public static int GetInt(dynamic value, int valueIfNull = 0)
        {
            if (value == null) return valueIfNull;

            return (int)value;
        }

        /// <summary>
        /// sweep dynamic array enumerating it
        /// </summary>        
        public static IEnumerable<T> Enum<T>(dynamic arr)
        {
            foreach (var x in arr) yield return x;
        }

        /// <summary>
        /// sweep dynamic array enumerating it
        /// </summary>        
        public static List<T> ToList<T>(dynamic arr)
        {
            return ((IEnumerable<T>)Enum<T>(arr)).ToList();
        }

        /// <summary>
        /// sweep dynamic array enumerating it and retrieving an hashset
        /// </summary>        
        public static HashSet<T> ToHashSet<T>(dynamic arg)
        {
            return new HashSet<T>(ToList<T>(arg));
        }

    }

}
