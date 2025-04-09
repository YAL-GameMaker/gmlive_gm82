# GMLive for GM8.2

**Quick links:** [documentation](https://yal-gamemaker.github.io/gmlive_gm82/)

This is like [GMLive.gml](https://yellowafterlife.itch.io/gamemaker-live),
but for GameMaker 8.2.

The general situation is as following:

- To exist, GMLive needs a way to dynamically load code and a way to watch the project contents for changes in code.
- GameMaker: Studio and newer GM versions have a directory-based project structure, but no way to dynamically load code.\
	To make GMLive.gml work, I re-implement the entirety of GameMaker Language in Haxe and compile it to GML.
- GameMaker 8.1 and older GM versions can dynamically load code, but all of the project resources are packed into a single file in proprietary format.\
	This makes it harder to quickly pull individual resources out of the file and harder to tell which resources have changed without going over all of them.
- GameMaker 8.2, a community mod for GM8.1, offers an option of a directory-based project format.\
	This makes it relatively easy to watch resources for changes and relatively easy to dynamically load code.

## How does this work

The DLL uses `FileSystemWatcher` to watch the project directory for changes and does all of the heavy lifting.

A game-side script fetches lists of changes to apply.

For object events, it uses `object_event_clear` and `object_event_add`.

For timeline moments, it uses `timeline_moment_clear` and `timeline_moment_add`.

There's no `script_add`/etc. so live-reloading a script adds an event to an
empty object and `live_call` executes it using `event_perform_object`.
[Some patching](./gmlive82/LiveScriptPatcher.cs) is involved to make `argument` and `return` work.

## Meta

Code by YellowAfterlife.

MIT license?