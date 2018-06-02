using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeConversion : MonoBehaviour {
    public float currentHour;
    public float currentMinute;
    public float currentMonth;
    public float currentDay;

    public Text dispText;
    public Text dText;
    float temp;
    System.TimeSpan timeInput;
    public Slider hourSlider;
    public Slider minuteSlider;
    public Slider monthSlider;
    public Slider daySlider;
    string activeMonth;
    List<string> MonthIndex = new List<string>() { "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    private readonly Dictionary<string, int> months = new Dictionary<string, int>()
    {
        {"January", 31},
        {"Febuary", 28},
        {"March", 31},
        {"April", 30},
        {"May", 31},
        {"June", 30},
        {"July", 31},
        {"August", 31},
        {"September", 30},
        {"October", 31},
        {"November", 30},
        {"December", 31}
    };
    // Use this for initialization
    void Start () {
        dispText.text = " ";
        dText.text = " ";

        if (PlayerPrefs.HasKey("Hour") == true)
        {
            currentHour = PlayerPrefs.GetFloat("Hour");
            currentMinute = PlayerPrefs.GetFloat("Minute");
            currentMonth = PlayerPrefs.GetFloat("Month");
            currentDay = PlayerPrefs.GetFloat("Day");
        }
        else
        {
            currentHour = DateTime.Now.Hour;
            currentMinute = DateTime.Now.Minute;
            currentDay = DateTime.Now.Day;
            currentMonth = DateTime.Now.Month;
            PlayerPrefs.SetFloat("Hour", currentHour);
            PlayerPrefs.SetFloat("Minute", currentMinute);
            PlayerPrefs.SetFloat("Month", currentMonth);
            PlayerPrefs.SetFloat("Day", currentDay);
            monthSlider.value = currentMonth;
            daySlider.value = currentDay;
            hourSlider.value = currentHour;
            minuteSlider.value = currentMinute;

        }
        daySlider.minValue = 1;
        monthSlider.value = currentMonth;
        daySlider.value = currentDay;
        hourSlider.value = currentHour;
        minuteSlider.value = currentMinute;

    }
	
	// Update is called once per frame
	void Update () {
        temp = (int)currentHour + (currentMinute / 60);
        timeInput = System.TimeSpan.FromHours(temp);
        dispText.text = timeInput.ToString("h\\:mm");
        daySlider.maxValue = months[ConvertMonth((int)currentMonth - 1)];
        
        dText.text = (MonthIndex[(int)currentMonth - 1]) + ":" + ((int)currentDay).ToString("F0");

        if (Input.GetKeyDown(KeyCode.T))
        {
            currentDay++;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            currentMonth++;
        }
        PlayerPrefs.SetFloat("Time", temp);
        PlayerPrefs.SetFloat("Hour", currentHour);
        PlayerPrefs.SetFloat("Minute", currentMinute);
        PlayerPrefs.SetFloat("Month", currentMonth);
        PlayerPrefs.SetFloat("Day", currentDay);
    }
    public string ConvertMonth(int m)
    {
        return MonthIndex[m];
    }
    public void adjustHour(float newHour)
    {
        currentHour = newHour;
    }
    public void adjustMinute(float newMinute)
    {
        currentMinute = newMinute;
    }
    public void adjustMonth(float newMonth)
    {
        currentMonth = newMonth;
    }
    public void adjustDay(float newDay)
    {
        currentDay = newDay;
    }
}
