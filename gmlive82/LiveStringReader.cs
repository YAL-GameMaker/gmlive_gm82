using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LiveStringReader {
	public string str;
	public int pos = 0;
	public int length;
	public bool loop {
		get { return pos < length; }
	}
	//
	public LiveStringReader(string str) {
		this.str = str;
		length = str.Length;
	}
	//
	public char read() {
		return pos < length ? str[pos++] : default(char);
	}
	public char peek() {
		return pos < length ? str[pos] : default(char);
	}
	public void skip(int n = 1) {
		pos += n;
	}
	//
	public void skipWhile(Func<char, bool> fn) {
		while (loop) {
			var c = peek();
			if (fn.Invoke((char)c)) pos += 1; else break;
		}
	}
	public void skipSpaces() {
		while (loop) {
			var c = str[pos];
			if (char.IsWhiteSpace((char)c)) pos += 1; else break;
		}
	}
	public string readIdent(bool allowNumbers = false) {
		if (pos >= length) return null;
		var c = peek();
		if (!(c == '_'
			|| c >= 'a' && c <= 'z'
			|| c >= 'A' && c <= 'Z'
			|| (allowNumbers && c >= '0' && c <= '9')
		)) return null;
		var start = pos++;
		while (loop) {
			c = str[pos];
			if (c == '_'
				|| c >= '0' && c <= '9'
				|| c >= 'a' && c <= 'z'
				|| c >= 'A' && c <= 'Z'
			) {
				pos += 1;
			} else break;
		}
		return slice(start, pos);
	}
	//
	public string substr(int start, int count) {
		return str.Substring(start, count);
	}
	public string slice(int start, int end) {
		return str.Substring(start, end - start);
	}
	//
	public bool skipIfEqu(char c) {
		if (pos >= length) return false;
		if (str[pos] != c) return false;
		pos += 1;
		return true;
	}
	public bool skipIfEqu(string s) {
		var n = s.Length;
		if (pos + n > length) return false;
		for (int i = 0; i < n; i++) {
			if (str[pos + i] != s[i]) return false;
		}
		pos += n;
		return true;
	}
	//
	public const string defineSnip = "#define";
	public bool isDefine() {
		if (pos >= length || str[pos] != '#') return false;
		var s = defineSnip;
		var n = s.Length;
		if (pos + n + 1 > length) return false;
		if (pos > 0 && str[pos - 1] != '\n') return false;
		for (int i = 0; i < n; i++) {
			if (str[pos + i] != s[i]) return false;
		}
		return char.IsWhiteSpace(str[pos + n]);
	}
	public void skipDefine() {
		pos += defineSnip.Length;
	}
	//
	public void skipLine() {
		while (loop) {
			var c = str[pos++];
			if (c == '\n') break;
		}
	}
	public void skipCommentBlock() {
		var prev = '*';
		while (loop) {
			if (isDefine()) break;
			var c = str[pos++];
			if (prev == '*' && c == '/') break;
			prev = c;
		}
	}
	public void skipString(char quote) {
		while (loop) {
			if (isDefine()) break;
			var c = str[pos++];
			if (c == quote) break;
		}
	}
}
