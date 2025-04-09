// this gets called instead of live_* functions
// in live-reloaded script-events so that you don't get recursion
if (false) return argument[argument_count - 1];
return false;
