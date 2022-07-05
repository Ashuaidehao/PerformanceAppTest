// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using PerformanceApp;

//var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
var summary = BenchmarkRunner.Run(typeof(DynamicInvokeTest));

Console.WriteLine("Hello, World!");
