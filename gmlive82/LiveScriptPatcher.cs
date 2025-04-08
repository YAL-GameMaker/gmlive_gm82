using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LiveScriptPatcher {
	static Dictionary<string, string> mapper = new Dictionary<string, string> {
		{ "argument", "live_argument" },
		{ "argument_count", "live_argument_count" },
		{ "argument0", "live_argument[0]" },
		{ "argument1", "live_argument[1]" },
		{ "argument2", "live_argument[2]" },
		{ "argument3", "live_argument[3]" },
		{ "live_call", "live_void" },
	};
	public static string run(string code) {
		var q = new LiveStringReader(code);
		var result = new StringBuilder();
		var start = 0;
		while (q.loop) {
			var c = q.peek();
			switch (c) {
				case '#':
					if (q.isDefine()) {
						var till = q.pos;
						q.skipDefine();
						q.skipLine();
						continue;
					} else q.skip();
					break;
				case '/':
					q.skip();
					var coKind = q.read();
					if (coKind == '/') {
						q.skipLine();
					} else if (coKind == '*') {
						q.skipCommentBlock();
					}
					break;
				case '"':
				case '\'':
					q.skip();
					q.skipString(c);
					break;
				default:
					if (c == '_'
						|| c >= 'a' && c <= 'z'
						|| c >= 'A' && c <= 'Z'
					) {
						var till = q.pos;
						var id = q.readIdent();
						if (mapper.TryGetValue(id, out var next)) {
							result.Append(q.slice(start, till));
							result.Append(next);
							start = q.pos;
						}
					} else {
						q.skip();
					}
					break;
			} // switch, can continue
		} // reader loop
		if (start < q.pos) {
			result.Append(q.slice(start, q.pos));
		}
		return result.ToString();
	}
}