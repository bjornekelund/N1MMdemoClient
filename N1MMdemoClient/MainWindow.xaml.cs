// Demo broadcast listener for N1MM Logger+
// Receives UDP broadcasts on listenPort, parses XML into an object and prints info
// Intended as starting point for development of more applications such as 
// big screen score board, out-of-band alarm, etc.
// By Björn Ekelund SM7IUN sm7iun@ssa.se 2019-02-05

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

//using N1MMdemoClient.Properties;

namespace N1MMdemoClient
{
    // Definition of N1MM XML datagrams based on
    // http://n1mm.hamdocs.com/tiki-index.php?page=UDP+Broadcasts

    [XmlRoot(ElementName = "RadioInfo")]
    public class RadioInfo
    {
        [XmlElement(ElementName = "StationName")]
        public string StationName;
        [XmlElement(ElementName = "RadioNr")]
        public int RadioNr;
        [XmlElement(ElementName = "Freq")]
        public int Freq;
        [XmlElement(ElementName = "TXFreq")]
        public int TXFreq;
        [XmlElement(ElementName = "Mode")]
        public string Mode;
        [XmlElement(ElementName = "OpCall")]
        public string OpCall;
        [XmlElement(ElementName = "IsRunning")]
        public string IsRunning;
        [XmlElement(ElementName = "FocusEntry")]
        public string FocusEntry;
        [XmlElement(ElementName = "Antenna")]
        public string Antenna;
        [XmlElement(ElementName = "Rotors")]
        public string Rotors;
        [XmlElement(ElementName = "FocusRadioNr")]
        public int FocusRadioNr;
        [XmlElement(ElementName = "IsStereo")]
        public string IsStereo;
        [XmlElement(ElementName = "ActiveRadioNr")]
        public int ActiveRadioNr;
        [XmlElement(ElementName = "Technique")]
        public string Technique;
        [XmlElement(ElementName = "StationType")]
        public string StationType;
        [XmlElement(ElementName = "IsSplit")]
        public string IsSplit;
        [XmlElement(ElementName = "InactiveFreq")]
        public int InactiveFreq;
    }

    [XmlRoot(ElementName = "AppInfo")]
    public class AppInfo
    {
        [XmlElement(ElementName = "dbname")]
        public string Dbname;
        [XmlElement(ElementName = "contestnr")]
        public string Contestnr;
        [XmlElement(ElementName = "contestname")]
        public string Contestname;
        [XmlElement(ElementName = "StationName")]
        public string StationName;
    }

    [XmlRoot(ElementName = "spot")]
    public class Spot
    {
        [XmlElement(ElementName = "StationName")]
        public string StationName;
        [XmlElement(ElementName = "dxcall")]
        public string Dxcall;
        [XmlElement(ElementName = "frequency")]
        public string Frequency;
        [XmlElement(ElementName = "spottercall")]
        public string Spottercall;
        [XmlElement(ElementName = "comment")]
        public string Comment;
        [XmlElement(ElementName = "action")]
        public string Action;
        [XmlElement(ElementName = "status")]
        public string Status;
        [XmlElement(ElementName = "statuslist")]
        public string Statuslist;
        [XmlElement(ElementName = "timestamp")]
        public string Timestamp;
    }

