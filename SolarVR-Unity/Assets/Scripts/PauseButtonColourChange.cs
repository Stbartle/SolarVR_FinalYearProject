using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonColourChange : MonoBehaviour {
    public Transform pause;
    private Button theButton;
    private ColorBlock theColors;
	// Use this for initialization
	void Awake () {
        theButton = GetComponent<Button>();
        theColors = GetComponent<Button>().colors;
        pause.gameObject.transform.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (pause.gameObject.transform.localScale == new Vector3(0, 0, 0))
        {
            theColors.normalColor = Color.red;
            theColors.highlightedColor = Color.cyan;
        }
        else
        {
            theColors.normalColor = Color.green;
            theColors.highlightedColor = Color.green;
        }
        theButton.colors = theColors;
	}
}
