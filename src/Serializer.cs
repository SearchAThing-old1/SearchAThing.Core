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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace SearchAThing.Core
{

    public static partial class Extensions
    {

        public static void Serialize<T>(this T obj, string dstPathfilename, bool binary = true, IEnumerable<Type> knownTypes = null)
        {
            var settings = new DataContractSerializerSettings()
            {
                PreserveObjectReferences = true
            };

            if (knownTypes != null) settings.KnownTypes = knownTypes;

            var serializer = new DataContractSerializer(typeof(T), settings);

            if (System.IO.File.Exists(dstPathfilename)) System.IO.File.Delete(dstPathfilename);

            using (var fs = new FileStream(dstPathfilename, FileMode.CreateNew))
            {
                if (binary)
                {
                    using (var bw = XmlDictionaryWriter.CreateBinaryWriter(fs))
                    {
                        serializer.WriteObject(bw, obj);
                    }
                }
                else
                {
                    var wrSettings = new XmlWriterSettings() { Indent = true };
                    using (var tw = XmlWriter.Create(fs, wrSettings))
                    {                        
                        serializer.WriteObject(tw, obj);
                    }
                }
            }
        }

        public static T Deserialize<T>(this string srcPathfilename, bool binary = true, IEnumerable<Type> knownTypes = null)
        {
            var settings = new DataContractSerializerSettings()
            {
                PreserveObjectReferences = true
            };

            if (knownTypes != null) settings.KnownTypes = knownTypes;

            var serializer = new DataContractSerializer(typeof(T), settings);

            T res;
            using (var fs = new FileStream(srcPathfilename, FileMode.Open))
            {
                if (binary)
                {
                    using (var br = XmlDictionaryReader.CreateBinaryReader(fs, XmlDictionaryReaderQuotas.Max))
                    {
                        res = (T)serializer.ReadObject(br);
                    }
                }
                else
                {
                    using (var tr = XmlDictionaryReader.CreateTextReader(fs, XmlDictionaryReaderQuotas.Max))
                    {
                        res = (T)serializer.ReadObject(tr);
                    }
                }
            }

            return res;
        }

    }

}
