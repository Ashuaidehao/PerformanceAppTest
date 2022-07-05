using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace PerformanceApp
{
    /// <summary>
    /// byte[] 比较性能测试
    /// Fast； Span Equal
    /// medium: Sequence Equal
    /// Lowest: Unsafe Equal
    /// </summary>
    [Config(typeof(MyConfig))]
    //[RPlotExporter]
    public class ByteArrayEqualTest
    {

        private byte[] x;
        private byte[] y;


        [ParamsSource(nameof(ByteCounts))]
        public int ByteCount;
        public IEnumerable<int> ByteCounts => new int[]
        {
            15,
            (1 << 10) + 2,
            (1 << 20) + 9,
            //0x7FFFFFC7,
        };

        [GlobalSetup]
        public void Setup()
        {
            x = new byte[ByteCount];
            y = new byte[ByteCount];
        }
        //public ByteArrayEqualTest()
        //{
        //    x=new byte[ByteCount];
        //    y=new byte[ByteCount];
        //    //new Random(42).NextBytes(x);
        //    ////x[999] = 1;
        //    //x.CopyTo(y, 0);

        //}

        [Benchmark(Baseline = true)]
        public bool SpanEquals()
        {
            //if (ReferenceEquals(x, y)) return true;
            //if (x is null || y is null) return false;
            //int len = x.Length;
            //if (len != y.Length) return false;
            //if (len == 0) return true;
            return x.AsSpan().SequenceEqual(y.AsSpan());
        }


        [Benchmark]
        public unsafe bool UnsafeEquals()
        {
            //if (ReferenceEquals(x, y)) return true;
            //if (x is null || y is null) return false;
            int len = x.Length;
            //if (len != y.Length) return false;
            //if (len == 0) return true;
            fixed (byte* xp = x, yp = y)
            {
                long* xlp = (long*)xp, ylp = (long*)yp;
                for (; len >= 8; len -= 8)
                {
                    if (*xlp != *ylp) return false;
                    xlp++;
                    ylp++;
                }
                byte* xbp = (byte*)xlp, ybp = (byte*)ylp;
                for (; len > 0; len--)
                {
                    if (*xbp != *ybp) return false;
                    xbp++;
                    ybp++;
                }
            }
            return true;
        }



        [Benchmark]
        public bool SequenceEquals()
        {
            //if (ReferenceEquals(x, y)) return true;
            //if (x is null || y is null) return false;
            //int len = x.Length;
            //if (len != y.Length) return false;
            //if (len == 0) return true;
            return x.SequenceEqual(y);
        }
    }
}
