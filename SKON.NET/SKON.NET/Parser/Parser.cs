using System.Collections.Generic;
using System.Globalization;



using System;

namespace SKON.NET {



public class Parser {
	public const int _EOF = 0;
	public const int _colon = 1;
	public const int _comma = 2;
	public const int _lbrace = 3;
	public const int _rbrace = 4;
	public const int _lbracket = 5;
	public const int _rbracket = 6;
	public const int _ident = 7;
	public const int _string_ = 8;
	public const int _badString = 9;
	public const int _integer_ = 10;
	public const int _double_ = 11;
	public const int _datetime_ = 12;
	public const int maxT = 16;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public Map map;

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
        if (value[0] == '@')
        {
            return new DateTime(int.Parse(value.Substring(1)));
        }else
        {
            DateTime dateTime;

            if (DateTime.TryParseExact(value, dateTimeFormats, null, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new FormatException("Could not parse " + value + "as a datetime!");
            }
        }
    }

	// Return the n-th token after the current lookahead token
	Token Peek (int n) {
		scanner.ResetPeek();
		Token x = la;
		while (n > 0) { x = scanner.Peek(); n--; }
		return x;
	}

	/* True, if the comma is not a trailing one, *
	 * like the last one in: a, b, c,            */
	bool NotFinalComma () {
		int peek = Peek(1).kind;
		return la.kind == _comma && peek != _rbrace && peek != _rbracket;
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
		Dictionary<string, SKONObject> mapElements;
		string key; SKONObject value;
		mapElements = new Dictionary<string, SKONObject>();
		int n = 0;
		
		if (la.kind == 7) {
			map_element(out key, out value);
			mapElements[key] = value; 
			while (NotFinalComma()) {
				Expect(2);
				map_element(out key, out value);
				mapElements[key] = value; 
			}
			if (la.kind == 2) {
				Get();
			}
		}
		this.map = new Map(mapElements); 
	}

	void map_element(out string key, out SKONObject obj) {
		string name; SKONObject skonObject; 
		Ident(out name);
		key = name; 
		Expect(1);
		value(out skonObject);
		obj = skonObject; 
	}

	void skon_map(out Map map) {
		Dictionary<string, SKONObject> mapElements; 
		Expect(3);
		open_map(out mapElements);
		map = new Map(mapElements); 
		Expect(4);
	}

	void open_map(out Dictionary<string, SKONObject> mapElements ) {
		string key; SKONObject value; mapElements = new Dictionary<string, SKONObject>(); 
		if (la.kind == 7) {
			map_element(out key, out value);
			mapElements[key] = value; 
			while (NotFinalComma()) {
				Expect(2);
				map_element(out key, out value);
				mapElements[key] = value; 
			}
			if (la.kind == 2) {
				Get();
			}
		}
	}

	void skon_array(out Array array) {
		List<SKONObject> arrayElements; 
		Expect(5);
		open_array(out arrayElements);
		array = new Array(arrayElements.ToArray()); 
		Expect(6);
	}

	void open_array(out List<SKONObject> arrayElements ) {
		SKONObject skonObject; arrayElements = new List<SKONObject>(); 
		if (StartOf(1)) {
			value(out skonObject);
			arrayElements.Add(skonObject); 
			while (NotFinalComma()) {
				Expect(2);
				value(out skonObject);
				arrayElements.Add(skonObject); 
			}
			if (la.kind == 2) {
				Get();
			}
		}
	}

	void Ident(out string name) {
		Expect(7);
		name = t.val; 
	}

	void value(out SKONObject skonObject) {
		skonObject = null; 
		switch (la.kind) {
		case 8: {
			Get();
			skonObject = new SKONObject(t.val); 
			break;
		}
		case 10: {
			Get();
			skonObject = new SKONObject(int.Parse(t.val)); 
			break;
		}
		case 11: {
			Get();
			skonObject = new SKONObject(double.Parse(t.val, CultureInfo.InvariantCulture)); 
			break;
		}
		case 12: {
			Get();
			skonObject = new SKONObject(ParseDatetime(t.val)); 
			break;
		}
		case 3: {
			Map map; 
			skon_map(out map);
			skonObject = map; 
			break;
		}
		case 5: {
			Array array; 
			skon_array(out array);
			skonObject = array; 
			break;
		}
		case 13: {
			Get();
			skonObject = new SKONObject(true); 
			break;
		}
		case 14: {
			Get();
			skonObject = new SKONObject(false); 
			break;
		}
		case 15: {
			Get();
			skonObject = new SKONObject(); 
			break;
		}
		default: SynErr(17); break;
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
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x},
		{_x,_x,_x,_T, _x,_T,_x,_x, _T,_x,_T,_T, _T,_T,_T,_T, _x,_x}

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
			case 1: s = "colon expected"; break;
			case 2: s = "comma expected"; break;
			case 3: s = "lbrace expected"; break;
			case 4: s = "rbrace expected"; break;
			case 5: s = "lbracket expected"; break;
			case 6: s = "rbracket expected"; break;
			case 7: s = "ident expected"; break;
			case 8: s = "string_ expected"; break;
			case 9: s = "badString expected"; break;
			case 10: s = "integer_ expected"; break;
			case 11: s = "double_ expected"; break;
			case 12: s = "datetime_ expected"; break;
			case 13: s = "\"true\" expected"; break;
			case 14: s = "\"false\" expected"; break;
			case 15: s = "\"null\" expected"; break;
			case 16: s = "??? expected"; break;
			case 17: s = "invalid value"; break;

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