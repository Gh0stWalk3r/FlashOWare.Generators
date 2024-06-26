using Verifier = FlashOWare.Tests.CodeAnalysis.CSharp.Testing.CSharpIncrementalGeneratorVerifier<FlashOWare.Generators.Enumerations.EnumGetNameGenerator>;

namespace FlashOWare.Tests.Generators.Enumerations;

public class EnumGetNameGeneratorTests
{
	[Fact]
	public async Task Execute_NoAttributes_NoEmit()
	{
		string code = """
			using System;
			using FlashOWare.Generators;

			namespace Namespace;

			public class Class
			{
				public void Method()
				{
					_ = Enum.GetName(StringComparison.Ordinal);
				}
			}
			""";

		await Verifier.VerifyAsync(code);
	}

	[Fact]
	public async Task Execute_WithAttribute_AddSource()
	{
		string code = """
			using System;
			using FlashOWare.Generators;

			namespace Namespace;

			[GeneratedEnumGetNameAttribute<StringComparison>]
			//[GeneratedEnumGetNameAttribute<AttributeTargets>]
			public static partial class Class
			{
			}
			""";

		string generated = $$"""
			{{AutoGenerated.Header}}
			namespace Namespace;

			partial class Class
			{
				{{AutoGenerated.GeneratedCodeAttribute}}
				public static string? GetName(global::System.StringComparison value)
				{
					return value switch
					{
						global::System.StringComparison.CurrentCulture => nameof(global::System.StringComparison.CurrentCulture),
						global::System.StringComparison.CurrentCultureIgnoreCase => nameof(global::System.StringComparison.CurrentCultureIgnoreCase),
						global::System.StringComparison.InvariantCulture => nameof(global::System.StringComparison.InvariantCulture),
						global::System.StringComparison.InvariantCultureIgnoreCase => nameof(global::System.StringComparison.InvariantCultureIgnoreCase),
						global::System.StringComparison.Ordinal => nameof(global::System.StringComparison.Ordinal),
						global::System.StringComparison.OrdinalIgnoreCase => nameof(global::System.StringComparison.OrdinalIgnoreCase),
						_ => null,
					};
				}
			}

			""";

		await Verifier.VerifyAsync(code, ("Namespace.Class.GetName.g.cs", generated));
	}

	[Fact]
	public async Task Execute_WithAttributes_AddSources()
	{
		string[] code = ["""
			using System;
			using FlashOWare.Generators;

			namespace Namespace;

			[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			[GeneratedEnumGetNameAttribute<DayOfWeek>]
			public static partial class Class
			{
			}
			""", """
			using System;
			using FlashOWare.Generators;
			using Alias = FlashOWare.Generators.GeneratedEnumGetNameAttribute<System.MidpointRounding>;

			namespace Namespace;

			[Alias]
			[GeneratedEnumGetNameAttribute<DateTimeKind>]
			public static partial class Class
			{
			}

			[Obsolete("Test")]
			[GeneratedEnumGetNameAttribute<DateTimeKind>]
			public partial class NotStatic
			{
			}

			[{|#0:GeneratedEnumGetNameAttribute<DateTimeKind>|}]
			public partial struct ValueType
			{
			}
			""",
		];

		DiagnosticResult diagnostic = Diagnostic.CS0592(0, "GeneratedEnumGetNameAttribute<>", "class");

		(string, string)[] generated = [
			("Namespace.Class.GetName.g.cs", $$"""
				{{AutoGenerated.Header}}
				namespace Namespace;

				partial class Class
				{
					{{AutoGenerated.GeneratedCodeAttribute}}
					public static string? GetName(global::System.DayOfWeek value)
					{
						return value switch
						{
							global::System.DayOfWeek.Sunday => nameof(global::System.DayOfWeek.Sunday),
							global::System.DayOfWeek.Monday => nameof(global::System.DayOfWeek.Monday),
							global::System.DayOfWeek.Tuesday => nameof(global::System.DayOfWeek.Tuesday),
							global::System.DayOfWeek.Wednesday => nameof(global::System.DayOfWeek.Wednesday),
							global::System.DayOfWeek.Thursday => nameof(global::System.DayOfWeek.Thursday),
							global::System.DayOfWeek.Friday => nameof(global::System.DayOfWeek.Friday),
							global::System.DayOfWeek.Saturday => nameof(global::System.DayOfWeek.Saturday),
							_ => null,
						};
					}

					{{AutoGenerated.GeneratedCodeAttribute}}
					public static string? GetName(global::System.MidpointRounding value)
					{
						return value switch
						{
							global::System.MidpointRounding.ToEven => nameof(global::System.MidpointRounding.ToEven),
							global::System.MidpointRounding.AwayFromZero => nameof(global::System.MidpointRounding.AwayFromZero),
							global::System.MidpointRounding.ToZero => nameof(global::System.MidpointRounding.ToZero),
							global::System.MidpointRounding.ToNegativeInfinity => nameof(global::System.MidpointRounding.ToNegativeInfinity),
							global::System.MidpointRounding.ToPositiveInfinity => nameof(global::System.MidpointRounding.ToPositiveInfinity),
							_ => null,
						};
					}

					{{AutoGenerated.GeneratedCodeAttribute}}
					public static string? GetName(global::System.DateTimeKind value)
					{
						return value switch
						{
							global::System.DateTimeKind.Unspecified => nameof(global::System.DateTimeKind.Unspecified),
							global::System.DateTimeKind.Utc => nameof(global::System.DateTimeKind.Utc),
							global::System.DateTimeKind.Local => nameof(global::System.DateTimeKind.Local),
							_ => null,
						};
					}
				}

				"""),
			("Namespace.NotStatic.GetName.g.cs", $$"""
				{{AutoGenerated.Header}}
				namespace Namespace;

				partial class NotStatic
				{
					{{AutoGenerated.GeneratedCodeAttribute}}
					public static string? GetName(global::System.DateTimeKind value)
					{
						return value switch
						{
							global::System.DateTimeKind.Unspecified => nameof(global::System.DateTimeKind.Unspecified),
							global::System.DateTimeKind.Utc => nameof(global::System.DateTimeKind.Utc),
							global::System.DateTimeKind.Local => nameof(global::System.DateTimeKind.Local),
							_ => null,
						};
					}
				}

				"""),
		];

		await Verifier.VerifyAsync(code, diagnostic, generated);
	}

