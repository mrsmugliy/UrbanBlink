using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public GameObject WallModel;
    public Transform BotSpawner;
    public Transform Player;
    public Camera Camera;
    public int Neighbours;
    public int LevelValue;
    public int IndexW;
    public int IndexH;
    public IEnumerator ActivateBuildings;
    public IEnumerator CheckRadius;
    public bool radiusCheck;
    public Transform minNeighbour;
    public int minLevel;
    public float checkTimer;
    public float checkStep;
    public float minStep;
    public float normStep;
    public float checkRadius;
    public float minRadius;
    public Vector3 BuildingVector;
    public Vector3 PlayerVector;
    public float angleBetween;
    public float anglePlayer;
    public float checkAngle;


    void Start()
    {

        checkTimer = 0;
        normStep = 0.5f; // Step can me modifyed to be small only on turns
        checkStep = normStep; 
        minStep = 0.01f;
        checkRadius = 250.0f;
        minRadius = 75.0f;
        radiusCheck = true;
    
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Camera = Camera.main;
        checkAngle = Camera.fieldOfView;

        Neighbours = 0;
        minLevel = 20;
        LevelValue = (int)(Mathf.PerlinNoise(IndexW / 3.0f, IndexH / 3.0f) * 10);

        //Vector3 pos = new Vector3(IndexW * pieceXsize, 0, IndexH * pieceZsize);
        // if (result < resultCompare)
        //Instantiate(road, pos, Quaternion.identity);
        ActivateBuildings = WaitAndExecute();
       // CheckRadius = WaitAndCheck();
        StartCoroutine(ActivateBuildings);

        
        
        // this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    void Update()
    {
        if (Player.GetComponent<PlayerHitDetection>().isTurning && minStep < checkStep)
        {
            checkStep = minStep;
        }
        else if (!Player.GetComponent<PlayerHitDetection>().isTurning && minStep == checkStep)
        {
            checkStep = normStep;
        }
        checkTimer += Time.deltaTime;

        if (checkTimer > checkStep)
        {
            checkTimer = 0;
            //тут сравнивать векторы
            if (Vector3.Distance(this.transform.position, Player.position) < checkRadius)
            {
                BuildingVector = Player.position - this.transform.position;
                //PlayerVector = Player.position;
                angleBetween = Vector3.Angle(BuildingVector, Player.GetChild(0).forward);
                //anglePlayer = Player.transform.GetChild(0).eulerAngles.y;
                //if (anglePlayer > 180) anglePlayer -= 360; 
            }

            if (Vector3.Distance(this.transform.position, Player.position) < minRadius && this.transform.GetComponent<MeshRenderer>().enabled == false)
            {
                if (angleBetween > 60)
                {
                    this.transform.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            if (Vector3.Distance(this.transform.position, Player.position) < minRadius && this.transform.GetComponent<MeshRenderer>().enabled == true)
            {
                if (angleBetween < 60)
                {
                    this.transform.GetComponent<MeshRenderer>().enabled = false;
                }
            }

            if (Vector3.Distance(this.transform.position, Player.position) < checkRadius && Vector3.Distance(this.transform.position, Player.position) > minRadius && this.transform.GetComponent<MeshRenderer>().enabled == false)
            {
                if (angleBetween > 180 - checkAngle)
                {
                    this.transform.GetComponent<MeshRenderer>().enabled = true;
                }
            }

            if (Vector3.Distance(this.transform.position, Player.position) < checkRadius && Vector3.Distance(this.transform.position, Player.position) > minRadius && this.transform.GetComponent<MeshRenderer>().enabled == true)
            {
                if (angleBetween < 180 - checkAngle)
                {
                    this.transform.GetComponent<MeshRenderer>().enabled = false;
                }
            }


            if (Vector3.Distance(this.transform.position, Player.position) > checkRadius && this.transform.GetComponent<MeshRenderer>().enabled == true)
            {
                this.transform.GetComponent<MeshRenderer>().enabled = false;
            }
            //StartCoroutine(CheckRadius);
            /*

            if (Physics.CheckSphere(this.transform.position, 150f, layerMask))
            {
                if (this.transform.GetComponent<MeshRenderer>().enabled == false)
                {
                    this.transform.GetComponent<MeshRenderer>().enabled = true;

                }
            }
            if (!Physics.CheckSphere(this.transform.position, 150f, layerMask))
            {
                if (this.transform.GetComponent<MeshRenderer>().enabled == true)
                {
                    this.transform.GetComponent<MeshRenderer>().enabled = false;

                }
            }
            */
        }
      
    }

    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(checkStep * 2);
        Vector3 spawnPos = this.transform.position;

        if (Neighbours < 4)
        {
            this.transform.tag = "WallE";
            checkRadius = checkRadius * 2;
           // checkStep = checkStep * 4;
            checkAngle = checkAngle / 3;
        }
        
        this.transform.GetComponent<MeshRenderer>().enabled = true; // <----- change to true
        //  this.transform.GetComponent<MeshRenderer>().bounds.size.y += LevelValue * 12;
        this.transform.localScale += new Vector3(0, LevelValue * 8, 0);
        this.transform.position += new Vector3(0, (LevelValue * 8) / 2, 0);

        
    }
    /*
    IEnumerator WaitAndCheck()
    {
       // Debug.Log("Radius Check");
        //radiusCheck = false;
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Player");
        //LayerMask layerMask = LayerMask.GetMask("Player");
        // int layerMask = 10;

        if (Physics.CheckSphere(this.transform.position, 150f, layerMask))
        {
            this.transform.GetComponent<MeshRenderer>().enabled = true;
        }
        if (!Physics.CheckSphere(this.transform.position, 150f, layerMask))
        {
            this.transform.GetComponent<MeshRenderer>().enabled = false;
        }
        //radiusCheck = true;
        yield return new WaitForSeconds(0.1f);
        

    }
    */



    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Bot"){
            //BotSpawner.GetComponent<BotSpawner>().BotList.Remove(collider.transform.Find("Grid").gameObject;);
            Destroy(collider.transform.GetComponent<PlayerHitDetection>().Grid.gameObject);
            Destroy(collider.transform.gameObject);
        }
        if (collider.gameObject.tag == "Wall") 
        {
            if ((collider.transform.position - this.transform.position).normalized.z == 1.0f)
            {
                Neighbours++; // N
                minNeighbour = collider.transform;
            }
            else if ((collider.transform.position - this.transform.position).normalized.x == 1.0f)
            {
                Neighbours++;  // E
            }
            else if ((collider.transform.position - this.transform.position).normalized.z == -1.0f)
            {
                Neighbours++; // S
            }
            else if ((collider.transform.position - this.transform.position).normalized.x == -1.0f)
            {
                Neighbours++; // W
            }
            if ((collider.transform.position - this.transform.position).normalized.z == 1.0f)
            {
                if (collider.GetComponent<BuildingScript>().LevelValue < minLevel)
                {
                    minLevel = collider.GetComponent<BuildingScript>().LevelValue;
                    minNeighbour = collider.transform;
                } // N

            }
            else if ((collider.transform.position - this.transform.position).normalized.x == 1.0f)
            {
                if (collider.GetComponent<BuildingScript>().LevelValue < minLevel)
                {
                    minLevel = collider.GetComponent<BuildingScript>().LevelValue;
                    minNeighbour = collider.transform;
                }// E
            }
            else if ((collider.transform.position - this.transform.position).normalized.z == -1.0f)
            {
                if (collider.GetComponent<BuildingScript>().LevelValue < minLevel)
                {
                    minLevel = collider.GetComponent<BuildingScript>().LevelValue;
                    minNeighbour = collider.transform;
                }// S
            }
            else if ((collider.transform.position - this.transform.position).normalized.x == -1.0f)
            {
                if (collider.GetComponent<BuildingScript>().LevelValue < minLevel)
                {
                    minLevel = collider.GetComponent<BuildingScript>().LevelValue;
                    minNeighbour = collider.transform;
                } // W
            }

        }
    }

}
