using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace LoginManager
{
    public delegate void RfidRead(string id);
    public delegate void RfidFail(string reason);
    public class RFID
    {
        private SerialPort p;

        private const int baud = 9600;
        private const Parity parity = Parity.None;
        private const int dataBits = 8;
        private const StopBits stopBits = StopBits.One;

        public event RfidRead IncomingRfid;
        public event RfidFail RfidFailure;

        private Thread readThread;
        private bool continueReading = false;

        private string Port = "COM3";

        public RFID()
        {
            init();
        }

        public RFID(int port)
        {
            Port = "COM" + port;

            init();
        }

        public RFID(string port)
        {
            Port = port;

            init();
        }

        private void init()
        {
            p = new SerialPort(Port, baud, parity, dataBits, stopBits);
            p.ReadTimeout = 10000;

            if (!continueReading)
            {
                continueReading = true;
                readThread = new Thread(Read);
                //readThread.SetApartmentState(ApartmentState.STA);
                readThread.Start();
            }


        }

        public void Close()
        {
            continueReading = false;
        }

        private void Read()
        {

            p.Open();

            while (continueReading)
            {
                if (p.BytesToRead != 0)
                {
                    //Console.WriteLine(p.ReadLine());
                    string result = p.ReadLine();

                    // Strip out meta-text
                    result = result.Replace("\u0003", "");
                    result = result.Replace("\u0002", "");
                    result = result.Replace("\r", "");
                    result = result.Replace("\n", "");

                    IncomingRfid(result);
                }
                else
                {
                    RfidFailure(string.Format("No Bytes to read on port {0}", p.PortName));
                }
            }

            p.Close();
        }
    }
}