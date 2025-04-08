/// (script, ...args)
var _script; _script = argument[0];
if (!ds_grid_get(global.__live_script_has_event, _script, 0)) return false;

// store args
var prev_argument_count; prev_argument_count = live_argument_count;
live_argument_count = argument_count - 1;
var prev_argument;
var i; i = 0;
repeat (argument_count - 1) {
    prev_argument[i] = live_argument[i];
    live_argument[i] = argument[i + 1];
    i += 1;
}

//
event_perform_object(global.__live_script_object, ev_alarm, _script);

// restore args
live_argument_count = prev_argument_count;
i = 0;
repeat (prev_argument_count) {
    live_argument[i] = prev_argument[i];
    i += 1;
}
return true;
