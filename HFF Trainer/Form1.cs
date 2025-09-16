using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Memory;

namespace HFF_Trainer
{
    public partial class Form1 : Form
    {
        public const string PROCESS_NAME = "Human.exe";
        public const string JUMP_SIGNATURE = "D9 05 ?? ?? ?? ?? D9 5D DC C7";
        public const string SPEED_SIGNATURE = "5D A4 D9 05 ?? ?? ?? ?? D9 5D A4";
        public const float JUMP_MULTIPLIER = 3.0f;
        public const float SPEED_MULTIPLIER = 4.0f;

        public Mem m = new Mem();
        private GlobalKeyboardHook _keyboardHook;

        private IntPtr jumpConstantAddress = IntPtr.Zero;
        private IntPtr speedConstantAddress = IntPtr.Zero;
        private float originalJumpValue = 0f;
        private float originalSpeedValue = 0f;

        private bool jumpReady = false;
        private bool speedReady = false;
        private bool jumpApplied = false;
        private bool speedApplied = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _keyboardHook = new GlobalKeyboardHook();
            _keyboardHook.HookKey(Keys.NumPad1, () => { if (jumpReady || !chkHighJump.Checked) this.Invoke((MethodInvoker)delegate { chkHighJump.Checked = !chkHighJump.Checked; }); });
            _keyboardHook.HookKey(Keys.NumPad2, () => { if (speedReady || !chkSpeedHack.Checked) this.Invoke((MethodInvoker)delegate { chkSpeedHack.Checked = !chkSpeedHack.Checked; }); });

            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (jumpReady) WriteValue(jumpConstantAddress, originalJumpValue);
            if (speedReady) WriteValue(speedConstantAddress, originalSpeedValue);
            _keyboardHook?.Dispose();
            base.OnFormClosing(e);
        }

        void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!m.Attach(PROCESS_NAME))
                {
                    if (jumpReady)
                    {
                        jumpReady = false;
                        speedReady = false;
                        this.Invoke((MethodInvoker)delegate {
                            lblStatus.Text = "Статус: Ожидание игры...";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        });
                    }
                    Thread.Sleep(1000);
                    continue;
                }

                if (!jumpReady || !speedReady)
                {
                    this.Invoke((MethodInvoker)delegate { lblStatus.Text = "Статус: Поиск сигнатур..."; });
                    InitializeFeatures();
                }

                if (jumpReady && speedReady)
                {
                    this.Invoke((MethodInvoker)delegate {
                        lblStatus.Text = "Статус: Трейнер готов!";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    });

                    while (m.Attach(PROCESS_NAME))
                    {
                        try
                        {
                            HandleCheat(chkHighJump, ref jumpApplied, jumpConstantAddress, originalJumpValue, JUMP_MULTIPLIER);
                            HandleCheat(chkSpeedHack, ref speedApplied, speedConstantAddress, originalSpeedValue, SPEED_MULTIPLIER);
                        }
                        catch
                        {
                            jumpReady = false;
                            speedReady = false;
                            break;
                        }

                        this.Invoke((MethodInvoker)delegate {
                            chkHighJump.Text = $"Высокий прыжок (x{JUMP_MULTIPLIER}) [Num 1] {(chkHighJump.Checked ? "[ВКЛ]" : "[ВЫКЛ]")}";
                            chkSpeedHack.Text = $"Ускорение (x{SPEED_MULTIPLIER}) [Num 2] {(chkSpeedHack.Checked ? "[ВКЛ]" : "[ВЫКЛ]")}";
                        });
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate {
                        lblStatus.Text = "Статус: Сигнатуры не найдены. Загрузите уровень.";
                        lblStatus.ForeColor = System.Drawing.Color.Orange;
                    });
                    Thread.Sleep(2000);
                }
            }
        }

        private void HandleCheat(CheckBox chk, ref bool applied, IntPtr address, float originalValue, float multiplier)
        {
            if (chk.Checked)
            {
                bool needsWrite = chkMonitoring.Checked || !applied;
                if (needsWrite)
                {
                    float newValue = originalValue * multiplier;
                    if (m.ReadFloat(address) != newValue)
                    {
                        WriteValue(address, newValue);
                        applied = true;
                    }
                }
            }
            else
            {
                if (m.ReadFloat(address) != originalValue)
                {
                    WriteValue(address, originalValue);
                }
                applied = false;
            }
        }

        private void InitializeFeatures()
        {
            try
            {
                if (!jumpReady)
                {
                    IntPtr jumpSigAddr = AobScan(JUMP_SIGNATURE);
                    if (jumpSigAddr != IntPtr.Zero)
                    {
                        IntPtr ptr = m.ReadIntPtr(jumpSigAddr + 2);
                        jumpConstantAddress = ptr;
                        originalJumpValue = m.ReadFloat(jumpConstantAddress);
                        jumpReady = true;
                    }
                }

                if (!speedReady)
                {
                    IntPtr speedSigAddr = AobScan(SPEED_SIGNATURE);
                    if (speedSigAddr != IntPtr.Zero)
                    {
                        IntPtr ptr = m.ReadIntPtr(speedSigAddr + 4);
                        speedConstantAddress = ptr;
                        originalSpeedValue = m.ReadFloat(speedConstantAddress);
                        speedReady = true;
                    }
                }
            }
            catch
            {
                jumpReady = false;
                speedReady = false;
            }
        }

        private void WriteValue(IntPtr address, float value)
        {
            try { m.WriteFloat(address, value); }
            catch (Exception ex) { Debug.WriteLine($"Failed to write value at {address}: {ex.Message}"); }
        }

        private void chkHighJump_CheckedChanged(object sender, EventArgs e)
        {
            if (!jumpReady) { chkHighJump.Checked = false; return; }
            jumpApplied = false;
        }

        private void chkSpeedHack_CheckedChanged(object sender, EventArgs e)
        {
            if (!speedReady) { chkSpeedHack.Checked = false; return; }
            speedApplied = false;
        }

        private IntPtr AobScan(string signature)
        {
            var process = Process.GetProcessesByName(PROCESS_NAME.Replace(".exe", "")).FirstOrDefault();
            if (process == null) return IntPtr.Zero;

            var pattern = signature.Split(' ').Select(b => b == "??" ? -1 : Convert.ToInt32(b, 16)).ToArray();

            IntPtr currentAddr = IntPtr.Zero;
            MEMORY_BASIC_INFORMATION mbi;

            while (VirtualQueryEx(process.Handle, currentAddr, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0)
            {
                bool isReadable = (mbi.Protect == 0x02 || mbi.Protect == 0x04 || mbi.Protect == 0x20 || mbi.Protect == 0x40);
                if (mbi.State == 0x1000 && isReadable && (mbi.Protect & 0x100) == 0)
                {
                    byte[] buffer = m.ReadBytes(mbi.BaseAddress, (int)mbi.RegionSize.ToInt64());
                    for (int i = 0; i < buffer.Length - pattern.Length; i++)
                    {
                        bool found = true;
                        for (int j = 0; j < pattern.Length; j++)
                        {
                            if (pattern[j] != -1 && pattern[j] != buffer[i + j])
                            {
                                found = false;
                                break;
                            }
                        }
                        if (found)
                        {
                            return new IntPtr(mbi.BaseAddress.ToInt64() + i);
                        }
                    }
                }
                currentAddr = new IntPtr(mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64());
            }
            return IntPtr.Zero;
        }

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }
    }
}