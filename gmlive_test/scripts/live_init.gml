/// (dllPath = "GMLive.dll", ?project_directory)
var _dll; if (argument_count > 0) _dll = argument[0]; else _dll = "GMLive.dll";
var _project_directory; if (argument_count > 1) _project_directory = argument[1]; else _project_directory = working_directory;
if (_dll != "" && file_exists(_dll)) {
    global.__live_enable = true;
    var _ct; _ct = dll_cdecl;
    var _init; _init = external_define(_dll, "init", _ct, ty_real, 1, ty_string);
    external_call(_init, _project_directory);
    
    // functions:
    global.__live_update = external_define(_dll, "update", _ct, ty_real, 0);
    global.__live_next_name = external_define(_dll, "nextName", _ct, ty_string, 0);
    global.__live_next_kind = external_define(_dll, "nextKind", _ct, ty_real, 0);
    global.__live_next_event_type = external_define(_dll, "nextEventType", _ct, ty_real, 0);
    global.__live_next_event_numb = external_define(_dll, "nextEventNumb", _ct, ty_real, 0);
    global.__live_next_event_object = external_define(_dll, "nextEventObject", _ct, ty_string, 0);
    global.__live_next_code = external_define(_dll, "nextCode", _ct, ty_string, 0);
} else {
    global.__live_enable = false;
}

// objects:
var lastObject; lastObject = object_add();
global.__live_object_ids = ds_map_create();
for ({var i; i = 0}; i < lastObject; i += 1) {
    ds_map_add(global.__live_object_ids, object_get_name(i), i);
}
global.__live_script_object = lastObject;

// scripts:
global.__live_script_ids = ds_map_create();
var lastScript; lastScript = -1;
for ({var i, gap; i = 0; gap = 0}; gap < 1000; i += 1) {
    if (script_exists(i)) {
        lastScript = i;
        gap = 0;
        ds_map_add(global.__live_script_ids, script_get_name(i), i);
    } else gap += 1;
}
global.__live_script_has_event = ds_grid_create(lastScript + 1, 1);

// timelines:
var _last_tl; _last_tl = timeline_add();
global.__live_timeline_ids = ds_map_create();
for ({var i; i = 0}; i < _last_tl; i += 1) {
    ds_map_add(global.__live_timeline_ids, timeline_get_name(i), i);
}
timeline_delete(_last_tl);

// for live_call_ext:
globalvar live_argument; live_argument[15] = 0;
globalvar live_argument_count; live_argument_count = 0;

// for internal use:
global.__live_argument[15] = 0;
global.__live_argument_count = 0;

// both:
globalvar live_result; live_result = 0;