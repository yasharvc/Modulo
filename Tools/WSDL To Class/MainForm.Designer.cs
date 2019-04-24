﻿namespace WSDL_To_Class
{
	partial class MainForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.WSDLUrlTextBox = new System.Windows.Forms.TextBox();
			this.WSDLDownloadButton = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.TypesListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(111, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "WEB Service URL:";
			// 
			// WSDLUrlTextBox
			// 
			this.WSDLUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.WSDLUrlTextBox.Location = new System.Drawing.Point(12, 29);
			this.WSDLUrlTextBox.Name = "WSDLUrlTextBox";
			this.WSDLUrlTextBox.Size = new System.Drawing.Size(635, 23);
			this.WSDLUrlTextBox.TabIndex = 1;
			this.WSDLUrlTextBox.Text = "http://shonizit:180/WmsService.svc";
			// 
			// WSDLDownloadButton
			// 
			this.WSDLDownloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.WSDLDownloadButton.Location = new System.Drawing.Point(653, 29);
			this.WSDLDownloadButton.Name = "WSDLDownloadButton";
			this.WSDLDownloadButton.Size = new System.Drawing.Size(75, 23);
			this.WSDLDownloadButton.TabIndex = 2;
			this.WSDLDownloadButton.Text = "Download";
			this.WSDLDownloadButton.UseVisualStyleBackColor = true;
			this.WSDLDownloadButton.Click += new System.EventHandler(this.WSDLDownloadButton_Click);
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BackColor = System.Drawing.Color.White;
			this.textBox1.Location = new System.Drawing.Point(12, 58);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(716, 168);
			this.textBox1.TabIndex = 3;
			// 
			// TypesListBox
			// 
			this.TypesListBox.FormattingEnabled = true;
			this.TypesListBox.ItemHeight = 16;
			this.TypesListBox.Location = new System.Drawing.Point(12, 233);
			this.TypesListBox.Name = "TypesListBox";
			this.TypesListBox.ScrollAlwaysVisible = true;
			this.TypesListBox.Size = new System.Drawing.Size(716, 84);
			this.TypesListBox.TabIndex = 4;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(740, 438);
			this.Controls.Add(this.TypesListBox);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.WSDLDownloadButton);
			this.Controls.Add(this.WSDLUrlTextBox);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox WSDLUrlTextBox;
		private System.Windows.Forms.Button WSDLDownloadButton;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ListBox TypesListBox;
	}
}

