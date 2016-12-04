using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;

namespace EDDNRecorder
{
    public partial class EDDNRecorder : Form
    {

        private string appdatapath;

        public EDDNRecorder()
        {

            InitializeComponent();


            appdatapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EDDNRecorder");

            if (!Directory.Exists(appdatapath))
                Directory.CreateDirectory(appdatapath);
        }

        private void button1_Click(object sender, EventArgs e)
        {


            GetEDDNData();
        }


        private void GetEDDNData()
        {
            using (StreamWriter file = new StreamWriter(Path.Combine(appdatapath, "EDDN" + DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss") + ".log")))
            {
                using (NetMQContext ctx = NetMQContext.Create())
                {
                    using (var subscriber = ctx.CreateSubscriberSocket())
                    {
                        subscriber.Connect("tcp://eddn-relay.elite-markets.net:9500");
                        subscriber.Subscribe(Encoding.Unicode.GetBytes(string.Empty));

                        while (true) //(this.eddnSubscriberSocket.HasIn)
                        {
                            byte[] response;
                            try
                            {
                                response = subscriber.ReceiveFrameBytes();
                            }
                            catch (NetMQException)
                            {
                                return;
                            }

                            var decompressedFileStream = new MemoryStream();
                            using (decompressedFileStream)
                            {
                                var stream = new MemoryStream(response);

                                // Don't forget to ignore the first two bytes of the stream (!)
                                stream.ReadByte();
                                stream.ReadByte();
                                using (var decompressionStream = new DeflateStream(stream, CompressionMode.Decompress))
                                {
                                    decompressionStream.CopyTo(decompressedFileStream);
                                }

                                decompressedFileStream.Position = 0;
                                var sr = new StreamReader(decompressedFileStream);
                                var myStr = sr.ReadToEnd();
                                //Log.Debug(myStr);

                                if (myStr.Contains("http://schemas.elite-markets.net/eddn/journal/"))
                                    file.WriteLine(myStr);

                                decompressedFileStream.Position = 0;
                                //var serializer = new DataContractJsonSerializer(typeof(EddnRequest));
                                //var rootObject = (EddnRequest)serializer.ReadObject(decompressedFileStream);
                                //var message = rootObject.Message;
                                /*                    Log.Debug(rootObject.SchemaRef);
                                                    Log.DebugFormat(
                                                        "Station: {0}, Item: {1}, BuyPrice: {2}",
                                                        message.StationName,
                                                        message.ItemName,
                                                        message.BuyPrice);
                                                        */
                                decompressedFileStream.Close();
                            }
                        }
                    }
                }
            }
        }
    }
}
