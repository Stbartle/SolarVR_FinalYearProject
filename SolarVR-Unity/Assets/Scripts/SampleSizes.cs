using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSizes : MonoBehaviour {
    GameObject[] objs;
    public Text dispText;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        objs = GameObject.FindGameObjectsWithTag("Panel");
        int objectCount = objs.Length;
        dispText.text = objectCount.ToString("F1");
    }
}
