using System.Diagnostics;
using System.Runtime.InteropServices;

public class GlobalMouseHook : IDisposable
{
	// Events
	public event Action<int, int> MouseMoved;
	public event Action<MouseButtons, int, int> MouseDown;
	public event Action<MouseButtons, int, int> MouseUp;
	public event Action<int, int, int> MouseWheel; // delta, x, y

	// WinAPI constants and structs
	private const int WH_MOUSE_LL = 14;
	private const int WM_MOUSEMOVE = 0x0200;
	private const int WM_LBUTTONDOWN = 0x0201;
	private const int WM_LBUTTONUP = 0x0202;
	private const int WM_RBUTTONDOWN = 0x0204;
	private const int WM_RBUTTONUP = 0x0205;
	private const int WM_MBUTTONDOWN = 0x0207;
	private const int WM_MBUTTONUP = 0x0208;
	private const int WM_MOUSEWHEEL = 0x020A;
	private const int WM_XBUTTONDOWN = 0x020B;
	private const int WM_XBUTTONUP = 0x020C;

	[StructLayout(layoutKind: LayoutKind.Sequential)]
	private struct MSLLHOOKSTRUCT
	{
		public POINT pt;
		public int mouseData;
		public int flags;
		public int time;
		public IntPtr dwExtraInfo;
	}

	[StructLayout(layoutKind: LayoutKind.Sequential)]
	private struct POINT
	{
		public int x;
		public int y;
	}

	private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

	// Keep delegate alive
	private readonly LowLevelMouseProc proc;
	private IntPtr hookId = IntPtr.Zero;

	public GlobalMouseHook()
	{
		proc = HookCallback;
	}

	public void Start()
	{
		if (hookId != IntPtr.Zero)
		{
			return;
		}

		using Process curProcess = Process.GetCurrentProcess();
		using ProcessModule curModule = curProcess.MainModule;
		IntPtr moduleHandle = GetModuleHandle(lpModuleName: curModule.ModuleName);
		hookId = SetWindowsHookEx(idHook: WH_MOUSE_LL, lpfn: proc, hMod: moduleHandle, dwThreadId: 0);
		if (hookId == IntPtr.Zero)
		{
			throw new System.ComponentModel.Win32Exception(error: Marshal.GetLastWin32Error(), message: "SetWindowsHookEx failed");
		}
	}

	public void Stop()
	{
		if (hookId == IntPtr.Zero)
		{
			return;
		}

		UnhookWindowsHookEx(hhk: hookId);
		hookId = IntPtr.Zero;
	}

	private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
	{
		if (nCode >= 0)
		{
			int wm = wParam.ToInt32();
			MSLLHOOKSTRUCT data = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(ptr: lParam);
			int x = data.pt.x;
			int y = data.pt.y;

			switch (wm)
			{
				case WM_MOUSEMOVE:
					MouseMoved?.Invoke(arg1: x, arg2: y);
					break;
				case WM_LBUTTONDOWN:
					MouseDown?.Invoke(arg1: MouseButtons.Left, arg2: x, arg3: y);
					break;
				case WM_LBUTTONUP:
					MouseUp?.Invoke(arg1: MouseButtons.Left, arg2: x, arg3: y);
					break;
				case WM_RBUTTONDOWN:
					MouseDown?.Invoke(arg1: MouseButtons.Right, arg2: x, arg3: y);
					break;
				case WM_RBUTTONUP:
					MouseUp?.Invoke(arg1: MouseButtons.Right, arg2: x, arg3: y);
					break;
				case WM_MBUTTONDOWN:
					MouseDown?.Invoke(arg1: MouseButtons.Middle, arg2: x, arg3: y);
					break;
				case WM_MBUTTONUP:
					MouseUp?.Invoke(arg1: MouseButtons.Middle, arg2: x, arg3: y);
					break;
				case WM_MOUSEWHEEL:
					int delta = (short)((data.mouseData >> 16) & 0xffff);
					MouseWheel?.Invoke(arg1: delta, arg2: x, arg3: y);
					break;
				case WM_XBUTTONDOWN:
				case WM_XBUTTONUP:
					int xButton = (data.mouseData >> 16) & 0xffff;
					MouseButtons btn = (xButton == 1) ? MouseButtons.XButton1 : MouseButtons.XButton2;
					if (wm == WM_XBUTTONDOWN)
					{
						MouseDown?.Invoke(arg1: btn, arg2: x, arg3: y);
					}
					else
					{
						MouseUp?.Invoke(arg1: btn, arg2: x, arg3: y);
					}
					break;
				default:
					break;
			}
		}

		return CallNextHookEx(hhk: hookId, nCode: nCode, wParam: wParam, lParam: lParam);
	}

	#region WinAPI
	[DllImport(dllName: "user32.dll", SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

	[DllImport(dllName: "user32.dll", SetLastError = true)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhk);

	[DllImport(dllName: "user32.dll")]
	private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

	[DllImport(dllName: "kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr GetModuleHandle(string lpModuleName);
	#endregion

	public void Dispose()
	{
		Stop();
		GC.SuppressFinalize(obj: this);
	}
}
