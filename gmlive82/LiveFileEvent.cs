using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LiveFileEvent {
	public string name;
	public string code;
	//
	public int eventType;
	public int eventNumb;
	public string eventObject = null;
	public static List<string> eventTypes = new List<string> {
		"Create",
		"Destroy",
		"Alarm",
		"Step",
		"Collision", // 4
		"Keyboard",
		"Mouse",
		"Other",
		"Draw",
		"KeyPress",
		"KeyRelease",
		"Trigger", // 11
	};
	//
	public LiveFileEvent(string name, string code) {
		this.name = name;
		this.code = code;
		var at = name.IndexOf("_");
		var type = name.Substring(0, at);
		var numb = name.Substring(at + 1);
		eventType = eventTypes.IndexOf(type);
		if (eventType == 4 || eventType == 11) {
			eventObject = numb;
		} else {
			eventNumb = int.Parse(numb);
		}
	}
	public static LiveFileEvent fromSection(LiveFileSection section) {
		return new LiveFileEvent(section.name, section.code);
	}
	//
	public LiveDelta createDelta(string objName) {
		var kind = eventType == 4 ? LiveDeltaKind.CollisionEvent : LiveDeltaKind.Event;
		var d = new LiveDelta(objName, code, kind);
		d.eventType = eventType;
		d.eventNumb = eventNumb;
		d.eventObject = eventObject;
		return d;
	}
}