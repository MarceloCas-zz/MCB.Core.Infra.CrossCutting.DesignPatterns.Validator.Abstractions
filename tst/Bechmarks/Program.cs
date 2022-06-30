using Bechmarks.Benchs.ModelsBenchs;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run(typeof(ValidationResultBench));