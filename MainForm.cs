
namespace MouseInspector
{
	public partial class MainForm : Form
	{
		private readonly GlobalMouseHook hook;

		public MainForm()
		{
			InitializeComponent();
			hook = new GlobalMouseHook();
			hook.MouseMoved += Hook_MouseMoved;
			hook.MouseDown += Hook_MouseDown;
			hook.MouseUp += Hook_MouseUp;
			hook.MouseWheel += Hook_MouseWheel;
			hook.Start();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			SetMousePositionLabels();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			hook?.Dispose();
		}

		private static (int X, int Y) GetMousePosition()
		{
			Point pos = Cursor.Position;
			return (pos.X, pos.Y);
		}

		private void SetMousePositionLabels()
		{
			(int X, int Y) = GetMousePosition();
			labelMousePositionX.Text = $"Mouse X: {X}";
			labelMousePositionY.Text = $"Mouse Y: {Y}";
		}

		private void Hook_MouseMoved(int x, int y)
		{
			// UI-Thread beachten
			if (InvokeRequired)
			{
				BeginInvoke(method: new Action(SetMousePositionLabels));
			}
			else
			{
				SetMousePositionLabels();
			}
		}

		private void Hook_MouseDown(MouseButtons btn, int x, int y)
		{
			labelStatus.Text = btn.ToString();
		}
		private void Hook_MouseUp(MouseButtons btn, int x, int y)
		{
			labelStatus.Text = btn.ToString();
		}
		private void Hook_MouseWheel(int delta, int x, int y)
		{
			labelStatus.Text = delta.ToString();
		}
	}
}