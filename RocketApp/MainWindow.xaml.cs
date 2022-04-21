using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RocketApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<DispatcherTimer> timers = new List<DispatcherTimer>();
        public MainWindow()
        {
            InitializeComponent();
            _ = Worker();
        }
        private async Task Worker()
        {
            bool tryAgain = true;
            while (tryAgain)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5000/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-Key", "API_KEY_1");
                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("rockets");
                    if (response.IsSuccessStatusCode)
                    {
                        List<RocketApiModel> rockets = await response.Content.ReadAsAsync<List<RocketApiModel>>();
                        tryAgain = false;
                        foreach (var item in rockets)
                        {
                            TcpManager manager = new TcpManager(item.telemetry.host, item.telemetry.port, ClientServer.client);
                            manager.OpenConnection();
                            DispatcherTimer timer = new DispatcherTimer();
                            timer.Tick += (sender, e) => { TimerEvent(sender, e, manager); };
                            timer.Interval = TimeSpan.FromMilliseconds(100);
                            timer.Start();
                            timers.Add(timer);
                            ListView1.Items.Add(JsonConvert.SerializeObject(item));

                        }

                    }
                    else
                    {
                    }
                }
            }
        }
        private void TimerEvent(object sender, EventArgs e, TcpManager manager)
        {
            byte[] receivedData;
            manager.Read(out receivedData, 36);
            if(receivedData != null && receivedData.Length == 36)
            {
                byte[] header = Common.SubArray(receivedData, 1, 10);
                var telemetri = new TelemetriSystem()
                {
                    header = new header
                    {
                        startByte = receivedData[0],
                        Id = Encoding.Default.GetString(header),
                        packetNumber = receivedData[12],
                        packetSize = receivedData[13]
                    },
                    playload = new playloadTelemetri
                    {
                        Altitude = BitConverter.ToSingle(Common.SubArray(receivedData, 14, 4), 0),
                        Speed = BitConverter.ToSingle(Common.SubArray(receivedData, 18, 4), 0),
                        Acceleration = BitConverter.ToSingle(Common.SubArray(receivedData, 22, 4), 0),
                        Thrust = BitConverter.ToSingle(Common.SubArray(receivedData, 26, 4), 0),
                        Temperature = BitConverter.ToSingle(Common.SubArray(receivedData, 30, 4), 0),
                    },
                    footer = new footer
                    {
                        CRC16 = BitConverter.ToInt16(Common.SubArray(receivedData, 34, 2)),
                        Delimiter = receivedData[36]
                    }

                };
                ListView2.Items.Add(JsonConvert.SerializeObject(telemetri));
            }
            

        }
    }
}

