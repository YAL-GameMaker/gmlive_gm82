using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class LiveEventPatcher {
	static Regex rxAction = new Regex("/\\*\""
		+ "/\\*'"
		+ "/\\*\\*/"
		+ "/\\* YYD ACTION"
		+ "[\\s\\S]+?"
		+ "\\*/"
		+ "\r?\n", RegexOptions.Multiline);
	public static string run(string code) {
		//Console.WriteLine($"<gml>{code}</gml>");
		code = rxAction.Replace(code, string.Empty);
		//Console.WriteLine($"<gml>{code}</gml>");
		if (code.Contains("\n") && !code.Contains("\r")) {
			code = code.Replace("\n", "\r\n");
		}
		return code;
	}
}