using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPieceScript : MonoBehaviour
{
    public string Name = "";
    public Transform BotSpawner;
    IEnumerator StartRunning;
    public bool isSet;
    public int CountCars;

    void Start()
    {
        CountCars = 0;
        BotSpawner = GameObject.FindGameObjectWithTag("BotSpawner").transform;
        isSet = false;
        StartRunning = WaitAndExecute();
        StartCoroutine(StartRunning);
    }
    void Update()
    {
    }
    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(0.1f);
        isSet = true;


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bot")
        {
            CountCars--;
        }
    }
    void OnTriggerEnter(Collider collider)
    { 
        if (collider.gameObject.tag =="Player" || collider.gameObject.tag == "Bot")
        {
            CountCars++;
        }
        if (collider.gameObject.tag == "Road")
        {
            // Debug.Log(this.gameObject.GetComponent<Renderer>().bounds.size.x);
            if (Vector3.Distance(this.transform.position,collider.transform.position) < this.gameObject.GetComponent<Renderer>().bounds.size.x / 3 && collider.GetComponent<RoadPieceScript>().isSet)
            {
                //Debug.Log(Vector3.Distance(this.transform.position, collider.transform.position));
                if (BotSpawner.GetComponent<BotSpawner>().SpawnposOption.Contains(collider.gameObject))
                {
                    BotSpawner.GetComponent<BotSpawner>().SpawnposOption.Remove(collider.gameObject);
                }
                Destroy(collider.gameObject);
            }
            else 
            if ((collider.transform.position - this.transform.position).normalized.z == 1.0f)
            {
                SortName("1"); // N
            }
            else if((collider.transform.position - this.transform.position).normalized.x == 1.0f)
            {
                SortName("2");  // E
            }
            else if ((collider.transform.position - this.transform.position).normalized.z == -1.0f)
            {
                SortName("3"); // S
            }
            else if ((collider.transform.position - this.transform.position).normalized.x == -1.0f)
            {
                SortName("4"); // W
            }

        }
    }

    void SortName(string s)
    {
        if (!Name.Contains(s))
        {
            Name += s;
            char[] sort = Name.ToCharArray();
            Array.Sort(sort);
            Name = null;
            foreach (char c in sort)
            {
                Name += c;
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (Name.Contains(this.transform.GetChild(i).name))
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
                switch (this.transform.GetChild(i).name)
                {
                    case "1":
                        this.transform.GetChild(7).gameObject.SetActive(false);
                        break;
                    case "2":
                        this.transform.GetChild(8).gameObject.SetActive(false);
                        break;
                    case "3":
                        this.transform.GetChild(6).gameObject.SetActive(false);
                        break;
                    case "4":
                        this.transform.GetChild(5).gameObject.SetActive(false);
                        break;
                }
            }
                if (Name == "13")
                {
                    if (BotSpawner != null && !BotSpawner.GetComponent<BotSpawner>().SpawnposOption.Contains(this.gameObject))
                    {
                        BotSpawner.GetComponent<BotSpawner>().SpawnposOption.Add(this.gameObject);
                    }
                    if (this.transform.GetChild(i).name == "W")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "E")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "U")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "D")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "Plane")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else
                        this.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (Name == "24")
                {
                    if (BotSpawner != null && !BotSpawner.GetComponent<BotSpawner>().SpawnposOption.Contains(this.gameObject))
                    {
                        BotSpawner.GetComponent<BotSpawner>().SpawnposOption.Add(this.gameObject);
                    }
                    if (this.transform.GetChild(i).name == "N")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "S")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "U")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "D")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else if (this.transform.GetChild(i).name == "Plane")
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    else
                        this.transform.GetChild(i).gameObject.SetActive(false);
                }
        }
    }  
}
