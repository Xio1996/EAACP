using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EAACP
{
    public partial class SearchResults : Form
    {
        private Stellarium Stellarium = new Stellarium();
        private List<string[]> ResultsList = null;
        private DataTable ResultsTable = null;
        DataTable dt = new DataTable();
        DataTable dtUniqueCatalogues = new DataTable();
        int totalResults = 0;
        string stellariumDSOFilter;
        string displayMode = "All";

        public frmCP EAACP;

        public SearchResults()
        {
            InitializeComponent();
        }

        public List<string[]> Results
        {
            set
            {
                ResultsList = value;
            }
        }

        public DataTable ResultsDataTable
        {
            set
            {
                ResultsTable = value;
            }
        }

        public double CentreRA;
        public double CentreDec;
        public string CentreID;

        private void SearchResults_Load(object sender, EventArgs e)
        {
            Stellarium.ScriptFolder = Properties.Settings.Default.StScriptFolder;

            stellariumDSOFilter = Stellarium.GetStelProperty("NebulaMgr.catalogFilters");
            cbStellariumShowDSOImage.Checked = bool.Parse(Stellarium.GetStelProperty("actionShow_DSO_Textures", true));
            cbStellariumMinorBodyMarkers.Checked = bool.Parse(Stellarium.GetStelProperty("actionShow_Planets_ShowMinorBodyMarkers", true));

            if (ResultsList != null || ResultsTable != null)
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("Names");
                dt.Columns.Add("Type");
                dt.Columns.Add("Mag", typeof(double));
                dt.Columns.Add("Mag2", typeof(double));
                dt.Columns.Add("Dist Mpc", typeof(double));
                dt.Columns.Add("Galaxy Type");
                dt.Columns.Add("Size");
                dt.Columns.Add("Comp");
                dt.Columns.Add("PA", typeof(double));
                dt.Columns.Add("Sep", typeof(double));
                dt.Columns.Add("RA");
                dt.Columns.Add("Dec");
                dt.Columns.Add("dRA");
                dt.Columns.Add("dDec");
                dt.Columns.Add("Const");
                dt.Columns.Add("Catalogue");

                if (ResultsTable != null)
                {
                    dt = ResultsTable;
                }
                else
                {
                    foreach (string[] APObject in ResultsList)
                    {
                        DataRow row = dt.NewRow();
                        row["ID"] = APObject[0];
                        row["Names"] = APObject[1];
                        row["Type"] = APObject[2];

                        if (double.TryParse(APObject[3], out double Mag))
                        {
                            row["Mag"] = Mag;
                        }
                        else row["Mag"] = DBNull.Value;

                        if (double.TryParse(APObject[5], out double Dist))
                        {
                            row["Dist Mpc"] = Math.Round(Dist, 2);
                        }
                        else row["Dist Mpc"] = DBNull.Value;

                        row["Galaxy Type"] = APObject[4];
                        row["Catalogue"] = APObject[6];
                        row["RA"] = APObject[7];
                        row["Dec"] = APObject[8];
                        row["dRA"] = APObject[9];
                        row["dDec"] = APObject[10];
                        row["Size"] = APObject[11];
                        row["Const"] = APObject[13];

                        if (double.TryParse(APObject[12], out double PA))
                        {
                            if (PA == -999) row["PA"] = DBNull.Value;
                            else
                                row["PA"] = Math.Round(PA, 2);
                        }
                        else row["PA"] = DBNull.Value;

                        row["Const"] = APObject[13];
                        row["Comp"] = APObject[14];

                        if (double.TryParse(APObject[15], out double Sep))
                        {
                            if (Sep == -999) row["Sep"] = DBNull.Value;
                            else
                                row["Sep"] = Math.Round(Sep, 2);
                        }
                        else row["Sep"] = DBNull.Value;

                        if (double.TryParse(APObject[16], out double Mag2))
                        {
                            if (Mag2 == 999) row["Mag2"] = DBNull.Value;
                            else
                                row["Mag2"] = Math.Round(Mag2, 2);
                        }
                        else row["Mag2"] = DBNull.Value;

                        dt.Rows.Add(row);

                    }
                }


                // Fetch and display list of unique catalogues
                cbCataloguesFilter.SelectedIndexChanged -= cbCataloguesFilter_SelectedIndexChanged;

                dtUniqueCatalogues = Stellarium.UniqueCataloguesInSearchResults(dt);
                cbCataloguesFilter.DataSource = dtUniqueCatalogues;
                cbCataloguesFilter.DisplayMember = "Catalogue";

                cbCataloguesFilter.SelectedIndexChanged += cbCataloguesFilter_SelectedIndexChanged;


                dgvSearchResults.DataSource = dt;
                dgvSearchResults.Columns["dRA"].Visible = false;
                dgvSearchResults.Columns["dDec"].Visible = false;
                dgvSearchResults.Columns["Size"].Visible = true;
                dgvSearchResults.Columns["PA"].Visible = true;
                dgvSearchResults.Columns["Const"].Visible = true;

                dgvSearchResults.Sort(dgvSearchResults.Columns["Mag"], System.ComponentModel.ListSortDirection.Ascending);

                // Removes first selection columnm
                dgvSearchResults.RowHeadersVisible = false;

                // Freeze the important columns
                dgvSearchResults.Columns["ID"].Frozen = true;
                dgvSearchResults.Columns["Names"].Frozen = true;
                dgvSearchResults.Columns["Type"].Frozen = true;
                dgvSearchResults.Columns["Mag"].Frozen = true;

                // Resize to accomodate content
                dgvSearchResults.AutoResizeColumns();

                // Set the colour of the important columns
                dgvSearchResults.Columns["ID"].DefaultCellStyle.BackColor = Color.LightBlue;
                dgvSearchResults.Columns["Names"].DefaultCellStyle.BackColor = Color.LightBlue;
                dgvSearchResults.Columns["Type"].DefaultCellStyle.BackColor = Color.LightBlue;
                dgvSearchResults.Columns["Mag"].DefaultCellStyle.BackColor = Color.LightBlue;

                InitDataGridViewContextMenu(dgvSearchResults);

                totalResults = dt.Rows.Count;
                UpdateSearchInfo(totalResults);
            }


        }

        private void UpdateSearchInfo(int viewCount)
        {
            this.Text = $"Search Results: {totalResults} objects, Current View: {viewCount} objects";
        }

        private void DrawSelectedObjects()
        {
            DataTable Selected = new DataTable();

            Selected.Columns.Add("ID");
            Selected.Columns.Add("Names");
            Selected.Columns.Add("Type");
            Selected.Columns.Add("Mag", typeof(double));
            Selected.Columns.Add("Mag2", typeof(double));
            Selected.Columns.Add("Dist Mpc", typeof(double));
            Selected.Columns.Add("Galaxy Type");
            Selected.Columns.Add("Size");
            Selected.Columns.Add("Comp");
            Selected.Columns.Add("PA", typeof(double));
            Selected.Columns.Add("Sep", typeof(double));
            Selected.Columns.Add("RA");
            Selected.Columns.Add("Dec");
            Selected.Columns.Add("dRA");
            Selected.Columns.Add("dDec");
            Selected.Columns.Add("Const");
            Selected.Columns.Add("Catalogue");


            foreach (DataGridViewRow row in dgvSearchResults.SelectedRows)
            {
                DataRow SelectedRow = Selected.NewRow();
                SelectedRow["ID"] = row.Cells["ID"].Value;
                SelectedRow["Names"] = row.Cells["Names"].Value;
                SelectedRow["Type"] = row.Cells["Type"].Value;

                if (double.TryParse(row.Cells["Mag"].Value.ToString(), out double Mag))
                {
                    SelectedRow["Mag"] = Mag;
                }
                else SelectedRow["Mag"] = DBNull.Value;

                if (double.TryParse(row.Cells["Dist Mpc"].Value.ToString(), out double Dist))
                {
                    SelectedRow["Dist Mpc"] = Math.Round(Dist, 2);
                }
                else SelectedRow["Dist Mpc"] = DBNull.Value;

                if (double.TryParse(row.Cells["Mag2"].Value.ToString(), out double Mag2))
                {
                    SelectedRow["Mag2"] = Mag2;
                }
                else SelectedRow["Mag2"] = DBNull.Value;

                if (double.TryParse(row.Cells["PA"].Value.ToString(), out double PA))
                {
                    if (PA == -999) row.Cells["PA"].Value = DBNull.Value;
                    else
                        row.Cells["PA"].Value = Math.Round(PA, 2);
                }
                else row.Cells["PA"].Value = DBNull.Value;

                if (double.TryParse(row.Cells["Sep"].Value.ToString(), out double Sep))
                {
                    if (Sep == -999) row.Cells["Sep"].Value = DBNull.Value;
                    else
                        row.Cells["Sep"].Value = Math.Round(Sep, 2);
                }
                else row.Cells["Sep"].Value = DBNull.Value;


                SelectedRow["Galaxy Type"] = row.Cells["Galaxy Type"].Value;
                SelectedRow["Catalogue"] = row.Cells["Catalogue"].Value;
                SelectedRow["RA"] = row.Cells["RA"].Value;
                SelectedRow["Dec"] = row.Cells["Dec"].Value;
                SelectedRow["dRA"] = row.Cells["dRA"].Value;
                SelectedRow["dDec"] = row.Cells["dDec"].Value;
                SelectedRow["Size"] = row.Cells["Size"].Value;
                SelectedRow["PA"] = row.Cells["PA"].Value;
                SelectedRow["Const"] = row.Cells["Const"].Value;
                SelectedRow["Comp"] = row.Cells["Comp"].Value;

                Selected.Rows.Add(SelectedRow);
            }

            Stellarium.DrawObjects(Selected);

            UpdateSearchInfo(dgvSearchResults.SelectedRows.Count);
        }

        private void btnDrawSelection_Click(object sender, EventArgs e)
        {
            DrawSelectedObjects();
        }

        private void ResetCatalogueFilter()
        {
            cbCataloguesFilter.SelectedIndexChanged -= cbCataloguesFilter_SelectedIndexChanged;
            cbCataloguesFilter.SelectedIndex = 0;
            cbCataloguesFilter.SelectedIndexChanged += cbCataloguesFilter_SelectedIndexChanged;
        }

        private void btnPlotAll_Click(object sender, EventArgs e)
        {
            displayMode = "All";

            // Stop the catalogues filter from firing
            ResetCatalogueFilter();

            // Remove any filtering
            dt.DefaultView.RowFilter = "";
            Stellarium.DrawObjects(dt);

            UpdateSearchInfo(totalResults);
        }

        private void btnAddToAP_Click(object sender, EventArgs e)
        {
            List<APCmdObject> apObjects = new List<APCmdObject>();

            DialogResult result = MessageBox.Show("Adding many objects to AstroPlanner, can take sometime and cannot be cancelled. Do you wish to continue?", "Add Objects to AstroPlanner", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                return;
            }

            if (dgvSearchResults.SelectedRows.Count == 0)
            {
                EAACP.Speak("No objects selected");
                return;
            }

            foreach (DataGridViewRow row in dgvSearchResults.SelectedRows)
            {
                APCmdObject obj = new APCmdObject();
                obj.ID = row.Cells["ID"].Value.ToString();
                obj.Name = row.Cells["Names"].Value.ToString();
                obj.Type = row.Cells["Type"].Value.ToString();
                obj.RA2000 = double.Parse(row.Cells["dRA"].Value.ToString());
                obj.Dec2000 = double.Parse(row.Cells["dDec"].Value.ToString());
                obj.Catalogue = row.Cells["Catalogue"].Value.ToString();
                obj.Distance = row.Cells["Dist Mpc"].Value.ToString();
                obj.GalaxyType = row.Cells["Galaxy Type"].Value.ToString();
                obj.Size = row.Cells["Size"].Value.ToString();
                obj.Constellation = row.Cells["Const"].Value.ToString();

                if (double.TryParse(row.Cells["Mag"].Value.ToString(), out double Mag))
                {
                    obj.Magnitude = Mag;
                }

                if (double.TryParse(row.Cells["Mag2"].Value.ToString(), out double Mag2))
                {
                    obj.Magnitude2 = Mag2;
                }

                if (double.TryParse(row.Cells["PA"].Value.ToString(), out double PA))
                {
                    obj.PosAngle = PA;
                }

                if (double.TryParse(row.Cells["Sep"].Value.ToString(), out double Sep))
                {
                    obj.Separation = Sep;
                }

                apObjects.Add(obj);
            }

            APPutCmd aPPutCmd = new APPutCmd();
            aPPutCmd.script = "EAAControl2";
            aPPutCmd.parameters = new APPutCmdParams();
            aPPutCmd.parameters.Cmd = 2;
            aPPutCmd.parameters.Option = 1;
            aPPutCmd.parameters.Objects = apObjects;

            string sOut = EAACP.APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APPutCmd>(aPPutCmd)));

            EAACP.Speak(dgvSearchResults.SelectedRows.Count.ToString() + " Objects added");

        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            using (StelFOVOptions frmOpt = new StelFOVOptions())
            {
                frmOpt.Mode = 1;
                frmOpt.TopMost = true;
                if (frmOpt.ShowDialog() == DialogResult.OK)
                {
                    // If the user hs changed the display attributes then update the current plot selection on ext.
                    switch (displayMode)
                    {
                        case "All":
                            Stellarium.DrawObjects(dt);
                            break;
                        case "Filtered":
                            DrawCatalogueFiltered();
                            break;
                        case "Selected":
                            DrawSelectedObjects();
                            break;
                    }
                }
            }
        }

        private void btnRecentre_Click(object sender, EventArgs e)
        {
            Stellarium.SyncStellariumToAPObject(CentreID, CentreRA.ToString(), CentreDec.ToString(), "");
        }

        private void btnCentreSelected_Click(object sender, EventArgs e)
        {
            if (dgvSearchResults.SelectedRows.Count > 0)
            {
                double RA = double.Parse(dgvSearchResults.SelectedRows[0].Cells["dRA"].Value.ToString());
                double Dec = double.Parse(dgvSearchResults.SelectedRows[0].Cells["dDec"].Value.ToString());
                Stellarium.SyncStellariumToPosition(RA, Dec);
            }
        }

        private void DrawCatalogueFiltered()
        {
            displayMode = "Filtered";

            string apCatalogue = cbCataloguesFilter.Text;
            if (apCatalogue == "All Catalogues")
            {
                dt.DefaultView.RowFilter = "";
                Stellarium.DrawObjects(dt);
                UpdateSearchInfo(totalResults);
            }
            else
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = "Catalogue = '" + apCatalogue + "'";

                Stellarium.DrawObjects(dv.ToTable());

                UpdateSearchInfo(dv.ToTable().Rows.Count);
            }
        }

        private void cbCataloguesFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawCatalogueFiltered();
        }

        private void btnAllCats_Click(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "0");
        }

        private void btnDSOStandard_Click(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "7");
        }

        private void btnDSOAll_Click(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "255852279");
        }

        private void btnClearPlot_Click(object sender, EventArgs e)
        {
            displayMode = "All";
            Stellarium.ClearObjects();

            // Stop the catalogues filter from firing
            ResetCatalogueFilter();

            UpdateSearchInfo(0);
        }

        private void SearchResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", stellariumDSOFilter);
        }

        private void cbStellariumShowDSOImage_CheckedChanged(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("actionShow_DSO_Textures", cbStellariumShowDSOImage.Checked.ToString());
        }

        private void cbStellariumMinorBodyMarkers_CheckedChanged(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("actionShow_Planets_ShowMinorBodyMarkers", cbStellariumMinorBodyMarkers.Checked.ToString());
        }

        private void cbStellariumSatellites_CheckStateChanged(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("actionShow_Satellite_Hints", cbStellariumSatellites.Checked.ToString());
        }

        // add fields
        private ContextMenuStrip dgvContextMenu;
        private ToolStripMenuItem miCopyCell;
        private ToolStripMenuItem miCopyTable;
        private ToolStripMenuItem miCopySelectedRows;
        private ToolStripMenuItem miSelectStarSystem;

        // helper to check the row meets the "Dbl" + (Washington|WDS) requirement
        private bool IsDoubleWDSRow(DataGridViewRow row)
        {
            if (row == null) return false;

            var typeCell = row.Cells["Type"]?.Value;
            var catCell = row.Cells["Catalogue"]?.Value;
            if (typeCell == null || catCell == null) return false;

            string type = typeCell.ToString().Trim();
            string cat = catCell.ToString();

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(cat)) return false;

            // Accept exact "Dbl" or values that start with "Dbl" (case-insensitive)
            if (!type.Equals("Dbl", StringComparison.OrdinalIgnoreCase) &&
                !type.StartsWith("Dbl", StringComparison.OrdinalIgnoreCase))
                return false;

            // Catalogue must contain "Washington" or "WDS" (case-insensitive)
            if (cat.IndexOf("Washington", StringComparison.OrdinalIgnoreCase) >= 0 ||
                cat.IndexOf("WDS", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            return false;
        }

        // call this from the form constructor after InitializeComponent()
        private void InitDataGridViewContextMenu(DataGridView dgv)
        {
            dgvContextMenu = new ContextMenuStrip();
            miCopyCell = new ToolStripMenuItem("Copy Cell", null, CopyCell_Click);
            miCopySelectedRows = new ToolStripMenuItem("Copy Selected Rows", null, CopySelectedRows_Click);
            miCopyTable = new ToolStripMenuItem("Copy Table", null, CopyTable_Click);

            miSelectStarSystem = new ToolStripMenuItem("Show Star System", null, SelectStarSystem_Click);

            // include the new "Copy Selected Rows" menu item and keep logical separators
            dgvContextMenu.Items.AddRange(new ToolStripItem[] {
                miCopyCell,
                miCopySelectedRows,
                miCopyTable,
                new ToolStripSeparator(),
                miSelectStarSystem
            });

            dgv.ContextMenuStrip = dgvContextMenu;

            dgv.MouseDown += Dgv_MouseDown;               // detect right-click and select cell/row
            dgvContextMenu.Opening += DgvContextMenu_Opening;
        }

        private void Dgv_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var dgv = (DataGridView)sender;
            var hit = dgv.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                int rowIndex = hit.RowIndex;
                int colIndex = hit.ColumnIndex;
                if (rowIndex < 0 || colIndex < 0)
                {
                    // clicked invalid cell area -> clear selection
                    dgv.ClearSelection();
                    return;
                }

                var clickedRow = dgv.Rows[rowIndex];

                if (clickedRow.Selected)
                {
                    // Row under cursor already selected -> keep entire selection,
                    // just make clicked cell the current cell so context menu operations use it.
                    try
                    {
                        dgv.CurrentCell = dgv[colIndex, rowIndex];
                    }
                    catch { /* ignore if cell cannot be made current */ }
                }
                else
                {
                    // Clicked an unselected row -> select only that row
                    dgv.ClearSelection();
                    clickedRow.Selected = true;
                    try
                    {
                        dgv.CurrentCell = dgv[colIndex, rowIndex];
                    }
                    catch { /* ignore */ }
                }
            }
            else if (hit.Type == DataGridViewHitTestType.RowHeader)
            {
                int rowIndex = hit.RowIndex;
                if (rowIndex >= 0)
                {
                    var clickedRow = dgv.Rows[rowIndex];
                    if (!clickedRow.Selected)
                    {
                        dgv.ClearSelection();
                        clickedRow.Selected = true;
                    }
                    // set current cell to first non-new cell in the row if possible
                    for (int c = 0; c < dgv.ColumnCount; c++)
                    {
                        if (!dgv.Rows[rowIndex].IsNewRow)
                        {
                            try
                            {
                                dgv.CurrentCell = dgv[c, rowIndex];
                                break;
                            }
                            catch { }
                        }
                    }
                }
            }
            else
            {
                // clicked outside cells -> clear selection
                dgv.ClearSelection();
            }
        }
        private void DgvContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var cms = (ContextMenuStrip)sender;
            var dgv = (DataGridView)cms.SourceControl;

            miCopyCell.Enabled = dgv.CurrentCell != null;
            miCopyTable.Enabled = dgv.Rows.Count > 0;
            miCopySelectedRows.Enabled = dgv.SelectedRows.Count > 0;

            // Enable "Select as Star System" only when at least one of the selected rows (or current row if none selected)
            // matches: Type is Dbl and Catalogue contains "Washington" or "WDS"
            IEnumerable<DataGridViewRow> rowsToCheck;
            if (dgv.SelectedRows.Count > 0)
            {
                rowsToCheck = dgv.SelectedRows.Cast<DataGridViewRow>();
            }
            else if (dgv.CurrentRow != null)
            {
                rowsToCheck = new[] { dgv.CurrentRow };
            }
            else
            {
                rowsToCheck = Enumerable.Empty<DataGridViewRow>();
            }

            // Only one row selected and it is Dbl+WDS.
            miSelectStarSystem.Enabled = dgvSearchResults.SelectedRows.Count ==1 && rowsToCheck.Any(r => IsDoubleWDSRow(r)) ;
        }

        private void CopyCell_Click(object sender, EventArgs e)
        {
            var dgv = (DataGridView)dgvContextMenu.SourceControl;
            var cell = dgv.CurrentCell;
            if (cell != null)
            {
                Clipboard.SetText(cell.FormattedValue?.ToString() ?? "");
            }
        }

        private void CopyTable_Click(object sender, EventArgs e)
        {
            var dgv = (DataGridView)dgvContextMenu.SourceControl;
            var sb = new StringBuilder();

            // optional: include header row
            var headers = dgv.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText);
            sb.AppendLine(string.Join("\t", headers));

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                var values = row.Cells.Cast<DataGridViewCell>().Select(c => c.FormattedValue?.ToString() ?? "");
                sb.AppendLine(string.Join("\t", values));
            }

            Clipboard.SetText(sb.ToString());
        }
        private void CopySelectedRows_Click(object sender, EventArgs e)
        {
            var dgv = (DataGridView)dgvContextMenu.SourceControl;
            var sb = new StringBuilder();

            // include header row once
            var headers = dgv.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText);
            sb.AppendLine(string.Join("\t", headers));

            // SelectedRows collection is not guaranteed to be in display order.
            // Order by row index so output is top-to-bottom.
            var selectedRows = dgv.SelectedRows.Cast<DataGridViewRow>()
                                   .Where(r => !r.IsNewRow)
                                   .OrderBy(r => r.Index);

            foreach (DataGridViewRow row in selectedRows)
            {
                var values = row.Cells.Cast<DataGridViewCell>().Select(c => c.FormattedValue?.ToString() ?? "");
                sb.AppendLine(string.Join("\t", values));
            }

            Clipboard.SetText(sb.ToString());
        }

        private void SelectStarSystem_Click(object sender, EventArgs e)
        {
            // Determine source row: prefer first selected row, otherwise current row
            DataGridViewRow sourceRow = null;
            if (dgvSearchResults.SelectedRows.Count > 0)
                sourceRow = dgvSearchResults.SelectedRows[0];
            else if (dgvSearchResults.CurrentRow != null)
                sourceRow = dgvSearchResults.CurrentRow;

            if (sourceRow == null) return;

            var idObj = sourceRow.Cells["ID"]?.Value;
            var typeObj = sourceRow.Cells["Type"]?.Value;
            var catObj = sourceRow.Cells["Catalogue"]?.Value;
            if (idObj == null || typeObj == null || catObj == null) return;

            string sourceID = idObj.ToString().Trim();
            string sourceType = typeObj.ToString().Trim();
            string sourceCatalogue = catObj.ToString().Trim();

            if (string.IsNullOrEmpty(sourceID) || string.IsNullOrEmpty(sourceType) || string.IsNullOrEmpty(sourceCatalogue))
                return;

            // Gather matching rows (match ID, Type and Catalogue). Use case-insensitive comparison for type/catalogue.
            var matchingRows = new List<DataRow>();
            var otherRows = new List<DataRow>();

            foreach (DataRow dr in dt.Rows)
            {
                var drID = dr["ID"]?.ToString();
                var drType = dr["Type"]?.ToString();
                var drCat = dr["Catalogue"]?.ToString();

                if (drID == null || drType == null || drCat == null)
                {
                    otherRows.Add(dr);
                    continue;
                }

                bool idMatch = string.Equals(drID.Trim(), sourceID, StringComparison.OrdinalIgnoreCase);
                bool typeMatch = string.Equals(drType.Trim(), sourceType, StringComparison.OrdinalIgnoreCase);
                bool catMatch = string.Equals(drCat.Trim(), sourceCatalogue, StringComparison.OrdinalIgnoreCase);

                if (idMatch && typeMatch && catMatch)
                    matchingRows.Add(dr);
                else
                    otherRows.Add(dr);
            }

            if (matchingRows.Count == 0)
            {
                MessageBox.Show("No matching rows found for the selected ID/Type/Catalogue.", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Build new DataTable with matching rows first (preserve original order)
            var newDt = dt.Clone();
            foreach (var dr in matchingRows)
                newDt.ImportRow(dr);
            foreach (var dr in otherRows)
                newDt.ImportRow(dr);

            // Replace underlying data and rebind
            dt = newDt;
            dgvSearchResults.DataSource = dt;

            // Reapply expected column visibility / formatting (match existing layout)
            if (dgvSearchResults.Columns.Contains("dRA")) dgvSearchResults.Columns["dRA"].Visible = false;
            if (dgvSearchResults.Columns.Contains("dDec")) dgvSearchResults.Columns["dDec"].Visible = false;
            if (dgvSearchResults.Columns.Contains("Size")) dgvSearchResults.Columns["Size"].Visible = true;
            if (dgvSearchResults.Columns.Contains("PA")) dgvSearchResults.Columns["PA"].Visible = true;
            if (dgvSearchResults.Columns.Contains("Const")) dgvSearchResults.Columns["Const"].Visible = true;

            dgvSearchResults.RowHeadersVisible = false;
            if (dgvSearchResults.Columns.Contains("ID")) dgvSearchResults.Columns["ID"].Frozen = true;
            if (dgvSearchResults.Columns.Contains("Names")) dgvSearchResults.Columns["Names"].Frozen = true;
            if (dgvSearchResults.Columns.Contains("Type")) dgvSearchResults.Columns["Type"].Frozen = true;
            if (dgvSearchResults.Columns.Contains("Mag")) dgvSearchResults.Columns["Mag"].Frozen = true;

            if (dgvSearchResults.Columns.Contains("ID")) dgvSearchResults.Columns["ID"].DefaultCellStyle.BackColor = Color.LightBlue;
            if (dgvSearchResults.Columns.Contains("Names")) dgvSearchResults.Columns["Names"].DefaultCellStyle.BackColor = Color.LightBlue;
            if (dgvSearchResults.Columns.Contains("Type")) dgvSearchResults.Columns["Type"].DefaultCellStyle.BackColor = Color.LightBlue;
            if (dgvSearchResults.Columns.Contains("Mag")) dgvSearchResults.Columns["Mag"].DefaultCellStyle.BackColor = Color.LightBlue;

            dgvSearchResults.AutoResizeColumns();

            // Select moved rows at top and set first match as current cell
            DataGridViewRow firstMatch = null;
            foreach (DataGridViewRow row in dgvSearchResults.Rows)
            {
                if (row.IsNewRow) continue;
                var rowID = row.Cells["ID"]?.Value?.ToString();
                var rowType = row.Cells["Type"]?.Value?.ToString();
                var rowCat = row.Cells["Catalogue"]?.Value?.ToString();

                bool isMatch = rowID != null && rowType != null && rowCat != null &&
                               string.Equals(rowID.Trim(), sourceID, StringComparison.OrdinalIgnoreCase) &&
                               string.Equals(rowType.Trim(), sourceType, StringComparison.OrdinalIgnoreCase) &&
                               string.Equals(rowCat.Trim(), sourceCatalogue, StringComparison.OrdinalIgnoreCase);

                row.Selected = isMatch;

                if (isMatch && firstMatch == null)
                    firstMatch = row;
            }

            if (firstMatch != null)
            {
                for (int c = 0; c < dgvSearchResults.ColumnCount; c++)
                {
                    try
                    {
                        dgvSearchResults.CurrentCell = firstMatch.Cells[c];
                        break;
                    }
                    catch { }
                }
            }

            UpdateSearchInfo(dgvSearchResults.SelectedRows.Count);
        }
    }
}
