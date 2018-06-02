using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class ChangeSceneDefault : MonoBehaviour
{
    public Transform opMain;
    public Transform pause;
    public bool tog;
    public GameObject server;
    void Start()
    {
        opMain.gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
    public void StartGameBtn(string newGameLevel)
    {
        SceneManager.LoadScene(newGameLevel);
    }
    public void closeProcess()
    {
        //Process temp = new Process();
        //YourScript script = server.GetComponent<NamedPipesServerStream>();
       // if (server.gameObject.GetComponent< NamedPipesServerStream >()  != null)
        //{
        //    temp = server.gameObject.GetComponentInChildren<NamedPipesServerStream>().myProc;
       // }
        //else
       // {
       //     temp = server.gameObject.GetComponentInChildren<YearSampler>().myProc;
       // }
            /*if ((server.gameObject.GetComponent("NamedPipesServerStream") as NamedPipesServerStream) != null)
        {
            UnityEngine.Debug.Log("NamedPipesServerSream Close server");
            temp = server.gameObject.GetComponentInChildren<NamedPipesServerStream>().myProc;
        }
        else
        {
            UnityEngine.Debug.Log("YearSampler Server Close");
            temp = server.gameObject.GetComponentInChildren<YearSampler>().myProc;
        }*/
       // temp.Kill();
    }
    public void ExitGameBtn()
    {
        Application.Quit();
    }
    public void optionsShowMain()
    {
        if (opMain.gameObject.transform.localScale == new Vector3(0, 0, 0))
        {
            opMain.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            opMain.gameObject.transform.localScale = new Vector3(0, 0, 0);
            tog = false;
        }

    }


    public void resumeGame()
    {
        pause.gameObject.transform.localScale = new Vector3(0, 0, 0);
        if (opMain.gameObject.transform.localScale == new Vector3(1, 1, 1))
        {
            opMain.gameObject.transform.localScale = new Vector3(0, 0, 0);
            tog = true;
        }
    }
    public void pauseGame()
    {
        if (tog == true)
        {
            opMain.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        pause.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
