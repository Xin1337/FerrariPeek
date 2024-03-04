using Spectre.Console;
using System.Runtime.InteropServices;
using System.Text;

class LeanController
{
    private const int VK_END = 0x23;
    private const int VK_Q = 0x51;
    private const int VK_E = 0x45;
    private const int VK_NUMPAD1 = 0x61;
    private const int VK_NUMPAD2 = 0x62;
    private const int VK_NUMPAD4 = 0x64;
    private const int VK_NUMPAD5 = 0x65;

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    static extern ushort GetAsyncKeyState(int vKey);

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public uint Type;
        public INPUTUNION Data;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct INPUTUNION
    {
        [FieldOffset(0)]
        public MOUSEINPUT MouseInput;
        [FieldOffset(0)]
        public KEYBDINPUT KeyboardInput;
        [FieldOffset(0)]
        public HARDWAREINPUT HardwareInput;
    }


    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int Dx;
        public int Dy;
        public uint MouseData;
        public uint DwFlags;
        public uint Time;
        public IntPtr DwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort Vk;
        public ushort Scan;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    {
        public uint Msg;
        public ushort ParamL;
        public ushort ParamH;
    }

    const uint INPUT_KEYBOARD = 1;
    const uint KEYEVENTF_KEYDOWN = 0x0000;
    const uint KEYEVENTF_KEYUP = 0x0002;

    static void Main()
    {

        Console.Clear();
        AnsiConsole.Write(new Markup("[u][#800000]https://www.github.com/Xin1337/FerrariPeek[/][/]")
            .Centered());
        AnsiConsole.Write(new Markup("[u][#DADBDD]Press END to close the program[/][/]")
            .Centered());


        LeanAction[] leanActions = { new LeanLeft(), new LeanRight() };
        Random random = new Random();

        while (true)
        {
            if (GetAsyncKeyState(VK_END) != 0)
                Environment.Exit(0);

            if (GetActiveWindowTitle() == "EscapeFromTarkov")
            {
                foreach (LeanAction action in leanActions)
                {
                    action.PerformAction(random);
                }
            }

            Thread.Sleep(10);
        }
    }

    static string GetActiveWindowTitle()
    {
        IntPtr hwnd = GetForegroundWindow();
        StringBuilder title = new StringBuilder(256);
        GetWindowText(hwnd, title, 256);
        return title.ToString();
    }

    abstract class LeanAction
    {
        protected byte triggerKey;
        protected byte normalKey;
        protected byte slowKey;

        protected abstract bool IsTriggerKeyPressed();

        protected abstract void KeyPress(byte keyCode);

        protected abstract void KeyRelease(byte keyCode);

        public async void PerformAction(Random random)
        {
            if (IsTriggerKeyPressed())
            {
                int currentDelay = random.Next(10, 50);
                //Console.WriteLine($"Current Thread Delay: {currentDelay} ms");

                KeyPress(normalKey);
                Thread.Sleep(currentDelay);
                KeyPress(slowKey);

                while (IsTriggerKeyPressed())
                    Thread.Sleep(10);

                KeyRelease(slowKey);
                Thread.Sleep(currentDelay);
                KeyRelease(normalKey);
            }
        }
    }

    class LeanLeft : LeanAction
    {
        public LeanLeft()
        {
            triggerKey = VK_Q;
            normalKey = VK_NUMPAD1;
            slowKey = VK_NUMPAD4;
        }

        protected override bool IsTriggerKeyPressed()
        {
            return GetAsyncKeyState(triggerKey) != 0;
        }

        protected override void KeyPress(byte keyCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0] = new INPUT();
            inputs[0].Type = INPUT_KEYBOARD;
            inputs[0].Data.KeyboardInput.Vk = keyCode;
            inputs[0].Data.KeyboardInput.Flags = KEYEVENTF_KEYDOWN;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        protected override void KeyRelease(byte keyCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0] = new INPUT();
            inputs[0].Type = INPUT_KEYBOARD;
            inputs[0].Data.KeyboardInput.Vk = keyCode;
            inputs[0].Data.KeyboardInput.Flags = KEYEVENTF_KEYUP;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }

    class LeanRight : LeanAction
    {
        public LeanRight()
        {
            triggerKey = VK_E;
            normalKey = VK_NUMPAD2;
            slowKey = VK_NUMPAD5;
        }

        protected override bool IsTriggerKeyPressed()
        {
            return GetAsyncKeyState(triggerKey) != 0;
        }

        protected override void KeyPress(byte keyCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0] = new INPUT();
            inputs[0].Type = INPUT_KEYBOARD;
            inputs[0].Data.KeyboardInput.Vk = keyCode;
            inputs[0].Data.KeyboardInput.Flags = KEYEVENTF_KEYDOWN;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        protected override void KeyRelease(byte keyCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0] = new INPUT();
            inputs[0].Type = INPUT_KEYBOARD;
            inputs[0].Data.KeyboardInput.Vk = keyCode;
            inputs[0].Data.KeyboardInput.Flags = KEYEVENTF_KEYUP;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}