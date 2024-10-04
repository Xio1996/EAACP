namespace EAACP
{
    partial class Config
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabConfig = new System.Windows.Forms.TabControl();
            this.tabAP = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAPPort = new System.Windows.Forms.TextBox();
            this.txtAPIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAuthentication = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabStel = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStelPort = new System.Windows.Forms.TextBox();
            this.txtStelIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabConfig.SuspendLayout();
            this.tabAP.SuspendLayout();
            this.tabStel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(149, 216);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(231, 216);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.tabAP);
            this.tabConfig.Controls.Add(this.tabStel);
            this.tabConfig.Location = new System.Drawing.Point(5, 5);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.SelectedIndex = 0;
            this.tabConfig.Size = new System.Drawing.Size(305, 207);
            this.tabConfig.TabIndex = 8;
            // 
            // tabAP
            // 
            this.tabAP.Controls.Add(this.label3);
            this.tabAP.Controls.Add(this.txtAPPort);
            this.tabAP.Controls.Add(this.txtAPIP);
            this.tabAP.Controls.Add(this.label2);
            this.tabAP.Controls.Add(this.txtAuthentication);
            this.tabAP.Controls.Add(this.label1);
            this.tabAP.Location = new System.Drawing.Point(4, 22);
            this.tabAP.Name = "tabAP";
            this.tabAP.Padding = new System.Windows.Forms.Padding(3);
            this.tabAP.Size = new System.Drawing.Size(297, 181);
            this.tabAP.TabIndex = 0;
            this.tabAP.Text = "AstroPlanner";
            this.tabAP.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "AP Port (default 8080)";
            // 
            // txtAPPort
            // 
            this.txtAPPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAPPort.Location = new System.Drawing.Point(9, 143);
            this.txtAPPort.MaxLength = 31;
            this.txtAPPort.Name = "txtAPPort";
            this.txtAPPort.Size = new System.Drawing.Size(73, 24);
            this.txtAPPort.TabIndex = 12;
            // 
            // txtAPIP
            // 
            this.txtAPIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAPIP.Location = new System.Drawing.Point(9, 89);
            this.txtAPIP.MaxLength = 31;
            this.txtAPIP.Name = "txtAPIP";
            this.txtAPIP.Size = new System.Drawing.Size(273, 24);
            this.txtAPIP.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "AP Hostname/IP (default localhost)";
            // 
            // txtAuthentication
            // 
            this.txtAuthentication.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthentication.Location = new System.Drawing.Point(9, 35);
            this.txtAuthentication.MaxLength = 31;
            this.txtAuthentication.Name = "txtAuthentication";
            this.txtAuthentication.Size = new System.Drawing.Size(273, 24);
            this.txtAuthentication.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "AP Authentication string (Max. 31 characters)";
            // 
            // tabStel
            // 
            this.tabStel.Controls.Add(this.label4);
            this.tabStel.Controls.Add(this.txtStelPort);
            this.tabStel.Controls.Add(this.txtStelIP);
            this.tabStel.Controls.Add(this.label5);
            this.tabStel.Location = new System.Drawing.Point(4, 22);
            this.tabStel.Name = "tabStel";
            this.tabStel.Padding = new System.Windows.Forms.Padding(3);
            this.tabStel.Size = new System.Drawing.Size(297, 181);
            this.tabStel.TabIndex = 1;
            this.tabStel.Text = "Stellarium";
            this.tabStel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "Stellarium Port (default 8090)";
            // 
            // txtStelPort
            // 
            this.txtStelPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStelPort.Location = new System.Drawing.Point(9, 89);
            this.txtStelPort.MaxLength = 31;
            this.txtStelPort.Name = "txtStelPort";
            this.txtStelPort.Size = new System.Drawing.Size(73, 24);
            this.txtStelPort.TabIndex = 16;
            // 
            // txtStelIP
            // 
            this.txtStelIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStelIP.Location = new System.Drawing.Point(9, 35);
            this.txtStelIP.MaxLength = 31;
            this.txtStelIP.Name = "txtStelIP";
            this.txtStelIP.Size = new System.Drawing.Size(273, 24);
            this.txtStelIP.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(237, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Stellarium Hostname/IP (default localhost)";
            // 
            // Config
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(313, 245);
            this.Controls.Add(this.tabConfig);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Config";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EAA CP Configuration";
            this.Load += new System.EventHandler(this.Config_Load);
            this.tabConfig.ResumeLayout(false);
            this.tabAP.ResumeLayout(false);
            this.tabAP.PerformLayout();
            this.tabStel.ResumeLayout(false);
            this.tabStel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabConfig;
        private System.Windows.Forms.TabPage tabAP;
        private System.Windows.Forms.TextBox txtAPPort;
        private System.Windows.Forms.TextBox txtAPIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAuthentication;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabStel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStelPort;
        private System.Windows.Forms.TextBox txtStelIP;
        private System.Windows.Forms.Label label5;
    }
}