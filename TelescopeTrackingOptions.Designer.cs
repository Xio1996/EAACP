namespace EAACP
{
    partial class TelescopeTrackingOptions
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTTASCOMTelescope = new System.Windows.Forms.TextBox();
            this.cbTTUpdate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTelescope = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbStGraphic = new System.Windows.Forms.ComboBox();
            this.txtGraphicSize = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbColour = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTTASCOMTelescope);
            this.groupBox2.Controls.Add(this.cbTTUpdate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnTelescope);
            this.groupBox2.Location = new System.Drawing.Point(4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 99);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ASCOM Source";
            // 
            // txtTTASCOMTelescope
            // 
            this.txtTTASCOMTelescope.Location = new System.Drawing.Point(88, 18);
            this.txtTTASCOMTelescope.Multiline = true;
            this.txtTTASCOMTelescope.Name = "txtTTASCOMTelescope";
            this.txtTTASCOMTelescope.ReadOnly = true;
            this.txtTTASCOMTelescope.Size = new System.Drawing.Size(165, 47);
            this.txtTTASCOMTelescope.TabIndex = 47;
            // 
            // cbTTUpdate
            // 
            this.cbTTUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTTUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbTTUpdate.FormattingEnabled = true;
            this.cbTTUpdate.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbTTUpdate.Location = new System.Drawing.Point(80, 71);
            this.cbTTUpdate.Name = "cbTTUpdate";
            this.cbTTUpdate.Size = new System.Drawing.Size(53, 21);
            this.cbTTUpdate.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(139, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 45;
            this.label5.Text = "seconds";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Update Rate";
            // 
            // btnTelescope
            // 
            this.btnTelescope.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTelescope.Location = new System.Drawing.Point(6, 18);
            this.btnTelescope.Name = "btnTelescope";
            this.btnTelescope.Size = new System.Drawing.Size(75, 47);
            this.btnTelescope.TabIndex = 42;
            this.btnTelescope.Text = "Choose Scope";
            this.btnTelescope.UseVisualStyleBackColor = true;
            this.btnTelescope.Click += new System.EventHandler(this.btnTelescope_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbColour);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbStGraphic);
            this.groupBox3.Controls.Add(this.txtGraphicSize);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(5, 110);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 78);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tracking Graphic";
            // 
            // cbStGraphic
            // 
            this.cbStGraphic.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cbStGraphic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStGraphic.FormattingEnabled = true;
            this.cbStGraphic.Items.AddRange(new object[] {
            "cross",
            "circle",
            "ellipse",
            "square",
            "dotted-circle",
            "circled-cross",
            "dashed-square",
            "squared-dotted-circle",
            "crossed-circle",
            "target",
            "gear",
            "disk"});
            this.cbStGraphic.Location = new System.Drawing.Point(52, 47);
            this.cbStGraphic.Name = "cbStGraphic";
            this.cbStGraphic.Size = new System.Drawing.Size(107, 21);
            this.cbStGraphic.TabIndex = 30;
            // 
            // txtGraphicSize
            // 
            this.txtGraphicSize.Location = new System.Drawing.Point(54, 21);
            this.txtGraphicSize.Name = "txtGraphicSize";
            this.txtGraphicSize.Size = new System.Drawing.Size(35, 20);
            this.txtGraphicSize.TabIndex = 29;
            this.txtGraphicSize.Text = "70.3";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(27, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Size";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(195, 205);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(113, 205);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Shape";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Colour";
            // 
            // cbColour
            // 
            this.cbColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColour.FormattingEnabled = true;
            this.cbColour.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "White"});
            this.cbColour.Location = new System.Drawing.Point(161, 19);
            this.cbColour.Name = "cbColour";
            this.cbColour.Size = new System.Drawing.Size(91, 21);
            this.cbColour.TabIndex = 34;
            // 
            // TelescopeTrackingOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 235);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TelescopeTrackingOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Telescope Trailing Options";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TelescopeTrackingOptions_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbStGraphic;
        private System.Windows.Forms.TextBox txtGraphicSize;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbTTUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnTelescope;
        private System.Windows.Forms.TextBox txtTTASCOMTelescope;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbColour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}