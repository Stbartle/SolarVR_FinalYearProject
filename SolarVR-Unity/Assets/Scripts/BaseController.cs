using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    //--- Structures ---//
    Plane planeMovement; //Movement plane of the panel
    RaycastHit hit; //For surface detection in Raycast
    Vector3 surfaceNormal; //Surface normal for the rotation of the panel(base)
    Vector3 forwardRelativeToSurfaceNormal; //For Look Rotation of the panel
    Vector3 offset; //Parent offset for system-movement

    //--- Global Variables ---//
    public bool Drag = false; //Boolean value for the active state in dragging the panel
    public bool Master = false; //Boolean value for the active status of the system master in movement

    public float xRotation; //Lateral (x) rotation of the panel
    public float yRotation; //Y rotation of the panel

    public GameObject prefabSingle; //Indicator for what this object is, and how it must replicate itself
    public GameObject panelChild; //Public Variable as the parent.

    public Transform Par; //Container for the parent transform when in system movement

    public RawImage dispImage;

    //Unsorted
    bool oneTime = false; //Boolean value to cut of dupliction of panels after single intantiation for each button press
    float groundDistance; //Used in the topology angle correction process
    Quaternion targetRotation;//" "
    Vector3 locPos; //" "
    Ray camRay;
    GameObject newPanel;
    RenderTexture rt;
    bool planeEnable;
    bool singleRemainder;
    GameObject[] objs;
    private Quaternion prevRot = new Quaternion(0, 0, 0, 0);
    public TextMesh valueText;
    public Transform pause;


    void Start()
    {
        Par = this.transform.parent;
        //dispImage.texture = this.GetComponentInChildren<TextureSample>().captureTexture;
        valueText = GetComponentInChildren<TextMesh>();
        valueText.transform.localScale = transform.localScale.Inverse();
        valueText.transform.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.cyan);
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // If mouse button released, end dragging
            Drag = false;
            //planeMovement.destroy();
            valueText.transform.localScale = new Vector3(this.transform.localScale.x * 0.9f, this.transform.localScale.y * 0.9f, this.transform.localScale.z * 0.9f);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Drag = false;
            Master = false;
            command(false);
            valueText.transform.localScale = new Vector3(this.transform.localScale.x * 0.9f, this.transform.localScale.y * 0.9f, this.transform.localScale.z * 0.9f);
        }
        if (Input.GetMouseButtonUp(2))
        {
            Debug.Log("Release Middle click.");
        }

        if (Drag)
        {   // If drag is true, object follow the mouse pointer in plane
            camRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Get the poisiton of the mouse to the screen.
            float distance; //Initialise the distance variable

            if (planeMovement.Raycast(camRay, out distance))
            {
                // Transform Position to hit position
                transform.position = camRay.GetPoint(distance);
                //Transform Rotation
                topologyRotation(this.gameObject);
            }

            // Alter the X/Y rotation from key presses 

            if (Input.GetKeyUp(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift)) xRotation -= 45f;
            else if (Input.GetKeyUp(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) xRotation += 45f;
            else if (Input.GetKeyUp(KeyCode.Q)) xRotation -= 5f;
            else if (Input.GetKeyUp(KeyCode.W)) xRotation += 5f;
            if (Input.GetKeyUp(KeyCode.E)) xRotation = 0;

            if (Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) yRotation -= 45f;
            else if (Input.GetKeyUp(KeyCode.A)) yRotation -= 10f;
            if (Input.GetKeyUp(KeyCode.S) && Input.GetKey(KeyCode.LeftShift)) yRotation += 45f;
            else if (Input.GetKeyUp(KeyCode.S)) yRotation += 10f;
            if (Input.GetKeyUp(KeyCode.D)) yRotation = 0;
            
        }
        if (Master)
        {
            Par.transform.position = this.transform.position + offset;
            for (int i = 0; i < Par.transform.childCount; i++)
            {
                Transform child = Par.transform.GetChild(i);
                if (Input.GetKeyUp(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift)) masterRotation(child, true, -45);
                else if (Input.GetKeyUp(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) masterRotation(child, true, 45);
                else if (Input.GetKeyUp(KeyCode.Q)) masterRotation(child, true, -5);
                else if (Input.GetKeyUp(KeyCode.W)) masterRotation(child, true, 5);
                if (Input.GetKeyUp(KeyCode.E)) masterRotation(child, true, 0);

                if (Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) masterRotation(child, false, -45);
                else if (Input.GetKeyUp(KeyCode.A)) masterRotation(child, false, -10);
                if (Input.GetKeyUp(KeyCode.S) && Input.GetKey(KeyCode.LeftShift)) masterRotation(child, false, 45);
                else if (Input.GetKeyUp(KeyCode.S)) masterRotation(child, false, 10);
                if (Input.GetKeyUp(KeyCode.D)) masterRotation(child, false, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            oneTime = true;
            addPanel();
            System.GC.Collect();
        }
        if (Input.GetKeyDown(KeyCode.Minus) && Drag == true && singleRemainder != true)
        {
            System.GC.Collect();
            //GetComponentInChildren<TextureSample>().rend.Release();
            Destroy(this.gameObject);

        }
        if (transform.hasChanged || Par.transform.hasChanged)
        {
            panelChild.transform.localEulerAngles = new Vector3(xRotation - transform.localEulerAngles.x, 0.0f, 0.0f);
            topologyRotation(this.gameObject);
            transform.hasChanged = false;
        }


        objs = GameObject.FindGameObjectsWithTag("Panel");
        int objectCount = objs.Length;
        if (objectCount == 1)
        {
            singleRemainder = true;
        }
        else
        {
            singleRemainder = false;
        }
        for (int i = 0; i < objectCount; i++)
        {
            if (objs[i] == this.transform.gameObject)
            {
                valueText.text = (i+1).ToString("F0");
            }
        }
        valueText.transform.rotation = Camera.main.transform.rotation;
        //Quaternion.Euler(90, 0, 0);
    }
    void OnMouseOver()
    {
        if (pause.gameObject.transform.localScale == new Vector3(0, 0, 0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                newPlane(); // Create the movement plane
                Drag = true; // Start dragging operation
                valueText.transform.localScale = transform.localScale.Inverse();

            }
            else if (Input.GetMouseButtonDown(1))
            {
                newPlane();
                Drag = true;
                Master = true;
                command(true);
                valueText.transform.localScale = transform.localScale.Inverse();
            }
            else if (Input.GetMouseButtonDown(2))
            {
                Debug.Log("Pressed middle click.");
            }
        }
    }
    void newPlane()
    {
        //Create new plane in the forward and right directions, green and red axes
        planeMovement = new Plane(transform.up, transform.position);
    }
    void addPanel()
    {
        if (oneTime && Drag == true)
        {
            newPanel = Instantiate(prefabSingle, new Vector3(-1, transform.localScale.y / 2, -1), new Quaternion(0, this.transform.rotation.y, 0, 0), this.transform.parent);
            oneTime = false;
            newPanel.GetComponent<BaseController>().Drag = false; //Stops recursive duplication, copy -> copy + copy(copy) -> copy + copty(copy) + copy(copy(copy)) etc. for multiple button presses
        }
    }
    void topologyRotation(GameObject g)
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 8))
        {
            groundDistance = hit.distance; //Set the distance from the ray to the screen as the distance to the ground
            surfaceNormal = hit.normal; // Assign the normal of the surface to surfaceNormal
            forwardRelativeToSurfaceNormal = Vector3.Cross(g.transform.right, surfaceNormal);

            targetRotation = Quaternion.LookRotation(forwardRelativeToSurfaceNormal, surfaceNormal); //check For target Rotation.
            g.transform.rotation = targetRotation; //Rotate Character accordingly.
            g.transform.eulerAngles = new Vector3(g.transform.eulerAngles.x, yRotation, g.transform.eulerAngles.z);
            if (prevRot != targetRotation)
            {
                newPlane();
                prevRot = targetRotation;
            }

            //Keep global y position to be 50% above the centroid, i.e. the bottom of the object to be underneath ontop of the surface. Only work if centroid is the origin
            locPos = g.transform.localPosition;
            locPos.y = (g.transform.localPosition.y - groundDistance + g.transform.localScale.y / 2 + 0.001f);
            g.transform.localPosition = locPos;

        }
    }
    void command(bool status)
    {
        if (status)
        {
            if (this.transform.parent == null) return;
            Par = this.transform.parent;
            this.transform.parent = null;
            offset = Par.transform.position - this.transform.position;
        }
        else
        {
            if (this.transform.parent != null) return;
            this.transform.parent = Par;
        }
    }
    void masterRotation(Transform chi, bool rot, float amount)
    {
        if (rot)
        {
            if (amount == 0) chi.gameObject.GetComponent<BaseController>().xRotation = 0;
            else chi.gameObject.GetComponent<BaseController>().xRotation += amount;
        }
        else
        {
            if (amount == 0) chi.gameObject.GetComponent<BaseController>().yRotation = 0;
            else chi.gameObject.GetComponent<BaseController>().yRotation += amount;
        }
    }
}
public static class Vector3Extensions
{
    public static Vector3 Inverse(this Vector3 v)
    {
        return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
    }
}