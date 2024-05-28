using Verifier = FlashOWare.Tests.CodeAnalysis.CSharp.Testing.CSharpIncrementalGeneratorVerifier<FlashOWare.Generators.Enumerations.EnumInterceptorGenerator>;

namespace FlashOWare.Tests.Generators.Enumerations;

public class EnumInterceptorGeneratorTests
{
	[Fact]
	public async Task Execute_NoInvocations_NoEmit()
	{
		string[] code = ["""
			using System;

			namespace Namespace;

			public class Class
			{
				public void Method()
				{
					_ = Enum.GetName(typeof(DateTimeKind), DateTimeKind.Utc);
					_ = Enum.IsDefined(typeof(DateTimeKind), DateTimeKind.Utc);

					_ = GetName<StringSplitOptions>(StringSplitOptions.RemoveEmptyEntries);
					_ = IsDefined<StringSplitOptions>(StringSplitOptions.RemoveEmptyEntries);
				}

				private static string? GetName<TEnum>(TEnum value) where TEnum : struct, Enum
					=> Enum.GetName<TEnum>(value);

				private static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, Enum
					=> Enum.IsDefined<TEnum>(value);
			}
			""", """
			using System;

			namespace Namespace;

			public class Other
			{
				public void Method()
				{
					_ = Enum.GetName(DateTimeKind.Utc);
					_ = Enum.IsDefined(StringSplitOptions.RemoveEmptyEntries);
				}
			}

			file abstract class Enum
			{
				public static string? GetName<TEnum>(TEnum value) where TEnum : struct, System.Enum
					=> System.Enum.GetName(value);

				public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, System.Enum
					=> System.Enum.IsDefined(value);
			}
			""",
		];

		await Verifier.VerifyAsync(code);
	}

	[Fact]
	public async Task Execute_WithInvocations_AddSource()
	{
		string[] code = ["""
			using System;

			namespace Namespace;

			public class Class
			{
				public void Method()
				{
					_ = Enum.GetName(DateTimeKind.Utc);
					_ = Enum.GetName<StringSplitOptions>(StringSplitOptions.None | StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
					_ = Enum.IsDefined(DateTimeKind.Utc);
					_ = Enum.IsDefined<StringSplitOptions>(StringSplitOptions.None | StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				}
			}
			""", """
			using System;
			using Alias = System.Enum;
			using static System.Enum;
			using Enum1 = System.UriKind;
			using Enum2 = System.UriFormat;

			namespace Namespace;

			public class Other
			{
				private const UriKind field1 = UriKind.Absolute;
				private const UriFormat field2 = UriFormat.Unescaped;

				public void Method(UriKind parameter1, UriFormat parameter2)
				{
					UriKind local1 = UriKind.Absolute;
					UriFormat local2 = UriFormat.Unescaped;
					//_ = Enum.GetName(UriKind.Absolute);
					//_ = Enum.IsDefined(UriFormat.Unescaped);
					_ = Enum.GetName(field1);
					_ = Enum.IsDefined(field2);
					_ = Enum.GetName(parameter1);
					_ = Enum.IsDefined(parameter2);
					_ = Enum.GetName(local1);
					_ = Enum.IsDefined(local2);
					_ = Enum.GetName<Enum1>(default);
					_ = Enum.IsDefined<Enum2>(default);
					_ = Enum.GetName(default(Enum1));
					_ = Enum.IsDefined(default(Enum2));
					_ = Alias.GetName(UriKind.Absolute);
					_ = Alias.IsDefined(UriFormat.Unescaped);
					_ = GetName(UriKind.Absolute);
					_ = IsDefined(UriFormat.Unescaped);
					_ = Enum.GetName((UriKind)0);
					_ = Enum.IsDefined((UriFormat)0);
					_ = global::System.Enum.GetName(global::System.UriKind.Absolute);
					_ = global::System.Enum.IsDefined(global::System.UriFormat.Unescaped);
				}
			}
			""",
		];

		string generated = $$"""
			{{AutoGenerated.Header}}
			{{AutoGenerated.InterceptsLocationAttribute}}

			namespace FlashOWare.Generated
			{
				using System.Runtime.CompilerServices;

				{{AutoGenerated.GeneratedCodeAttribute}}
				file static class EnumInterceptors
				{
					[InterceptsLocation(@"/0/Test0.cs", 9, 12)]
					internal static string? GetName(global::System.DateTimeKind value)
					{
						return value switch
						{
							global::System.DateTimeKind.Unspecified => nameof(global::System.DateTimeKind.Unspecified),
							global::System.DateTimeKind.Utc => nameof(global::System.DateTimeKind.Utc),
							global::System.DateTimeKind.Local => nameof(global::System.DateTimeKind.Local),
							_ => null,
						};
					}
					[InterceptsLocation(@"/0/Test0.cs", 10, 12)]
					internal static string? GetName(global::System.StringSplitOptions value)
					{
						return value switch
						{
							global::System.StringSplitOptions.None => nameof(global::System.StringSplitOptions.None),
							global::System.StringSplitOptions.RemoveEmptyEntries => nameof(global::System.StringSplitOptions.RemoveEmptyEntries),
							global::System.StringSplitOptions.TrimEntries => nameof(global::System.StringSplitOptions.TrimEntries),
							_ => null,
						};
					}
					[InterceptsLocation(@"/0/Test0.cs", 11, 12)]
					internal static bool IsDefined(global::System.DateTimeKind value)
					{
						return value is
							global::System.DateTimeKind.Unspecified or
							global::System.DateTimeKind.Utc or
							global::System.DateTimeKind.Local;
					}
					[InterceptsLocation(@"/0/Test0.cs", 12, 12)]
					internal static bool IsDefined(global::System.StringSplitOptions value)
					{
						return value is
							global::System.StringSplitOptions.None or
							global::System.StringSplitOptions.RemoveEmptyEntries or
							global::System.StringSplitOptions.TrimEntries;
					}
					[InterceptsLocation(@"/0/Test1.cs", 20, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 22, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 24, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 26, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 28, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 30, 13)]
					[InterceptsLocation(@"/0/Test1.cs", 32, 07)]
					[InterceptsLocation(@"/0/Test1.cs", 34, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 36, 27)]
					internal static string? GetName(global::System.UriKind value)
					{
						return value switch
						{
							global::System.UriKind.RelativeOrAbsolute => nameof(global::System.UriKind.RelativeOrAbsolute),
							global::System.UriKind.Absolute => nameof(global::System.UriKind.Absolute),
							global::System.UriKind.Relative => nameof(global::System.UriKind.Relative),
							_ => null,
						};
					}
					[InterceptsLocation(@"/0/Test1.cs", 21, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 23, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 25, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 27, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 29, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 31, 13)]
					[InterceptsLocation(@"/0/Test1.cs", 33, 07)]
					[InterceptsLocation(@"/0/Test1.cs", 35, 12)]
					[InterceptsLocation(@"/0/Test1.cs", 37, 27)]
					internal static bool IsDefined(global::System.UriFormat value)
					{
						return value is
							global::System.UriFormat.UriEscaped or
							global::System.UriFormat.Unescaped or
							global::System.UriFormat.SafeUnescaped;
					}
				}
			}

			""";

		await Verifier.VerifyAsync(code, ("FlashOWare.Generated.EnumInterceptors.g.cs", generated));
	}

