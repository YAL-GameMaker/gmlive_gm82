#define Create_0
/*"/*'/**//* YYD ACTION
lib_id=1
action_id=603
applies_to=self
*/
// prevent the console from crashing the game if this object is made twice:
if (!variable_global_exists("has_console")) {
    console_open('gmlive_test');
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

if (error_occurred) {
    console_write(error_last + vk_crlf);
    error_last = "";
    error_occurred = false;
}
#define Draw_0
/*"/*'/**//* YYD ACTION
lib_id=1
action_id=603
applies_to=self
*/
draw_set_color(c_white);
draw_set_font(fnt_test);
//var i = scr_test("You");
draw_text(5, 5, "Test: " + string(scr_sum(1, 2, 3)));
//2
