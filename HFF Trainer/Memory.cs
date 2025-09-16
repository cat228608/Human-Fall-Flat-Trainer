using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Memory
{
    public class Mem
    {
        // --- ИСПРАВЛЕНИЕ: Используем IntPtr для хэндлов и адресов ---
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        private Process process;
        private IntPtr processHandle;
        const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        public bool Attach(string procName)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(procName.Replace(".exe", ""));
                if (processes.Length > 0)
                {
                    process = processes[0];
                    processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);
                    return !process.HasExited && processHandle != IntPtr.Zero;
                }
                return false;
            }
            catch { return false; }
        }

        // --- ИСПРАВЛЕНИЕ: Все методы теперь принимают IntPtr ---
        public byte[] ReadBytes(IntPtr address, int length)
        {
            byte[] buffer = new byte[length];
            ReadProcessMemory(processHandle, address, buffer, buffer.Length, out _);
            return buffer;
        }

        public bool WriteBytes(IntPtr address, byte[] bytes)
        {
            return WriteProcessMemory(processHandle, address, bytes, bytes.Length, out _);
        }

        // --- ИСПРАВЛЕНИЕ: Методы-обертки тоже используют IntPtr ---
        public long ReadInt64(IntPtr address) => BitConverter.ToInt64(ReadBytes(address, 8), 0);
        public int ReadInt32(IntPtr address) => BitConverter.ToInt32(ReadBytes(address, 4), 0);
        public IntPtr ReadIntPtr(IntPtr address) => (IntPtr)ReadInt32(address); // Для 32-бит
        public float ReadFloat(IntPtr address) => BitConverter.ToSingle(ReadBytes(address, 4), 0);

        public bool WriteFloat(IntPtr address, float value) => WriteBytes(address, BitConverter.GetBytes(value));
    }
}