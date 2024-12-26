using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Timers;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Net.Http;
using System.Data;
using System.Drawing;

namespace EAACP
{
    public partial class frmCP : Form
    {
        // Error handling class
        private EAAError aAError = new EAAError();

        // AstroPlanner Helper class - replaces AP functions in EAACtrl Panel
        private APHelper APHelper = new APHelper();

        // Stellarium Communication and processing class
        private Stellarium Stellarium = new Stellarium();

        // HTTP Client
        private static readonly HttpClient httpClient = new HttpClient();
        private string sMsg = "";

        private static System.Timers.Timer aTimer;

        private void OnTimedEvent2(Object source, ElapsedEventArgs e)
        {
            try
            {
                aTimer.Enabled = false;

                string sOut = APRunScript(0, 0,"");

            }
            catch (Exception) { }

        }

        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += OnTimedEvent2;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void EnableButtons(bool enabled)
        {
            foreach(Control control in this.Controls)
            control.Enabled = enabled;
        }

        public frmCP()
        {
            InitializeComponent();
            InitializeFlowLayoutPanel();

            Stellarium.IPAddress = Properties.Settings.Default.StelIP;
            Stellarium.Port = Properties.Settings.Default.StelPort;
            Stellarium.ScriptFolder = Properties.Settings.Default.StScriptFolder;

            if (Properties.Settings.Default.YPos == -1)
            {
                this.CenterToScreen();
            }
            else
            {
                this.Width = Properties.Settings.Default.frmWidth;
                this.Height = Properties.Settings.Default.frmHeight;

                if (Screen.PrimaryScreen.Bounds.Width > Properties.Settings.Default.XPos + this.Width)
                {
                    this.Left = Properties.Settings.Default.XPos;
                    if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height > Properties.Settings.Default.YPos + this.Height)
                    {
                        this.Top = Properties.Settings.Default.YPos;
                    }
                    else
                    {
                        this.CenterToScreen();
                    }
                }
                else
                {
                    this.CenterToScreen();
                }
            }

            SetTimer();

        }

