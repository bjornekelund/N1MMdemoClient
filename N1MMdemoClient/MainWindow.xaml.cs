// Demo broadcast listener for N1MM Logger+
// Receives UDP broadcasts on listenPort, parses XML into an object and prints info
// Intended as starting point for development of more applications such as 
// big screen score board, out-of-band alarm, etc.
// By Björn Ekelund SM7IUN sm7iun@ssa.se 2019-02-05

using System;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;

//using N1MMdemoClient.Properties;

namespace N1MMdemoClient
{
    // Definition of N1MM XML datagrams based on
    // http://n1mm.hamdocs.com/tiki-index.php?page=UDP+Broadcasts

    [XmlRoot(ElementName = "RadioInfo")]
    public class RadioInfo
    {
        [XmlElement(ElementName = "StationName")]
        public string StationName { get; set; }
        [XmlElement(ElementName = "RadioNr")]
        public int RadioNr { get; set; }
        [XmlElement(ElementName = "Freq")]
        public int Freq { get; set; }
        [XmlElement(ElementName = "TXFreq")]
        public int TXFreq { get; set; }
        [XmlElement(ElementName = "Mode")]
        public string Mode { get; set; }
        [XmlElement(ElementName = "OpCall")]
        public string OpCall { get; set; }
        [XmlElement(ElementName = "IsRunning")]
        public string IsRunning { get; set; }
        [XmlElement(ElementName = "FocusEntry")]
        public string FocusEntry { get; set; }
        [XmlElement(ElementName = "Antenna")]
        public string Antenna { get; set; }
        [XmlElement(ElementName = "Rotors")]
        public string Rotors { get; set; }
        [XmlElement(ElementName = "FocusRadioNr")]
        public int FocusRadioNr { get; set; }
        [XmlElement(ElementName = "IsStereo")]
        public string IsStereo { get; set; }
        [XmlElement(ElementName = "ActiveRadioNr")]
        public int ActiveRadioNr { get; set; }
        [XmlElement(ElementName = "Technique")]
        public string Technique { get; set; }
        [XmlElement(ElementName = "StationType")]
        public string StationType { get; set; }
        [XmlElement(ElementName = "IsSplit")]
        public string IsSplit { get; set; }
        [XmlElement(ElementName = "InactiveFreq")]
        public int InactiveFreq { get; set; }
    }

    [XmlRoot(ElementName = "AppInfo")]
    public class AppInfo
    {
        [XmlElement(ElementName = "dbname")]
        public string Dbname { get; set; }
        [XmlElement(ElementName = "contestnr")]
        public string Contestnr { get; set; }
        [XmlElement(ElementName = "contestname")]
        public string Contestname { get; set; }
        [XmlElement(ElementName = "StationName")]
        public string StationName { get; set; }
    }

