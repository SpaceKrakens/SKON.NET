using System.Collections.Generic;
using System.Globalization;
using SKON.Internal.Utils;



#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parser.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;

namespace SKON.SKEMA.Internal {



public class Parser {
	public const int _EOF = 0;
	public const int _dash = 1;
	public const int _colon = 2;
	public const int _comma = 3;
	public const int _lbrace = 4;
	public const int _rbrace = 5;
	public const int _lbracket = 6;
	public const int _rbracket = 7;
	public const int _ident = 8;
	public const int _version = 9;
	public const int _docver = 10;
	public const int _string_ = 11;
	public const int _badString = 12;
	public const int _integer_ = 13;
	public const int _ref = 14;
	public const int _def = 15;
	public const int _opt = 16;
	public const int maxT = 23;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SKONMetadata metadata = new SKONMetadata();

    public SKEMAObject data = SKEMAObject.Any;

	public Dictionary<string, SKEMAObject> definitions = new Dictionary<string, SKEMAObject>();

/*-------------------------------------------------------------------------*/


	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void SKEMA() {
		Dictionary<string, SKEMAObject> mapElements;
		Dictionary<string, bool> optionalMap;
		int version; string docVersion; string skema; 
		meta_version(out version);
		metadata.LanguageVersion = version; 
		meta_docVersion(out docVersion);
		metadata.DocuemntVersion = docVersion; 
		open_skema_map(out mapElements, out optionalMap);
		this.data = new SKEMAObject(mapElements, optionalMap); 
	}

	void meta_version(out int ver) {
		Expect(9);
		Expect(2);
		Expect(13);
		if (int.TryParse(t.val, out ver) == false) ver = -1; 
		Expect(1);
	}

	void meta_docVersion(out string ver) {
		Expect(10);
		Expect(2);
		Expect(11);
		if (t.val.Length > 2) ver = ParserUtils.EscapeString(t.val.Substring(1, t.val.Length - 2)); else ver = "INVALID"; 
		Expect(1);
	}

	void open_skema_map(out Dictionary<string, SKEMAObject> mapElements, out Dictionary<string, bool> optionalMap ) {
		string key; SKEMAObject value; bool optional; mapElements = new Dictionary<string, SKEMAObject>(); optionalMap = new Dictionary<string, bool>(); 
		while (la.kind == 8 || la.kind == 15 || la.kind == 16) {
			if (la.kind == 8 || la.kind == 16) {
				skema_map_element(out key, out value, out optional);
				mapElements[key] = value; if(optional) { optionalMap[key] = true; } 
			} else {
				definition(out key, out value);
				definitions[key] = value; 
			}
			ExpectWeak(3, 1);
		}
	}

	void skema_map(out SKEMAObject map) {
		Dictionary<string, SKEMAObject> mapElements; Dictionary<string, bool> optionalMap; 
		Expect(4);
		open_skema_map(out mapElements, out optionalMap);
		map = new SKEMAObject(mapElements, optionalMap); 
		Expect(5);
	}

	void skema_array(out SKEMAObject array) {
		SKEMAObject skemaObj; 
		Expect(6);
		skema_value(out skemaObj);
		array = SKEMAObject.ArrayOf(skemaObj); 
		Expect(7);
	}

	void skema_value(out SKEMAObject skemaObj) {
		skemaObj = null; 
		if (StartOf(2)) {
			type(out skemaObj);
		} else if (la.kind == 4) {
			skema_map(out skemaObj);
		} else if (la.kind == 6) {
			skema_array(out skemaObj);
		} else if (la.kind == 14) {
			Get();
			skemaObj = new SKEMAObject(t.val.Substring(1)); 
		} else SynErr(24);
	}

	void skema_map_element(out string key, out SKEMAObject obj, out bool optional) {
		optional = false; 
		if (la.kind == 16) {
			Get();
			optional = true; 
		}
		Ident(out key);
		Expect(2);
		skema_value(out obj);
	}

	void definition(out string key, out SKEMAObject def) {
		Expect(15);
		Ident(out key);
		Expect(2);
		skema_value(out def);
	}

	void Ident(out string name) {
		Expect(8);
		name = t.val; 
	}

	void type(out SKEMAObject skemaObj) {
		skemaObj = null; 
		switch (la.kind) {
		case 17: {
			Get();
			skemaObj = SKEMAObject.Any; 
			break;
		}
		case 18: {
			Get();
			skemaObj = SKEMAObject.String; 
			break;
		}
		case 19: {
			Get();
			skemaObj = SKEMAObject.Integer; 
			break;
		}
		case 20: {
			Get();
			skemaObj = SKEMAObject.Float; 
			break;
		}
		case 21: {
			Get();
			skemaObj = SKEMAObject.Boolean; 
			break;
		}
		case 22: {
			Get();
			skemaObj = SKEMAObject.DateTime; 
			break;
		}
		default: SynErr(25); break;
		}
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		SKEMA();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_T,_x,_x,_x, _x,_T,_x,_x, _T,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_x, _x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "dash expected"; break;
			case 2: s = "colon expected"; break;
			case 3: s = "comma expected"; break;
			case 4: s = "lbrace expected"; break;
			case 5: s = "rbrace expected"; break;
			case 6: s = "lbracket expected"; break;
			case 7: s = "rbracket expected"; break;
			case 8: s = "ident expected"; break;
			case 9: s = "version expected"; break;
			case 10: s = "docver expected"; break;
			case 11: s = "string_ expected"; break;
			case 12: s = "badString expected"; break;
			case 13: s = "integer_ expected"; break;
			case 14: s = "ref expected"; break;
			case 15: s = "def expected"; break;
			case 16: s = "opt expected"; break;
			case 17: s = "\"Any\" expected"; break;
			case 18: s = "\"String\" expected"; break;
			case 19: s = "\"Integer\" expected"; break;
			case 20: s = "\"Float\" expected"; break;
			case 21: s = "\"Boolean\" expected"; break;
			case 22: s = "\"DateTime\" expected"; break;
			case 23: s = "??? expected"; break;
			case 24: s = "invalid skema_value"; break;
			case 25: s = "invalid type"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}