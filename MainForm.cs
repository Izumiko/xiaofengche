using LibVLCSharp.Shared;
using System;
using System.Diagnostics;
using System.Reflection;

namespace VideoWallpaper
{

    public partial class MainForm : Form
    {
        public LibVLC _libVLC;
        public MediaPlayer _mp;

        private IntPtr parentIntPtr = IntPtr.Zero;
        private readonly List<Rectangle> screens = [];
        private readonly string basePath = "";

        public MainForm()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }

            InitializeComponent();

            basePath = Directory.GetCurrentDirectory();

            string configFile = Path.Combine(basePath, "config", "vlc-options.txt");
            string[] vlcOptions = [];

            if (File.Exists(configFile))
            {
                vlcOptions = [.. File.ReadAllLines(configFile).Where(static line => !string.IsNullOrWhiteSpace(line) && !line.Trim().StartsWith('#'))];
            }
            else
            {
                vlcOptions =
                [
                    "--no-audio",            // 关闭音频
                    "--avcodec-hw=auto",     // 硬件解码
                    "--no-spu",              // 禁用字幕、OSD渲染
                    "--no-video-title-show", // 不在视频顶部显示短暂的标题
                    "--quiet"                // 减少日志输出
                ];
            }
            _libVLC = new LibVLC(vlcOptions);
            _mp = new MediaPlayer(_libVLC);

            videoView1.MediaPlayer = _mp;

            MakeScreenList();

            string wallpaperPath = basePath + "\\wallpaper";
            if (Directory.Exists(wallpaperPath))
            {
                string lastWallpaper = "";
                string lastScreen = "0";
                try
                {
                    lastWallpaper = File.ReadAllText(basePath + "\\config\\wp.txt");
                    lastScreen = File.ReadAllText(basePath + "\\config\\s.txt");
                }
                catch { }

                var media = new Media(_libVLC, new Uri(basePath + "\\wallpaper\\" + lastWallpaper));
                media.AddOption(":input-repeat=2147483647");
                _mp.Play(media);
                media.Dispose();

                Thread.Sleep(1000);
                Init();
                SwitchScreen(int.Parse(lastScreen));

                notifyIcon1.Visible = true;
            }
            else
            {
                DirectoryInfo directoryInfo = new(wallpaperPath);
                directoryInfo.Create();
            }

