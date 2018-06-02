using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour
{
    public GameObject selectedObject; //Used in the selection Target
    Ray castRay;
    RaycastHit hitInfo;
    GameObject hitObject;
    public Transform pause;

    void Update()
    {
        castRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(castRay, out hitInfo) && pause.gameObject.transform.localScale == new Vector3(0, 0, 0))
        {
            hitObject = hitInfo.transform.root.gameObject;

            if (hitObject.GetComponentInChildren<Collider>().tag == "Panel")
            {
                
                if (hitObject.GetComponentInChildren<BaseController>().Drag != true)
                {

                    SelectObject(hitInfo.collider.gameObject);
                }
            }
            else
            {
                ClearSelection();
            }
        }
        else
        {
            ClearSelection();
        }
    }

    void SelectObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            if (obj == selectedObject)
                return;

            ClearSelection();
        }
        selectedObject = obj;
    }

    void ClearSelection()
    {
        if (selectedObject == null)
            return;
        selectedObject = null;
    }
}