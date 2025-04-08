#define Create_0
/*"/*'/**//* YYD ACTION
lib_id=1
action_id=603
applies_to=self
*/
if (!variable_global_exists("has_console")) {
    console_open('""GMLive""');
    global.has_console = true;
}
live_init();
#define Step_0
/*"/*'/**//* YYD ACTION
lib_id=1
action_id=603
applies_to=self
*/
live_update();
//!!!
#define Draw_0
/*"/*'/**//* YYD ACTION
lib_id=1
action_id=603
applies_to=self
*/
draw_set_color(c_white);
draw_set_font(fnt_test);
draw_text(5, 5, "and good night");
