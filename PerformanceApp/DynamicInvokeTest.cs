using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace PerformanceApp
{
    /// <summary>
    /// ReflectFuncCall 反射后并保存为泛型委托性能最佳 
    /// </summary>
    public class DynamicInvokeTest
    {
        //                        Method |        Mean |      Error |     StdDev |      Median |    Ratio |  RatioSD |
        //|----------------------------- |------------:|-----------:|-----------:|------------:|---------:|---------:|
        //|                   DirectCall |   0.5755 ns |  0.1848 ns |  0.5391 ns |   0.4221 ns |     1.00 |     0.00 |
        //|                 DelegateCall |   2.7674 ns |  0.1290 ns |  0.3617 ns |   2.7183 ns |    18.04 |    29.11 |
        //|              ReflectFuncCall |   3.2648 ns |  0.1188 ns |  0.3213 ns |   3.1828 ns |    20.08 |    40.68 |
        //|           ReflectFuncDynamic |  13.6692 ns |  0.3125 ns |  0.5556 ns |  13.6189 ns |    68.57 |   194.33 |
        //| ReflectDelegateDynamicInvoke | 413.1454 ns | 17.9699 ns | 49.7945 ns | 397.9581 ns | 2,781.25 | 5,170.63 |


        private int a = 1;
        private int b = 10;
        private Func<int, int, int> func = Calculator.Add;

        private Func<int, int, int> funcReflect;

        private dynamic funcReflectDynamic;
        private Delegate funcReflectDelegate;

        public DynamicInvokeTest()
        {
            var methodInfo = typeof(Calculator).GetMethod("Add");
            funcReflect = (Func<int, int, int>)Delegate.CreateDelegate(typeof(Func<int, int, int>), null, methodInfo);
            funcReflect(1, 1);

            funcReflectDynamic = Delegate.CreateDelegate(typeof(Func<int, int, int>), null, methodInfo);
            funcReflectDynamic(1, 1);

            funcReflectDelegate = Delegate.CreateDelegate(typeof(Func<int, int, int>), null, methodInfo);
            funcReflectDelegate.DynamicInvoke(1, 2);

        }

        [Benchmark(Baseline = true)]
        public void DirectCall()
        {
            Calculator.Add(a, b);
        }



        [Benchmark]
        public void DelegateCall()
        {
            func(a, b);
        }

        [Benchmark]
        public void ReflectFuncCall()
        {
            funcReflect(a, b);
        }



        [Benchmark]
        public void ReflectFuncDynamic()
        {
            funcReflectDynamic(a, b);
        }


        [Benchmark]
        public void ReflectDelegateDynamicInvoke()
        {
            funcReflectDelegate.DynamicInvoke(a, b);
        }
    }

    public class Calculator
    {
        public static int Add(int a, int b)
        {
            return a + b;
        }
    }
}
