/// scr_sum(...numbers)
live_argument_count = argument_count;
var a; for (a = 0; a < argument_count; a += 1) live_argument[a] = argument[a];
if (live_call_ext(scr_sum)) return live_result;

var r, i;
r = 0;
for (i = 0; i < argument_count; i += 1) r += argument[i];
return r;