	[Fact]
	public async Task Execute_Partial_AddSource()
	{
		string code = """
			using System;
			using FlashOWare.Generators;

			namespace Namespace;

			[GeneratedEnumGetNameAttribute<ConsoleModifiers>]
			public static partial class Class
			{
			}

			[GeneratedEnumGetNameAttribute<System.IO.FileAccess>]
			public static partial class Class
			{
			}

			[GeneratedEnumGetNameAttribute<ConsoleModifiers>]
			[GeneratedEnumGetNameAttribute<System.IO.FileAccess>]
			public static class NotPartial
			{
			}
			""";

		string generated = $$"""
			{{AutoGenerated.Header}}
			namespace Namespace;

			partial class Class
			{
				{{AutoGenerated.GeneratedCodeAttribute}}
				public static string? GetName(global::System.ConsoleModifiers value)
				{
					return value switch
					{
						global::System.ConsoleModifiers.Alt => nameof(global::System.ConsoleModifiers.Alt),
						global::System.ConsoleModifiers.Shift => nameof(global::System.ConsoleModifiers.Shift),
						global::System.ConsoleModifiers.Control => nameof(global::System.ConsoleModifiers.Control),
						_ => null,
					};
				}

				{{AutoGenerated.GeneratedCodeAttribute}}
				public static string? GetName(global::System.IO.FileAccess value)
				{
					return value switch
					{
						global::System.IO.FileAccess.Read => nameof(global::System.IO.FileAccess.Read),
						global::System.IO.FileAccess.Write => nameof(global::System.IO.FileAccess.Write),
						global::System.IO.FileAccess.ReadWrite => nameof(global::System.IO.FileAccess.ReadWrite),
						_ => null,
					};
				}
			}

			""";

		await Verifier.VerifyAsync(code, ("Namespace.Class.GetName.g.cs", generated));
	}

	[Fact]
	public async Task Execute_GlobalNamespace_AddSource()
	{
		string code = """
			using System;
			using FlashOWare.Generators;

			[GeneratedEnumGetNameAttribute<MyEnum>]
			public static partial class Class
			{
			}

			public enum MyEnum
			{
				Zero,
				One,
				Two,
				Three,
				Four,
			}
			""";

		string generated = $$"""
			{{AutoGenerated.Header}}
			partial class Class
			{
				{{AutoGenerated.GeneratedCodeAttribute}}
				public static string? GetName(global::MyEnum value)
				{
					return value switch
					{
						global::MyEnum.Zero => nameof(global::MyEnum.Zero),
						global::MyEnum.One => nameof(global::MyEnum.One),
						global::MyEnum.Two => nameof(global::MyEnum.Two),
						global::MyEnum.Three => nameof(global::MyEnum.Three),
						global::MyEnum.Four => nameof(global::MyEnum.Four),
						_ => null,
					};
				}
			}

			""";

		await Verifier.VerifyAsync(code, ("Class.GetName.g.cs", generated));
	}

