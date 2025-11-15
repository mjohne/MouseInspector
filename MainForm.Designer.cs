namespace MouseInspector
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			labelMousePositionX = new Label();
			labelMousePositionY = new Label();
			timer = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			// 
			// labelMousePositionX
			// 
			labelMousePositionX.AutoSize = true;
			labelMousePositionX.Location = new Point(12, 9);
			labelMousePositionX.Name = "labelMousePositionX";
			labelMousePositionX.Size = new Size(56, 15);
			labelMousePositionX.TabIndex = 0;
			labelMousePositionX.Text = "Mouse X:";
			// 
			// labelMousePositionY
			// 
			labelMousePositionY.AutoSize = true;
			labelMousePositionY.Location = new Point(12, 24);
			labelMousePositionY.Name = "labelMousePositionY";
			labelMousePositionY.Size = new Size(56, 15);
			labelMousePositionY.TabIndex = 1;
			labelMousePositionY.Text = "Mouse Y:";
			// 
			// timer
			// 
			timer.Enabled = true;
			timer.Interval = 1;
			timer.Tick += Timer_Tick;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(264, 56);
			Controls.Add(labelMousePositionY);
			Controls.Add(labelMousePositionX);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "MouseInspector";
			Load += MainForm_Load;
			MouseMove += MainForm_MouseMove;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label labelMousePositionX;
		private Label labelMousePositionY;
		private System.Windows.Forms.Timer timer;
	}
}
