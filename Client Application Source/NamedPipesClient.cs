using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NamedPipeClientStream pipeclient = new NamedPipeClientStream(".", "HarryThePipe", PipeDirection.InOut);

            Console.WriteLine("Connecting to the server.....");
            pipeclient.Connect();
            Console.WriteLine("Server Connected");
            StreamBytes clientStream = new StreamBytes(pipeclient);

            MLApp.MLApp matlab = new MLApp.MLApp();
            matlab.Execute(@"cd C:\Users\Sam\Desktop\Proj");
            matlab.Execute("PanelFunction");
            

            while (pipeclient.IsConnected)
            {
                Console.WriteLine(".");

                //Read Variables
                Thread.Sleep(100);
                double cellAlpha = clientStream.ReadBytes();
                double cellBeta = clientStream.ReadBytes();
                double cellGamma = clientStream.ReadBytes();
                double cellDelta = clientStream.ReadBytes();
                double cellMu = clientStream.ReadBytes();
                double cellTau = clientStream.ReadBytes();
                Console.WriteLine("Received from Server: " + cellAlpha + "," + cellBeta + "," + cellGamma + "," + cellDelta + "," + cellMu + "," + cellTau);
                Object resultOfMatlab = null;
                matlab.Feval("PanelFunction", 3, out resultOfMatlab, cellAlpha, cellBeta, cellGamma, cellDelta, cellMu, cellTau);
                object[] res = resultOfMatlab as object[];

                double powerOutput = Convert.ToDouble(res[0]);
                double voltageOutput = Convert.ToDouble(res[1]);
                double currentOutput = Convert.ToDouble(res[2]);

                //Send Message to Server
                clientStream.WriteBytes(powerOutput);
                pipeclient.Flush();
                clientStream.WriteBytes(voltageOutput);
                pipeclient.Flush();
                clientStream.WriteBytes(currentOutput);
                pipeclient.Flush();


                Console.WriteLine("WrtieBytes Passed");
                pipeclient.Flush();
            }
            pipeclient.Close();

        }

    }
    public class StreamBytes
    {
        private Stream ioStream;
        //private UnicodeEncoding streamEncoding;
        private BinaryWriter outStream;
        private BinaryReader inStream;

        public StreamBytes(Stream ioStream)
        {
            this.ioStream = ioStream;
            outStream = new BinaryWriter(ioStream);
            inStream = new BinaryReader(ioStream);
        }

        public double ReadBytes()
        {
            return inStream.ReadDouble();
        }

        public void WriteBytes(double outValue)
        {
            outStream.Write(outValue);
        }
    }
}
