using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GMLive {
    public static FileSystemWatcher watcher = null;
    public static string projectDirectory = null;
    public static string projectDirectoryPrefix;
    public static string getRelPath(string path) {
        if (path.StartsWith(projectDirectoryPrefix)) {
            return path.Substring(projectDirectoryPrefix.Length);
        } else return null;
    }

	// what we know and care about
	public static Dictionary<string, LiveFile> fileMap = new Dictionary<string, LiveFile>();

	// change events can dispatch twice so we don't act on them immediately
	public static Dictionary<string, LiveFile> checkMap = new Dictionary<string, LiveFile>();
    public static List<LiveFile> checkList = new List<LiveFile>();

    //
    public static Queue<LiveDelta> deltas = new Queue<LiveDelta>();

    static void indexDirectory(string directory, LiveDeltaKind kind) {
        if (!Directory.Exists(directory)) return;
        foreach (var path in Directory.GetFiles(directory, "*.gml")) {
            var rel = getRelPath(path);
            fileMap[rel] = new LiveFile(rel, path, kind);
		}
	}

    [DllExport]
    public static double init(string path) {
        if (watcher != null) { // already watching?
            if (projectDirectory == path) return 1; // OK!
            watcher.Dispose();
        }
        try {
            projectDirectory = path;
            projectDirectoryPrefix = path + "\\";
            indexDirectory(Path.Combine(path, "scripts"), LiveDeltaKind.Script);
            indexDirectory(Path.Combine(path, "objects"), LiveDeltaKind.Event);
            indexDirectory(Path.Combine(path, "timelines"), LiveDeltaKind.Moment);
            watcher = new FileSystemWatcher(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += fileChanged;
			watcher.Error += watchError;
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.gml";
            watcher.EnableRaisingEvents = true;
            Console.WriteLine($"[live] Watching \"{projectDirectory}\"");
            return 1;
        } catch (Exception e) {
            Console.WriteLine("Error!" + e);
            return 0;
		}
    }

    [DllExport]
    public static double update() {
        deltas.Clear();
        foreach (var file in checkList) {
            file.update(deltas);
		}
        foreach (var delta in deltas) {
            Console.WriteLine($"[live] Updating {delta}");
        }
        return deltas.Count;
	}

    static LiveDelta nextDelta;
    [DllExport]
    public static string nextName() {
        if (deltas.Count == 0) return "";
        nextDelta = deltas.Dequeue();
        return nextDelta.resourceName;
	}
    [DllExport]
    public static double nextKind() {
        return (int)nextDelta.kind;
	}
    [DllExport]
    public static double nextEventType() {
        return nextDelta.eventType;
	}
    [DllExport]
    public static double nextEventNumb() {
        return nextDelta.eventNumb;
    }
    [DllExport]
    public static string nextEventObject() {
        return nextDelta.eventObject ?? "";
    }
    [DllExport]
    public static string nextCode() {
        return nextDelta.code;
    }

    private static void watchError(object sender, ErrorEventArgs e) {
        Console.WriteLine("Watch error: " + e);
	}

	private static void fileChanged(object sender, FileSystemEventArgs e) {
        var fullPath = e.FullPath;
        var relPath = getRelPath(fullPath);
        if (relPath == null) return;
        if (checkMap.ContainsKey(relPath)) return;
        if (!fileMap.TryGetValue(relPath, out var file)) return;
        checkMap[relPath] = file;
        checkList.Add(file);
	}
}