	[Fact]
	public async Task Execute_GlobalNamespace_AddSource()
	{
		string code = """
			using System;

			public class Class
			{
				public void Method()
				{
					_ = Enum.GetName(MyEnum.One);
					_ = Enum.IsDefined(MyEnum.Two);
				}
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
			{{AutoGenerated.InterceptsLocationAttribute}}

			namespace FlashOWare.Generated
			{
				using System.Runtime.CompilerServices;

				{{AutoGenerated.GeneratedCodeAttribute}}
				file static class EnumInterceptors
				{
					[InterceptsLocation(@"/0/Test0.cs", 7, 12)]
					internal static string? GetName(global::MyEnum value)
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
					[InterceptsLocation(@"/0/Test0.cs", 8, 12)]
					internal static bool IsDefined(global::MyEnum value)
					{
						return value is
							global::MyEnum.Zero or
							global::MyEnum.One or
							global::MyEnum.Two or
							global::MyEnum.Three or
							global::MyEnum.Four;
					}
				}
			}

			""";

		await Verifier.VerifyAsync(code, ("FlashOWare.Generated.EnumInterceptors.g.cs", generated));
	}

	[Theory]
	[InlineData("GetName", "Enum.GetName<TEnum>(TEnum)")]
	[InlineData("IsDefined", "Enum.IsDefined<TEnum>(TEnum)")]
	public async Task Execute_Error_NoEmit(string method, string signature)
	{
		string code = $$"""
			using System;

			namespace Namespace;

			public class Class
			{
				public void Method()
				{
					_ = Enum.{|#0:{{method}}|}();
					_ = {|#1:Enum.{|#2:{{method}}<>|}|}();
					_ = Enum.{{method}}({|#3:Error|});
					_ = Enum.{|#4:{{method}}<{|#5:Error|}>|}();
					_ = Enum.{|#6:{{method}}|}(240);
					_ = {|#7:Enum.{{method}}<240|}>({|#8:)|};
					_ = Enum.{{method}}<DayOfWeek>({|#9:6|});

					_ = Enum.{|#10:{{method}}|}(default);
					_ = Enum.{|#11:{{method}}<int>|}(default);
				}
			}
			""";

		DiagnosticResult[] diagnostics = [
			Diagnostic.CS1501(0, method, 0),
			Diagnostic.CS0305(1, "method group", method, 1),
			Diagnostic.CS7036(2, "value", signature),
			Diagnostic.CS0103(3, "Error"),
			Diagnostic.CS7036(4, "value", signature),
			Diagnostic.CS0246(5, "Error"),
			Diagnostic.CS0315(6, "int", "TEnum", signature, "int", "System.Enum"),
			Diagnostic.CS0019(7, '<', "method group", "int"),
			Diagnostic.CS1525(8, ')'),
			Diagnostic.CS1503(9, "int", "System.DayOfWeek"),

			Diagnostic.CS0411(10, signature),
			Diagnostic.CS0315(11, "int", "TEnum", signature, "int", "System.Enum"),
		];

		await Verifier.VerifyAsync(code, diagnostics);
	}
}