
namespace MouseInspector
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		public static (int X, int Y) GetMousePosition()
		{
			Point pos = Cursor.Position;
			return (pos.X, pos.Y);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
		}

		private void MainForm_MouseMove(object sender, MouseEventArgs e)
		{
			//labelMouseXCoordinate.Text = $"X: {e.X}";
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			labelMousePositionX.Text = $"Mouse X: {GetMousePosition().X}";
			labelMousePositionY.Text = $"Mouse Y: {GetMousePosition().Y}";
		}
	}
}
