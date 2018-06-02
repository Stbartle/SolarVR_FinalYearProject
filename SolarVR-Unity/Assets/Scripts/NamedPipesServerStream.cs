using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Pipes;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class NamedPipesServerStream : MonoBehaviour
{
    public Text dispText; //Text to be displayed on the screen in testing
    private Thread pipeThread; // use to create new thread
    public Double sendBytes;
    public double[] cellSpace = new double[6];
    //GameObject[] objs;
    public int index = 0;
    public double dataFromClient;
    public double dataFromClient1;
    public double dataFromClient2;
    ProcessStartInfo theProcess = new ProcessStartInfo(@"C:\Users\Sam\Desktop\SolarVR-Unity\Assets\ClientApp.exe.lnk");
	public Process myProc;
    public bool activeValues = false;

    void Start()
    {
        Application.runInBackground = true;
        pipeThread = new Thread(ServerThread); //create new thread
        pipeThread.Start();
        UnityEngine.Debug.Log("StartThread");
        InvokeRepeating("getData", 10f, 0.5f); //starting at time 'a', and repeating every 'b' time interval 
        dispText.text = " ";
        InvokeRepeating("LaunchClient", 0.1f, 0);
        for (int i = 0; i < 6; i++)
        {
            cellSpace[i] = 0;
        }
        dataFromClient = 0;
    }

    void Update()
    {

    }
    void Awake()
    {
        SceneManager.sceneUnloaded += onLeaveScene;
    }

    private void getData()
    {

        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Panel");
        sendBytes = objs.Length;
        if (index >= sendBytes)
        {
            index = 0;
        }
        for (int i = 0; i < sendBytes; i++)
        {
            if (i == index)
            {
                for (int j = 0; j < 6; j++)
                {
                    cellSpace[j] = objs[i].gameObject.GetComponentInChildren<TextureSample>().cellString[j];
                    if (cellSpace[j] != 0)
                    {
                        activeValues = true;
                    }
                    if (cellSpace[j] == 115.6)
                    {
                        cellSpace[j] = 115.65;
                    }
                }
            }
        }
        dispText.text = "Power:" + dataFromClient.ToString("F2") + "W, at:" + dataFromClient1.ToString("F2") + "V," + dataFromClient2.ToString("F2") + "A";
        index++;
    }
    void LaunchClient()
    {
        theProcess.WindowStyle = ProcessWindowStyle.Minimized;
        myProc = Process.Start(theProcess);
    }
    private void ServerThread()//pipe server thread
    {
        NamedPipeServerStream pipeServer = new NamedPipeServerStream("HarryThePipe", PipeDirection.InOut); //Create the named pipe, named SuperPipe, can send and recieve data

        UnityEngine.Debug.Log("Wait For Client");

        pipeServer.WaitForConnection(); //Wait until client connects to server; wait for MATLAB to connect through attached c# code
        UnityEngine.Debug.Log("Client connected ");
        StreamBytes serverStream = new StreamBytes(pipeServer);


        while (pipeServer.IsConnected)
        {
            while (activeValues)
            {
                for (int i = 0; i < 6; i++)
                {
                    if(i == 5)
                    {
                        serverStream.WriteBytes(0);
                    }
                    else
                    {
                        serverStream.WriteBytes(cellSpace[i]);
                    }
                    
                    pipeServer.Flush();
                }
                Thread.Sleep(100);

                dataFromClient = serverStream.ReadBytes();
                dataFromClient1 = serverStream.ReadBytes();
                dataFromClient2 = serverStream.ReadBytes();
                Thread.Sleep(350);
            }
        }
        pipeServer.Close();
    }

    void OnApplicationQuit()
    {
        if (pipeThread != null)
        {
            pipeThread.Abort();
            UnityEngine.Debug.Log("End Thread");
            myProc.Kill();
        }
    }
    void onLeaveScene<Scene>(Scene scene)
    {
        if (pipeThread != null)
        {
            pipeThread.Abort();
            SceneManager.sceneUnloaded -= onLeaveScene;
            UnityEngine.Debug.Log("Why?");
            myProc.Kill();
        }
    }

    public class StreamBytes
    {
        //private Stream ioStream;
        private BinaryWriter outStream;
        private BinaryReader inStream;

        public StreamBytes(Stream ioStream)
        {
            //this.ioStream = ioStream;
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