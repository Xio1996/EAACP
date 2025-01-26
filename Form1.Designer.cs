namespace EAACP
{
    partial class frmCP
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCP));
            this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
            this.btnQuickObs = new System.Windows.Forms.Button();
            this.btnStelSync = new System.Windows.Forms.Button();
            this.btnDSA = new System.Windows.Forms.Button();
            this.btnAddtoAP = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.btnSearchOptions = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnInfoTextOption = new System.Windows.Forms.Button();
            this.btnSetInfoText = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.flpMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpMain
            // 
            this.flpMain.Controls.Add(this.btnQuickObs);
            this.flpMain.Controls.Add(this.btnStelSync);
            this.flpMain.Controls.Add(this.btnDSA);
            this.flpMain.Controls.Add(this.btnAddtoAP);
            this.flpMain.Controls.Add(this.panel1);
            this.flpMain.Controls.Add(this.panel2);
            this.flpMain.Controls.Add(this.btnConfig);
            this.flpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMain.Location = new System.Drawing.Point(0, 0);
            this.flpMain.Name = "flpMain";
            this.flpMain.Size = new System.Drawing.Size(154, 341);
            this.flpMain.TabIndex = 0;
            // 
            // btnQuickObs
            // 
            this.btnQuickObs.BackColor = System.Drawing.SystemColors.Control;
            this.btnQuickObs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuickObs.Location = new System.Drawing.Point(3, 3);
            this.btnQuickObs.Name = "btnQuickObs";
            this.btnQuickObs.Size = new System.Drawing.Size(146, 42);
            this.btnQuickObs.TabIndex = 0;
            this.btnQuickObs.Text = "Quick Observation";
            this.toolTip1.SetToolTip(this.btnQuickObs, "Logs observation for AP selected object");
            this.btnQuickObs.UseVisualStyleBackColor = true;
            this.btnQuickObs.Click += new System.EventHandler(this.btnQuickObs_Click);
            // 
            // btnStelSync
            // 
            this.btnStelSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStelSync.Location = new System.Drawing.Point(3, 51);
            this.btnStelSync.Name = "btnStelSync";
            this.btnStelSync.Size = new System.Drawing.Size(146, 42);
            this.btnStelSync.TabIndex = 1;
            this.btnStelSync.Text = "Stellarium Sync";
            this.toolTip1.SetToolTip(this.btnStelSync, "Syncs Stellarium\'s view with AP selected object");
            this.btnStelSync.UseVisualStyleBackColor = true;
            this.btnStelSync.Click += new System.EventHandler(this.btnStelSync_Click);
            // 
            // btnDSA
            // 
            this.btnDSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDSA.Location = new System.Drawing.Point(3, 99);
            this.btnDSA.Name = "btnDSA";
            this.btnDSA.Size = new System.Drawing.Size(146, 42);
            this.btnDSA.TabIndex = 2;
            this.btnDSA.Text = "SharpCap DSA";
            this.toolTip1.SetToolTip(this.btnDSA, "Creates SharpCap DSA on clipboard from AP selected object");
            this.btnDSA.UseVisualStyleBackColor = true;
            this.btnDSA.Click += new System.EventHandler(this.btnDSA_Click);
            // 
            // btnAddtoAP
            // 
            this.btnAddtoAP.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddtoAP.Location = new System.Drawing.Point(3, 147);
            this.btnAddtoAP.Name = "btnAddtoAP";
            this.btnAddtoAP.Size = new System.Drawing.Size(146, 42);
            this.btnAddtoAP.TabIndex = 3;
            this.btnAddtoAP.Text = "Add to AP";
            this.toolTip1.SetToolTip(this.btnAddtoAP, "Add Stellarium object to AstroPlanner");
            this.btnAddtoAP.UseVisualStyleBackColor = true;
            this.btnAddtoAP.Click += new System.EventHandler(this.btnAddtoAP_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClearSearch);
            this.panel1.Controls.Add(this.btnSearchOptions);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Location = new System.Drawing.Point(3, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(149, 42);
            this.panel1.TabIndex = 6;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSearch.Location = new System.Drawing.Point(71, 0);
            this.btnClearSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(34, 42);
            this.btnClearSearch.TabIndex = 8;
            this.btnClearSearch.Text = "X";
            this.toolTip1.SetToolTip(this.btnClearSearch, "Clear search results from Stellarium display");
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // btnSearchOptions
            // 
            this.btnSearchOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchOptions.Location = new System.Drawing.Point(106, 0);
            this.btnSearchOptions.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearchOptions.Name = "btnSearchOptions";
            this.btnSearchOptions.Size = new System.Drawing.Size(40, 42);
            this.btnSearchOptions.TabIndex = 7;
            this.btnSearchOptions.Text = "Opt";
            this.toolTip1.SetToolTip(this.btnSearchOptions, "Search options");
            this.btnSearchOptions.UseVisualStyleBackColor = true;
            this.btnSearchOptions.Click += new System.EventHandler(this.btnSearchOptions_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(0, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 42);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.toolTip1.SetToolTip(this.btnSearch, "Search AP around selected object and display in Stellarium");
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnInfoTextOption);
            this.panel2.Controls.Add(this.btnSetInfoText);
            this.panel2.Location = new System.Drawing.Point(3, 243);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(149, 42);
            this.panel2.TabIndex = 7;
            // 
            // btnInfoTextOption
            // 
            this.btnInfoTextOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfoTextOption.Location = new System.Drawing.Point(106, 0);
            this.btnInfoTextOption.Margin = new System.Windows.Forms.Padding(0);
            this.btnInfoTextOption.Name = "btnInfoTextOption";
            this.btnInfoTextOption.Size = new System.Drawing.Size(40, 42);
            this.btnInfoTextOption.TabIndex = 7;
            this.btnInfoTextOption.Text = "Opt";
            this.toolTip1.SetToolTip(this.btnInfoTextOption, "Object text options");
            this.btnInfoTextOption.UseVisualStyleBackColor = true;
            this.btnInfoTextOption.Click += new System.EventHandler(this.btnInfoTextOption_Click);
            // 
            // btnSetInfoText
            // 
            this.btnSetInfoText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetInfoText.Location = new System.Drawing.Point(0, 0);
            this.btnSetInfoText.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetInfoText.Name = "btnSetInfoText";
            this.btnSetInfoText.Size = new System.Drawing.Size(105, 42);
            this.btnSetInfoText.TabIndex = 6;
            this.btnSetInfoText.Text = "Object Text";
            this.toolTip1.SetToolTip(this.btnSetInfoText, "Set object text");
            this.btnSetInfoText.UseVisualStyleBackColor = true;
            this.btnSetInfoText.Click += new System.EventHandler(this.btnSetInfoText_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfig.Location = new System.Drawing.Point(3, 291);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(146, 42);
            this.btnConfig.TabIndex = 4;
            this.btnConfig.Text = "Configuration";
            this.toolTip1.SetToolTip(this.btnConfig, "Configuration settings");
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // frmCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(154, 341);
            this.Controls.Add(this.flpMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1087, 380);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(168, 88);
            this.Name = "frmCP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EAA CP (0.5b1)";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCP_FormClosing);
            this.flpMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpMain;
        private System.Windows.Forms.Button btnQuickObs;
        private System.Windows.Forms.Button btnStelSync;
        private System.Windows.Forms.Button btnDSA;
        private System.Windows.Forms.Button btnAddtoAP;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearchOptions;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnInfoTextOption;
        private System.Windows.Forms.Button btnSetInfoText;
    }
}

