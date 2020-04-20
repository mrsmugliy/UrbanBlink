using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript : MonoBehaviour
{
    public bool LineActive;
    public bool allActive;
    public GameObject R;
    public GameObject L;
    public GameObject blackR;
    public GameObject blackL;
    public Material m_black;
    public Material m_grey;
    public Material m_blue;

    void Start()
    {
        if (this.transform.name == "CR" && this.transform.position.y == 0)
        {
            LineActive = true;
            transform.parent.gameObject.GetComponent<TestMovement>().Player.GetComponent<PlayerHitDetection>().DirLane = this.transform;
           // Debug.Log("Nashlas");
        }
        else if (this.transform.name != "CR" && this.transform.position.y!= 0)
        {
            LineActive = false;
        }
        if (this.transform.position.y == 0)
        {
            allActive = true;
        }
        else
        {
            allActive = false;
        }
    }

    
    void Update()
    {
        if (allActive && blackR.GetComponent<Renderer>().enabled == false)
        {
            blackR.GetComponent<Renderer>().enabled = true;
            blackL.GetComponent<Renderer>().enabled = true;
            R.GetComponent<Renderer>().enabled = true;
            L.GetComponent<Renderer>().enabled = true;

        }
        else if (!allActive && blackR.GetComponent<Renderer>().enabled == true)
        {
            blackR.GetComponent<Renderer>().enabled = false;
            blackL.GetComponent<Renderer>().enabled = false;
            R.GetComponent<Renderer>().enabled = false;
            L.GetComponent<Renderer>().enabled = false;
        }

        if (LineActive && R.GetComponent<Renderer>().material != m_blue)
        {
            R.GetComponent<Renderer>().material = m_blue;
            L.GetComponent<Renderer>().material = m_blue;
        }
        else if (!LineActive && R.GetComponent<Renderer>().material != m_grey)
        {
            R.GetComponent<Renderer>().material = m_grey;
            L.GetComponent<Renderer>().material = m_grey;

        }
    }
}
