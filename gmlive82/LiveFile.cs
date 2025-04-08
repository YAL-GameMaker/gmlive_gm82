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
			var parts = LiveFileSection.split(code);
			foreach (var part in parts) {
				if (eventMap.TryGetValue(part.name, out var objEvent)) {
					if (objEvent.code == part.code) continue;
					objEvent.code = part.code;
				} else {
					eventMap[part.name] = LiveFileEvent.fromSection(part);
				}
				//Console.WriteLine($"[live] Updating {name}:{objEvent.name}");
				if (objEvent.eventType == 11) continue; // no triggers
				queue.Enqueue(objEvent.createDelta(name));
			}
		}
	}
}
