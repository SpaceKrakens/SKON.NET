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
	public const int _tilda = 1;
	public const int _colon = 2;
	public const int _comma = 3;
	public const int _lbrace = 4;
	public const int _rbrace = 5;
	public const int _lbracket = 6;
	public const int _rbracket = 7;
	public const int _ident = 8;
	public const int _string_ = 9;
	public const int _badString = 10;
	public const int _integer_ = 11;
	public const int _double_ = 12;
	public const int _datetime_ = 13;
	public const int _ref = 14;
	public const int _def = 15;
	public const int _opt = 16;
	public const int maxT = 26;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SKONObject metadata = new SKONObject();

    public SKEMAObject data = SKEMAObject.Any;

	public Dictionary<string, SKEMAObject> definitions = new Dictionary<string, SKEMAObject>();

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
		value = value.Substring(1);

        DateTime dateTime;

        if (DateTime.TryParseExact(value, dateTimeFormats, null, DateTimeStyles.None, out dateTime))
        {
            return dateTime;
        }
        else
        {
            return ParserUtils.UnixTimeStampToDateTime(long.Parse(value));
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

	
	void SKEMA() {
		Dictionary<string, SKONObject> metadataElements = new Dictionary<string, SKONObject>();
		Dictionary<string, SKEMAObject> mapElements = new Dictionary<string, SKEMAObject>();
		string key; SKONObject value; 
		while (la.kind == 1) {
			meta_data(out key, out value);
		}
		this.metadata = new SKONObject(metadataElements); 
		open_skema_map(out mapElements);
		this.data = new SKEMAObject(mapElements); 
	}

	void meta_data(out string key, out SKONObject obj) {
		Expect(1);
		skon_map_element(out key, out obj);
		Expect(1);
	}

	void open_skema_map(out Dictionary<string, SKEMAObject> mapElements ) {
		string key; SKEMAObject value; mapElements = new Dictionary<string, SKEMAObject>(); 
		while (la.kind == 8 || la.kind == 15 || la.kind == 16) {
			if (la.kind == 8 || la.kind == 16) {
				skema_map_element(out key, out value);
				mapElements[key] = value; 
			} else {
				definition(out key, out value);
				definitions[key] = value; 
			}
			ExpectWeak(3, 1);
		}
	}

	void skon_map_element(out string key, out SKONObject obj) {
		Ident(out key);
		Expect(2);
		skon_value(out obj);
	}

	void skema_map(out SKEMAObject map) {
		Dictionary<string, SKEMAObject> mapElements; 
		Expect(4);
		open_skema_map(out mapElements);
		map = new SKEMAObject(mapElements); 
		Expect(5);
	}

	void skon_map(out SKONObject map) {
		Dictionary<string, SKONObject> mapElements; 
		Expect(4);
		open_skon_map(out mapElements);
		map = new SKONObject(mapElements); 
		Expect(5);
	}

	void open_skon_map(out Dictionary<string, SKONObject> mapElements ) {
		string key; SKONObject value; mapElements = new Dictionary<string, SKONObject>(); 
		while (la.kind == 8) {
			skon_map_element(out key, out value);
			mapElements[key] = value; 
			ExpectWeak(3, 2);
		}
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
		if (StartOf(3)) {
			type(out skemaObj);
		} else if (la.kind == 4) {
			skema_map(out skemaObj);
		} else if (la.kind == 6) {
			skema_array(out skemaObj);
		} else if (la.kind == 14) {
			Get();
			skemaObj = new SKEMAObject(t.val.Substring(1)); 
		} else SynErr(27);
	}

	void skon_array(out SKONObject array) {
		List<SKONObject> arrayElements; 
		Expect(6);
		open_skon_array(out arrayElements);
		array = new SKONObject(arrayElements); 
		Expect(7);
	}

	void open_skon_array(out List<SKONObject> arrayElements ) {
		SKONObject skonObject; arrayElements = new List<SKONObject>(); 
		while (StartOf(4)) {
			skon_value(out skonObject);
			arrayElements.Add(skonObject); 
			ExpectWeak(3, 5);
		}
	}

	void skema_map_element(out string key, out SKEMAObject obj) {
		if (la.kind == 16) {
			Get();
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

	void skon_value(out SKONObject skonObject) {
		skonObject = null; 
		switch (la.kind) {
		case 9: {
			Get();
			skonObject = new SKONObject(ParserUtils.EscapeString(t.val.Substring(1, t.val.Length - 2))); 
			break;
		}
		case 11: {
			Get();
			skonObject = new SKONObject(int.Parse(t.val)); 
			break;
		}
		case 12: {
			Get();
			skonObject = new SKONObject(double.Parse(t.val, CultureInfo.InvariantCulture)); 
			break;
		}
		case 13: {
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
		case 19: {
			Get();
			skonObject = new SKONObject(); 
			break;
		}
		default: SynErr(28); break;
		}
	}

	void type(out SKEMAObject skemaObj) {
		skemaObj = null; 
		switch (la.kind) {
		case 20: {
			Get();
			skemaObj = SKEMAObject.Any; 
			break;
		}
		case 21: {
			Get();
			skemaObj = SKEMAObject.String; 
			break;
		}
		case 22: {
			Get();
			skemaObj = SKEMAObject.Integer; 
			break;
		}
		case 23: {
			Get();
			skemaObj = SKEMAObject.Float; 
			break;
		}
		case 24: {
			Get();
			skemaObj = SKEMAObject.Boolean; 
			break;
		}
		case 25: {
			Get();
			skemaObj = SKEMAObject.DateTime; 
			break;
		}
		default: SynErr(29); break;
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
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x},
		{_T,_x,_x,_x, _x,_T,_x,_x, _T,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x},
		{_T,_x,_x,_x, _x,_T,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_T, _T,_T,_x,_x},
		{_x,_x,_x,_x, _T,_x,_T,_x, _x,_T,_x,_T, _T,_T,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x},
		{_T,_x,_x,_x, _T,_x,_T,_T, _x,_T,_x,_T, _T,_T,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x}

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
			case 1: s = "tilda expected"; break;
			case 2: s = "colon expected"; break;
			case 3: s = "comma expected"; break;
			case 4: s = "lbrace expected"; break;
			case 5: s = "rbrace expected"; break;
			case 6: s = "lbracket expected"; break;
			case 7: s = "rbracket expected"; break;
			case 8: s = "ident expected"; break;
			case 9: s = "string_ expected"; break;
			case 10: s = "badString expected"; break;
			case 11: s = "integer_ expected"; break;
			case 12: s = "double_ expected"; break;
			case 13: s = "datetime_ expected"; break;
			case 14: s = "ref expected"; break;
			case 15: s = "def expected"; break;
			case 16: s = "opt expected"; break;
			case 17: s = "\"true\" expected"; break;
			case 18: s = "\"false\" expected"; break;
			case 19: s = "\"null\" expected"; break;
			case 20: s = "\"Any\" expected"; break;
			case 21: s = "\"String\" expected"; break;
			case 22: s = "\"Integer\" expected"; break;
			case 23: s = "\"Float\" expected"; break;
			case 24: s = "\"Boolean\" expected"; break;
			case 25: s = "\"DateTime\" expected"; break;
			case 26: s = "??? expected"; break;
			case 27: s = "invalid skema_value"; break;
			case 28: s = "invalid skon_value"; break;
			case 29: s = "invalid type"; break;

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