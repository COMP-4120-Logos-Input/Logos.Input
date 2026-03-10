namespace Logos.Input.Sdl3;

/*
  Simple DirectMedia Layer
  Copyright (C) 1997-2026 Sam Lantinga <slouken@libsdl.org>

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.
*/

using SDL_Keycode = System.UInt32;
using SDL_Keymod = System.UInt16;

public static class SDL_Keycodes
{
    public const uint SDLK_EXTENDED_MASK = (1u << 29);
    public const uint SDLK_SCANCODE_MASK = (1u << 30);

    public static uint SDL_SCANCODE_TO_KEYCODE(uint X)
    {
        return (X | SDLK_SCANCODE_MASK);
    }

    public const uint SDLK_UNKNOWN = 0x00000000u;
    public const uint SDLK_RETURN = 0x0000000Du;
    public const uint SDLK_ESCAPE = 0x0000001Bu;
    public const uint SDLK_BACKSPACE = 0x00000008u;
    public const uint SDLK_TAB = 0x00000009u;
    public const uint SDLK_SPACE = 0x00000020u;

    public const uint SDLK_EXCLAIM = 0x00000021u /**< '!' */;
    public const uint SDLK_DBLAPOSTROPHE = 0x00000022u /**< '"' */;
    public const uint SDLK_HASH = 0x00000023u /**< '#' */;
    public const uint SDLK_DOLLAR = 0x00000024u /**< '$' */;
    public const uint SDLK_PERCENT = 0x00000025u /**< '%' */;
    public const uint SDLK_AMPERSAND = 0x00000026u /**< '&' */;
    public const uint SDLK_APOSTROPHE = 0x00000027u /**< '\'' */;
    public const uint SDLK_LEFTPAREN = 0x00000028u /**< '(' */;
    public const uint SDLK_RIGHTPAREN = 0x00000029u /**< ')' */;
    public const uint SDLK_ASTERISK = 0x0000002au /**< '*' */;
    public const uint SDLK_PLUS = 0x0000002bu /**< '+' */;
    public const uint SDLK_COMMA = 0x0000002cu /**< ',' */;
    public const uint SDLK_MINUS = 0x0000002du /**< '-' */;
    public const uint SDLK_PERIOD = 0x0000002eu /**< '.' */;
    public const uint SDLK_SLASH = 0x0000002fu /**< '/' */;
    public const uint SDLK_0 = 0x00000030u /**< '0' */;
    public const uint SDLK_1 = 0x00000031u /**< '1' */;
    public const uint SDLK_2 = 0x00000032u /**< '2' */;
    public const uint SDLK_3 = 0x00000033u /**< '3' */;
    public const uint SDLK_4 = 0x00000034u /**< '4' */;
    public const uint SDLK_5 = 0x00000035u /**< '5' */;
    public const uint SDLK_6 = 0x00000036u /**< '6' */;
    public const uint SDLK_7 = 0x00000037u /**< '7' */;
    public const uint SDLK_8 = 0x00000038u /**< '8' */;
    public const uint SDLK_9 = 0x00000039u /**< '9' */;
    public const uint SDLK_COLON = 0x0000003au /**< ':' */;
    public const uint SDLK_SEMICOLON = 0x0000003bu /**< ';' */;
    public const uint SDLK_LESS = 0x0000003cu /**< '<' */;
    public const uint SDLK_EQUALS = 0x0000003du /**< '=' */;
    public const uint SDLK_GREATER = 0x0000003eu /**< '>' */;
    public const uint SDLK_QUESTION = 0x0000003fu /**< '?' */;
    public const uint SDLK_AT = 0x00000040u /**< '@' */;
    public const uint SDLK_LEFTBRACKET = 0x0000005bu /**< '[' */;
    public const uint SDLK_BACKSLASH = 0x0000005cu /**< '\\' */;
    public const uint SDLK_RIGHTBRACKET = 0x0000005du /**< ']' */;
    public const uint SDLK_CARET = 0x0000005eu /**< '^' */;
    public const uint SDLK_UNDERSCORE = 0x0000005fu /**< '_' */;
    public const uint SDLK_GRAVE = 0x00000060u /**< '`' */;
    public const uint SDLK_A = 0x00000061u /**< 'a' */;
    public const uint SDLK_B = 0x00000062u /**< 'b' */;
    public const uint SDLK_C = 0x00000063u /**< 'c' */;
    public const uint SDLK_D = 0x00000064u /**< 'd' */;
    public const uint SDLK_E = 0x00000065u /**< 'e' */;
    public const uint SDLK_F = 0x00000066u /**< 'f' */;
    public const uint SDLK_G = 0x00000067u /**< 'g' */;
    public const uint SDLK_H = 0x00000068u /**< 'h' */;
    public const uint SDLK_I = 0x00000069u /**< 'i' */;
    public const uint SDLK_J = 0x0000006au /**< 'j' */;
    public const uint SDLK_K = 0x0000006bu /**< 'k' */;
    public const uint SDLK_L = 0x0000006cu /**< 'l' */;
    public const uint SDLK_M = 0x0000006du /**< 'm' */;
    public const uint SDLK_N = 0x0000006eu /**< 'n' */;
    public const uint SDLK_O = 0x0000006fu /**< 'o' */;
    public const uint SDLK_P = 0x00000070u /**< 'p' */;
    public const uint SDLK_Q = 0x00000071u /**< 'q' */;
    public const uint SDLK_R = 0x00000072u /**< 'r' */;
    public const uint SDLK_S = 0x00000073u /**< 's' */;
    public const uint SDLK_T = 0x00000074u /**< 't' */;
    public const uint SDLK_U = 0x00000075u /**< 'u' */;
    public const uint SDLK_V = 0x00000076u /**< 'v' */;
    public const uint SDLK_W = 0x00000077u /**< 'w' */;
    public const uint SDLK_X = 0x00000078u /**< 'x' */;
    public const uint SDLK_Y = 0x00000079u /**< 'y' */;
    public const uint SDLK_Z = 0x0000007au /**< 'z' */;
    public const uint SDLK_LEFTBRACE = 0x0000007bu /**< '{' */;
    public const uint SDLK_PIPE = 0x0000007cu /**< '|' */;
    public const uint SDLK_RIGHTBRACE = 0x0000007du /**< '}' */;
    public const uint SDLK_TILDE = 0x0000007eu /**< '~' */;
    public const uint SDLK_DELETE = 0x0000007fu /**< '\x7F' */;
    public const uint SDLK_PLUSMINUS = 0x000000b1u /**< '\xB1' */;
    public const uint SDLK_CAPSLOCK = 0x40000039u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CAPSLOCK) */;
    public const uint SDLK_F1 = 0x4000003au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F1) */;
    public const uint SDLK_F2 = 0x4000003bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F2) */;
    public const uint SDLK_F3 = 0x4000003cu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F3) */;
    public const uint SDLK_F4 = 0x4000003du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F4) */;
    public const uint SDLK_F5 = 0x4000003eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F5) */;
    public const uint SDLK_F6 = 0x4000003fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F6) */;
    public const uint SDLK_F7 = 0x40000040u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F7) */;
    public const uint SDLK_F8 = 0x40000041u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F8) */;
    public const uint SDLK_F9 = 0x40000042u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F9) */;
    public const uint SDLK_F10 = 0x40000043u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F10) */;
    public const uint SDLK_F11 = 0x40000044u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F11) */;
    public const uint SDLK_F12 = 0x40000045u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F12) */;
    public const uint SDLK_PRINTSCREEN = 0x40000046u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_PRINTSCREEN) */;
    public const uint SDLK_SCROLLLOCK = 0x40000047u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_SCROLLLOCK) */;
    public const uint SDLK_PAUSE = 0x40000048u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_PAUSE) */;
    public const uint SDLK_INSERT = 0x40000049u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_INSERT) */;
    public const uint SDLK_HOME = 0x4000004au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_HOME) */;
    public const uint SDLK_PAGEUP = 0x4000004bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_PAGEUP) */;
    public const uint SDLK_END = 0x4000004du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_END) */;
    public const uint SDLK_PAGEDOWN = 0x4000004eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_PAGEDOWN) */;
    public const uint SDLK_RIGHT = 0x4000004fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_RIGHT) */;
    public const uint SDLK_LEFT = 0x40000050u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_LEFT) */;
    public const uint SDLK_DOWN = 0x40000051u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_DOWN) */;
    public const uint SDLK_UP = 0x40000052u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_UP) */;
    public const uint SDLK_NUMLOCKCLEAR = 0x40000053u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_NUMLOCKCLEAR) */;
    public const uint SDLK_KP_DIVIDE = 0x40000054u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_DIVIDE) */;
    public const uint SDLK_KP_MULTIPLY = 0x40000055u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MULTIPLY) */;
    public const uint SDLK_KP_MINUS = 0x40000056u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MINUS) */;
    public const uint SDLK_KP_PLUS = 0x40000057u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_PLUS) */;
    public const uint SDLK_KP_ENTER = 0x40000058u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_ENTER) */;
    public const uint SDLK_KP_1 = 0x40000059u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_1) */;
    public const uint SDLK_KP_2 = 0x4000005au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_2) */;
    public const uint SDLK_KP_3 = 0x4000005bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_3) */;
    public const uint SDLK_KP_4 = 0x4000005cu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_4) */;
    public const uint SDLK_KP_5 = 0x4000005du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_5) */;
    public const uint SDLK_KP_6 = 0x4000005eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_6) */;
    public const uint SDLK_KP_7 = 0x4000005fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_7) */;
    public const uint SDLK_KP_8 = 0x40000060u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_8) */;
    public const uint SDLK_KP_9 = 0x40000061u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_9) */;
    public const uint SDLK_KP_0 = 0x40000062u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_0) */;
    public const uint SDLK_KP_PERIOD = 0x40000063u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_PERIOD) */;
    public const uint SDLK_APPLICATION = 0x40000065u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_APPLICATION) */;
    public const uint SDLK_POWER = 0x40000066u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_POWER) */;
    public const uint SDLK_KP_EQUALS = 0x40000067u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_EQUALS) */;
    public const uint SDLK_F13 = 0x40000068u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F13) */;
    public const uint SDLK_F14 = 0x40000069u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F14) */;
    public const uint SDLK_F15 = 0x4000006au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F15) */;
    public const uint SDLK_F16 = 0x4000006bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F16) */;
    public const uint SDLK_F17 = 0x4000006cu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F17) */;
    public const uint SDLK_F18 = 0x4000006du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F18) */;
    public const uint SDLK_F19 = 0x4000006eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F19) */;
    public const uint SDLK_F20 = 0x4000006fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F20) */;
    public const uint SDLK_F21 = 0x40000070u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F21) */;
    public const uint SDLK_F22 = 0x40000071u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F22) */;
    public const uint SDLK_F23 = 0x40000072u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F23) */;
    public const uint SDLK_F24 = 0x40000073u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_F24) */;
    public const uint SDLK_EXECUTE = 0x40000074u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_EXECUTE) */;
    public const uint SDLK_HELP = 0x40000075u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_HELP) */;
    public const uint SDLK_MENU = 0x40000076u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MENU) */;
    public const uint SDLK_SELECT = 0x40000077u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_SELECT) */;
    public const uint SDLK_STOP = 0x40000078u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_STOP) */;
    public const uint SDLK_AGAIN = 0x40000079u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AGAIN) */;
    public const uint SDLK_UNDO = 0x4000007au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_UNDO) */;
    public const uint SDLK_CUT = 0x4000007bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CUT) */;
    public const uint SDLK_COPY = 0x4000007cu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_COPY) */;
    public const uint SDLK_PASTE = 0x4000007du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_PASTE) */;
    public const uint SDLK_FIND = 0x4000007eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_FIND) */;
    public const uint SDLK_MUTE = 0x4000007fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MUTE) */;
    public const uint SDLK_VOLUMEUP = 0x40000080u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_VOLUMEUP) */;
    public const uint SDLK_VOLUMEDOWN = 0x40000081u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_VOLUMEDOWN) */;
    public const uint SDLK_KP_COMMA = 0x40000085u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_COMMA) */;
    public const uint SDLK_KP_EQUALSAS400 = 0x40000086u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_EQUALSAS400) */;
    public const uint SDLK_ALTERASE = 0x40000099u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_ALTERASE) */;
    public const uint SDLK_SYSREQ = 0x4000009au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_SYSREQ) */;
    public const uint SDLK_CANCEL = 0x4000009bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CANCEL) */;
    public const uint SDLK_CLEAR = 0x4000009cu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CLEAR) */;
    public const uint SDLK_PRIOR = 0x4000009du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_PRIOR) */;
    public const uint SDLK_RETURN2 = 0x4000009eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_RETURN2) */;
    public const uint SDLK_SEPARATOR = 0x4000009fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_SEPARATOR) */;
    public const uint SDLK_OUT = 0x400000a0u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_OUT) */;
    public const uint SDLK_OPER = 0x400000a1u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_OPER) */;
    public const uint SDLK_CLEARAGAIN = 0x400000a2u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CLEARAGAIN) */;
    public const uint SDLK_CRSEL = 0x400000a3u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CRSEL) */;
    public const uint SDLK_EXSEL = 0x400000a4u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_EXSEL) */;
    public const uint SDLK_KP_00 = 0x400000b0u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_00) */;
    public const uint SDLK_KP_000 = 0x400000b1u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_000) */;

    public const uint
        SDLK_THOUSANDSSEPARATOR = 0x400000b2u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_THOUSANDSSEPARATOR) */;

    public const uint
        SDLK_DECIMALSEPARATOR = 0x400000b3u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_DECIMALSEPARATOR) */;

    public const uint SDLK_CURRENCYUNIT = 0x400000b4u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CURRENCYUNIT) */;
    public const uint SDLK_CURRENCYSUBUNIT = 0x400000b5u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CURRENCYSUBUNIT) */;
    public const uint SDLK_KP_LEFTPAREN = 0x400000b6u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_LEFTPAREN) */;
    public const uint SDLK_KP_RIGHTPAREN = 0x400000b7u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_RIGHTPAREN) */;
    public const uint SDLK_KP_LEFTBRACE = 0x400000b8u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_LEFTBRACE) */;
    public const uint SDLK_KP_RIGHTBRACE = 0x400000b9u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_RIGHTBRACE) */;
    public const uint SDLK_KP_TAB = 0x400000bau /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_TAB) */;
    public const uint SDLK_KP_BACKSPACE = 0x400000bbu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_BACKSPACE) */;
    public const uint SDLK_KP_A = 0x400000bcu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_A) */;
    public const uint SDLK_KP_B = 0x400000bdu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_B) */;
    public const uint SDLK_KP_C = 0x400000beu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_C) */;
    public const uint SDLK_KP_D = 0x400000bfu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_D) */;
    public const uint SDLK_KP_E = 0x400000c0u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_E) */;
    public const uint SDLK_KP_F = 0x400000c1u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_F) */;
    public const uint SDLK_KP_XOR = 0x400000c2u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_XOR) */;
    public const uint SDLK_KP_POWER = 0x400000c3u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_POWER) */;
    public const uint SDLK_KP_PERCENT = 0x400000c4u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_PERCENT) */;
    public const uint SDLK_KP_LESS = 0x400000c5u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_LESS) */;
    public const uint SDLK_KP_GREATER = 0x400000c6u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_GREATER) */;
    public const uint SDLK_KP_AMPERSAND = 0x400000c7u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_AMPERSAND) */;
    public const uint SDLK_KP_DBLAMPERSAND = 0x400000c8u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_DBLAMPERSAND) */;
    public const uint SDLK_KP_VERTICALBAR = 0x400000c9u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_VERTICALBAR) */;

    public const uint
        SDLK_KP_DBLVERTICALBAR = 0x400000cau /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_DBLVERTICALBAR) */;

    public const uint SDLK_KP_COLON = 0x400000cbu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_COLON) */;
    public const uint SDLK_KP_HASH = 0x400000ccu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_HASH) */;
    public const uint SDLK_KP_SPACE = 0x400000cdu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_SPACE) */;
    public const uint SDLK_KP_AT = 0x400000ceu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_AT) */;
    public const uint SDLK_KP_EXCLAM = 0x400000cfu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_EXCLAM) */;
    public const uint SDLK_KP_MEMSTORE = 0x400000d0u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MEMSTORE) */;
    public const uint SDLK_KP_MEMRECALL = 0x400000d1u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MEMRECALL) */;
    public const uint SDLK_KP_MEMCLEAR = 0x400000d2u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MEMCLEAR) */;
    public const uint SDLK_KP_MEMADD = 0x400000d3u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MEMADD) */;
    public const uint SDLK_KP_MEMSUBTRACT = 0x400000d4u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MEMSUBTRACT) */;
    public const uint SDLK_KP_MEMMULTIPLY = 0x400000d5u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MEMMULTIPLY) */;
    public const uint SDLK_KP_MEMDIVIDE = 0x400000d6u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_MEMDIVIDE) */;
    public const uint SDLK_KP_PLUSMINUS = 0x400000d7u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_PLUSMINUS) */;
    public const uint SDLK_KP_CLEAR = 0x400000d8u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_CLEAR) */;
    public const uint SDLK_KP_CLEARENTRY = 0x400000d9u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_CLEARENTRY) */;
    public const uint SDLK_KP_BINARY = 0x400000dau /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_BINARY) */;
    public const uint SDLK_KP_OCTAL = 0x400000dbu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_OCTAL) */;
    public const uint SDLK_KP_DECIMAL = 0x400000dcu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_DECIMAL) */;
    public const uint SDLK_KP_HEXADECIMAL = 0x400000ddu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_KP_HEXADECIMAL) */;
    public const uint SDLK_LCTRL = 0x400000e0u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_LCTRL) */;
    public const uint SDLK_LSHIFT = 0x400000e1u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_LSHIFT) */;
    public const uint SDLK_LALT = 0x400000e2u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_LALT) */;
    public const uint SDLK_LGUI = 0x400000e3u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_LGUI) */;
    public const uint SDLK_RCTRL = 0x400000e4u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_RCTRL) */;
    public const uint SDLK_RSHIFT = 0x400000e5u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_RSHIFT) */;
    public const uint SDLK_RALT = 0x400000e6u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_RALT) */;
    public const uint SDLK_RGUI = 0x400000e7u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_RGUI) */;
    public const uint SDLK_MODE = 0x40000101u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MODE) */;
    public const uint SDLK_SLEEP = 0x40000102u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_SLEEP) */;
    public const uint SDLK_WAKE = 0x40000103u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_WAKE) */;

    public const uint
        SDLK_CHANNEL_INCREMENT = 0x40000104u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CHANNEL_INCREMENT) */;

    public const uint
        SDLK_CHANNEL_DECREMENT = 0x40000105u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CHANNEL_DECREMENT) */;

    public const uint SDLK_MEDIA_PLAY = 0x40000106u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_PLAY) */;
    public const uint SDLK_MEDIA_PAUSE = 0x40000107u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_PAUSE) */;
    public const uint SDLK_MEDIA_RECORD = 0x40000108u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_RECORD) */;

    public const uint
        SDLK_MEDIA_FAST_FORWARD = 0x40000109u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_FAST_FORWARD) */;

    public const uint SDLK_MEDIA_REWIND = 0x4000010au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_REWIND) */;

    public const uint
        SDLK_MEDIA_NEXT_TRACK = 0x4000010bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_NEXT_TRACK) */;

    public const uint
        SDLK_MEDIA_PREVIOUS_TRACK = 0x4000010cu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_PREVIOUS_TRACK) */;

    public const uint SDLK_MEDIA_STOP = 0x4000010du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_STOP) */;
    public const uint SDLK_MEDIA_EJECT = 0x4000010eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_EJECT) */;

    public const uint
        SDLK_MEDIA_PLAY_PAUSE = 0x4000010fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_PLAY_PAUSE) */;

    public const uint SDLK_MEDIA_SELECT = 0x40000110u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_MEDIA_SELECT) */;
    public const uint SDLK_AC_NEW = 0x40000111u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_NEW) */;
    public const uint SDLK_AC_OPEN = 0x40000112u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_OPEN) */;
    public const uint SDLK_AC_CLOSE = 0x40000113u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_CLOSE) */;
    public const uint SDLK_AC_EXIT = 0x40000114u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_EXIT) */;
    public const uint SDLK_AC_SAVE = 0x40000115u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_SAVE) */;
    public const uint SDLK_AC_PRINT = 0x40000116u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_PRINT) */;
    public const uint SDLK_AC_PROPERTIES = 0x40000117u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_PROPERTIES) */;
    public const uint SDLK_AC_SEARCH = 0x40000118u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_SEARCH) */;
    public const uint SDLK_AC_HOME = 0x40000119u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_HOME) */;
    public const uint SDLK_AC_BACK = 0x4000011au /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_BACK) */;
    public const uint SDLK_AC_FORWARD = 0x4000011bu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_FORWARD) */;
    public const uint SDLK_AC_STOP = 0x4000011cu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_STOP) */;
    public const uint SDLK_AC_REFRESH = 0x4000011du /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_REFRESH) */;
    public const uint SDLK_AC_BOOKMARKS = 0x4000011eu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_AC_BOOKMARKS) */;
    public const uint SDLK_SOFTLEFT = 0x4000011fu /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_SOFTLEFT) */;
    public const uint SDLK_SOFTRIGHT = 0x40000120u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_SOFTRIGHT) */;
    public const uint SDLK_CALL = 0x40000121u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_CALL) */;
    public const uint SDLK_ENDCALL = 0x40000122u /**< SDL_SCANCODE_TO_KEYCODE(SDL_SCANCODE_ENDCALL) */;
    public const uint SDLK_LEFT_TAB = 0x20000001u /**< Extended key Left Tab */;
    public const uint SDLK_LEVEL5_SHIFT = 0x20000002u /**< Extended key Level 5 Shift */;
    public const uint SDLK_MULTI_KEY_COMPOSE = 0x20000003u /**< Extended key Multi-key Compose */;
    public const uint SDLK_LMETA = 0x20000004u /**< Extended key Left Meta */;
    public const uint SDLK_RMETA = 0x20000005u /**< Extended key Right Meta */;
    public const uint SDLK_LHYPER = 0x20000006u /**< Extended key Left Hyper */;
    public const uint SDLK_RHYPER = 0x20000007u /**< Extended key Right Hyper */;

    /**
     * Valid key modifiers (possibly OR'd together).
     *
     * \since This datatype is available since SDL 3.2.0.
     */
    
    
    public const ushort SDL_KMOD_NONE = 0x0000 /**< no modifier is applicable. */;

    public const ushort SDL_KMOD_LSHIFT = 0x0001 /**< the left Shift key is down. */;
    public const ushort SDL_KMOD_RSHIFT = 0x0002 /**< the right Shift key is down. */;
    public const ushort SDL_KMOD_LEVEL5 = 0x0004 /**< the Level 5 Shift key is down. */;
    public const ushort SDL_KMOD_LCTRL = 0x0040 /**< the left Ctrl (Control) key is down. */;
    public const ushort SDL_KMOD_RCTRL = 0x0080 /**< the right Ctrl (Control) key is down. */;
    public const ushort SDL_KMOD_LALT = 0x0100 /**< the left Alt key is down. */;
    public const ushort SDL_KMOD_RALT = 0x0200 /**< the right Alt key is down. */;
    public const ushort SDL_KMOD_LGUI = 0x0400 /**< the left GUI key (often the Windows key) is down. */;
    public const ushort SDL_KMOD_RGUI = 0x0800 /**< the right GUI key (often the Windows key) is down. */;
    public const ushort SDL_KMOD_NUM = 0x1000 /**< the Num Lock key (may be located on an extended keypad) is down. */;
    public const ushort SDL_KMOD_CAPS = 0x2000 /**< the Caps Lock key is down. */;
    public const ushort SDL_KMOD_MODE = 0x4000 /**< the !AltGr key is down. */;
    public const ushort SDL_KMOD_SCROLL = 0x8000 /**< the Scroll Lock key is down. */;

    /**< Any Ctrl key is down. */
    public static ushort SDL_KMOD_CTRL()
    {
        return (SDL_KMOD_LCTRL | SDL_KMOD_RCTRL);
    }

    public static ushort SDL_KMOD_SHIFT()
    {
        return (SDL_KMOD_LSHIFT | SDL_KMOD_RSHIFT);
    }

    public static ushort SDL_KMOD_ALT()
    {
        return (SDL_KMOD_LALT | SDL_KMOD_RALT);
    }

    public static ushort SDL_KMOD_GUI()
    {
        return (SDL_KMOD_LGUI | SDL_KMOD_RGUI);
    }
}