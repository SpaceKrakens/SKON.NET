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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="SKONParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5.3")]
[System.CLSCompliant(false)]
public interface ISKONVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.skon"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSkon([NotNull] SKONParser.SkonContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.open_map"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpen_map([NotNull] SKONParser.Open_mapContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.map"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMap([NotNull] SKONParser.MapContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.pair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPair([NotNull] SKONParser.PairContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.open_array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpen_array([NotNull] SKONParser.Open_arrayContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArray([NotNull] SKONParser.ArrayContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.simple_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSimple_value([NotNull] SKONParser.Simple_valueContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.complex_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComplex_value([NotNull] SKONParser.Complex_valueContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SKONParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValue([NotNull] SKONParser.ValueContext context);
}
} // namespace SKON.NET