        private void frmCP_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.XPos = this.Left;
            Properties.Settings.Default.YPos = this.Top;
            Properties.Settings.Default.frmWidth = this.Width;
            Properties.Settings.Default.frmHeight = this.Height;
            Properties.Settings.Default.Save();
        }

        private Control draggedControl;
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;

        private void InitializeFlowLayoutPanel()
        {
            flpMain.AllowDrop = true;
            foreach (Control control in flpMain.Controls)
            {
                control.MouseDown += Control_MouseDown;
                control.MouseMove += Control_MouseMove;
                control.MouseUp += Control_MouseUp;

                if (control is Panel)
                {
                    foreach (Control subControl in control.Controls)
                    {
                        subControl.MouseDown += Control_MouseDown;
                        subControl.MouseMove += Control_MouseMove;
                        subControl.MouseUp += Control_MouseUp;
                    }
                }
            }
            flpMain.DragEnter += flpMain_DragEnter;
            flpMain.DragDrop += flpMain_DragDrop;
            flpMain.DragOver += flpMain_DragOver;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                draggedControl = sender as Control;
                DoDragDrop(draggedControl, DragDropEffects.Move);
            }
            else
            {
                isDragging = false;
                OnControlClick(sender, e);
            }
        }

        private void OnControlClick(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null && control is Button)
            {
                ((Button)control).PerformClick();
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentScreenPos = ((Control)sender).Parent.PointToScreen(e.Location);
                Point newLocation = new Point(
                    currentScreenPos.X - dragStartPoint.X,
                    currentScreenPos.Y - dragStartPoint.Y);
                if (((Control)sender).Parent is Panel)
                {
                    ((Control)sender).Parent.Location = newLocation;
                }
                else 
                {
                   this.Location = newLocation;
                }
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isDragging = false;
            }
        }

        private void flpMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Control)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void flpMain_DragDrop(object sender, DragEventArgs e)
        {
            Point point = flpMain.PointToClient(new Point(e.X, e.Y));
            Control target = flpMain.GetChildAtPoint(point);

            int targetIndex = flpMain.Controls.GetChildIndex(target, false);
            if (!(draggedControl.Parent is FlowLayoutPanel))
            {
                flpMain.Controls.SetChildIndex(draggedControl.Parent, targetIndex);
            }
            else
            {
                flpMain.Controls.SetChildIndex(draggedControl, targetIndex);
            }
            flpMain.Invalidate();
        }

        private void flpMain_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void Speak(string Speech)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak(Speech);
        }
        
        private string GetRequest(string url)
        {
            string result = "";
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = httpClient.GetStringAsync(url).GetAwaiter().GetResult();

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = $"Request to {url} {elapsedTime}\r\n";
            }
            catch (HttpRequestException e)
            {
                sMsg = $"Request {url} ERROR {e.Message}\r\n";
                result = "exception";
            }

            return result;
        }

        public string APExecuteScript(string ScriptPayload)
        {
            string result = "";

            string apWebServices = $"http://{Properties.Settings.Default.APIP}:{Properties.Settings.Default.APPort}?cmd=launch&auth={Properties.Settings.Default.Auth}&cmdformat=json&responseformat=json&payload=";
            apWebServices += ScriptPayload;

            try
            {
                aTimer.Enabled = false;
                result = GetRequest(apWebServices);
            }
            catch (Exception) { }

            aTimer.Enabled = true;
            return result;
        }

        private string APRunScript(int Cmd, int Option, string Notes)
        {
            string sOut = "";
            try
            {
                APGetCmd getCmd = new APGetCmd();
                getCmd.script = "EAACP";
                getCmd.parameters = new APGetCmdParams();
                getCmd.parameters.Cmd = Cmd;
                getCmd.parameters.Option = Option;
                getCmd.parameters.Params = Notes;

                sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
            }
            catch (Exception) { }

            return sOut; // Nothing selected
        }

        private APCmdObject APGetSelectedObject()
        {
            try
            {
                aAError.Reset();

                APGetCmd getCmd = new APGetCmd();
                getCmd.script = "EAACP";
                getCmd.parameters = new APGetCmdParams();
                getCmd.parameters.Cmd = 1;
                getCmd.parameters.Option = 1;

                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
                if (aAError.ErrorNumber != 0)
                {
                    Speak(aAError.Message);
                }
                else
                {
                    APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
                    if (apObjects.error == 0 && apObjects.results.Objects != null)
                    {
                        return apObjects.results.Objects[0];
                    }
                    else if (apObjects.error != 0)
                    {
                        Speak(aAError.ErrorMapping[apObjects.error]);
                    }
                }
            }
            catch (Exception) { }

            return null; // Nothing selected
        }

        private APGetCmdResult APGetObjects(int Cmd, int Option, string ObjType)
        {
            aAError.Reset();

            APGetCmd getCmd = new APGetCmd();
            getCmd.script = "EAACP";
            getCmd.parameters = new APGetCmdParams();
            getCmd.parameters.Cmd = Cmd;
            getCmd.parameters.Option = Option;
            getCmd.parameters.ObjType = ObjType;

            string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
            if (aAError.ErrorNumber != 0)
            {
                Speak(aAError.Message);
            }
            else
            {
                // Corrects a bug in AP that does not close the JSON documents correctly (missing })
                if (sOut.Contains("}]}") && !sOut.Contains("}]}}"))
                {
                    sOut += "}";
                }
                APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
                if (apObjects.error == 0 && apObjects.results != null)
                {
                    return apObjects;
                }
                else if (apObjects.error != 0)
                {
                    Speak(aAError.ErrorMapping[apObjects.error]);
                }
            }
            return null;
        }

        private APGetCmdResult APGetObjects(int Cmd, int Option, string ObjType, string Params)
        {
            APGetCmd getCmd = new APGetCmd();
            getCmd.script = "EAAControl2";
            getCmd.parameters = new APGetCmdParams();
            getCmd.parameters.Cmd = Cmd;
            getCmd.parameters.Option = Option;
            getCmd.parameters.ObjType = ObjType;
            getCmd.parameters.Params = Params;

            string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
            // Corrects a bug in AP that does not close the JSON documents correctly (missing })
            if (sOut.Contains("}]}") && !sOut.Contains("}]}}"))
            {
                sOut += "}";
            }
            APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
            if (apObjects.error == 0 && apObjects.results != null)
            {
                return apObjects;
            }

            return null;
        }


        private void btnQuickObs_Click(object sender, EventArgs e)
        {
            bool bMultiple =false;
            bool bAssociated = false;
            string sObjectName = "";

            APGetCmdResult apOut = APGetObjects(1, 2, "");
            if (aAError.ErrorNumber == 0 && apOut == null)
            {
                Speak("No object selected");
                //MessageBox.Show("No objects selected in AstroPlanner.", "EAACP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (apOut == null)
            {
                return;
            }

            int AssociatedCount = apOut.results.Objects[0].Associated;

            if (apOut.results.Objects.Count > 1)
            {
                bMultiple = true;
            }
            else if (AssociatedCount > 0)
            {
                bAssociated = true;
            }

            sObjectName = apOut.results.Objects[0].ID;

            using (QuickObs formObs = new QuickObs())
            {
                formObs.TopMost = true; 

                // Associated objects
                formObs.Controls["chkAssociated"].Visible = bAssociated;
                if (bAssociated)
                {
                    ((CheckBox)formObs.Controls["chkAssociated"]).Checked = Properties.Settings.Default.QOAssociated;
                    formObs.Controls["chkAssociated"].Text = "Log " + AssociatedCount.ToString() + " associated object" + $"{(AssociatedCount > 1 ? "s" : "")}"; 
                }

                // Multiple objects
                int iCount = apOut.results.Objects.Count - 1;
                if (bMultiple) 
                { 
                    sObjectName += " +" + iCount.ToString() + " object" + $"{(iCount > 1 ? "s" : "")}"; 
                }
                formObs.Controls["lblObjectID"].Text = sObjectName;
                                                
                if (formObs.ShowDialog() == DialogResult.OK)
                {
                    btnQuickObs.Text = "Processing...";
                    btnQuickObs.Refresh();
                    EnableButtons(false);

                    try
                    {
                        string result = formObs.ObsNote;
                        bool bLogAssociated = formObs.Associated;
                        Properties.Settings.Default.QOAssociated = bLogAssociated;

                        // Log the observation in AP
                        APRunScript(3, bLogAssociated ? 1 : 0, result);
                    }
                    catch (Exception) { }
                    finally
                    {
                        btnQuickObs.Text = "Quick Observation";
                        EnableButtons(true);
                    }
                }
            }
        }

        private void btnStelSync_Click(object sender, EventArgs e)
        {
            APCmdObject SelectedObject = APGetSelectedObject();
            if (aAError.ErrorNumber == 0 && SelectedObject == null)
            {
                Speak("No object selected");
                //MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SelectedObject == null)
            {
                return;
            }

            string RA = ""; string Dec = "";
            // Format RA/Dec to hms and dms
            RA = APHelper.RADecimalHoursToHMS(SelectedObject.RA2000, @"hh\hmm\mss\.ff\s");
            Dec = APHelper.DecDecimalToDMS(SelectedObject.Dec2000);
            
            Stellarium.IPAddress = Properties.Settings.Default.StelIP;
            Stellarium.Port = Properties.Settings.Default.StelPort;

            string sResult = "";
            sResult = Stellarium.SyncStellariumToAPObject(SelectedObject.ID, RA, Dec, SelectedObject.Type);
            if ("ok" == sResult)
            {
                Speak("Selected");
            }
            else 
            { 
                if (Stellarium.Message == "StConnection")
                {
                    Speak("Cannot connect to planetarium, is planetarium running and remote control configured?");
                }
            }
        }

        private void btnAddtoAP_Click(object sender, EventArgs e)
        {
            APCmdObject obj = Stellarium.StellariumGetSelectedObjectInfo();
            if (Stellarium.Message!="exception" && obj!=null)
            {
                APPutCmd aPPutCmd = new APPutCmd();
                aPPutCmd.script = "EAACP";
                aPPutCmd.parameters = new APPutCmdParams();
                aPPutCmd.parameters.Cmd = 2;
                aPPutCmd.parameters.Option = 1;
                aPPutCmd.parameters.Objects = new List<APCmdObject> { obj };
                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APPutCmd>(aPPutCmd)));
                if (aAError.ErrorNumber != 0)
                {
                    Speak(aAError.Message);
                    return;
                }
            }
            else 
            {
                if (Stellarium.Message == "StConnection")
                {
                    Speak("Cannot connect to planetarium, is planetarium running and remote control configured?");
                }
            }
        }

        private string DSAFormat(APCmdObject obj)
        {
            // IDs|Names|Type|RA(decimal hours)|Dec(degrees)|VMag|RMax(arcmin)|RMin(arcmin)|PosAngle
            string sOut = obj.ID + "|" + obj.Name + "|";
            sOut += APHelper.DisplayTypeFromAPType(obj.Type) + "|";
            sOut += obj.RA2000.ToString() + "|" + obj.Dec2000.ToString() + "|";
            sOut += obj.Magnitude.ToString() + "|";

            APHelper.SizeInfoString sizeInfo = APHelper.ObjectSizeString(obj.Size);
            sOut += sizeInfo.MajorAxis + "|" + sizeInfo.MinorAxis + "|";
            if (obj.PosAngle != -999)
            {
                sOut += obj.PosAngle.ToString();
            }

            sOut += "\r\n";

            return sOut;
        }

        private void btnDSA_Click(object sender, EventArgs e)
        {
            //Creates DSA for AP objects - ToDo if minor body then optionally use JPL webservices.
            APCmdObject SelectedObject = APGetSelectedObject();
            if (SelectedObject == null)
            {
                Speak("No object selected");
                //MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Clipboard.SetText(DSAFormat(SelectedObject));
            Speak("DSA copied to clipboard");
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            Config frmConfig = new Config();
            frmConfig.TopMost = true;   
            frmConfig.ShowDialog();
        }

        private void btnSearchOptions_Click(object sender, EventArgs e)
        {
            using (StelFOVOptions frmOpt = new StelFOVOptions())
            {
                frmOpt.TopMost = true;
                if (frmOpt.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }

        private string CreateSearchParams(double SearchRA, double SearchDec)
        {
            string sParams, sType = string.Empty;

            sParams = Properties.Settings.Default.sfMagnitude;
            if (Properties.Settings.Default.sfAll)
            {
                sType = "All";
            }
            else if (Properties.Settings.Default.sfStars)
            {
                sType = "Star";
            }
            else if (Properties.Settings.Default.sfGalaxies)
            {
                sType = "Galaxy";
            }
            else if (Properties.Settings.Default.sfQuasars)
            {
                sType = "Quasar";
            }
            else if (Properties.Settings.Default.sfDouble)
            {
                sType = "Double";
            }
            else if (Properties.Settings.Default.sfVariable)
            {
                sType = "Variable";
            }
            else if (Properties.Settings.Default.sfGlobulars)
            {
                sType = "Cluster";
            }
            else if (Properties.Settings.Default.sfNebulae)
            {
                sType = "Nebula";
            }

            sParams += "|" + sType;
            if (Properties.Settings.Default.sfNoMag)
            {
                sParams += "|1";
            }
            else
            {
                sParams += "|0";
            }

            if (Properties.Settings.Default.SearchRadius > 0)
            {
                sParams += "|" + Properties.Settings.Default.SearchRadius.ToString();
            }
            else
            {
                sParams += "|0.5";
            }

            sParams += "|" + SearchRA.ToString() + "|" + SearchDec.ToString();

            return sParams;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            double SearchRA = 999, SearchDec = 999;
            if (Properties.Settings.Default.sfPlanetarium)
            {
                APCmdObject ap = null;
                ap = Stellarium.StellariumGetSelectedObjectInfo();

                if (ap == null)
                {
                    Speak("No object selected in Stellarium");
                    return;
                }

                SearchRA = ap.RA2000;
                SearchDec = ap.Dec2000;
            }

            if (Properties.Settings.Default.sfDatasource == 0)
            {
                // Store the search results for DSA and Search List display
                List<string[]> listOfSearchResults;

                APGetCmdResult apOut = APGetObjects(5, 2, "", CreateSearchParams(SearchRA, SearchDec));
                if (apOut == null)
                {
                    Speak("No search results");
                    //MessageBox.Show("No objects selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                listOfSearchResults = Stellarium.DrawObjects(apOut);

                // Add search objects in SharpCap DSA format to the clipboard
                if (Properties.Settings.Default.sSharpCapDSA)
                {
                    string sDSA = "";
                    foreach (APCmdObject obj in apOut.results.Objects)
                    {
                        sDSA += DSAFormat(obj);
                    }
                    Clipboard.SetText(sDSA);
                }

                // Show search results window
                if (Properties.Settings.Default.sResultsList)
                {
                    using (SearchResults frmOpt = new SearchResults())
                    {
                        frmOpt.EAACP = this;
                        frmOpt.TopMost = true;
                        frmOpt.Results = listOfSearchResults;
                        if (frmOpt.ShowDialog() == DialogResult.OK)
                        {

                        }
                    }
                }
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            Stellarium.ClearObjects();
        }
    }

    public class APPutCmd
    {
        public string script { get; set; }
        public APPutCmdParams parameters { get; set; }
    }

    public class APPutCmdParams
    {
        public int Cmd { get; set; }
        public int Option { get; set; }
        public List<APCmdObject> Objects { get; set; }
    }

    public class APGetCmd
    {
        public string script { get; set; }
        public APGetCmdParams parameters { get; set; }
    }

    public class APGetCmdParams
    {
        public int Cmd { get; set; }
        public int Option { get; set; }
        public string ObjType { get; set; }
        public string Params { get; set; }
    }

    public class APGetCmdResult
    {
        public int error { get; set; }
        public APGetResults results { get; set; }
    }

    public class APGetResults
    {
        public List<APCmdObject> Objects { get; set; }
    }

    public class APCmdObject
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Constellation { get; set; }
        public string Catalogue { get; set; }
        public string Distance { get; set; }
        public string GalaxyType { get; set; }
        public int PosAngle { get; set; }
        public double Magnitude { get; set; }
        public double RA2000 { get; set; }
        public double Dec2000 { get; set; }
        public double ParallacticAngle { get; set; }
        public int Associated { get; set; }
    }
}
