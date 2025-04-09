using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LiveFile {
	public string fullPath;
	public string relPath;
	public string name;
	public string code;
	public LiveDeltaKind kind;
	public Dictionary<string, LiveFileEvent> eventMap = null;
	public Dictionary<string, LiveFileMoment> momentMap = null;
	public LiveFile(string relPath, string fullPath, LiveDeltaKind kind) {
		this.relPath = relPath;
		this.fullPath = fullPath;
		this.kind = kind;
		//
		name = Path.GetFileNameWithoutExtension(fullPath);
		code = File.ReadAllText(fullPath);
		//
		if (kind == LiveDeltaKind.Event) {
			eventMap = new Dictionary<string, LiveFileEvent>();
			var parts = LiveFileSection.split(code);
			foreach (var part in parts) {
				eventMap[part.name] = LiveFileEvent.fromSection(part);
			}
		} else if (kind == LiveDeltaKind.Moment) {
			momentMap = new Dictionary<string, LiveFileMoment>();
			var parts = LiveFileSection.split(code);
			foreach (var part in parts) {
				momentMap[part.name] = LiveFileMoment.fromSection(part);
			}
		}
	}
	public void update(Queue<LiveDelta> queue) {
		var code = File.ReadAllText(fullPath);
		if (code == this.code) return;
		this.code = code;
		if (kind == LiveDeltaKind.Script) {
			//Console.WriteLine($"[live] Updating {name}");
			code = LiveScriptPatcher.run(code);
			queue.Enqueue(new LiveDelta(name, code, LiveDeltaKind.Script));
		} else if (kind == LiveDeltaKind.Event) {
			foreach (var @event in eventMap.Values) @event.exists = false;

			var parts = LiveFileSection.split(code);
			foreach (var part in parts) {
				if (eventMap.TryGetValue(part.name, out var @event)) {
					@event.exists = true;
					if (@event.code == part.code) continue;
					@event.code = part.code;
				} else {
					eventMap[part.name] = @event = LiveFileEvent.fromSection(part);
				}
				@event.exists = true;
				//Console.WriteLine($"[live] Updating {name}:{objEvent.name}");
				if (@event.eventType == 11) continue; // no triggers
				queue.Enqueue(@event.createDelta(name));
			}

			foreach (var @event in eventMap.Values) {
				if (!@event.exists) {
					@event.code = "";
					queue.Enqueue(@event.createDelta(name));
				}
			}
		} else if (kind == LiveDeltaKind.Moment) {
			foreach (var moment in momentMap.Values) moment.exists = false;

			var parts = LiveFileSection.split(code);
			foreach (var part in parts) {
				//Console.WriteLine(part);
				if (momentMap.TryGetValue(part.name, out var moment)) {
					moment.exists = true;
					if (moment.code == part.code) continue;
					moment.code = part.code;
				} else {
					momentMap[part.name] = moment = LiveFileMoment.fromSection(part);
				}
				moment.exists = true;
				queue.Enqueue(moment.createDelta(name));
			}

			foreach (var moment in momentMap.Values) {
				//Console.WriteLine($"{moment.name}: {moment.exists}");
				if (!moment.exists) {
					moment.code = "";
					queue.Enqueue(moment.createDelta(name));
				}
			}
		}
	}
}
