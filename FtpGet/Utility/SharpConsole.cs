using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Utility
{
    public class SharpConsole
    {
        public SharpConsole()
        {
        }

        [DllImport("kernel32")]
        private static extern bool AddConsoleAlias(string Source, string Target, string ExeName);

        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32")]
        private static extern bool AttachConsole(int ProcessId);

        [DllImport("kernel32")]
        private static extern IntPtr CreateConsoleScreenBuffer(CONSOLE_BUFFER_SECURITY DesiredAccess, FILE_SHARE_MODE ShareMode, SECURITY_ATTRIBUTES SecurityAttributes, int Flags, object ScreenBufferData);

        [DllImport("kernel32")]
        private static extern bool FillConsoleOutputAttribute(IntPtr ConsoleOutput, CHAR_ATTRIBUTES Attribute, int Length, COORD WriteCoord, out int NumberOfAttributesWritten);

        [DllImport("kernel32")]
        private static extern bool FillConsoleOutputCharacter(IntPtr ConsoleOutput, char Character, int Length, COORD WriteCoord, out int NumberOfAttributesWritten);

        [DllImport("kernel32")]
        private static extern bool FlushConsoleInputBuffer(IntPtr ConsoleInput);

        [DllImport("kernel32")]
        private static extern bool FreeConsole();

        [DllImport("kernel32")]
        private static extern bool GetConsoleAlias(string Source, ref StringBuilder TargetBuffer, int TargetBufferLength, string ExeName);

        [DllImport("kernel32")]
        private static extern bool GetConsoleAliases(ref StringBuilder AliasBuffer, int AliasBufferLength, string ExeName);

        [DllImport("kernel32")]
        private static extern int GetConsoleAliasesLength(string ExeName);

        [DllImport("kernel32")]
        private static extern bool GetConsoleAliasExes(ref StringBuilder ExeNameBuffer, int ExeNameBufferLength);

        [DllImport("kernel32")]
        private static extern int GetConsoleAliasExesLength();

        [DllImport("kernel32")]
        private static extern uint GetConsoleCP();

        [DllImport("kernel32")]
        private static extern bool GetConsoleCursorInfo(IntPtr ConsoleOutput, out CONSOLE_CURSOR_INFO ConsoleCursorInfo);

        [DllImport("kernel32")]
        private static extern bool GetConsoleDisplayMode(out CONSOLE_DISPLAY_MODE ModeFlags);

        [DllImport("kernel32")]
        private static extern COORD GetConsoleFontSize(IntPtr ConsoleOutput, int Font);

        [DllImport("kernel32")]
        private static extern bool GetConsoleMode(IntPtr ConsoleHandle, out CONSOLE_MODE Mode);

        [DllImport("kernel32")]
        private static extern uint GetConsoleOutputCP();

        [DllImport("kernel32")]
        private static extern int GetConsoleProcessList(out int[] ProcessList, int ProcessCount);

        [DllImport("kernel32")]
        private static extern bool GetConsoleScreenBufferInfo(IntPtr ConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO ConsoleScreenBufferInfo);

        [DllImport("kernel32")]
        private static extern bool GetConsoleSelectionInfo(out CONSOLE_SELECTION_INFO ConsoleSelectionInfo);

        [DllImport("kernel32")]
        private static extern int GetConsoleTitle(ref StringBuilder ConsoleTitle, int Size);

        [DllImport("kernel32")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32")]
        private static extern bool GetCurrentConsoleFont(IntPtr ConsoleOutput, bool MaximumWindow, out CONSOLE_FONT_INFO ConsoleCurrentFont);

        [DllImport("kernel32")]
        private static extern COORD GetLargestConsoleWindowSize(IntPtr ConsoleOutput);


    }

    internal struct CONSOLE_CURSOR_INFO
    {
        public int Size;
        public bool Visible;
    }

    internal struct CONSOLE_FONT_INFO
    {
        public int Font;
        public COORD FontSize;
    }

    internal struct CONSOLE_SCREEN_BUFFER_INFO
    {
        public COORD Size;
        public COORD CursorPosition;
        public CHAR_ATTRIBUTES Attributes;
        public SMALL_RECT Window;
        public COORD MaximumWindowSize;
    }

    internal struct CONSOLE_SELECTION_INFO
    {
        public SELECTION_FLAGS Flags;
        public COORD SelectionAnchor;
        public SMALL_RECT Selection;
    }

    internal struct COORD
    {
        public short X;
        public short Y;
    }

    internal struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    internal struct SECURITY_ATTRIBUTES
    {
        public int Length;
        public string SecurityDescriptor;
        public bool InheritHandle;
    }

    [Flags]
    internal enum CHAR_ATTRIBUTES
    {
        FOREGROUND_BLUE = 0x0001,
        FOREGROUND_GREEN = 0x0002,
        FOREGROUND_RED = 0x0004,
        FOREGROUND_INTENSITY = 0x0008,
        BACKGROUND_BLUE = 0x0010,
        BACKGROUND_GREEN = 0x0020,
        BACKGROUND_RED = 0x0040,
        BACKGROUND_INTENSITY = 0x0080,
        COMMON_LVB_LEADING_BYTE = 0x0100,
        COMMON_LVB_TRAILING_BYTE = 0x0200,
        COMMON_LVB_GRID_HORIZONTAL = 0x0400,
        COMMON_LVB_GRID_LVERTICAL = 0x0800,
        COMMON_LVB_GRID_RVERTICAL = 0x1000,
        COMMON_LVB_REVERSE_VIDEO = 0x4000,
        COMMON_LVB_UNDERSCORE = 0x8000,
        COMMON_LVB_SBCSDBCS = 0x0300,
    }

    [Flags]
    internal enum SELECTION_FLAGS
    {
        NoSelection = 0x00,
        SelectionInProgress = 0x01,
        SelectionNotEmpty = 0x02,
        MouseSelection = 0x04,
        MouseDown = 0x08,
    }

    [Flags]
    internal enum FILE_SHARE_MODE
    {
        FILE_SHARE_READ = 0x01,
        FILE_SHARE_WRITE = 0x02,
    }

    [Flags]
    internal enum CONSOLE_BUFFER_SECURITY : long
    {
        GENERIC_READ = 0x80000000L,
        GENERIC_WRITE = 0x40000000L,
    }

    [Flags]
    internal enum CONSOLE_DISPLAY_MODE
    {
        CONSOLE_FULLSCREEN = 0x01,
        CONSOLE_FULLSCREEN_HARDWARE = 0x02,
    }

    [Flags]
    internal enum CONSOLE_MODE
    {
        ENABLE_PROCESSED_INPUT = 0x0001,
        ENABLE_LINE_INPUT = 0x0002,
        ENABLE_ECHO_INPUT = 0x0004,
        ENABLE_WINDOW_INPUT = 0x0008,
        ENABLE_MOUSE_INPUT = 0x0010,
        ENABLE_INSERT_MODE = 0x0020,
        ENABLE_QUICK_EDIT_MODE = 0x0040,

        ENABLE_PROCESSED_OUTPUT = 0x0001,
        ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
    }
}
