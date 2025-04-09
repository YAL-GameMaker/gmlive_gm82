using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LiveDelta {
	public string resourceName;
	public LiveDeltaKind kind;
	public int eventType;
	public int eventNumb;
	public string eventObject;
	public string code;
	public LiveDelta(string resourceName, string code, LiveDeltaKind kind) {
		this.resourceName = resourceName;
		this.code = code;
		this.kind = kind;
	}
	override public string ToString() {
		switch (kind) {
			case LiveDeltaKind.Script:
				return $"script:{resourceName}";
			case LiveDeltaKind.Event:
				var eventStr = LiveFileEvent.eventTypes[eventType];
				var eventArg = eventObject ?? ("" + eventNumb);
				return $"event:{resourceName}(type: {eventStr}, numb: {eventArg})";
			case LiveDeltaKind.Moment:
				return $"moment:{resourceName}(time: {eventNumb})";
			default:
				return $"delta:{kind}";
		}
	}
}
public enum LiveDeltaKind {
	Script = 0,
	Event = 1,
	Moment = 2,
}