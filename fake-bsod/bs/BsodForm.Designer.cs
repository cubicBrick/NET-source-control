namespace bs
{
	partial class BsodForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BsodForm));
			this.bsodTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// bsodTextBox
			// 
			this.bsodTextBox.BackColor = System.Drawing.Color.Blue;
			this.bsodTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.bsodTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.bsodTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bsodTextBox.ForeColor = System.Drawing.Color.White;
			this.bsodTextBox.Location = new System.Drawing.Point(0, 0);
			this.bsodTextBox.Multiline = true;
			this.bsodTextBox.Name = "bsodTextBox";
			this.bsodTextBox.ReadOnly = true;
			this.bsodTextBox.Size = new System.Drawing.Size(615, 237);
			this.bsodTextBox.TabIndex = 0;
			this.bsodTextBox.TabStop = false;
			this.bsodTextBox.Text = resources.GetString("bsodTextBox.Text");
			// 
			// BsodForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Blue;
			this.ClientSize = new System.Drawing.Size(640, 480);
			this.ControlBox = false;
			this.Controls.Add(this.bsodTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BsodForm";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Not Entirely Real Blue Screen of Death";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox bsodTextBox;
	}
}

