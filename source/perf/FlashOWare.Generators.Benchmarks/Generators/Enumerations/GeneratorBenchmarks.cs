using FlashOWare.Generators.Enumerations;
using Microsoft.CodeAnalysis;

namespace FlashOWare.Benchmarks.Generators.Enumerations;

public class GeneratorBenchmarks
{
	[Benchmark]
	public IIncrementalGenerator[] Generators()
	{
		return [new EnumGetNameGenerator(), new EnumIsDefinedGenerator(), new EnumInterceptorGenerator()];
	}
}
