using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

public class TextureSample : MonoBehaviour
{
    public RawImage dispImage; //Image to be displayed on the screen during testing
    public RenderTexture rend; //RenderTexture sampled from the panel, used in the colour detection

    public int xPos = 6; //Pixel position for starting sample on x-axis
    public int yPos = 10; //Pixel position for starting sample on y-axis
    int xVal = 0; //Variable pixel position on x-axis
    int yVal = 0; //Variable pixel position on y-axis
    public int span = 15; //Height/width of sample area of pixels for each cell
    int cellCountWidth = 6; //Amount of cells along the width of the panel, teh amount of columns
    int cellCountHeight = 12; //Amount of cells along the height of the panel, the amount of rows

    public int UserControlX = 6;
    public int UserControlY = 10;


    int widthRender = 0; //RenderTexture Rend Width
    int heightRender = 0; //RednerTexture Rend height

    public Color[][] cellData = new Color[72][]; //Container for colours present on each cell [i][], i represents the cell on the panel, cell[][j] represents the colours of cell i
    public double[] cellString = new double[6];

    double grayscaleMinimum = 60.94f/*96.15f*/; //Absolute minimum illumination on cell
    double grayscaleMaximum = 195.00f/*300.03f*/; //Absolute maximum illumination on cell

    public GameObject singlePanel;
    public Texture2D captureTexture;
    Texture2D tempTexture;
    Texture2D a;

    void Start()
    {
        rend = new RenderTexture(128, 256, 24, RenderTextureFormat.ARGB32);
        rend.Create();
        Camera childCam = GetComponentInChildren<Camera>();
        childCam.targetTexture = rend;
        //rend.Release();
        widthRender = rend.width; //Sample the renderTexture width
        heightRender = rend.height; //Sample the renderTexture Heigt
        //singlePanel.transform.hasChanged = true;
        InvokeRepeating("getCellStrings", 1f, 0.2f); //starting at time 'a', and repeating every 'b' time interval
    }
    void getCellStrings()
    {
        for (int i = 0; i < 6; i++)
        {
            double[] cells = new double[12];
            for (int j = 0; j < 12; j++)
            {
                cells[j] = Math.Round((double)map(averageColour(cellData[i*12 + j])), 2);
            }
            cellString[i] =  cells.Min();
            if (cellString[i] < 0)
            {
                UnityEngine.Debug.Log("Check");
                cellString[i] = 0.001;
            }
        }
    }
    double averageColour(Color[] c)
    {
        double avg = 0;
        double sumRed = 0, sumGreen = 0, sumBlue = 0; //Summation of R/G/B components in the Color[] array
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
    double map(double s)
    {
        //Function to map a value in a range i.e. grayscale range to another value in a differing range i.e. percentage in a range of 0 to 1000.
        return ((s - grayscaleMinimum) * 1000) / (grayscaleMaximum - grayscaleMinimum);
    }
    void Update()
    {
        
        captureTexture = toTexture2D(rend, widthRender, heightRender); //Obtain the texture from the renderTexture
        imageUpdate(captureTexture);
        xVal = xPos; //Reset the x-axis pixel value
        yVal = yPos; //Reset the y-axis pixel value

        for (int i = 0; i < cellCountWidth; i++) //Iterate through the x-coordinates of the cells
        {
            for (int j = 0; j < cellCountHeight; j++)//Iterate through the y-coordinates of the cells
            {
                if (j == 3 || j == 6 || j == 9) //Compensate for texture dialation along y-axis
                {
                    yVal++; //Additional pixel off set every 3 cells
                }
                cellData[i * cellCountHeight + j] = captureTexture.GetPixels(xVal, yVal, span, span - 1); //Sample the cell colours into the holding array

                yVal += 20; //Increase pixel origin along the y-axis

            }
            yVal = yPos; //Reset y-axis to origin
            xVal += 20; //Increase pixel origin along the x-axis

        }

        if (Input.GetKeyUp(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift)) UserControlX -= 20;
        else if (Input.GetKeyUp(KeyCode.V) && Input.GetKey(KeyCode.LeftShift)) UserControlX += 20;
        else if (Input.GetKeyUp(KeyCode.X) && Input.GetKey(KeyCode.LeftShift)) UserControlX -= 1;
        else if (Input.GetKeyUp(KeyCode.C) && Input.GetKey(KeyCode.LeftShift)) UserControlX += 1;
        else if (Input.GetKeyUp(KeyCode.Z)) UserControlY -= 20;
        else if (Input.GetKeyUp(KeyCode.V)) UserControlY += 20;
        else if (Input.GetKeyUp(KeyCode.X)) UserControlY -= 1;
        else if (Input.GetKeyUp(KeyCode.C)) UserControlY += 1;
        //Reset to Origin
        if (Input.GetKeyUp(KeyCode.B))
        {
            UserControlX = 5;
            UserControlY = 9;
        }



    }
   
    //Used in Testing to show each cell or the entire panel view
    void imageUpdate(Texture2D te)
    {
        //a = new Texture2D(span, span-1); //Create Texture2D for the display on screen
        //a.SetPixels(te.GetPixels(UserControlX, UserControlY, span, span-1)); //Sample pixels in the defined range from the exracted renderTexture

        //Apply the changes to the screen
        dispImage.texture = te; //full texture
        //dispImage.texture = a; //single cell sample


    }
    
    Texture2D toTexture2D(RenderTexture rTex, int wid, int hei) //RenderTexture to Texture2D Conversion Function
    {
        Destroy(tempTexture);
        tempTexture = new Texture2D(wid, hei, TextureFormat.RGB24, false); //Create a new Texture2D

        /*() From reference ()*/RenderTexture currentActiveRT = RenderTexture.active;

        RenderTexture.active = rTex; //Set the current renderTexture to active i.e. enable the render
        tempTexture.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0); //Sample all pixels of the renderTexture

        /*() From reference ()*/RenderTexture.active = currentActiveRT;

        tempTexture.Apply(); //Apply the previous ReadPixels method to the texture
        return tempTexture; //Return the newly configured texture
    }
}