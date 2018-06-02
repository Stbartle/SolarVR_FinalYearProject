using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour {
    public Text dispText; //Text to be displayed on the screen in testing
    public int PanelIndicator;
    int objectCount;
    Texture2D a;
    float grayscaleMinimum = 60.94f/*96.15f*/; //Absolute minimum illumination on cell
    float grayscaleMaximum = 195.00f/*300.03f*/; //Absolute maximum illumination on cell
    public Transform pause;

    // Use this for initialization
    void Start () {
        PanelIndicator = 0;
        objectCount = 0;
        dispText.text = " "; //Set the initial text to be blank so that no unknown values will be present
    }
    
	// Update is called once per frame
	void LateUpdate () {    
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Panel");
        objectCount = objs.Length;

        if (PanelIndicator >= objectCount)
        {
            PanelIndicator--;
            textUpdate(a);
        }
        else
        {
            for (int i = 0; i < objectCount; i++)
            {

                if (i == PanelIndicator)
                {
                    Destroy(a);
                    a = new Texture2D(15, 14); //Create Texture2D for the display on screen 
                    a.SetPixels(objs[i].GetComponentInChildren<TextureSample>().captureTexture.GetPixels(6, 10, 15, 14)); //Sample pixels in the defined range from the exracted renderTexture
                    //////float temp = objs[i].GetComponentInChildren<BaseController>().xRotation;
                    textUpdate(a);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            if (PanelIndicator < objectCount) PanelIndicator++;
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
           if (PanelIndicator > 0) PanelIndicator--;
        }
        
        if (Input.GetKeyUp(KeyCode.I)) PanelIndicator = 0;
        
    }


    void textUpdate(Texture2D c) //Text display function used in testing
    {
        Color[] alpha = new Color[210];
        alpha = c.GetPixels(0, 0, 15, 14);
        dispText.text = (PanelIndicator+1).ToString("F0")+ "/" + objectCount.ToString("F0")+ " " +map(averageColour(alpha), grayscaleMinimum, grayscaleMaximum).ToString("F1");
        //dispText.text = (PanelIndicator + 1).ToString("F0") + "/" + objectCount.ToString("F0") + ", Panel Angle:" + angle.ToString("F0") + " degrees";
    }
    float averageColour(Color[] c)
    {
        float avg = 0;
        float sumRed = 0, sumGreen = 0, sumBlue = 0; //Summation of R/G/B components in the Color[] array
        for (int i = 0; i < c.Length; i++)
        {
            //Cycle through the array, then sum the R/G/B components with one-another
            sumRed += c[i].r;
            sumGreen += c[i].g;
            sumBlue += c[i].b;
        }
        avg = (sumRed + sumBlue + sumGreen) / 3;
        return avg;
    }

    float map(float s, float aMin, float aMax)
    {
        //Function to map a value in a range i.e. grayscale range to another value in a differing range i.e. percentage in a range of 0 to 1000.
        float value = 0;

        value = ((s - aMin) * 1000) / (aMax - aMin);

        return value;
    }
}
