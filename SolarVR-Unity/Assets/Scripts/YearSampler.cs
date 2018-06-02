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
using System.Runtime.Serialization.Formatters.Binary;

public class YearSampler : MonoBehaviour
{
    public Text dispText; //Text to be displayed on the screen in testing
    private Thread pipeThread; // use to create new thread
    public Double sendBytes;
    public double[] cellSpace = new double[6];
    public int index = 0;
    public double dataFromClient;
    public double dataFromClient1;
    public double dataFromClient2;
    ProcessStartInfo theProcess = new ProcessStartInfo("ClientApp.exe.lnk");
    //ProcessStartInfo theProcess = new ProcessStartInfo(@"C:\Users\Sam\Desktop\Move and Rotate Test\Assets\ClientApp.exe.lnk");
    public Process myProc;
    public bool activeValues = false;

    public float hourSun = 1;
    float minuteSun = 1;
    public float monthSun = 1;
    public float daySun = 1;

    public Slider monthSl;
    public Slider daySl;
    public Slider hourSl;
    DateTime time;

    public int dayStart;
    public int dayFinish;
    public int hourStart;
    public int hourFinish;
    public int monthStart;
    public int monthFinish;

    void Start()
    {
        Application.runInBackground = true;
        pipeThread = new Thread(ServerThread); //create new thread
        pipeThread.Start();
        UnityEngine.Debug.Log("StartThread");
        InvokeRepeating("getData", 10f, 0.1f); //starting at time 'a', and repeating every 'b' time interval 
        dispText.text = " ";
        InvokeRepeating("LaunchClient", 0.1f, 0);
        InvokeRepeating("LightUpdate", 2f, 0.05f);
        for (int i = 0; i < 6; i++)
        {
            cellSpace[i] = 0;
        }
        dataFromClient = 0;

        changeTime(1, 1, 7);
    }

    void Update()
    {

    }
    void Awake()
    {
        SceneManager.sceneUnloaded += onLeaveScene;
    }
    private void LightUpdate()
    {
        changeTime((int)monthSun, (int)daySun, (int)hourSun);
    }
    private void getData()
    {

        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Panel");
        sendBytes = objs.Length;

        if (index >= sendBytes) index = 0;

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
                }
            }
        }
        //dispText.text = "Power:" + dataFromClient.ToString("F2") + "W, at:" + dataFromClient1.ToString("F2") + "V," + dataFromClient2.ToString("F2") + "A";
        dispText.text = "Month:" + monthSun.ToString("F0") + ", Day:" + daySun.ToString("F0") + ", Hour:" + hourSun.ToString("F0");
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
                for (int simMonth = monthStart; simMonth <= monthFinish; simMonth++)
                {
                    monthSun = simMonth;
                    for (int simDay = dayStart; simDay <= dayFinish; simDay++)
                    {
                        daySun = simDay;
                        for (int simHour = hourStart; simHour <= hourFinish; simHour++)
                        {
                            hourSun = simHour;

                            Thread.Sleep(120);
                            for (int i = 0; i < 6; i++)
                            {
                                serverStream.WriteBytes(Math.Round(cellSpace[i], 1));
                                pipeServer.Flush();
                            }
                            Thread.Sleep(100);
                            dataFromClient = serverStream.ReadBytes();
                            dataFromClient1 = serverStream.ReadBytes();
                            dataFromClient2 = serverStream.ReadBytes();

                            Save(simMonth, simDay, simHour, Math.Round(dataFromClient, 1));
                            Thread.Sleep(850);
                        }
                        if ((simMonth == 2 && simDay == 28) || ((simMonth == 4 || simMonth == 6 || simMonth == 9 || simMonth == 11) && simDay == 30))
                        {
                            simDay = 32;
                        }
                        UnityEngine.Debug.Log("End of Day");
                    }
                    //Collect only July and January
                    //if (simMonth == 1)
                    //{
                    //    simMonth = 6;
                    //}
                    //else if (simMonth == 7)
                    //{
                    //     simMonth = 12;
                    //}
                }
                activeValues = false;
            }
        }
        pipeServer.Close();
    }
    public void Save(float mon, float d, float h, double pow)
    {
        StreamWriter file = new StreamWriter(@"C:\Users\Sam\Documents\outputInfo.txt", true);
        file.WriteLine(mon + ", " + d + ", " + h + ", " + pow);
        file.Close();
        // UnityEngine.Debug.Log("Hour Saved");
    }
    void changeTime(int month, int day, int hour)
    {
        monthSl.value = month;
        daySl.value = day;
        hourSl.value = hour;
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
            ///UnityEngine.Debug.Log("Why?");
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