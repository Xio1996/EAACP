using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAACP
{
    internal class Stellarium
    {
        private string sMsg = "";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        public string IPAddress = "127.0.0.1";
        public string Port = "8090";

        private Dictionary<string, string> StelTypeMappings = new Dictionary<string, string>()
        {
            {"galaxy","Galaxy"}, {"active galaxy","ActGal"}, {"globular star cluster","Globular"}, {"open star cluster","Open"},
            {"star cluster","Open"}, {"cluster","Open"}, {"HII region","Neb"}, {"planetary nebula","P Neb"},
            {"reflection nebula","R Neb"}, {"dark nebula","DkNeb"}, {"nebula","Neb"},
            {"double star","Dbl"},{"star","Star"},{"supernova remnant","SNR"},{"asteroid","Minor"},
            {"comet","Comet"},{"planet","Planet"},{"moon","Planetary Moon"},{"artificial satellite","Artificial Satellite"}
        };

        public string APTypeFromStellariumType(string ObjectType)
        {
            try
            {
                if (ObjectType != null || ObjectType != "")
                {
                    return StelTypeMappings[ObjectType];
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return ObjectType;
            }

            return "";
        }

        public double StelRAtoAPRA(double RA)
        {
            if (RA > 0)
            {
                return RA / 15.0;
            }
            return (RA + 360.0) / 15.0;
        }

        public string SetStelAction(string sName)
        {
            string result = "";
            sMsg = "";

            string sWebServiceURL = @"http://" + IPAddress + ":" + Port + "/api/stelaction/do";
            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.UploadString(sWebServiceURL, "POST", "id=" + sName);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StError";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }

            return result;
        }

        public string SetStelProperty(string sName, string sValue)
        {
            string result = "";
            sMsg = "";

            string sWebServiceURL = @"http://" + IPAddress + ":" + Port + "/api/stelproperty/set";
            WebClient lwebClient = new WebClient();
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.UploadString(sWebServiceURL, "POST", "id=" + sName + "&value=" + sValue);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StError";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string SetStellariumFOV(int iFOV)
        {
            string result = "";
            sMsg = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/fov";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("fov", iFOV.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StError";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string SyncStellariumToPosition(double RA, double Dec)
        {
            sMsg = "";
            string result = "";
            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/focus";
            string sPos = RA.ToString() + ", " + Dec.ToString();

            // Convert selected object's RA to degrees and then both RA and Dec to radians
            RA = RA * 15 * Math.PI / 180;
            Dec = Dec * Math.PI / 180;

            // Calculate 3D vector for Stellarium
            double dblX = Math.Cos(Dec) * Math.Cos(RA);
            double dblY = Math.Cos(Dec) * Math.Sin(RA);
            double dblZ = Math.Sin(Dec);

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("position", "[" + dblX.ToString() + "," + dblY.ToString() + "," + dblZ.ToString() + "]");

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StError";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string StellariumToAltAzPosition(double Alt, double Az)
        {
            sMsg = "";
            string result = "";
            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/view";
            string sPos = Alt.ToString() + ", " + Az.ToString();

            Az = 180 - Az;
            // Convert to radians
            Az = Az * Math.PI / 180;
            Alt = Alt * Math.PI / 180;

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("az", Az.ToString());
            nvcParams.Add("alt", Alt.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StError";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string StellariumMove(double X, double Y)
        {
            sMsg = "";
            string result = "";
            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/move";
            string sPos = X.ToString() + ", " + Y.ToString();

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("x", X.ToString());
            nvcParams.Add("y", Y.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StError";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string SyncStellariumToID(string sID)
        {
            string result = "";
            sMsg = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/focus";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("target", sID);

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "SyncStToID";
            }
            finally
            {
                lwebClient?.Dispose();
            }
            return result;
        }

        public string SyncStellariumToAPObject(string sID, string sRA, string sDec, string sType)
        {
            string result = "";
            sMsg = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/scripts/direct";

            string sInput = "sObject=\"" + sID + "\";sRA=\"" + sRA + "\";sDec=\"" + sDec + "\";sType=\"" + sType + "\";\r\n";
            string sCode = "objmap = core.getObjectInfo(sObject);\r\nsInfo = new String(core.mapToString(objmap));\r\nsFound=sInfo.slice(5,10);\r\n\r\nif (sFound==\"found\")\r\n{\r\n\tCustomObjectMgr.addCustomObject(sObject, sRA, sDec, true);\r\n\tcore.selectObjectByName(sObject,true);\r\n\tcore.moveToSelectedObject();\r\n\tStelMovementMgr.setFlagTracking(true);\r\n\tcore.addToSelectedObjectInfoString(\"AP Type: \" + sType,false)\r\n}\r\nelse\r\n{\r\n\tcore.output(\"Object found\");\r\n\tcore.selectObjectByName(sObject,true);\r\n\tcore.moveToSelectedObject();\r\n\tStelMovementMgr.setFlagTracking(true);\r\n}";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sInput + sCode);

            WebClient lwebClient = new WebClient();
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "SyncStToAPObj";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string StellariumRemoveMarker(string sMarkerName)
        {
            string result = "";
            sMsg = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/scripts/direct";

            string sInput = "sMarker=\"" + sMarkerName + "\";\r\n";
            string sCode = "if (sMarker==\"\")\r\n{\r\n\tCustomObjectMgr.removeCustomObjects();\r\n}\r\nelse\r\n{\r\n\tCustomObjectMgr.removeCustomObject(sMarker);\r\n}";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sInput + sCode);

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch (System.Net.WebException)
            {
                sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StRemoveMarker";
            }
            finally
            {
                lwebClient?.Dispose();
            }
            return result;
        }

        public string StellariumGetDesignation(string Designation, string Ignore, bool FirstOnly)
        {
            string sOut = "";
            string[] sIDs;
            bool bFirst = true;

            Designation.Replace(" ", "");
            if ( Designation !="")
            {
                sIDs = Designation.Split('-');

                if (FirstOnly)
                {
                    return sIDs[0].Trim();
                }

                foreach (string sID in sIDs) 
                { 
                    if (sID.Trim() != Ignore)
                    {
                        if (!bFirst)
                        {
                            sOut += ", ";
                        }

                        sOut+= sID.Trim();
                        bFirst = false;
                    }
                }
            }

            return sOut;
        }

        public string StDegtoArcmins(double StDegrees)
        {
            if (!double.IsNaN(StDegrees))
            {
                return (StDegrees * 60).ToString("#.00");
            }

            return "";
        }

        public APCmdObject StellariumGetSelectedObjectInfo()
        {
            string result = "";
            JsonNode oSelectedObject = null;
            APCmdObject apObject = new APCmdObject();
            sMsg = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/objects/info?format=json";

            WebClient lwebClient = new WebClient();
            lwebClient.Encoding = Encoding.UTF8;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(sWebServiceURL);
                if (result != "")
                {
                    oSelectedObject = JsonNode.Parse(result);

                    apObject.RA2000 = StelRAtoAPRA((double)oSelectedObject["raJ2000"]);
                    apObject.Dec2000 = (double)oSelectedObject["decJ2000"];
                    apObject.Type = APTypeFromStellariumType((string)oSelectedObject["object-type"]);
                    apObject.Catalogue = "Stellarium";
                    apObject.Constellation = (string)oSelectedObject["iauConstellation"];

                    string soName = (string)oSelectedObject["name"];
                    string soDesignation = (string)oSelectedObject["designations"];
                    string soLocalisedName = (string)oSelectedObject["localized-name"];
                    bool bUsedLocalised = false;

                    // Decide which name(s) we are going to use for AP ID
                    if (soLocalisedName != "")
                    {
                        apObject.ID = soLocalisedName;
                        bUsedLocalised = true;
                    }
                    else if (soName != "")
                    {
                        apObject.ID = soName;
                    }
                    else if (soDesignation != "")
                    {
                        apObject.ID = StellariumGetDesignation(soDesignation, "", true);
                    }
                    else 
                    {
                        apObject.ID = "Stellarium";
                    }

                    // Add other names to AP Name field
                    if (soDesignation != "")
                    {
                        apObject.Name = StellariumGetDesignation(soDesignation, apObject.ID, false);
                    }

                    if (bUsedLocalised) 
                    { 
                        if (apObject.Name != "")
                        {
                            apObject.Name += "," + soName;
                        }
                        else
                        {
                            apObject.Name = soName;
                        }
                    }

                    bool bMagFound = false;
                    if (!double.IsNaN((double)(oSelectedObject["vmag"])))
                    {
                        double vmag = (double)(oSelectedObject["vmag"]);
                        if (vmag < 99)
                        {
                            apObject.Magnitude = vmag;
                            bMagFound = true;
                        }
                    }

                    if (!bMagFound)
                    {
                        if (!double.IsNaN((double)(oSelectedObject["bmag"])))
                        {
                            double vmag = (double)(oSelectedObject["bmag"]);
                            if (vmag < 99)
                            {
                                apObject.Magnitude = vmag;
                            }
                        }
                    }

                    if (!double.IsNaN((double)(oSelectedObject["orientation-angle"])))
                    {
                        apObject.PosAngle = (int)oSelectedObject["orientation-angle"];
                    }

                    double dblMajorAxis = (double)(oSelectedObject["axis-major-dd"]);
                    double dblMinorAxis = (double)(oSelectedObject["axis-minor-dd"]);
                    if (!double.IsNaN(dblMajorAxis) && !double.IsNaN(dblMinorAxis))
                    {
                        if (dblMajorAxis > 0 && dblMinorAxis > 0)
                        {
                            // Convert degrees to arcminutes
                            apObject.Size = StDegtoArcmins(dblMajorAxis) + "x" + StDegtoArcmins(dblMinorAxis);
                        }
                    }
                    else
                    {
                        double dblSize = (double)oSelectedObject["size-dd"];
                        if (!double.IsNaN(dblSize))
                        {
                            if (dblSize > 0)
                            {
                                apObject.Size = StDegtoArcmins(dblSize);
                            }
                        }
                    }
                }

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
            catch(System.Net.WebException)
            {
                    sMsg = "StConnection";
            }
            catch (Exception)
            {
                sMsg = "StGetSelectedObj";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return apObject;
        }

    }
}
