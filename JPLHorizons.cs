using System;
using System.Diagnostics;
using System.Net.Http;

namespace EAACP
{
    internal class JPLHorizons
    {
        private double mLongitude;
        private double mLatitude;
        private double mAltitude;

        public JPLHorizons(double Longitude, double Latitude, double Elevation)
        {
            mLongitude = Longitude;
            mLatitude = Latitude;
            mAltitude = Elevation;
        }

        private string sMsg = "";
        private static readonly HttpClient httpClient = new HttpClient();

        public string Message
        {
            get
            {
                return sMsg;
            }
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

        // Returns the current RA/Dec of the passed object along with other object attributes
        public string QueryObject(string sID)
        {
            string result = "";

            string sWebServiceURL = "https://ssd.jpl.nasa.gov/api/horizons.api?format=text&OBJ_DATA='NO'&MAKE_EPHEM='YES'&EPHEM_TYPE='OBSERVER'&APPARENT='REFRACTED'&CSV_FORMAT='YES'&QUANTITIES='D'";
            sWebServiceURL += "&CENTER='coord'&COORD_TYPE='GEODETIC'&SITE_COORD='";
            sWebServiceURL += $"{mLongitude.ToString()},{mLatitude.ToString()},{(mAltitude / 1000).ToString()}'";
            sWebServiceURL += $"&TLIST='{DateTime.UtcNow.ToString()}'";
            sWebServiceURL += $"&COMMAND='{sID}'";

            try
            {
                result = GetRequest(sWebServiceURL);

                sMsg = $"JPL: QueryObject {sID}, {sMsg}\r\n";
            }
            catch (Exception)
            {
                sMsg = $"JPL:QueryObject ERR, {sMsg} \r\n";
                result = "exception";
            }
            return result;
        }

        // Searches for all minor bodies at a specified RA/Dec within a search box to a limiting magnitude
        // Format RA = (hh-mm-ss[.ss]   Dec = dd-mm-ss[.ss] Replace minus sign with M 
        public string SmallBodySearchBox(string RA, string Dec, int LimitingMagnitude, double Dimension)
        {
            string result = "";

            string sWebServiceURL = "https://ssd-api.jpl.nasa.gov/sb_ident.api?";
            sWebServiceURL += $"lat={mLatitude.ToString()}&lon={mLongitude.ToString()}&alt={(mAltitude / 1000).ToString()}";
            sWebServiceURL += "&two-pass=true&suppress-first-pass=true";
            sWebServiceURL += $"&obs-time={DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}";
            sWebServiceURL += $"&vmag-lim={LimitingMagnitude.ToString()}";
            sWebServiceURL += $"&fov-ra-center={RA}&fov-dec-center={Dec}&fov-ra-hwidth={Dimension.ToString()}&fov-dec-hwidth={Dimension.ToString()}";

            try
            {
                result = GetRequest(sWebServiceURL);

                sMsg = $"JPL:SmallBodySearchBox RA={RA} Dec={Dec} Mag={LimitingMagnitude} Size={Dimension}, {sMsg}\r\n";
            }
            catch (Exception)
            {
                sMsg = $"JPL:SmallBodySearchBox ERR, {sMsg} \r\n";
                result = "exception";
            }

            return result;
        }
    }
}
