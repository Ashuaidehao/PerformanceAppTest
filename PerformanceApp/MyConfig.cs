using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace PerformanceApp;

public class MyConfig : ManualConfig
{
    public MyConfig()
    {
        AddExporter(CsvMeasurementsExporter.Default);
        AddExporter(RPlotExporter.Default);
    }
}