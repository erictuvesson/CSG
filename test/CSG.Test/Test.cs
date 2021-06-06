namespace CSG
{
    using JetBrains.dotMemoryUnit;
    using JetBrains.dotMemoryUnit.Kernel;
    using System;
    using Xunit.Abstractions;

    public abstract class Test
    {
        protected readonly ITestOutputHelper output;

        protected Test(ITestOutputHelper output)
        {
            this.output = output;
            DotMemoryUnitTestOutput.SetOutputMethod(output.WriteLine);
        }

        protected T RunMemoryTest<T>(string name, Func<T> lambda)
        {
            var result = default(T);
            if (dotMemoryApi.IsEnabled)
            {
                dotMemory.Check(memory =>
                {
                    result = lambda();
                    output.WriteLine($"Memory Test \"{name}\", Objects: {memory.ObjectsCount}, Size: {memory.SizeInBytes} bytes.");
                });
            }
            else
            {
                result = lambda();
            }

            return result;
        }

        protected void RunMemoryTest(string name, Action lambda)
        {
            if (dotMemoryApi.IsEnabled)
            {
                dotMemory.Check(memory =>
                {
                    lambda();
                    output.WriteLine($"Memory Test \"{name}\", Objects: {memory.ObjectsCount}, Size: {memory.SizeInBytes} bytes.");
                });
            }
            else
            {
                lambda();
            }
        }
    }
}
