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

using System;
using System.Collections;
using System.Collections.Generic;
using static System.Math;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace SearchAThing
{

    namespace Core
    {

        public class ObjectDiff
        {

            /// <summary>
            /// If the property is of a nested object item, the path will be
            /// the composition of property names separated with a dot.
            /// </summary>
            public string PropertyFullPath { get; private set; }

            /// <summary>
            /// Old top comparing object
            /// </summary>
            public object OldObject { get; private set; }

            /// <summary>
            /// Value of changed property in the Old object.
            /// </summary>
            public object OldPropertyValue { get; private set; }

            /// <summary>
            /// New top comparing object.
            /// </summary>
            public object NewObject { get; private set; }

            /// <summary>
            /// Value of changed property in the New object.
            /// </summary>
            public object NewPropertyValue { get; private set; }

            public List<object> CollectionElementsToRemove { get; private set; }
            public List<object> CollectionElementsToAdd { get; private set; }

            public ObjectDiff(string propertyFullPath,
                object oldObject, object oldPropertyValue,
                object newObject, object newPropertyValue,
                List<object> collectionElementsToRemove,
                List<object> collectionElementsToAdd)
            {
                PropertyFullPath = propertyFullPath;
                OldObject = oldObject;
                OldPropertyValue = oldPropertyValue;
                NewObject = newObject;
                NewPropertyValue = newPropertyValue;
                CollectionElementsToAdd = collectionElementsToAdd;
                CollectionElementsToRemove = collectionElementsToRemove;
            }
        }

        public static partial class Extensions
        {

            static string PropertyFullname(string path, string propertyName)
            {
                if (string.IsNullOrEmpty(path))
                    return propertyName;
                else
                    return $"{path}.{propertyName}";
            }

            static HashSet<Type> _PredefinedAdditionalDirectComparision;
            static HashSet<Type> PredefinedAdditionalDirectComparision
            {
                get
                {
                    if (_PredefinedAdditionalDirectComparision == null)
                    {
                        _PredefinedAdditionalDirectComparision = new HashSet<Type>();
                        _PredefinedAdditionalDirectComparision.Add(typeof(string));
                    }
                    return _PredefinedAdditionalDirectComparision;
                }
            }

            static Type tICollection = typeof(ICollection);
            static Type tBsonIgnoreAttribute = typeof(BsonIgnoreAttribute);

            /// <summary>
            /// compare two objects reporting differences for:
            /// - public properties get, set
            /// </summary>            
            public static IEnumerable<ObjectDiff> Compare(this object o1, object o2, string prefix = "", Type type = null,
                Func<Type, bool> execDirectComparision = null, object o1Root = null, object o2Root = null)
            {
                type = type ?? o1.GetType();
                o1Root = o1Root ?? o1;
                o2Root = o2Root ?? o2;

                var props = type.GetProperties();

                foreach (var prop in props)
                {                    
                    if (prop.CustomAttributes.Any(r => r.AttributeType == tBsonIgnoreAttribute)) continue;

                    // exclude properties without public setter
                    if (prop.GetSetMethod(false) == null) continue;

                    var v1 = prop.GetValue(o1);
                    var v2 = prop.GetValue(o2);

                    if (v1 == null || v2 == null)
                    {
                        if (v1 == null && v2 == null) continue;

                        yield return new ObjectDiff(PropertyFullname(prefix, prop.Name), o1, v1, o2, v2, null, null);
                        continue;
                    }

                    if (prop.PropertyType.GetInterface(tICollection.Name) == tICollection)
                    {
                        var coll1 = (ICollection)prop.GetMethod.Invoke(o1, null);
                        var coll2 = (ICollection)prop.GetMethod.Invoke(o2, null);

                        // one set was null
                        if ((coll1 == null && coll2 != null) || (coll1 != null && coll2 == null))
                            yield return new ObjectDiff(PropertyFullname(prefix, prop.Name), o1, v1, o2, v2, null, null);

                        // either set non-null, proceed to analysis
                        else if (coll1 != null && coll2 != null)
                        {
                            var hsColl2 = new HashSet<object>();
                            foreach (var x in coll2) hsColl2.Add(x);

                            var toRemove = new List<object>();
                            var toAdd = new List<object>();

                            var en1 = coll1.GetEnumerator();
                            var en2 = coll2.GetEnumerator();
                            while (en1.MoveNext())
                            {
                                if (!en2.MoveNext()) // 2th coll finished, then remove remainings from the first
                                {
                                    do
                                    {
                                        toRemove.Add(en1.Current);
                                    }
                                    while (en1.MoveNext());
                                }
                                else
                                {
                                    var item1 = en1.Current;
                                    var item1Type = item1.GetType();
                                    var item2 = en2.Current;
                                    var item2Type = item2.GetType();
                                    var differs = false;
                                    if (item1Type != item2Type)
                                        differs = true;
                                    else
                                        differs = Compare(item1, item2, prefix, item1.GetType(), execDirectComparision, o1Root, o2Root)
                                            .Any();

                                    if (differs)
                                    {
                                        toRemove.Add(item1);
                                        toAdd.Add(item2);
                                    }
                                }
                            }

                            while (en2.MoveNext()) toAdd.Add(en2.Current);

                            if (toAdd.Count > 0 || toRemove.Count > 0)
                                yield return new ObjectDiff(PropertyFullname(prefix, prop.Name), null, null, null, null,
                                    toRemove, toAdd);
                        }
                    }
                    else if (prop.PropertyType.IsPrimitive || prop.PropertyType.IsValueType ||
                        PredefinedAdditionalDirectComparision.Contains(prop.PropertyType) ||
                        (execDirectComparision?.Invoke(prop.PropertyType)).GetValueOrDefault())
                    {
                        if (!Equals(v1, v2)) yield return new ObjectDiff(PropertyFullname(prefix, prop.Name), o1, v1, o2, v2, null, null);
                    }
                    else
                    {
                        foreach (var x in Compare(v1, v2, PropertyFullname(prefix, prop.Name), prop.PropertyType, execDirectComparision, o1Root, o2Root))
                        {
                            yield return x;
                        }
                    }
                }
            }

        }

    }

}
