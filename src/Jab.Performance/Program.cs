using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Jab.Performance.Basic.Complex;
using System.Reflection;

//BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);

//var config = ManualConfig.Create(DefaultConfig.Instance)
//                         .WithOptions(ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile);
//BenchmarkRunner.Run(Assembly.GetExecutingAssembly(), config);

var b = new BasicComplexBenchmark { NumbersOfCalls = 1000, NumbersOfClasses = 1 };
Console.WriteLine("Press to start...");
Console.ReadLine();
for (int i = 0; i < 1000; i++)
{
    b.Jab();
    b.Improved_Jab();
}
Console.WriteLine("End");
//Console.ReadLine();

