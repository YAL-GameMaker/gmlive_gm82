using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LiveFileSection {
	public string name;
	public string code;
	public LiveFileSection(string name, string code) {
		this.name = name;
		this.code = code;
	}
	public static List<LiveFileSection> split(string code) {
		var q = new LiveStringReader(code);
		var result = new List<LiveFileSection>();
		var start = 0;
		string sectionName = null;
		while (q.loop) {
			var c = q.peek();
			switch (c) {
				case '#':
					if (q.isDefine()) {
						var till = q.pos;
						q.skipDefine();
						q.skipSpaces();
						var name = q.readIdent(true);
						if (name == null) continue;
						if (sectionName != null) {
							var snip = q.slice(start, till);
							var section = new LiveFileSection(sectionName, snip);
							result.Add(section);
						}
						sectionName = name;
						q.skipLine();
						start = q.pos;
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
					q.skip();
					break;
			} // switch, can continue
		} // reader loop

		if (start < q.pos && sectionName != null) {
			var snip = q.slice(start, q.pos);
			var section = new LiveFileSection(sectionName, snip);
			result.Add(section);
		}
		return result;
	}
	public override string ToString() {
		return $"LiveFileSection(\"{name}\", <gml>{code}</gml>)";
	}
}