using System.Runtime.InteropServices;
using System.Text;

namespace VideoWallpaper
{
    internal partial class Win32
    {
        [LibraryImport("user32.dll", EntryPoint = "FindWindowW", StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr FindWindow(string? className, string? winName);

        [LibraryImport("user32.dll", EntryPoint = "SendMessageTimeoutW")]
        public static partial IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlag, uint timeout, out IntPtr result);

        [LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
        public static partial int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool EnumWindows(EnumWindowsProc proc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [LibraryImport("user32.dll", EntryPoint = "FindWindowExW", StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string? className, string? winName);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [LibraryImport("user32.dll")]
        public static partial IntPtr SetParent(IntPtr hWnd, IntPtr hParent);

        [LibraryImport("user32.dll", EntryPoint = "GetClassNameW")]
        public static partial int GetClassName(IntPtr hWnd, [Out] char[] lpClassName, int nMaxCount);

        [LibraryImport("user32.dll", SetLastError = true)]
        public static partial IntPtr GetWindow(IntPtr hWnd, uint uCmd);
    }
}
