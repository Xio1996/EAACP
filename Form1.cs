﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Timers;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Net.Http;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using ASCOM.DeviceInterface;

namespace EAACP
{
    public partial class frmCP : Form
    {
        // Error handling class
        private EAAError aAError = new EAAError();

        private const string StellariumSpeak = "stell-airium";
        private const string AstroPlannerSpeak = "Astrow-planner";   

        // AstroPlanner Helper class - replaces AP functions in EAACtrl Panel
        private APHelper APHelper = new APHelper();

        // Astro Class
        private AstroCalc AstroCalc = new AstroCalc();

        // Stellarium Communication and processing class
        private Stellarium Stellarium = new Stellarium();

        private string telescopeName = "";
        private EquatorialCoordinateType equatorialCoordinateType = EquatorialCoordinateType.equTopocentric;
        private bool StellariumTrailing = false;
        private int StellariumTrailingInterval = 2;
        private string StellariumTrailingGraphic = "circle";
        private string StellariumTrailingSize = "20";
        private string StellariumTrailingColour = "#ffff00";

        //ASCOM Telescope
        private EAATelescope Telescope = null;

        // HTTP Client
        private static readonly HttpClient httpClient = new HttpClient();
        private string sMsg = "";

        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer TrailingTimer;

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

        private bool MarkTelescopePosition()
        {
            if (Telescope.Connected)
            {
                double RAJ2000, DecJ2000;
                double RANOW = Telescope.RightAscension;
                double DecNOW = Telescope.Declination;

                // If the telescope uses JNOW then convert to J2000 
                if (equatorialCoordinateType == EquatorialCoordinateType.equTopocentric)
                {
                    AstroCalc.JNOWToJ2000(RANOW, DecNOW, out RAJ2000, out DecJ2000);
                }
                else
                {
                    // Being lazy I'm not going to implement the other types of equatorial coordinates - assume J2000
                    RAJ2000 = RANOW;
                    DecJ2000 = DecNOW;
                }


                Stellarium.MarkTelescopePosition(APHelper.RADecimalHoursToHMS(RAJ2000, @"hh\hmm\mss\.ff\s"),
                                                APHelper.DecDecimalToDMS(DecJ2000),
                                                (StellariumTrailingInterval * 1000)-600,
                                                StellariumTrailingGraphic,
                                                StellariumTrailingSize,
                                                StellariumTrailingColour);

                if (Stellarium.Message.Contains("HTTP 401"))
                {
                    Speak(StellariumSpeak + " password incorrect or not set");
                    return false;
                }
                return true;
            }
            return false;
        }
        private void OnTimedEventTrailing(Object source, ElapsedEventArgs e)
        {
            MarkTelescopePosition();
        }

        private void StellariumTrailingTimerON()
        {           
            TrailingTimer = new System.Timers.Timer(StellariumTrailingInterval*1000);
            TrailingTimer.Elapsed += OnTimedEventTrailing;
            TrailingTimer.AutoReset = true;
            TrailingTimer.Enabled = true;
        }

        private void StellariumTrailingTimerOFF()
        {
            TrailingTimer.Enabled = false;
            TrailingTimer.Dispose();
        }   

        private bool IsProcessNameRunning(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length > 0)
            {
                return true;
            }
            return false;
        }

        private bool IsStellariumRunning()
        {
            return IsProcessNameRunning("Stellarium");
        }

        private bool IsAPRunning()
        {
            return IsProcessNameRunning("AstroPlanner");
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
                this.Left = Properties.Settings.Default.XPos;
                this.Top = Properties.Settings.Default.YPos;

                // Center the form on the screen, if the form is larger off the screen then center the form on the screen
                WindowHelper.EnsureWindowVisible(this);
            }

