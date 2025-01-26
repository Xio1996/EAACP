namespace EAACP
{
    partial class ObjectTextOptions
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
            this.gbTextDisplay = new System.Windows.Forms.GroupBox();
            this.gbOBS = new System.Windows.Forms.GroupBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnFilePath = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtManualEntry = new System.Windows.Forms.TextBox();
            this.btnClearManualText = new System.Windows.Forms.Button();
            this.chkID = new System.Windows.Forms.CheckBox();
            this.chkName = new System.Windows.Forms.CheckBox();
            this.chkType = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaxChars = new System.Windows.Forms.TextBox();
            this.chkConstellation = new System.Windows.Forms.CheckBox();
            this.chkMagnitude = new System.Windows.Forms.CheckBox();
            this.gbTextDisplay.SuspendLayout();
            this.gbOBS.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTextDisplay
            // 
            this.gbTextDisplay.Controls.Add(this.chkMagnitude);
            this.gbTextDisplay.Controls.Add(this.chkConstellation);
            this.gbTextDisplay.Controls.Add(this.txtMaxChars);
            this.gbTextDisplay.Controls.Add(this.label1);
            this.gbTextDisplay.Controls.Add(this.chkType);
            this.gbTextDisplay.Controls.Add(this.chkName);
            this.gbTextDisplay.Controls.Add(this.chkID);
            this.gbTextDisplay.Location = new System.Drawing.Point(12, 12);
            this.gbTextDisplay.Name = "gbTextDisplay";
            this.gbTextDisplay.Size = new System.Drawing.Size(300, 68);
            this.gbTextDisplay.TabIndex = 0;
            this.gbTextDisplay.TabStop = false;
            this.gbTextDisplay.Text = "Object Text Display";
            // 
            // gbOBS
            // 
            this.gbOBS.Controls.Add(this.btnFilePath);
            this.gbOBS.Controls.Add(this.txtFilePath);
            this.gbOBS.Location = new System.Drawing.Point(12, 89);
            this.gbOBS.Name = "gbOBS";
            this.gbOBS.Size = new System.Drawing.Size(300, 54);
            this.gbOBS.TabIndex = 1;
            this.gbOBS.TabStop = false;
            this.gbOBS.Text = "Object Text Filepath (OBS)";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(7, 20);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(237, 20);
            this.txtFilePath.TabIndex = 0;
            // 
            // btnFilePath
            // 
            this.btnFilePath.Location = new System.Drawing.Point(250, 19);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(43, 23);
            this.btnFilePath.TabIndex = 1;
            this.btnFilePath.Text = "...";
            this.btnFilePath.UseVisualStyleBackColor = true;
            this.btnFilePath.Click += new System.EventHandler(this.btnFilePath_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClearManualText);
            this.groupBox1.Controls.Add(this.txtManualEntry);
            this.groupBox1.Location = new System.Drawing.Point(12, 154);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 98);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manual Text";
            // 
            // txtManualEntry
            // 
            this.txtManualEntry.Location = new System.Drawing.Point(7, 20);
            this.txtManualEntry.Multiline = true;
            this.txtManualEntry.Name = "txtManualEntry";
            this.txtManualEntry.Size = new System.Drawing.Size(286, 44);
            this.txtManualEntry.TabIndex = 0;
            // 
            // btnClearManualText
            // 
            this.btnClearManualText.Location = new System.Drawing.Point(7, 70);
            this.btnClearManualText.Name = "btnClearManualText";
            this.btnClearManualText.Size = new System.Drawing.Size(43, 23);
            this.btnClearManualText.TabIndex = 2;
            this.btnClearManualText.Text = "Clear";
            this.btnClearManualText.UseVisualStyleBackColor = true;
            this.btnClearManualText.Click += new System.EventHandler(this.btnClearManualText_Click);
            // 
            // chkID
            // 
            this.chkID.AutoSize = true;
            this.chkID.Location = new System.Drawing.Point(7, 19);
            this.chkID.Name = "chkID";
            this.chkID.Size = new System.Drawing.Size(37, 17);
            this.chkID.TabIndex = 0;
            this.chkID.Text = "ID";
            this.chkID.UseVisualStyleBackColor = true;
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.Location = new System.Drawing.Point(50, 19);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(54, 17);
            this.chkName.TabIndex = 1;
            this.chkName.Text = "Name";
            this.chkName.UseVisualStyleBackColor = true;
            // 
            // chkType
            // 
            this.chkType.AutoSize = true;
            this.chkType.Location = new System.Drawing.Point(205, 19);
            this.chkType.Name = "chkType";
            this.chkType.Size = new System.Drawing.Size(50, 17);
            this.chkType.TabIndex = 2;
            this.chkType.Text = "Type";
            this.chkType.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(237, 259);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(156, 259);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Maximum Characters";
            // 
            // txtMaxChars
            // 
            this.txtMaxChars.Location = new System.Drawing.Point(229, 40);
            this.txtMaxChars.Name = "txtMaxChars";
            this.txtMaxChars.Size = new System.Drawing.Size(62, 20);
            this.txtMaxChars.TabIndex = 5;
            this.txtMaxChars.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxChars_KeyPress);
            // 
            // chkConstellation
            // 
            this.chkConstellation.AutoSize = true;
            this.chkConstellation.Location = new System.Drawing.Point(111, 19);
            this.chkConstellation.Name = "chkConstellation";
            this.chkConstellation.Size = new System.Drawing.Size(86, 17);
            this.chkConstellation.TabIndex = 6;
            this.chkConstellation.Text = "Constellation";
            this.chkConstellation.UseVisualStyleBackColor = true;
            // 
            // chkMagnitude
            // 
            this.chkMagnitude.AutoSize = true;
            this.chkMagnitude.Location = new System.Drawing.Point(6, 41);
            this.chkMagnitude.Name = "chkMagnitude";
            this.chkMagnitude.Size = new System.Drawing.Size(76, 17);
            this.chkMagnitude.TabIndex = 7;
            this.chkMagnitude.Text = "Magnitude";
            this.chkMagnitude.UseVisualStyleBackColor = true;
            // 
            // ObjectTextOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(324, 290);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbOBS);
            this.Controls.Add(this.gbTextDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectTextOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Object Text Options";
            this.Load += new System.EventHandler(this.ObjectTextOptions_Load);
            this.gbTextDisplay.ResumeLayout(false);
            this.gbTextDisplay.PerformLayout();
            this.gbOBS.ResumeLayout(false);
            this.gbOBS.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTextDisplay;
        private System.Windows.Forms.GroupBox gbOBS;
        private System.Windows.Forms.Button btnFilePath;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClearManualText;
        private System.Windows.Forms.TextBox txtManualEntry;
        private System.Windows.Forms.CheckBox chkType;
        private System.Windows.Forms.CheckBox chkName;
        private System.Windows.Forms.CheckBox chkID;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaxChars;
        private System.Windows.Forms.CheckBox chkMagnitude;
        private System.Windows.Forms.CheckBox chkConstellation;
    }
}