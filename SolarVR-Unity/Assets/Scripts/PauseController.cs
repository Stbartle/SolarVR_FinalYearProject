using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public Transform cap;

    public void PauseGame()
    {
        cap.gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
    public void RestartGame()
    {
        cap.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