	[Fact]
	public async Task Execute_Duplicate_AddDistinct()
	{
		string[] code = ["""
			using System.IO;
			using FlashOWare.Generators;

			namespace Namespace;

			public static partial class Class
			{
			}

			[GeneratedEnumGetNameAttribute<SearchOption>]
			public static partial class Class
			{
			}

			[GeneratedEnumGetNameAttribute<SearchOption>]
			[GeneratedEnumGetNameAttribute<SearchOption>]
			public static partial class Class
			{
			}
			""", """
			using System.IO;
			using FlashOWare.Generators;

			namespace Namespace;

			public static partial class Class
			{
			}

			[GeneratedEnumGetNameAttribute<SearchOption>]
			public static partial class Class
			{
			}

			[GeneratedEnumGetNameAttribute<SearchOption>]
			[GeneratedEnumGetNameAttribute<SearchOption>]
			public static partial class Class
			{
			}
			"""
		];

		string generated = $$"""
			{{AutoGenerated.Header}}
			namespace Namespace;

			partial class Class
			{
				{{AutoGenerated.GeneratedCodeAttribute}}
				public static string? GetName(global::System.IO.SearchOption value)
				{
					return value switch
					{
						global::System.IO.SearchOption.TopDirectoryOnly => nameof(global::System.IO.SearchOption.TopDirectoryOnly),
						global::System.IO.SearchOption.AllDirectories => nameof(global::System.IO.SearchOption.AllDirectories),
						_ => null,
					};
				}
			}

			""";

		await Verifier.VerifyAsync(code, ("Namespace.Class.GetName.g.cs", generated));
	}

	[Fact]
	public async Task Execute_ErrorType_NoEmit()
	{
		string code = """
			using System;
			using FlashOWare.Generators;

			namespace Namespace;

			[GeneratedEnumGetNameAttribute<{|#0:Error|}>]
			[GeneratedEnumGetNameAttribute<{|#1:true|}>]
			[GeneratedEnumGetNameAttribute<{|#2:240|}>]
			[{|#3:GeneratedEnumGetNameAttribute<>|}]
			[GeneratedEnumGetNameAttribute<{|#4:]|}
			public static partial class Class
			{
			}
			""";

		DiagnosticResult[] diagnostics = [
			Diagnostic.CS0246(0, "Error"),
			Diagnostic.CS1031(1),
			Diagnostic.CS1031(2),
			Diagnostic.CS7003(3),
			Diagnostic.CS1003(4, '>'),
			Diagnostic.CS1031(4),
		];

		await Verifier.VerifyAsync(code, diagnostics);
	}

	[Fact]
	public async Task Execute_IncludeErrors_AddSource()
	{
		string code = """
			using System.Threading;
			using FlashOWare.Generators;

			namespace Namespace;

			[GeneratedEnumGetNameAttribute<EventResetMode>]
			[GeneratedEnumGetNameAttribute<{|#0:?|}>]
			[GeneratedEnumGetNameAttribute<{|#1:int|}>]
			public static partial class Class
			{
			}

			[{|#2:GeneratedEnumGetNameAttribute<EventResetMode, EventResetMode>|}]
			[{|#3:GeneratedEnumGetNameAttribute<,>|}]
			[{|#4:GeneratedEnumGetNameAttribute|#4}{|#5:>|#5}]
			[{|#6:GeneratedEnumGetNameAttribute|}]
			public static partial class Class
			{
			}
			""";

		DiagnosticResult[] diagnostics = [
			Diagnostic.CS1031(0),
			Diagnostic.CS0315(1, "int", "TEnum", "GeneratedEnumGetNameAttribute<TEnum>", "int", "System.Enum"),
			Diagnostic.CS0305(2, "type", "GeneratedEnumGetNameAttribute<TEnum>", 1),
			Diagnostic.CS0305(3, "type", "GeneratedEnumGetNameAttribute<TEnum>", 1),
			Diagnostic.CS0305(4, "type", "GeneratedEnumGetNameAttribute<TEnum>", 1),
			Diagnostic.CS1001(5),
			Diagnostic.CS0305(6, "type", "GeneratedEnumGetNameAttribute<TEnum>", 1),
		];

		string generated = $$"""
			{{AutoGenerated.Header}}
			namespace Namespace;

			partial class Class
			{
				{{AutoGenerated.GeneratedCodeAttribute}}
				public static string? GetName(global::System.Threading.EventResetMode value)
				{
					return value switch
					{
						global::System.Threading.EventResetMode.AutoReset => nameof(global::System.Threading.EventResetMode.AutoReset),
						global::System.Threading.EventResetMode.ManualReset => nameof(global::System.Threading.EventResetMode.ManualReset),
						_ => null,
					};
				}
			}

			""";

		await Verifier.VerifyAsync(code, diagnostics, ("Namespace.Class.GetName.g.cs", generated));
	}
}
