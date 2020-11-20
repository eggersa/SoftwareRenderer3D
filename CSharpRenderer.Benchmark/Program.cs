using BenchmarkDotNet.Running;

namespace CSharpRenderer.Benchmark
{
    class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
