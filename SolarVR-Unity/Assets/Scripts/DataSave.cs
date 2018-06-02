using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;

public class DataSave : MonoBehaviour
{
    public GameObject server;
    float month;
    float day;
    float hour;
    public int monthStart;
    public int monthFinish;
    public int dayStart;
    public int dayFinish;
    public int hourStart;
    public int hourFinish;
    public Slider hourSlider;
    public Slider monthSlider;
    public Slider daySlider;
    // Use this for initialization
    void Start()
    {
        hourSlider.value = hourStart;
        monthSlider.value = monthStart;
        daySlider.value = dayStart;
        hour = hourStart;
        day = dayStart;
        month = monthStart;
        //month = PlayerPrefs.GetFloat("Month");
        //day = PlayerPrefs.GetFloat("Day");
        //hour = PlayerPrefs.GetFloat("Hour");
        //InvokeRepeating("TimeUpdate", 11f, 2f);
        //InvokeRepeating("Record", 10.7f, 2f);
    }

    // Update is called once per frame
    void Update()
    {

        
        month = PlayerPrefs.GetFloat("Month");
        day = PlayerPrefs.GetFloat("Day");
        hour = PlayerPrefs.GetFloat("Hour");
        double pow = server.GetComponentInChildren<NamedPipesServerStream>().dataFromClient;
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Record();
        }
    }
    private void TimeUpdate()
    {
        //UnityEngine.Debug.Log("Time");
        hour++;
        if (hour > hourFinish)
        {
            hour = hourStart;
            day++;
        }
        if ((month == 2 && day == 28) || ((month == 4 || month == 6 || month == 9 || month == 11) && day == 30))
        {
            day = 32;
        }
        if (day > 31 || day > dayFinish)
        {
            day = dayStart;
            if (month == 1)
            {
                month = 7;
            }
            else if (month == 7)
            {
                month = 1;
            }
        }
        if (month > monthFinish)
        {
            month = monthStart;
        }



        hourSlider.value = hour;
        monthSlider.value = month;
        daySlider.value = day;
    }

    private void Record()
    {
        //UnityEngine.Debug.Log("not");
        double pow = server.GetComponentInChildren<NamedPipesServerStream>().dataFromClient;
        //UnityEngine.Debug.Log("2018" + month.ToString("00") + day.ToString("00") + ", " + ((int)hour).ToString("00") + ", " + Math.Round(pow, 2));

        StreamWriter sw = new StreamWriter(@"C:\Users\Sam\Documents\outputInfo.txt", true);
        sw.WriteLine(string.Format("2018" + month.ToString("00") + day.ToString("00") + ", " + ((int)hour).ToString("00") + ", " + Math.Round(pow, 2)));
        sw.Close();
        hour = hourSlider.value;
        hour++;
        hourSlider.value = hour;
        if (hourSlider.value > hourFinish)
        {
            hourSlider.value = hourStart;
            day = daySlider.value;
            day++;
            daySlider.value = day;
        }
    }
}
