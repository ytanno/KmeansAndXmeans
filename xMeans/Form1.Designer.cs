namespace xMeans
{
	partial class Form1
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.ReadFileButton = new System.Windows.Forms.Button();
			this.RndButton = new System.Windows.Forms.Button();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// ReadFileButton
			// 
			this.ReadFileButton.Location = new System.Drawing.Point(231, 12);
			this.ReadFileButton.Name = "ReadFileButton";
			this.ReadFileButton.Size = new System.Drawing.Size(102, 29);
			this.ReadFileButton.TabIndex = 4;
			this.ReadFileButton.Text = "ReadFile";
			this.ReadFileButton.UseVisualStyleBackColor = true;
			this.ReadFileButton.Click += new System.EventHandler(this.ReadFileButton_Click);
			// 
			// RndButton
			// 
			this.RndButton.Location = new System.Drawing.Point(12, 12);
			this.RndButton.Name = "RndButton";
			this.RndButton.Size = new System.Drawing.Size(61, 29);
			this.RndButton.TabIndex = 3;
			this.RndButton.Text = "Rndom";
			this.RndButton.UseVisualStyleBackColor = true;
			this.RndButton.Click += new System.EventHandler(this.RndBtn_Click);
			// 
			// chart1
			// 
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(12, 63);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(300, 300);
			this.chart1.TabIndex = 5;
			this.chart1.Text = "chart1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(340, 375);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.ReadFileButton);
			this.Controls.Add(this.RndButton);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button ReadFileButton;
		private System.Windows.Forms.Button RndButton;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
	}
}

