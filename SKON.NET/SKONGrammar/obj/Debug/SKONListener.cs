//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\juliu\Documents\GitHub\SKON.NET\SKON.NET\SKONGrammar\SKON.g4 by ANTLR 4.5.3

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace SKON.NET {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="SKONParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5.3")]
[System.CLSCompliant(false)]
public interface ISKONListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.skon"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSkon([NotNull] SKONParser.SkonContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.skon"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSkon([NotNull] SKONParser.SkonContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.open_map"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOpen_map([NotNull] SKONParser.Open_mapContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.open_map"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOpen_map([NotNull] SKONParser.Open_mapContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.map"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMap([NotNull] SKONParser.MapContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.map"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMap([NotNull] SKONParser.MapContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPair([NotNull] SKONParser.PairContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPair([NotNull] SKONParser.PairContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.open_array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOpen_array([NotNull] SKONParser.Open_arrayContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.open_array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOpen_array([NotNull] SKONParser.Open_arrayContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArray([NotNull] SKONParser.ArrayContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArray([NotNull] SKONParser.ArrayContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.simple_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSimple_value([NotNull] SKONParser.Simple_valueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.simple_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSimple_value([NotNull] SKONParser.Simple_valueContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.complex_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComplex_value([NotNull] SKONParser.Complex_valueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.complex_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComplex_value([NotNull] SKONParser.Complex_valueContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="SKONParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterValue([NotNull] SKONParser.ValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SKONParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitValue([NotNull] SKONParser.ValueContext context);
}
} // namespace SKON.NET