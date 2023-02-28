using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Mpv.NET.Player;

namespace VideoWallpaper
{
    public partial class MainForm : Form
    {
        private MpvPlayer player;
        private IntPtr parentIntPtr = IntPtr.Zero;
        private List<Rectangle> screens = new List<Rectangle>();
        //string basePath = "D:\\WorkSpace\\PersonalProjects\\VideoWallpaper";
        string basePath = "";

        public MainForm()
        {
            InitializeComponent();

            basePath = Directory.GetCurrentDirectory();

            player = new MpvPlayer(this.Handle, basePath + "\\mpv\\mpv-1.dll")
            {
                Loop = true,
                Volume = 0
            };

            MakeScreenList();

            string wallpaperPath = basePath + "\\wallpaper";
            if (Directory.Exists(wallpaperPath))
            {
                string lastWallpaper = "";
                string lastScreen = "0";
                try
                {
                    lastWallpaper = File.ReadAllText(basePath + "\\mpv\\wp.txt");
                    lastScreen = File.ReadAllText(basePath + "\\mpv\\s.txt");
                }
                catch { }
                
                player.LoadConfig(basePath + "\\mpv\\mpv.conf");
                player.Load(basePath + "\\wallpaper\\" + lastWallpaper);
                player.Resume();
                Thread.Sleep(1000);
                Init();
                SwitchScreen(Int32.Parse(lastScreen));

                notifyIcon1.Visible = true;
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(wallpaperPath);
                directoryInfo.Create();
            }

            MakeList(wallpaperPath);
        }

        public void MakeList(string wallpaperPath)
        {
            DirectoryInfo root = new DirectoryInfo(wallpaperPath);
            FileInfo[] fileInfos = root.GetFiles();
            foreach (FileInfo file in fileInfos)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = file.Name;
                item.Click += new EventHandler(wp_ItemClick);
                switchToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        public void MakeScreenList()
        {
            int i = 0;
            foreach (var screen in Screen.AllScreens)
            {
                ToolStripItem item = new ToolStripMenuItem();
                //item.Text = "Bounds: " + screen.Bounds.ToString();
                item.Text = "��Ļ " + i;
                item.Click += new EventHandler(ss_ItemClick);
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

        public void ss_ItemClick(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            int i = Int32.Parse(item.Text.Split(' ').Last());
            SwitchScreen(i);
            File.WriteAllText(basePath + "\\mpv\\s.txt", i.ToString());
        }

        public void SwitchScreen(int sid)
        {
            WindowState = FormWindowState.Normal;
            Location = screens[sid].Location;
            Size = screens[sid].Size;
        }

        async void wp_ItemClick(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            await Task.Delay(50);
            player.Load(basePath + "\\wallpaper\\" + item.Text);
            player.Resume();
            File.WriteAllText(basePath + "\\mpv\\wp.txt", item.Text);
        }

        public void Init()
        {
            parentIntPtr = Win32.FindWindow("Progman", null);

            if (parentIntPtr != IntPtr.Zero)
            {
                IntPtr result = IntPtr.Zero;

                // �� Program Manager ���ڷ��� 0x52c ��һ����Ϣ����ʱ����Ϊ0x3e8��1�룩��
                Win32.SendMessageTimeout(parentIntPtr, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 0x3e8, result);

                Win32.EnumWindows((hWnd, lParam) =>
                {
                    if (Win32.FindWindowEx(hWnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                    {
                        parentIntPtr = Win32.FindWindowEx(IntPtr.Zero, hWnd, "WorkerW", null);
                        Win32.ShowWindow(parentIntPtr, 1);
                    }
                    return true;
                }, IntPtr.Zero);
            }

            Win32.SetParent(this.Handle, parentIntPtr);
        }

        private void siteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://meta.appinn.net/t/topic/40295/2") { UseShellExecute = true });
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://afdian.net/a/ifwz1729") { UseShellExecute = true });
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("С�糵\n���ߣ��Ա��׻���С��");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.Dispose();
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            player.Dispose();
        }

        private void autostartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoRun();
        }

        //������
        private static void CreateShortcut(string lnkFilePath, string args = "")
        {
            var shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(shellType);
            var shortcut = shell.CreateShortcut(lnkFilePath);
            shortcut.TargetPath = Assembly.GetEntryAssembly().Location;
            shortcut.Arguments = args;
            shortcut.WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            shortcut.Save();
        }

        public async void AutoRun()
        {
            CreateShortcut(basePath + "\\С�糵.lnk");
            await Task.Delay(125);
            string StartupPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup);
            if (!File.Exists(StartupPath + @"\С�糵.lnk"))
            {
                File.Move(Directory.GetCurrentDirectory() + @"\С�糵.lnk", StartupPath + @"\С�糵.lnk");
                MessageBox.Show("С�糵������Ϊ������");
            }
        }
    }
}