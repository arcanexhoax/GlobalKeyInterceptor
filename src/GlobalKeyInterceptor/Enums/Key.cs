namespace GlobalKeyInterceptor
{
    public enum Key
    {
        Backspace = 0x8,
        Tab = 0x9,
        Clear = 0xC,
        Enter = 0xD,
        Shift = 0x10,
        Ctrl = 0x11,
        Alt = 0x12,
        Pause = 0x13,
        CapsLock = 0x14,
        Escape = 0x1B,
        Space = 0x20,
        PageUp = 0x21,
        PageDown = 0x22,
        End = 0x23,
        Home = 0x24,
        LeftArrow = 0x25,
        UpArrow = 0x26,
        RightArrow = 0x27,
        DownArrow = 0x28,
        Select = 0x29,
        Print = 0x2A,
        Execute = 0x2B,
        PrintScreen = 0x2C,
        Insert = 0x2D,
        Delete = 0x2E,
        Help = 0x2F,
        /// <summary>
        /// The 0 key
        /// </summary>
        D0 = 0x30,
        /// <summary>
        /// The 1 key
        /// </summary>
        D1 = 0x31,
        /// <summary>
        /// The 2 key
        /// </summary>
        D2 = 0x32,
        /// <summary>
        /// The 3 key
        /// </summary>
        D3 = 0x33,
        /// <summary>
        /// The 4 key
        /// </summary>
        D4 = 0x34,
        /// <summary>
        /// The 5 key
        /// </summary>
        D5 = 0x35,
        /// <summary>
        /// The 6 key
        /// </summary>
        D6 = 0x36,
        /// <summary>
        /// The 7 key
        /// </summary>
        D7 = 0x37,
        /// <summary>
        /// The 8 key
        /// </summary>
        D8 = 0x38,
        /// <summary>
        /// The 9 key
        /// </summary>
        D9 = 0x39,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        LeftWindows = 0x5B,
        RightWindows = 0x5C,
        Applications = 0x5D,
        Sleep = 0x5F,
        Num0 = 0x60,
        Num1 = 0x61,
        Num2 = 0x62,
        Num3 = 0x63,
        Num4 = 0x64,
        Num5 = 0x65,
        Num6 = 0x66,
        Num7 = 0x67,
        Num8 = 0x68,
        Num9 = 0x69,
        NumMultiply = 0x6A,
        NumAdd = 0x6B,
        Separator = 0x6C,
        NumSubtract = 0x6D,
        NumDecimal = 0x6E,
        NumDivide = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        NumLock = 0x90,
        ScrollLock = 0x91,
        LeftShift = 0xA0,
        RightShift = 0xA1,
        LeftCtrl = 0xA2,
        RightCtrl = 0xA3,
        LeftAlt = 0xA4,
        RightAlt = 0xA5,
        BrowserBack = 0xA6,
        BrowserForward = 0xA7,
        BrowserRefresh = 0xA8,
        BrowserStop = 0xA9,
        BrowserSearch = 0xAA,
        BrowserFavorites = 0xAB,
        BrowserHome = 0xAC,
        VolumeMute = 0xAD,
        VolumeDown = 0xAE,
        VolumeUp = 0xAF,
        MediaNext = 0xB0,
        MediaPrevious = 0xB1,
        MediaStop = 0xB2,
        MediaPlay = 0xB3,
        LaunchMail = 0xB4,
        LaunchMediaSelect = 0xB5,
        LaunchApp1 = 0xB6,
        LaunchApp2 = 0xB7,
        /// <summary>
        /// The ": ;" key
        /// </summary>
        Colon = 0xBA,
        /// <summary>
        /// The "+ =" key
        /// </summary>
        Plus = 0xBB,
        /// <summary>
        /// The ", <" key
        /// </summary>
        Comma = 0xBC,
        /// <summary>
        /// The "- _" key
        /// </summary>
        Minus = 0xBD,
        /// <summary>
        /// The ". >" key
        /// </summary>
        Period = 0xBE,
        /// <summary>
        /// The "/ ?" key 
        /// </summary>
        Slash = 0xBF,
        /// <summary>
        /// The "` ~" key
        /// </summary>
        Tilde = 0xC0,
        /// <summary>
        /// The "[ {" key
        /// </summary>
        OpenBracket = 0xDB, 
        /// <summary>
        /// The "\ |" key
        /// </summary>
        BackSlash = 0xDC,
        /// <summary>
        /// The "] }" key
        /// </summary>
        ClosingBracket = 0xDD,
        /// <summary>
        /// The ' " key
        /// </summary>
        Quote = 0xDE,
        Oem8 = 0xDF,  
        Oem102 = 0xE2,
        Process = 0xE5,
        Packet = 0xE7,
        Attention = 0xF6,
        CrSel = 0xF7,
        ExSel = 0xF8,
        EraseEndOfFile = 0xF9,
        Play = 0xFA,
        Zoom = 0xFB,
        Pa1 = 0xFD,
        OemClear = 0xFE,
    }
}