    [XmlRoot(ElementName = "contactinfo")]
    public class Contactinfo
    {
        [XmlElement(ElementName = "contestname")]
        public string Contestname;
        [XmlElement(ElementName = "contestnr")]
        public string Contestnr;
        [XmlElement(ElementName = "timestamp")]
        public string Timestamp;
        [XmlElement(ElementName = "mycall")]
        public string Mycall;
        [XmlElement(ElementName = "band")]
        public string Band;
        [XmlElement(ElementName = "rxfreq")]
        public string Rxfreq;
        [XmlElement(ElementName = "txfreq")]
        public string Txfreq;
        [XmlElement(ElementName = "operator")]
        public string Operator;
        [XmlElement(ElementName = "mode")]
        public string Mode;
        [XmlElement(ElementName = "call")]
        public string Call;
        [XmlElement(ElementName = "countryprefix")]
        public string Countryprefix;
        [XmlElement(ElementName = "wpxprefix")]
        public string Wpxprefix;
        [XmlElement(ElementName = "stationprefix")]
        public string Stationprefix;
        [XmlElement(ElementName = "continent")]
        public string Continent;
        [XmlElement(ElementName = "snt")]
        public string Snt;
        [XmlElement(ElementName = "sntnr")]
        public string Sntnr;
        [XmlElement(ElementName = "rcv")]
        public string Rcv;
        [XmlElement(ElementName = "rcvnr")]
        public string Rcvnr;
        [XmlElement(ElementName = "gridsquare")]
        public string Gridsquare;
        [XmlElement(ElementName = "exchange1")]
        public string Exchange1;
        [XmlElement(ElementName = "section")]
        public string Section;
        [XmlElement(ElementName = "comment")]
        public string Comment;
        [XmlElement(ElementName = "qth")]
        public string Qth;
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "power")]
        public string Power;
        [XmlElement(ElementName = "misctext")]
        public string Misctext;
        [XmlElement(ElementName = "zone")]
        public string Zone;
        [XmlElement(ElementName = "prec")]
        public string Prec;
        [XmlElement(ElementName = "ck")]
        public string Ck;
        [XmlElement(ElementName = "ismultiplier1")]
        public string Ismultiplier1;
        [XmlElement(ElementName = "ismultiplier2")]
        public string Ismultiplier2;
        [XmlElement(ElementName = "ismultiplier3")]
        public string Ismultiplier3;
        [XmlElement(ElementName = "points")]
        public string Points;
        [XmlElement(ElementName = "radionr")]
        public string Radionr;
        [XmlElement(ElementName = "RoverLocation")]
        public string RoverLocation;
        [XmlElement(ElementName = "RadioInterfaced")]
        public string RadioInterfaced;
        [XmlElement(ElementName = "NetworkedCompNr")]
        public string NetworkedCompNr;
        [XmlElement(ElementName = "IsOriginal")]
        public string IsOriginal;
        [XmlElement(ElementName = "NetBiosName")]
        public string NetBiosName;
        [XmlElement(ElementName = "IsRunQSO")]
        public string IsRunQSO;
        [XmlElement(ElementName = "Run1Run2")]
        public string Run1Run2;
        [XmlElement(ElementName = "ContactType")]
        public string ContactType;
        [XmlElement(ElementName = "StationName")]
        public string StationName;
    }

    [XmlRoot(ElementName = "class")]
    public class Class
    {
        [XmlAttribute(AttributeName = "power")]
        public string Power;
        [XmlAttribute(AttributeName = "assisted")]
        public string Assisted;
        [XmlAttribute(AttributeName = "transmitter")]
        public string Transmitter;
        [XmlAttribute(AttributeName = "ops")]
        public string Ops;
        [XmlAttribute(AttributeName = "bands")]
        public string Bands;
        [XmlAttribute(AttributeName = "mode")]
        public string Mode;
        [XmlAttribute(AttributeName = "overlay")]
        public string Overlay;
    }

    [XmlRoot(ElementName = "qth")]
    public class Qth
    {
        [XmlElement(ElementName = "dxcccountry")]
        public string Dxcccountry;
        [XmlElement(ElementName = "cqzone")]
        public string Cqzone;
        [XmlElement(ElementName = "iaruzone")]
        public string Iaruzone;
        [XmlElement(ElementName = "arrlsection")]
        public string Arrlsection;
        [XmlElement(ElementName = "grid6")]
        public string Grid6;
    }

    [XmlRoot(ElementName = "qso")]
    public class Qso
    {
        [XmlAttribute(AttributeName = "band")]
        public string Band;
        [XmlAttribute(AttributeName = "mode")]
        public string Mode;
        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "mult")]
    public class Mult
    {
        [XmlAttribute(AttributeName = "band")]
        public string Band;
        [XmlAttribute(AttributeName = "mode")]
        public string Mode;
        [XmlAttribute(AttributeName = "type")]
        public string Type;
        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "point")]
    public class Point
    {
        [XmlAttribute(AttributeName = "band")]
        public string Band;
        [XmlAttribute(AttributeName = "mode")]
        public string Mode;
        [XmlText]
        public string Text;
    }

    [XmlRoot(ElementName = "breakdown")]
    public class Breakdown
    {
        [XmlElement(ElementName = "qso")]
        public List<Qso> Qso;
        [XmlElement(ElementName = "mult")]
        public List<Mult> Mult;
        [XmlElement(ElementName = "point")]
        public List<Point> Point;
    }

    [XmlRoot(ElementName = "dynamicresults")]
    public class Dynamicresults
    {
        [XmlElement(ElementName = "contest")]
        public string Contest;
        [XmlElement(ElementName = "call")]
        public string Call;
        [XmlElement(ElementName = "ops")]
        public string Ops;
        [XmlElement(ElementName = "class")]
        public Class Class;
        [XmlElement(ElementName = "club")]
        public string Club;
        [XmlElement(ElementName = "qth")]
        public Qth Qth;
        [XmlElement(ElementName = "breakdown")]
        public Breakdown Breakdown;
        [XmlElement(ElementName = "score")]
        public string Score;
        [XmlElement(ElementName = "timestamp")]
        public string Timestamp;
    }

    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Rotator));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Rotator)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "Rotator")]
    public class RotatorInfo
    {
        [XmlElement(ElementName = "station")]
        public string station;
        [XmlElement(ElementName = "radio")]
        public string radio;
        [XmlElement(ElementName = "stop")]
        public string stop;
        [XmlElement(ElementName = "go")]
        public string go;
        [XmlElement(ElementName = "azimuth")]
        public string azimuth;
        [XmlElement(ElementName = "frequency")]
        public string frequency;
    }

    [XmlRoot(ElementName = "PST")]
    public class RotatorInfoPST
    {
        [XmlElement(ElementName = "STOP")]
        public string stop;
        [XmlElement(ElementName = "CALL")]
        public string call;
        [XmlElement(ElementName = "AZIMUTH")]
        public string azimuth;
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
            Top = Properties.Settings.Default.Top;
            Left = Properties.Settings.Default.Left;

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
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
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
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                if (radioInfo.RadioNr == 1)
                                    Radio1FreqLabel.Content = label;
                                else
                                    Radio2FreqLabel.Content = label;
                            }));
                        }
                        else if (doc.Element("Rotator") != null)
                        {
                            RotatorInfo rinfo = new RotatorInfo();
                            rinfo = XmlConvert.DeserializeObject<RotatorInfo>(message);
                            if (rinfo.go == "1")
                                label = $"UDPRotator: Go: Station {rinfo.station} Radio Nr {rinfo.radio} az: {rinfo.azimuth}";
                            else
                                label = $"UDPRotator: Stop";
                            Application.Current.Dispatcher.Invoke(new Action(() =>

                            {
                                rotorLabel.Content = label;
                            }));
                        }
                        else if (doc.Element("PST") != null)
                        {
                            RotatorInfoPST rinfo = new RotatorInfoPST();
                            rinfo = XmlConvert.DeserializeObject<RotatorInfoPST>(message);
                            if (rinfo.stop == "1")
                                label = $"PSTRotator: Stop";
                            else
                                label = $"PSTRotator: call: {rinfo.call} az: {rinfo.azimuth}";
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                rotorLabel.Content = label;
                            }));
                        }
                        else if (doc.Element("contactinfo") != null)
                        {
                            Contactinfo contactInfo = new Contactinfo();
                            contactInfo = XmlConvert.DeserializeObject<Contactinfo>(message);
                            label = string.Format("Most recently logged call: {0}", contactInfo.Call);
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                LogLabel.Content = label;
                            }));
                        }
                        else if (doc.Element("dynamicresults") != null)
                        {
                            Dynamicresults dynamicResults = new Dynamicresults();
                            dynamicResults = XmlConvert.DeserializeObject<Dynamicresults>(message);
                            label = string.Format("Score: {0}", dynamicResults.Score);
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
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
            Properties.Settings.Default.Top = Top;
            Properties.Settings.Default.Left = Left;
            Properties.Settings.Default.Save();
        }
    }
}
