using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LiveScriptPatcher {
	static Dictionary<string, string> mapper_init() {
		var never = "live_never";
		var mapper = new Dictionary<string, string> {
			{ "argument", "global.__live_argument" },
			{ "argument_count", "global.__live_argument_count" },
			{ "return", "for ({}; true; exit) live_result =" },
			// stub out live calls to avoid recursion in script-events
			{ "live_call", never },
			{ "live_call_ext", never },
		};
		for (var i = 0; i < 16; i += 1) {
			mapper[$"argument{i}"] = $"global.__live_argument[{i}]";
		}
		return mapper;
	}
	static Dictionary<string, string> mapper = mapper_init();
	public static string run(string code) {
		var q = new LiveStringReader(code);
		var result = new StringBuilder();
		var start = 0;
		while (q.loop) {
			var c = q.peek();
			switch (c) {
				case '#':
					if (q.isDefine()) {
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
		var resCode = result.ToString();
		if (resCode.Contains("\n") && !resCode.Contains("\r")) {
			resCode = resCode.Replace("\n", "\r\n");
		}
		return resCode;
	}
}