            MakeList(wallpaperPath);
        }

        public void MakeList(string wallpaperPath)
        {
            DirectoryInfo root = new(wallpaperPath);
            FileInfo[] fileInfos = root.GetFiles();
            foreach (FileInfo file in fileInfos)
            {
                ToolStripItem item = new ToolStripMenuItem
                {
                    Text = file.Name
                };
                item.Click += Wp_ItemClick;
                switchToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        public void MakeScreenList()
        {
            int i = 0;
            foreach (var screen in Screen.AllScreens)
            {
                ToolStripItem item = new ToolStripMenuItem
                {
                    Text = "屏幕 " + i
                };
                item.Click += Ss_ItemClick;
                screens.Add(screen.Bounds);
                i++;
                screenToolStripMenuItem.DropDownItems.Add(item);
            }
            int xmin = 0, ymin = 0;
            foreach (var s in screens)
            {
                xmin = s.X < xmin ? s.X : xmin;
                ymin = s.Y < ymin ? s.Y : ymin;
            }
            for (i = 0; i < screens.Count; i++)
            {
                Rectangle s = screens[i];
                s.Offset(-xmin, -ymin);
                screens[i] = s;
            }
        }

        public void Ss_ItemClick(object? sender, EventArgs e)
        {
            ToolStripItem? item = sender as ToolStripItem;
            if (item?.Text == null) return;
            int i = Int32.Parse(item.Text.Split(' ').Last());
            SwitchScreen(i);
            File.WriteAllText(basePath + "\\config\\s.txt", i.ToString());
        }

        public void SwitchScreen(int sid)
        {
            if (sid >= screens.Count)
            {
                sid = 0;
            }
            WindowState = FormWindowState.Normal;
            Location = screens[sid].Location;
            Size = screens[sid].Size;
        }

        async void Wp_ItemClick(object? sender, EventArgs e)
        {
            ToolStripItem? item = sender as ToolStripItem;
            if (item?.Text == null) return;
            await Task.Delay(50);
            var media = new Media(_libVLC, new Uri(basePath + "\\wallpaper\\" + item.Text));
            media.AddOption(":input-repeat=2147483647");
            _mp.Play(media);
            media.Dispose();
            File.WriteAllText(basePath + "\\config\\wp.txt", item.Text);
        }

        public void Init()
        {
            parentIntPtr = Win32.FindWindow("Progman", null);
            if (parentIntPtr == IntPtr.Zero)
            {
                return;
            }

            // 向 Program Manager 窗口发送 0x52c 的一个消息，超时设置为0x3e8（1秒）。
            _ = Win32.SendMessageTimeout(parentIntPtr, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 0x3e8, out IntPtr result);

            IntPtr workerwPtr = IntPtr.Zero;
            _ = Win32.EnumWindows((hWnd, lParam) =>
            {
                if (Win32.FindWindowEx(hWnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    workerwPtr = Win32.FindWindowEx(IntPtr.Zero, hWnd, "WorkerW", null);
                    return false;
                }
                return true;
            }, IntPtr.Zero);

            // For Win 11
            if (workerwPtr == IntPtr.Zero)
            {
                IntPtr shelldll_defview = Win32.FindWindowEx(parentIntPtr, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shelldll_defview != IntPtr.Zero)
                {
                    // 获取 SHELLDLL_DefView 后面的窗口
                    uint GW_HWNDNEXT = 2;
                    IntPtr desktopHandle = Win32.GetWindow(shelldll_defview, GW_HWNDNEXT);
                    if (desktopHandle != IntPtr.Zero)
                    {
                        char[] className = new char[256];
                        int length = Win32.GetClassName(desktopHandle, className, 256);
                        if (length > 0 && new string(className, 0, length) == "WorkerW")
                        {
                            workerwPtr = desktopHandle;
                        }
                    }
                }
            }
            Win32.ShowWindow(workerwPtr, 1);

            Win32.SetParent(this.Handle, workerwPtr);
        }

        private void SiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://meta.appinn.net/t/topic/40295/") { UseShellExecute = true });
        }

        private void DonateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://afdian.net/a/ifwz1729") { UseShellExecute = true });
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("小风车\n作者：吃爆米花的小熊");
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _mp.Stop();
            _mp.Dispose();
            _libVLC.Dispose();
        }

        private void AutostartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoRun();
        }

        //自启动
        private static bool CreateShortcut(string lnkFilePath, string args = "")
        {
            var shellType = Type.GetTypeFromProgID("WScript.Shell");
            if (shellType == null) return false;
            dynamic? shell = Activator.CreateInstance(shellType);
            dynamic? shortcut = shell?.CreateShortcut(lnkFilePath);
            if (shortcut == null) return false;
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return false;
            shortcut.TargetPath = entryAssembly.Location;
            shortcut.Arguments = args;
            shortcut.WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            shortcut.Save();
            return true;
        }

        public async void AutoRun()
        {
            var success = CreateShortcut(basePath + "\\小风车.lnk");
            if (!success)
            {
                MessageBox.Show("创建快捷方式失败", "创建失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await Task.Delay(125);
            string StartupPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Startup);
            if (!File.Exists(StartupPath + @"\小风车.lnk"))
            {
                File.Move(Directory.GetCurrentDirectory() + @"\小风车.lnk", StartupPath + @"\小风车.lnk");
            }
            if (File.Exists(StartupPath + @"\小风车.lnk"))
            {
                MessageBox.Show("小风车已设置为自启动");
            }
            else
            {
                MessageBox.Show("小风车设置自启动失败", "设置失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            videoView1.Size = this.Size;
        }
    }
}