            SetTimer();

        }

        private void frmCP_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Save the form location/size settings
                Properties.Settings.Default.XPos = this.Left;
                Properties.Settings.Default.YPos = this.Top;
                Properties.Settings.Default.frmWidth = this.Width;
                Properties.Settings.Default.frmHeight = this.Height;

                // Save the control order
                var controlOrder = new System.Collections.Specialized.StringCollection();
                foreach (Control control in flpMain.Controls)
                {
                    controlOrder.Add(control.Name);
                }
                Properties.Settings.Default.controlOrder = controlOrder;
                Properties.Settings.Default.Save();

                if (TrailingTimer.Enabled)
                {
                    StellariumTrailingTimerOFF();
                    Telescope.Disconnect();
                }
            }
            catch (Exception)
            {
            }
        }

        private Control draggedControl;
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;

        private void InitializeFlowLayoutPanel()
        {
            var controlOrder = Properties.Settings.Default.controlOrder;
            if (controlOrder != null && controlOrder.Count > 0)
            {
                for (int i = 0; i < controlOrder.Count; i++)
                {
                    Control control = flpMain.Controls[controlOrder[i]];
                    flpMain.Controls.SetChildIndex(control, i);
                }
            }

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

        public void Speak(string Speech)
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

            string apWebServices = $"http://{Properties.Settings.Default.APIP}:{Properties.Settings.Default.APPort}?cmd=launch&auth={EncryptionHelper.Decrypt(Properties.Settings.Default.Auth)}&cmdformat=json&responseformat=json&payload=";
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
                getCmd.script = "EAACP2";
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
                getCmd.script = "EAACP2";
                getCmd.parameters = new APGetCmdParams();
                getCmd.parameters.Cmd = 1;
                getCmd.parameters.Option = 1;

                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
                // Corrects a bug in AP that does not close the JSON documents correctly (missing })
                if (sOut.Contains("}]}") && !sOut.Contains("}]}}"))
                {
                    sOut += "}";
                }
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
                        aAError.ErrorNumber = apObjects.error;
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
            getCmd.script = "EAACP2";
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
                    aAError.ErrorNumber = apObjects.error;
                    Speak(aAError.ErrorMapping[apObjects.error]);
                }
            }
            return null;
        }

        private APGetCmdResult APGetObjects(int Cmd, int Option, string ObjType, string Params)
        {
            APGetCmd getCmd = new APGetCmd();
            getCmd.script = "EAACP2";
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
            else if (apObjects.error != 0)
            {
                aAError.ErrorNumber = apObjects.error;
                Speak(aAError.ErrorMapping[apObjects.error]);
            }

            return null;
        }


        private void btnQuickObs_Click(object sender, EventArgs e)
        {
            bool bMultiple =false;
            bool bAssociated = false;
            string sObjectName = "";

            if (!IsAPRunning())
            {
                Speak(AstroPlannerSpeak + " is not running");
                return;
            }

            APGetCmdResult apOut = APGetObjects(1, 2, "");
            if (aAError.ErrorNumber == 0 && apOut == null)
            {
                Speak("No object selected");
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
            if(!IsStellariumRunning())
            {
                Speak(StellariumSpeak + " is not running");
                return;
            }

            if(!IsAPRunning())
            {
                Speak(AstroPlannerSpeak + " is not running");
                return;
            }

            APCmdObject SelectedObject = APGetSelectedObject();
            if (aAError.ErrorNumber == 0 && SelectedObject == null)
            {
                Speak("No object selected");
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
                if (Stellarium.Message.Contains("HTTP 401"))
                {
                    Speak(StellariumSpeak + " password incorrect or not set");
                }

                if (Stellarium.Message == "StConnection")
                {
                    Speak("Cannot connect to " + StellariumSpeak + "is remote control plugin configured?");
                }
            }
        }

        private void btnAddtoAP_Click(object sender, EventArgs e)
        {
            if (!IsStellariumRunning())
            {
                Speak(StellariumSpeak + " is not running");
                return;
            }

            if (!IsAPRunning())
            {
                Speak(AstroPlannerSpeak + " is not running");
                return;
            }
            APCmdObject obj = Stellarium.StellariumGetSelectedObjectInfo();
            if (obj==null && Stellarium.Message.Contains("401"))
            {
                Speak(StellariumSpeak + " password incorrect or not set");
                return;
            }

            if (obj == null)
            {
                Speak("No object selected in " + StellariumSpeak);
                return;
            }

            if (Stellarium.Message!="exception" && obj!=null)
            {
                APPutCmd aPPutCmd = new APPutCmd();
                aPPutCmd.script = "EAACP2";
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

                if (sOut == "{\"error\":4}")
                {
                    Speak(AstroPlannerSpeak + ", Authentication string error");
                    return;
                }

                Speak("Object added to " + AstroPlannerSpeak);
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
            if (!IsAPRunning())
            {
                Speak(AstroPlannerSpeak + " is not running");
                return;
            }

            //Creates DSA for AP objects - ToDo if minor body then optionally use JPL webservices.
            APCmdObject SelectedObject = APGetSelectedObject();
            if (aAError.ErrorNumber ==0 &&  SelectedObject == null)
            {
                Speak("No object selected");
                //MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (aAError.ErrorNumber != 0)
            {
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
            string SearchID = "";

            if (Properties.Settings.Default.sfPlanetarium)
            {
                if (!IsStellariumRunning())
                {
                    Speak(StellariumSpeak + " is not running");
                    return;
                }

                APCmdObject ap = null;
                ap = Stellarium.StellariumGetSelectedObjectInfo();
                if (ap == null && Stellarium.Message.Contains("401"))
                {
                    Speak(StellariumSpeak + " password incorrect or not set");
                    return;
                }

                if (ap == null)
                {
                    Speak("No object selected in " + StellariumSpeak);
                    return;
                }

                SearchID = ap.ID;
                SearchRA = ap.RA2000;
                SearchDec = ap.Dec2000;
            }
            else 
            { 
                if (!IsAPRunning())
                {
                    Speak(AstroPlannerSpeak + " is not running");
                    return;
                }

                APCmdObject SelectedObject = APGetSelectedObject();
                if (aAError.ErrorNumber == 0 && SelectedObject == null)
                {
                    Speak("No object selected in " + AstroPlannerSpeak);
                    return;
                }
                else if (aAError.ErrorNumber != 0)
                {
                    return;
                }

                SearchID = SelectedObject.ID;
                SearchRA = SelectedObject.RA2000;
                SearchDec = SelectedObject.Dec2000;
            }

            if (Properties.Settings.Default.sfDatasource == 0)
            {
                // Store the search results for DSA and Search List display
                List<string[]> listOfSearchResults;

                if (!IsAPRunning())
                {
                    Speak(AstroPlannerSpeak + " is not running");
                    return;
                }

                Speak("Searching");

                APGetCmdResult apOut = APGetObjects(5, 2, "", CreateSearchParams(SearchRA, SearchDec));
                if (aAError.ErrorNumber == 0 && apOut == null)
                {
                    Speak("No search results");
                    
                    return;
                }
                else if (aAError.ErrorNumber != 0)
                {
                    return;
                }

                if (string.IsNullOrEmpty(Properties.Settings.Default.StScriptFolder))
                {
                    Speak("No " +  StellariumSpeak + " script folder specified");
                    return;
                }

                if (!Directory.Exists(Properties.Settings.Default.StScriptFolder))
                {
                    Speak("Invalid " + StellariumSpeak + " script folder specified");
                    return;
                }

                listOfSearchResults = Stellarium.DrawObjects(apOut);
                if (Stellarium.Message.Contains("HTTP 401"))
                {
                    Speak(StellariumSpeak + " password incorrect or not set");
                }

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
                        frmOpt.CentreID = SearchID;
                        frmOpt.CentreRA = SearchRA;
                        frmOpt.CentreDec = SearchDec;
                        if (frmOpt.ShowDialog() == DialogResult.OK)
                        {

                        }
                    }
                }
                else 
                {
                    Speak(apOut.results.Objects.Count.ToString() + " objects found");
                }
            }
            else 
            {
                JPLHorizons jPLHorizons = new JPLHorizons(Properties.Settings.Default.SiteLat,
                                                          Properties.Settings.Default.SiteLng,
                                                          Properties.Settings.Default.SiteElev);
                //jPLHorizons.SearchRadius = Properties.Settings.Default.SearchRadius;
                string results = jPLHorizons.SmallBodySearchBox(SearchRA.ToString(), SearchDec.ToString(), 18,1);
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            if (!IsStellariumRunning())
            {
                Speak(StellariumSpeak + " is not running");
                return;
            }

            Stellarium.ClearObjects();

            if (Stellarium.Message.Contains("HTTP 401"))
            {
                Speak(StellariumSpeak + " password incorrect or not set");
            }
        }

        private void WriteObjectTextToFile(string objectText)
        {
            string textFilePath = Properties.Settings.Default.objTxtFilePath;
            if (string.IsNullOrEmpty(textFilePath))
            {
                Speak("No object text file specified");
                return;
            }

            try
            {
                System.IO.File.WriteAllText(textFilePath, objectText);
            }
            catch (Exception) 
            {
                Speak("Error writing object text file");
            }
        }

        private void btnInfoTextOption_Click(object sender, EventArgs e)
        {
            using (ObjectTextOptions frmOpt = new ObjectTextOptions())
            {
                frmOpt.TopMost = true;
                if (frmOpt.ShowDialog() == DialogResult.OK)
                {               
                    string manualText = Properties.Settings.Default.objTxtManualText;

                    if (!string.IsNullOrEmpty(manualText))
                    {
                        WriteObjectTextToFile(manualText);
                        return;
                    }
                }
            }
        }

        private void btnSetInfoText_Click(object sender, EventArgs e)
        {
            if (!IsAPRunning())
            {
                Speak(AstroPlannerSpeak + " is not running");
                return;
            }

            APCmdObject SelectedObject = APGetSelectedObject();
            if (aAError.ErrorNumber == 0 && SelectedObject == null)
            {
                Speak("No object selected");
                return;
            }
            else if (aAError.ErrorNumber != 0)
            {
                return;
            }

            bool showID = Properties.Settings.Default.objTxtID;
            bool showName = Properties.Settings.Default.objTxtName;
            bool showType = Properties.Settings.Default.objTxtType;
            bool showMagnitude = Properties.Settings.Default.objTxtMagnitude;
            bool showConstellation = Properties.Settings.Default.objTxtConstellation;

            int maxiChars = 10000;
            string maxChars = Properties.Settings.Default.objTxtMaxChars;
            if (!string.IsNullOrEmpty(maxChars))
            {
                maxiChars = int.TryParse(maxChars, out int i) ? i : 10000;
            }
            
            string objectText = APHelper.TargetDisplay(SelectedObject, maxiChars,showID, showName,showConstellation, showType, showMagnitude );
            WriteObjectTextToFile(objectText);
        }

        private void btnTelescopeTracking_Click(object sender, EventArgs e)
        {
            if (IsStellariumRunning() == false)
            {
                Speak(StellariumSpeak + " is not running");
                return;
            }

            if (telescopeName == "")
            {
                Speak("No telescope selected");
                return;
            }

            bool trailing = !StellariumTrailing;
            if (trailing)
            {
                if (Telescope == null)
                {
                    Telescope = new EAATelescope(telescopeName);
                }

                if (!Telescope.Connected)
                {
                    // Connect to telescope
                    if (!Telescope.Connect())
                    {
                        Speak("Cannot connect to telescope");
                        Speak(Telescope.Message);
                        return;
                    }

                    equatorialCoordinateType = Telescope.EquatorialSystemECT;
                    Speak("Telescope connected");
                }
                
                if (!MarkTelescopePosition())
                {
                    return;
                }

                // Start timer to get telescope position
                StellariumTrailingTimerON();

                // Send to Stellarium
                btnTelescopeTracking.Text = "Telescope Trailing ON";
                StellariumTrailing = !StellariumTrailing;
            }
            else
            {
                StellariumTrailingTimerOFF();

                btnTelescopeTracking.Text = "Telescope Trailing OFF";
                StellariumTrailing = !StellariumTrailing;
            }
        }

        private void LoadStellariumTrailingParameters()
        {
            telescopeName = Properties.Settings.Default.ASCOMTelescope;

            if (int.TryParse(Properties.Settings.Default.TTASCOMUpdateRate, out int trailingInterval))
            {
                StellariumTrailingInterval = trailingInterval;
            }
            StellariumTrailingGraphic = Properties.Settings.Default.TTGraphicSymbol;
            StellariumTrailingSize = Properties.Settings.Default.TTGraphicSize;
            
            Color argb = Properties.Settings.Default.TTGraphicColour;
            StellariumTrailingColour = $"#{argb.R:X2}{argb.G:X2}{argb.B:X2}";
        }

        private void btnTelescopeTrackingOptions_Click(object sender, EventArgs e)
        {
            bool telescopeChanged = false;
            using (TelescopeTrackingOptions frmOpt = new TelescopeTrackingOptions())
            {
                frmOpt.TopMost = true;
                if (frmOpt.ShowDialog() == DialogResult.OK)
                {
                    // Telescope change - if connected to another telescope then disconnect
                    if (telescopeName != Properties.Settings.Default.ASCOMTelescope)
                    {
                        telescopeName = Properties.Settings.Default.ASCOMTelescope;
                        if (Telescope != null)
                        {
                            Telescope.Disconnect();
                            Telescope = new EAATelescope(telescopeName);
                            telescopeChanged = true;
                        }
                    }

                    LoadStellariumTrailingParameters();

                    if (StellariumTrailing)
                    {
                        if (telescopeChanged)
                        {
                            if (!Telescope.Connect())
                            {
                                Speak("Cannot connect to new telescope");
                                return;
                            }
                        }
       
                        StellariumTrailingTimerOFF();
                        MarkTelescopePosition();
                        StellariumTrailingTimerON();
                    }
                }
            }
        }

        private void frmCP_Load(object sender, EventArgs e)
        {
            LoadStellariumTrailingParameters();
        }

        private void btnShowTelescope_Click(object sender, EventArgs e)
        {
            if (telescopeName=="")
            {
                Speak("No telescope selected");
                return;
            }

            if (Telescope == null)
            {
                Speak("Telescope not connected");
                return;
            }

            if (Telescope.Connected)
            {
                double RAJ2000, DecJ2000;
                double RANOW = Telescope.RightAscension;
                double DecNOW = Telescope.Declination;

                // If the telescope uses JNOW then convert to J2000 
                if (equatorialCoordinateType == EquatorialCoordinateType.equTopocentric)
                {
                    AstroCalc.JNOWToJ2000(RANOW, DecNOW, out RAJ2000, out DecJ2000);
                }
                else
                {
                    // Being lazy I'm not going to implement the other types of equatorial coordinates - assume J2000
                    RAJ2000 = RANOW;
                    DecJ2000 = DecNOW;
                }

                Stellarium.SyncStellariumToPosition(RAJ2000, DecJ2000, true);
                
            }
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


