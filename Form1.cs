using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Timers;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace EAACP
{
    public partial class frmCP : Form
    {
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
        public frmCP()
        {
            InitializeComponent();
           
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
                aTimer.Enabled = false;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                result = lwebClient.DownloadString(apWebServices);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
                //this.Text = "EAA CP (0.1) " + elapsedTime + "s";
                this.Invoke(new Action(() => this.Text = "EAA CP (0.2) " + elapsedTime + "s"));
                stopwatch.Stop();
            }
            catch (Exception) { }
            finally { lwebClient.Dispose(); }
            
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
                APGetCmd getCmd = new APGetCmd();
                getCmd.script = "EAACP";
                getCmd.parameters = new APGetCmdParams();
                getCmd.parameters.Cmd = 1;
                getCmd.parameters.Option = 1;

                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
                APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
                if (apObjects.error == 0 && apObjects.results.Objects != null)
                {
                    return apObjects.results.Objects[0];
                }
            }
            catch (Exception) { }

            return null; // Nothing selected
        }

        private APGetCmdResult APGetObjects(int Cmd, int Option, string ObjType)
        {
            APGetCmd getCmd = new APGetCmd();
            getCmd.script = "EAACP";
            getCmd.parameters = new APGetCmdParams();
            getCmd.parameters.Cmd = Cmd;
            getCmd.parameters.Option = Option;
            getCmd.parameters.ObjType = ObjType;

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
            if (apOut == null)
            {
                Speak("No object selected");
                //MessageBox.Show("No objects selected in AstroPlanner.", "EAACP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    string result = formObs.ObsNote;
                    bool bLogAssociated = formObs.Associated;
                    Properties.Settings.Default.QOAssociated = bLogAssociated;
                   
                    // Log the observation in AP
                    APRunScript(3, bLogAssociated ? 1 : 0, result);
                }
            }
        }

        private void btnStelSync_Click(object sender, EventArgs e)
        {
            APCmdObject SelectedObject = APGetSelectedObject();
            if (SelectedObject == null)
            {
                Speak("No object selected");
                //MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string RA = ""; string Dec = "";
            // Format RA/Dec to hms and dms
            RA = APHelper.RADecimalHoursToHMS(SelectedObject.RA2000, @"hh\hmm\mss\.ff\s");
            Dec = APHelper.DecDecimalToDMS(SelectedObject.Dec2000);

            if ("ok" == Stellarium.SyncStellariumToAPObject(SelectedObject.ID, RA, Dec, SelectedObject.Type))
            {
                Speak("Selected");
            }
        }

        private void btnAddtoAP_Click(object sender, EventArgs e)
        {
            APRunScript(6, 0, "");
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
            /*         // TODO: Add JPL query for minor bodies    
                       string sOut = "";

                       APGetCmdResult apOut = APGetObjects(1, 2, "");
                       if (apOut == null)
                       {
                           Speak("No object selected");
                           //MessageBox.Show("No objects selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                           return;
                       }

                       foreach (APCmdObject obj in apOut.results.Objects)
                       {
                           // SharpCap DSA format sourced from AP only.
                           sOut += DSAFormat(obj);
                       }

                       Clipboard.SetText(sOut);
            */
            APRunScript(5, 0, "");
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            Config frmConfig = new Config();
            frmConfig.TopMost = true;   
            frmConfig.ShowDialog();
        }
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
