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
using static System.Math;

namespace SearchAThing
{

    namespace Core
    {

        public class TaggedObject<O, T>
        {

            public TaggedObject(O o, T t) { Obj = o; Tag = t; }

            public O Obj { get; set; }

            public T Tag { get; set; }

        }

        public class TaggedObject<O, T1, T2>
        {

            public TaggedObject(O o, T1 t1, T2 t2) { Obj = o; Tag1 = t1; Tag2 = t2; }

            public O Obj { get; set; }

            public T1 Tag1 { get; set; }
            public T2 Tag2 { get; set; }

        }

        public class TaggedObject<O, T1, T2, T3>
        {
            public TaggedObject(O o, T1 t1, T2 t2, T3 t3) { Obj = o; Tag1 = t1; Tag2 = t2; Tag3 = t3; }

            public O Obj { get; set; }

            public T1 Tag1 { get; set; }
            public T2 Tag2 { get; set; }
            public T3 Tag3 { get; set; }

        }

    }

    public static partial class Extensions
    {

        public static TaggedObject<O, T> TaggedObject<O, T>(this O o, T t) { return new TaggedObject<O, T>(o, t); }
        public static TaggedObject<O, T1, T2> TaggedObject<O, T1, T2>(this O o, T1 t1, T2 t2) { return new TaggedObject<O, T1, T2>(o, t1, t2); }
        public static TaggedObject<O, T1, T2, T3> TaggedObject<O, T1, T2, T3>(this O o, T1 t1, T2 t2, T3 t3) { return new TaggedObject<O, T1, T2, T3>(o, t1, t2, t3); }

    }

}
