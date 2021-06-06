namespace CSG.Shapes
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;
    using System.Numerics;

    [SimpleJob(RuntimeMoniker.Net50, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    [SimpleJob(RuntimeMoniker.CoreRt30)]
    [RPlotExporter]
    public class CubeBenchmark
    {
        [Params(1000, 10000)]
        public int N;

        private Cube cubeA, cubeB; 

        [GlobalSetup]
        public void Setup()
        {
            this.cubeA = new Cube() { Position = new Vector3(0, 0, 0) };
            this.cubeB = new Cube() { Position = new Vector3(1, 1, 1) };
            var c1 = this.cubeA.Cache;
            var c2 = this.cubeB.Cache;
        }


        [Benchmark]
        public ShapeCache Build()
        {
            return new Cube().Cache;
        }

        [Benchmark]
        public GeneratedShape Subtract_two()
        {
            return this.cubeA.Subtract(this.cubeB);
        }
    }
}
