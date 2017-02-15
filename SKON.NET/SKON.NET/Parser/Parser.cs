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

namespace SKON.Internal {



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
	public const int _skema = 11;
	public const int _string_ = 12;
	public const int _badString = 13;
	public const int _integer_ = 14;
	public const int _float_ = 15;
	public const int _datetime_ = 16;
	public const int maxT = 19;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SKONMetadata metadata = new SKONMetadata();

    public SKONObject data = new SKONObject();

	private string[] dateTimeFormats = {
        "yyyy-MM-dd",
        "hh:mm:ssZ",
        "hh:mm:ss.fffZ",
        "hh:mm:sszzz",
        "hh:mm:ss.fffzzz",
        "yyyy-MM-ddThh:mm:ssZ",
        "yyyy-MM-ddThh:mm:ss.fffZ",
        "yyyy-MM-ddThh:mm:sszzz",
        "yyyy-MM-ddThh:mm:ss.fffzzz"
    };

    private DateTime ParseDatetime(string value)
    {
        DateTime dateTime;

        if (DateTime.TryParseExact(value, dateTimeFormats, null, DateTimeStyles.None, out dateTime))
        {
            return dateTime;
        }
        else
        {
			errors.errorStream.WriteLine("Could not parse DateTime: " + value);
			errors.count++;
            return default(DateTime);
        }
    }

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

	
	void SKON() {
		Dictionary<string, SKONObject> mapElements = new Dictionary<string, SKONObject>();
		int version; string docVersion; string skema; 
		meta_version(out version);
		metadata.LanguageVersion = version; 
		meta_docVersion(out docVersion);
		metadata.DocuemntVersion = docVersion; 
		if (la.kind == 11) {
			meta_SKEMA(out skema);
			metadata.SKEMA = skema; 
		}
		open_map(out mapElements);
		this.data = new SKONObject(mapElements); 
	}

	void meta_version(out int ver) {
		Expect(9);
		Expect(2);
		Expect(14);
		if (int.TryParse(t.val, out ver) == false) ver = -1; 
		Expect(1);
	}

	void meta_docVersion(out string ver) {
		Expect(10);
		Expect(2);
		Expect(10);
		Expect(12);
		if (t.val.Length > 2) ver = ParserUtils.EscapeString(t.val.Substring(1, t.val.Length - 2)); else ver = "INVALID"; 
		Expect(1);
	}

	void meta_SKEMA(out string skema) {
		Expect(11);
		Expect(2);
		Expect(12);
		if (t.val.Length > 2) skema = ParserUtils.EscapeString(t.val.Substring(1, t.val.Length - 2)); else skema = "INVALID"; 
		Expect(1);
	}

	void open_map(out Dictionary<string, SKONObject> mapElements ) {
		string key; SKONObject value; mapElements = new Dictionary<string, SKONObject>(); 
		while (la.kind == 8) {
			map_element(out key, out value);
			mapElements[key] = value; 
			ExpectWeak(3, 1);
		}
	}

	void skon_map(out SKONObject map) {
		Dictionary<string, SKONObject> mapElements; 
		Expect(4);
		open_map(out mapElements);
		map = new SKONObject(mapElements); 
		Expect(5);
	}

	void skon_array(out SKONObject array) {
		List<SKONObject> arrayElements; 
		Expect(6);
		open_array(out arrayElements);
		array = new SKONObject(arrayElements); 
		Expect(7);
	}

	void open_array(out List<SKONObject> arrayElements ) {
		SKONObject skonObject; arrayElements = new List<SKONObject>(); 
		while (StartOf(2)) {
			value(out skonObject);
			arrayElements.Add(skonObject); 
			ExpectWeak(3, 3);
		}
	}

	void map_element(out string key, out SKONObject obj) {
		string name; SKONObject skonObject; 
		Ident(out name);
		key = name; 
		value(out skonObject);
		obj = skonObject; 
	}

	void Ident(out string name) {
		Expect(8);
		name = t.val; 
	}

	void value(out SKONObject skonObject) {
		skonObject = null; 
		switch (la.kind) {
		case 12: {
			Get();
			skonObject = new SKONObject(ParserUtils.EscapeString(t.val.Substring(1, t.val.Length - 2))); 
			break;
		}
		case 14: {
			Get();
			skonObject = new SKONObject(int.Parse(t.val)); 
			break;
		}
		case 15: {
			Get();
			skonObject = new SKONObject(double.Parse(t.val, CultureInfo.InvariantCulture)); 
			break;
		}
		case 16: {
			Get();
			skonObject = new SKONObject(ParseDatetime(t.val)); 
			break;
		}
		case 4: {
			skon_map(out skonObject);
			break;
		}
		case 6: {
			skon_array(out skonObject);
			break;
		}
		case 17: {
			Get();
			skonObject = new SKONObject(true); 
			break;
		}
		case 18: {
			Get();
			skonObject = new SKONObject(false); 
			break;
		}
		default: SynErr(20); break;
		}
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		SKON();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_T,_x,_x,_x, _x,_T,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_x,_x,_x, _T,_x,_T,_x, _x,_x,_x,_x, _T,_x,_T,_T, _T,_T,_T,_x, _x},
		{_T,_x,_x,_x, _T,_x,_T,_T, _x,_x,_x,_x, _T,_x,_T,_T, _T,_T,_T,_x, _x}

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
			case 11: s = "skema expected"; break;
			case 12: s = "string_ expected"; break;
			case 13: s = "badString expected"; break;
			case 14: s = "integer_ expected"; break;
			case 15: s = "float_ expected"; break;
			case 16: s = "datetime_ expected"; break;
			case 17: s = "\"true\" expected"; break;
			case 18: s = "\"false\" expected"; break;
			case 19: s = "??? expected"; break;
			case 20: s = "invalid value"; break;

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