    [XmlRoot(ElementName = "spot")]
    public class Spot
    {
        [XmlElement(ElementName = "StationName")]
        public string StationName { get; set; }
        [XmlElement(ElementName = "dxcall")]
        public string Dxcall { get; set; }
        [XmlElement(ElementName = "frequency")]
        public string Frequency { get; set; }
        [XmlElement(ElementName = "spottercall")]
        public string Spottercall { get; set; }
        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }
        [XmlElement(ElementName = "action")]
        public string Action { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "statuslist")]
        public string Statuslist { get; set; }
        [XmlElement(ElementName = "timestamp")]
        public string Timestamp { get; set; }
    }

    [XmlRoot(ElementName = "contactinfo")]
    public class Contactinfo
    {
        [XmlElement(ElementName = "contestname")]
        public string Contestname { get; set; }
        [XmlElement(ElementName = "contestnr")]
        public string Contestnr { get; set; }
        [XmlElement(ElementName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlElement(ElementName = "mycall")]
        public string Mycall { get; set; }
        [XmlElement(ElementName = "band")]
        public string Band { get; set; }
        [XmlElement(ElementName = "rxfreq")]
        public string Rxfreq { get; set; }
        [XmlElement(ElementName = "txfreq")]
        public string Txfreq { get; set; }
        [XmlElement(ElementName = "operator")]
        public string Operator { get; set; }
        [XmlElement(ElementName = "mode")]
        public string Mode { get; set; }
        [XmlElement(ElementName = "call")]
        public string Call { get; set; }
        [XmlElement(ElementName = "countryprefix")]
        public string Countryprefix { get; set; }
        [XmlElement(ElementName = "wpxprefix")]
        public string Wpxprefix { get; set; }
        [XmlElement(ElementName = "stationprefix")]
        public string Stationprefix { get; set; }
        [XmlElement(ElementName = "continent")]
        public string Continent { get; set; }
        [XmlElement(ElementName = "snt")]
        public string Snt { get; set; }
        [XmlElement(ElementName = "sntnr")]
        public string Sntnr { get; set; }
        [XmlElement(ElementName = "rcv")]
        public string Rcv { get; set; }
        [XmlElement(ElementName = "rcvnr")]
        public string Rcvnr { get; set; }
        [XmlElement(ElementName = "gridsquare")]
        public string Gridsquare { get; set; }
        [XmlElement(ElementName = "exchange1")]
        public string Exchange1 { get; set; }
        [XmlElement(ElementName = "section")]
        public string Section { get; set; }
        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }
        [XmlElement(ElementName = "qth")]
        public string Qth { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "power")]
        public string Power { get; set; }
        [XmlElement(ElementName = "misctext")]
        public string Misctext { get; set; }
        [XmlElement(ElementName = "zone")]
        public string Zone { get; set; }
        [XmlElement(ElementName = "prec")]
        public string Prec { get; set; }
        [XmlElement(ElementName = "ck")]
        public string Ck { get; set; }
        [XmlElement(ElementName = "ismultiplier1")]
        public string Ismultiplier1 { get; set; }
        [XmlElement(ElementName = "ismultiplier2")]
        public string Ismultiplier2 { get; set; }
        [XmlElement(ElementName = "ismultiplier3")]
        public string Ismultiplier3 { get; set; }
        [XmlElement(ElementName = "points")]
        public string Points { get; set; }
        [XmlElement(ElementName = "radionr")]
        public string Radionr { get; set; }
        [XmlElement(ElementName = "RoverLocation")]
        public string RoverLocation { get; set; }
        [XmlElement(ElementName = "RadioInterfaced")]
        public string RadioInterfaced { get; set; }
        [XmlElement(ElementName = "NetworkedCompNr")]
        public string NetworkedCompNr { get; set; }
        [XmlElement(ElementName = "IsOriginal")]
        public string IsOriginal { get; set; }
        [XmlElement(ElementName = "NetBiosName")]
        public string NetBiosName { get; set; }
        [XmlElement(ElementName = "IsRunQSO")]
        public string IsRunQSO { get; set; }
        [XmlElement(ElementName = "Run1Run2")]
        public string Run1Run2 { get; set; }
        [XmlElement(ElementName = "ContactType")]
        public string ContactType { get; set; }
        [XmlElement(ElementName = "StationName")]
        public string StationName { get; set; }
    }

    [XmlRoot(ElementName = "class")]
    public class Class
    {
        [XmlAttribute(AttributeName = "power")]
        public string Power { get; set; }
        [XmlAttribute(AttributeName = "assisted")]
        public string Assisted { get; set; }
        [XmlAttribute(AttributeName = "transmitter")]
        public string Transmitter { get; set; }
        [XmlAttribute(AttributeName = "ops")]
        public string Ops { get; set; }
        [XmlAttribute(AttributeName = "bands")]
        public string Bands { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string Mode { get; set; }
        [XmlAttribute(AttributeName = "overlay")]
        public string Overlay { get; set; }
    }

    [XmlRoot(ElementName = "qth")]
    public class Qth
    {
        [XmlElement(ElementName = "dxcccountry")]
        public string Dxcccountry { get; set; }
        [XmlElement(ElementName = "cqzone")]
        public string Cqzone { get; set; }
        [XmlElement(ElementName = "iaruzone")]
        public string Iaruzone { get; set; }
        [XmlElement(ElementName = "arrlsection")]
        public string Arrlsection { get; set; }
        [XmlElement(ElementName = "grid6")]
        public string Grid6 { get; set; }
    }

    [XmlRoot(ElementName = "qso")]
    public class Qso
    {
        [XmlAttribute(AttributeName = "band")]
        public string Band { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string Mode { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "mult")]
    public class Mult
    {
        [XmlAttribute(AttributeName = "band")]
        public string Band { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string Mode { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "point")]
    public class Point
    {
        [XmlAttribute(AttributeName = "band")]
        public string Band { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string Mode { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "breakdown")]
    public class Breakdown
    {
        [XmlElement(ElementName = "qso")]
        public List<Qso> Qso { get; set; }
        [XmlElement(ElementName = "mult")]
        public List<Mult> Mult { get; set; }
        [XmlElement(ElementName = "point")]
        public List<Point> Point { get; set; }
    }

    [XmlRoot(ElementName = "dynamicresults")]
    public class Dynamicresults
    {
        [XmlElement(ElementName = "contest")]
        public string Contest { get; set; }
        [XmlElement(ElementName = "call")]
        public string Call { get; set; }
        [XmlElement(ElementName = "ops")]
        public string Ops { get; set; }
        [XmlElement(ElementName = "class")]
        public Class Class { get; set; }
        [XmlElement(ElementName = "club")]
        public string Club { get; set; }
        [XmlElement(ElementName = "qth")]
        public Qth Qth { get; set; }
        [XmlElement(ElementName = "breakdown")]
        public Breakdown Breakdown { get; set; }
        [XmlElement(ElementName = "score")]
        public string Score { get; set; }
        [XmlElement(ElementName = "timestamp")]
        public string Timestamp { get; set; }
    }

    // Helper class to parse XML datagrams
    public static class XmlConvert
    {
        public static T DeserializeObject<T>(string xml)
             where T : new()
        {
            if (string.IsNullOrEmpty(xml))
                return new T();
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }

    public partial class MainWindow : Window
    {
        public const int listenPort = 12060;
        
        public MainWindow()
        {
            string message;
            string label;

            // Fetch window location from saved settings
            this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;

            InitializeComponent();

            Task.Run(async () =>
            {
                using (var udpClient = new UdpClient(listenPort))
                {
                    while (true)
                    {
                        //IPEndPoint object will allow us to read datagrams sent from any source.
                        var receivedResults = await udpClient.ReceiveAsync();
                        message = Encoding.ASCII.GetString(receivedResults.Buffer);

                        XDocument doc = XDocument.Parse(message);

                        if (doc.Element("spot") != null)
                        {
                            Spot spot = new Spot();
                            spot = XmlConvert.DeserializeObject<Spot>(message);
                            DateTime date = DateTime.Parse(spot.Timestamp, System.Globalization.CultureInfo.CurrentCulture);
                            label = string.Format("Spot {0} : {1} QRG:{2,9:N1} DX: {3} DE: {4}",
                                        spot.Action, date.ToLongTimeString(), float.Parse(spot.Frequency), spot.Dxcall, 
                                        spot.Spottercall);
                            if (spot.Action == "add")
                                Application.Current.Dispatcher.Invoke(new Action(() =>  {
                                    SpotLabel.Content = label; 
                                }));
                        }
                        else if (doc.Element("AppInfo") != null)
                        {
                            AppInfo appInfo = new AppInfo();
                            appInfo = XmlConvert.DeserializeObject<AppInfo>(message);
                            // Do something with appInfo data
                        }
                        else if (doc.Element("RadioInfo") != null)
                        {
                            RadioInfo radioInfo = new RadioInfo();
                            radioInfo = XmlConvert.DeserializeObject<RadioInfo>(message);
                            label = string.Format("Radio Nr {0} Rx: {1, 9:N2} Tx: {2, 9:N2} Split: {3} InFreq: {4, 9:N2} ActR: {5} FocR: {6} RSP: {7} Tech: {8} ST: {9}",
                                    radioInfo.RadioNr, radioInfo.Freq / 100f, radioInfo.TXFreq / 100f, radioInfo.IsSplit, radioInfo.InactiveFreq / 100f, 
                                    radioInfo.ActiveRadioNr, radioInfo.FocusRadioNr, radioInfo.IsRunning.ToUpper() == "TRUE" ? "Run" : "S&P", 
                                    radioInfo.Technique, radioInfo.StationType);
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                    if (radioInfo.RadioNr == 1)
                                        Radio1FreqLabel.Content = label;
                                    else
                                        Radio2FreqLabel.Content = label;
                                }));
                        }
                        else if (doc.Element("contactinfo") != null)
                        {
                            Contactinfo contactInfo = new Contactinfo();
                            contactInfo = XmlConvert.DeserializeObject<Contactinfo>(message);
                            label = string.Format("Most recently logged call: {0}", contactInfo.Call);
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                LogLabel.Content = label;
                            }));
                        }
                        else if (doc.Element("dynamicresults") != null)
                        {
                            Dynamicresults dynamicResults = new Dynamicresults();
                            dynamicResults = XmlConvert.DeserializeObject<Dynamicresults>(message);
                            label = string.Format("Score: {0}", dynamicResults.Score);
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                ScoreLabel.Content = label;
                            }));
                        }
                    }
                }
                
            });
        }

        private void SaveLocation(object sender, EventArgs e)
        {
            // Remember window location 
            Properties.Settings.Default.Top = this.Top;
            Properties.Settings.Default.Left = this.Left;
            Properties.Settings.Default.Save();
        }
    }
}
