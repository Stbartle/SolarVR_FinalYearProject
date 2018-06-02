using UnityEngine;
using System.Collections;

public class SelectionIndicator : MonoBehaviour
{
    MouseManager mm;
    Color32 def;
    Bounds bigBounds;
    Vector3 rot;
    Vector3 pos;
    Vector3 scale;
    float diameter;

    void Start()
    {
        mm = GameObject.FindObjectOfType<MouseManager>();
        def = this.gameObject.GetComponentInChildren<Renderer>().material.color;
    }

    void Update()
    {
        if (mm.selectedObject != null)
        {
            if (mm.selectedObject.GetComponent<BaseController>().Drag == true)
            {
                if (mm.selectedObject.GetComponent<BaseController>().Master != true)
                {
                    selected(mm.selectedObject, def);
                }
                else
                {
                    selected(mm.selectedObject, def * new Color(0.2f, 0.9f, 0.5f));
                }
            }
            else
            {
                selected(mm.selectedObject, def * new Color(0.6f, 0.2f, 0.9f));
            }

            
        }
        else
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
    }
    void selected(GameObject pass, Color32 c)
    {
        this.gameObject.GetComponentInChildren<Renderer>().material.color = c;
        bigBounds = pass.GetComponentInChildren<Renderer>().bounds;
        rot = pass.GetComponentInChildren<Transform>().eulerAngles;
        pos = pass.GetComponentInChildren<Transform>().position;
        scale = pass.GetComponentInChildren<Transform>().localScale;

        diameter = bigBounds.size.z;
        diameter *= 1.25f;

        this.transform.position = new Vector3(bigBounds.center.x, pos.y - scale.y / 2 + 0.001f, bigBounds.center.z);
        this.transform.localScale = new Vector3(bigBounds.size.x * 1.5f, bigBounds.size.y * 1.5f, bigBounds.size.z * 1.5f);
        this.transform.localEulerAngles = new Vector3(rot.x, rot.y, rot.z);
    }
}