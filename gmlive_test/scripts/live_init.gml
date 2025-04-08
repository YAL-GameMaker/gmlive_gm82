var dll; dll = "gmlive.dll";
var ct; ct = dll_cdecl;
var init; init = external_define(dll, "init", ct, ty_real, 1, ty_string);
external_call(init, working_directory);
//
global.__live_update = external_define(dll, "update", ct, ty_real, 0);
global.__live_next_name = external_define(dll, "nextName", ct, ty_string, 0);
global.__live_next_kind = external_define(dll, "nextKind", ct, ty_real, 0);
global.__live_next_event_type = external_define(dll, "nextEventType", ct, ty_real, 0);
global.__live_next_event_numb = external_define(dll, "nextEventNumb", ct, ty_real, 0);
global.__live_next_event_object = external_define(dll, "nextEventObject", ct, ty_string, 0);
global.__live_next_code = external_define(dll, "nextCode", ct, ty_string, 0);
//
var lastObject; lastObject = object_add();
global.__live_script_object = lastObject;
global.__live_object_ids = ds_map_create();
for ({var i; i = 0}; i < lastObject; i += 1) {
    ds_map_add(global.__live_object_ids, object_get_name(i), i);
}
//
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
//
globalvar live_argument; live_argument[15] = 0;
globalvar live_argument_count; live_argument_count = 0;
