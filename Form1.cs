﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Timers;
using System.Windows.Forms;
using System.Speech.Synthesis;
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

            Stellarium.IPAddress = Properties.Settings.Default.StelIP;
            Stellarium.Port = Properties.Settings.Default.StelPort;
           
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

            //SetTimer();

        }

        private void frmCP_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.XPos = this.Left;
            Properties.Settings.Default.YPos = this.Top;
            Properties.Settings.Default.frmWidth = this.Width;
            Properties.Settings.Default.frmHeight = this.Height;
            Properties.Settings.Default.Save();
        }

        private void Speak(string Speech)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak(Speech);
        }

        private string APExecuteScript(string ScriptPayload)
        {
            string result = "";
            string sIP = Properties.Settings.Default.APIP + ":" + Properties.Settings.Default.APPort;
            string apWebServices = "http://" + sIP + "?cmd=launch&auth=" + Properties.Settings.Default.Auth + "&cmdformat=json&responseformat=json&payload=";
            apWebServices += ScriptPayload;
            
            WebClient lwebClient = new WebClient();
            lwebClient.Encoding = Encoding.UTF8;
            lwebClient.Timeout = 120000; // 120 seconds timeout

            try
            {
               // aTimer.Enabled = false;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                result = lwebClient.DownloadString(apWebServices);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);

                this.Invoke(new Action(() => this.Text = "EAA CP (0.3) " + elapsedTime + "s"));
                stopwatch.Stop();
            }
            catch (System.Net.WebException)
            {
                aAError.ErrorNumber = -1;
                result = "ConnFailed";
            }
            catch { }
            finally { lwebClient.Dispose(); }
            
            // aTimer.Enabled = true;
            
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
                getCmd.parameters.Notes = Notes;

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
            if (Stellarium.Message=="" && obj!=null)
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

            // AstroPlanner script to perform the same action
            //APRunScript(6, 0, "");
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
            /* Creates DSA for AP objects - ToDo if minor body then optionally use JPL webservices.
            APCmdObject SelectedObject = APGetSelectedObject();
            if (SelectedObject == null)
            {
                Speak("No object selected");
                //MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Clipboard.SetText(DSAFormat(SelectedObject));
            */

            string sOut = APRunScript(5, 0, "");
            if (aAError.ErrorNumber != 0)
            {
                Speak(aAError.Message);
                return;
            }

            APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
            if (apObjects.error != 0)
            {
                Speak(aAError.ErrorMapping[apObjects.error]);
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            Config frmConfig = new Config();
            frmConfig.TopMost = true;   
            frmConfig.ShowDialog();
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
        public string Notes { get; set; }
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
        public int PosAngle { get; set; }
        public double Magnitude { get; set; }
        public double RA2000 { get; set; }
        public double Dec2000 { get; set; }
        public int Associated { get; set; }
    }

    public class WebClient : System.Net.WebClient
    {
        public int Timeout { get; set; } = 120*1000; // Default to 120 second timeout

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest lWebRequest = base.GetWebRequest(uri);
            lWebRequest.Timeout = Timeout;
            ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
            return lWebRequest;
        }
    }
}
