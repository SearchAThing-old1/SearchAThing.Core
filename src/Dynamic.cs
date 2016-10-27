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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SearchAThing
{

    public static partial class Dynamic
    {

        public static bool ContainsField(dynamic value, string field)
        {
            if (value is JObject)
            {
                var jo = (JObject)value;

                var q = jo.Children().Cast<JProperty>().ToList();

                return q.Any(w => w != null && w.Name == field);
            }

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

        static Type JValueType = typeof(Newtonsoft.Json.Linq.JValue);

        /// <summary>
        /// sweep dynamic array enumerating it
        /// </summary>        
        public static IEnumerable<T> Enum<T>(dynamic arr)
        {
            foreach (var x in arr)
            {
                if (x.GetType() == JValueType)
                    yield return x.Value;
                else
                    yield return x;
            }
        }

        /// <summary>
        /// sweep dynamic array enumerating it
        /// </summary>        
        public static List<T> ToList<T>(dynamic arr)
        {
            return ((IEnumerable<T>)Enum<T>(arr)).ToList();
        }

        /// <summary>
        /// sweep dynamic array
        /// </summary>        
        public static T[] ToArray<T>(dynamic arr)
        {
            return ((IEnumerable<T>)Enum<T>(arr)).ToArray();
        }

        /// <summary>
        /// sweep dynamic array enumerating it and retrieving an hashset
        /// </summary>        
        public static HashSet<T> ToHashSet<T>(dynamic arg)
        {
            return new HashSet<T>(ToList<T>(arg));
        }

        /// <summary>
        /// retrieve an expando object, ready for members add
        /// </summary>        
        public static IDictionary<string, object> NewExpandoObject()
        {
            return new ExpandoObject();
        }

        /// <summary>
        /// retrieve a new empty list of ExpandoObject type
        /// </summary>        
        public static List<IDictionary<string, object>> NewExpandoObjectList()
        {
            return new List<IDictionary<string, object>>();
        }

        /// <summary>
        /// convert given object to an ExpandoObject
        /// </summary>        
        public static ExpandoObject ToExpando(this object obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            var type = obj.GetType();

            foreach (var property in type.GetProperties()) expando.Add(property.Name, property.GetValue(obj));

            return expando as ExpandoObject;
        }

        /// <summary>
        /// convert given expando object to a JObject
        /// </summary>        
        public static JObject AsJObject(dynamic expando)
        {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(expando));
        }

        /// <summary>
        /// creates a dynamic from an anonymous lambda
        /// </summary>        
        public static dynamic Eval<T>(this Func<T> fn)
        {
            return ToExpando(fn());
        }

        public static string SerializeToJson<T>(this Func<T> fn)
        {
            return JsonConvert.SerializeObject(Eval(fn));
        }

    }

}
