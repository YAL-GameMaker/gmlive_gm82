/// ()
if (!global.__live_enable) exit;
var n; n = external_call(global.__live_update);
repeat (n) {
    var _name; _name = external_call(global.__live_next_name);
    //console_write("Updating " + _name + vk_crlf)
    var _kind; _kind = external_call(global.__live_next_kind);
    var _code; _code = external_call(global.__live_next_code);
    switch (_kind) {
        case 0: // script
            if (!ds_map_exists(global.__live_script_ids, _name)) continue;
            var _script; _script = ds_map_find_value(global.__live_script_ids, _name);
            //trace("scr", _name, _script, _code);
            ds_grid_set(global.__live_script_has_event, _script, 0, true);
            object_event_clear(global.__live_script_object, ev_alarm, _script);
            object_event_add(global.__live_script_object, ev_alarm, _script, _code);
            break;
        case 1: // event
            if (!ds_map_exists(global.__live_object_ids, _name)) continue;
            var _object; _object = ds_map_find_value(global.__live_object_ids, _name);
            var _event_type; _event_type = external_call(global.__live_next_event_type);
            var _event_number;
            if (_event_type != ev_collision) {
                _event_number = external_call(global.__live_next_event_numb);
            } else {
                var _event_object; _event_object = external_call(global.__live_next_event_object);
                if (!ds_map_exists(global.__live_object_ids, _event_object)) continue;
                _event_number = ds_map_find_value(global.__live_object_ids, _event_object);
            }
            /*console_write("Updating " + _name
                + " event " + string(_event_type)
                + " numb " + string(_event_number)
                + " to <" + _code + ">"
                + vk_crlf
            );*/
            object_event_clear(_object, _event_type, _event_number);
            if (_code != "") object_event_add(_object, _event_type, _event_number, _code);
            break;
        case 2: // moment
            if (!ds_map_exists(global.__live_timeline_ids, _name)) continue;
            var _timeline; _timeline = ds_map_find_value(global.__live_timeline_ids, _name);
            var _moment; _moment = external_call(global.__live_next_event_numb);
            timeline_moment_clear(_timeline, _moment);
            if (_code != "") timeline_moment_add(_timeline, _moment, _code);
            //trace("tl", _name, _moment, "<gml>" + _code + "</gml>");
            break;
    }
}
