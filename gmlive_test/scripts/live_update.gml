var n; n = external_call(global.__live_update);
repeat (n) {
    var resName; resName = external_call(global.__live_next_name);
    //console_write("Updating " + resName + vk_crlf)
    var kind; kind = external_call(global.__live_next_kind);
    var code; code = external_call(global.__live_next_code);
    switch (kind) {
        case 0: // script
            if (!ds_map_exists(global.__live_script_ids, resName)) continue;
            var _script; _script = ds_map_find_value(global.__live_script_ids, resName);
            trace("scr", resName, _script);
            ds_grid_set(global.__live_script_has_event, _script, 0, true);
            object_event_clear(global.__live_script_object, ev_alarm, _script);
            object_event_add(global.__live_script_object, ev_alarm, _script, code);
            break;
        case 1: case 2: // events
            if (!ds_map_exists(global.__live_object_ids, resName)) continue;
            var _object; _object = ds_map_find_value(global.__live_object_ids, resName);
            var _event_type; _event_type = external_call(global.__live_next_event_type);
            var _event_number;
            if (kind == 1) {
                _event_number = external_call(global.__live_next_event_numb);
            } else {
                var _event_object; _event_object = external_call(global.__live_next_event_object);
                if (!ds_map_exists(global.__live_object_ids, _event_object)) continue;
                _event_number = ds_map_find_value(global.__live_object_ids, _event_object);
            }
            /*console_write("Updating " + resName
                + " event " + string(_event_type)
                + " numb " + string(_event_number)
                + " to <" + code + ">"
                + vk_crlf
            );*/
            object_event_clear(_object, _event_type, _event_number);
            object_event_add(_object, _event_type, _event_number, code);
            break;
    }
}
