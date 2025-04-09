using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LiveFileMoment {
	public string name;
	public string code;
	public bool exists = true;
	//
	public int time = 0;
	//
	public LiveFileMoment(string name, string code) {
		this.name = name;
		this.code = code;
		int.TryParse(name, out time);
	}
	public static LiveFileMoment fromSection(LiveFileSection section) {
		return new LiveFileMoment(section.name, section.code);
	}
	//
	public LiveDelta createDelta(string tlName) {
		var deltaCode = LiveEventPatcher.run(code);
		var d = new LiveDelta(tlName, deltaCode, LiveDeltaKind.Moment);
		d.eventNumb = time;
		return d;
	}
}