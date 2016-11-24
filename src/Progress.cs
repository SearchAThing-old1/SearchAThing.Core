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
using System.Globalization;
using System.Text;
using static System.Math;
using System.Linq;
using System.Diagnostics;

namespace SearchAThing
{


    public class Progress : IDisposable
    {

        /// <summary>
        /// current elements
        /// </summary>
        public int Current { get; private set; } = 0;

        /// <summary>
        /// total elements
        /// </summary>
        public int Total { get; private set; }

        Stopwatch sw = new Stopwatch();

        /// <summary>
        /// init the progress stat and start internal timer
        /// </summary>        
        public Progress(int total)
        {
            Total = total;
            sw.Start();
        }

        /// <summary>
        /// start the progress stopwatch timer manually
        /// </summary>
        public void Start()
        {
            sw.Start();
        }

        /// <summary>
        /// stop the progress stopwatch timer
        /// </summary>
        public void Stop()
        {
            sw.Stop();
        }

        public TimeSpan Elapsed { get { return sw.Elapsed; } }

        public string ElapsedStr { get { return $"Elapsed: {TimeSpan.FromSeconds((int)Elapsed.TotalSeconds)}"; } }

        public double? ETASeconds
        {
            get
            {
                if (ProgressFactor == 0) return null;
                return ItemsLeftCount / ItemRateSec;
            }
        }

        /// <summary>
        /// estimated remaining time
        /// </summary>
        public TimeSpan? ETA
        {
            get
            {
                if (ETASeconds.HasValue)
                    return TimeSpan.FromSeconds(ETASeconds.Value);
                else
                    return null;
            }
        }

        /// <summary>
        /// item/sec
        /// </summary>
        public double ItemRateSec
        {
            get
            {
                return ((double)Current) / Elapsed.TotalSeconds;
            }
        }

        /// <summary>
        /// items left
        /// </summary>
        public int ItemsLeftCount { get { return Total - Current; } }

        /// <summary>
        /// retrieve a progress factor 0..1
        /// </summary>
        public double ProgressFactor
        {
            get
            {
                if (Total == 0) return 0;
                return ((double)Current) / Total;
            }
        }

        /// <summary>
        /// retrieve a progress percent string ( eg. "20.4%" )
        /// </summary>
        public string ProgressPercentString { get { return string.Format(CultureInfo.InvariantCulture, "{0,6:0.0}%", ProgressFactor * 100); } }

        /// <summary>
        /// retrieve an estimated remaining time ( eg. "ETA 00:02:30" )
        /// </summary>
        public string ETAString
        {
            get
            {
                var eta_sec = ETASeconds;

                if (eta_sec.HasValue)
                    return $"ETA {TimeSpan.FromSeconds((int)eta_sec.Value)}";
                else
                    return "ETA unk";

            }
        }

        /// <summary>
        /// add a value to the current
        /// </summary>        
        public void AddToCurrent(int delta)
        {
            Current += delta;
        }

        /// <summary>
        /// increment current
        /// </summary>
        public void Increment()
        {
            Current++;
        }

        /// <summary>
        /// ram used (bytes)
        /// </summary>        
        public long RamUsedBytes()
        {
            return GC.GetTotalMemory(false);
        }

        /// <summary>
        /// ram used string ( eg. "20.3 Mb" )
        /// </summary>        
        public string RamUsedString()
        {
            return RamUsedBytes().HumanReadable(onlyBytesUnit: false);
        }

        /// <summary>
        /// detailed representation ( eg. "20% (10 Mb) [ETA=02:10:00] rate=200/sec" )
        /// </summary>
        public string Detail
        {
            get
            {
                return $"{ProgressPercentString} ({RamUsedString()}) [{ETAString} / {ElapsedStr}] rate={Round(ItemRateSec, 1)}/sec ) {Current} of {Total}";
            }
        }

        public override string ToString()
        {
            return Detail;
        }

        public void Dispose()
        {
            sw.Stop();
        }
    }

}