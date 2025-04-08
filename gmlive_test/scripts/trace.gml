/// (...)
for ({var _argi; _argi = 0}; _argi < argument_count; _argi += 1) {
    if (_argi > 0) console_write(" ");
    console_write(string(argument[_argi]));
}
console_write(vk_crlf);
