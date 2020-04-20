using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    public Transform MapSpawner;
    public Transform Grid;
    public Transform RoadPiece;
    public Camera Camera;

    public Transform minlane;
    public float mindist;
    public List<GameObject> laneObjects;

    Collider[] hitColliders;
    public List<Transform> lanes;
    public List<Transform> AllLanes;


    public string DirFrom;     //  From which direction did we come
    public Transform DirLane;     //  Lane in which our car is positioned
    public string DirWhere;    //  What directions can we go

    IEnumerator StartRunning;
    public bool isRunning;
    public bool isTurning;
    public LayerMask layerMask;
    public float checkRadius;
    public int checkCount;
    float pieceSize;


    public float hitRange;

    public Vector3 spawnPoint;
    public float plyrIdX;
    public float plyrIdY;


    void Awake()
    {
    }
    

    void Start()
    {
        Camera = Camera.main;
        laneObjects = new List<GameObject>();
        mindist = int.MaxValue;
        layerMask = 1 << LayerMask.NameToLayer("Bot");
        minlane = null;
        checkRadius = 2.0f;
        checkCount = 0;
        isRunning = false;
        Grid.transform.GetComponent<TestMovement>().MoveForward = false;
        pieceSize = RoadPiece.GetComponent<Renderer>().bounds.size.x;

        StartRunning = WaitAndExecute();
        StartCoroutine(StartRunning);
        lanes = new List<Transform>();
        AllLanes = new List<Transform>();
    }

    IEnumerator WaitAndExecute()
    {

        yield return new WaitForSeconds(0.1f); // нужена пауза для компилирования всех Start() в других скриптах
        //Grid.gameObject.GetComponent<TestMovement>().MoveForward = true;
        if (MapSpawner != null)
        {
            MapSpawner.GetComponent<CitySpawner>().InitialSpawn();

        }
        if (this.tag == "Player")
        {
            yield return new WaitForSeconds(2.5f);

        } else if (this.tag == "Bot")
        {
            yield return new WaitForSeconds(0.5f);
        }

        Transform[] children = Grid.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.tag == "Lane")
            {
                AllLanes.Add(child);

            }
        }
        Grid.transform.GetComponent<TestMovement>().MoveForward = true;
        isRunning = true;


    }
    
    void Update()
    {
        plyrIdX =(int) (transform.position.x / pieceSize - spawnPoint.x / pieceSize);
        plyrIdY =(int) (transform.position.z / pieceSize - spawnPoint.z / pieceSize);
       // Debug.Log("Player is currently in sector: " + plyrIdX + ", " + plyrIdY);

        if (Grid.GetComponent<TestMovement>().TurnL || Grid.GetComponent<TestMovement>().TurnR)
        {
            isTurning = true;

        }
        else if (isTurning == true)
        {
            isTurning = false;
        }
    }
    
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Lane" && this.transform!=null)
        {
            if (lanes.Count > 1)
            {
                mindist = int.MaxValue;
                minlane = null;
                foreach (Transform lane in lanes)
                {
                    if (this.transform != null)
                    {
                        if (transform != null && lane != null) //checking for fatal error
                        {
                            if (Vector3.Distance(lane.position, this.transform.position) < mindist)
                            {
                                mindist = Vector3.Distance(lane.position, this.transform.position);
                                minlane = lane;

                            }
                        }
                        
                    }
                }
                if (DirLane != minlane)
                {
                    //laneObjects.Clear();
                    //laneObjects = GameObject.FindGameObjectsWithTag("Lane");
                    /*
                    Transform[] children = Grid.GetComponentsInChildren<Transform>();
                    foreach (Transform child in children)
                    {
                        if (child.tag == "Lane")
                        {
                            Debug.Log("Hop!");
                            laneObjects.Add(child.gameObject);

                        }
                    }
                      */ 
                    //Debug.Log("We are on " + minlane.name + "lane, position:" + minlane.position);
                    foreach (Transform lane in AllLanes)
                    {
                        if (lane.transform.position != minlane.position && lane.GetComponent<LaneScript>().LineActive == true)
                        {
                            lane.GetComponent<LaneScript>().LineActive = false;
                        }

                        if ((lane.transform.position == minlane.position) && lane.GetComponent<LaneScript>().LineActive == false)
                        {
                            lane.GetComponent<LaneScript>().LineActive = true;
                        }

                        if (lane.transform.localPosition.y != minlane.localPosition.y && lane.GetComponent<LaneScript>().allActive == true)
                        {
                            lane.GetComponent<LaneScript>().allActive = false;
                            Debug.Log(lane.transform.localPosition.y);
                        }

                        if ((lane.transform.localPosition.y == minlane.localPosition.y) && lane.GetComponent<LaneScript>().allActive == false)
                        {
                            lane.GetComponent<LaneScript>().allActive = true;
                            Debug.Log(lane.transform.localPosition.y);

                        }
                    }
                    DirLane = minlane;

                }

                //foreach (GameObject lane in laneObjects)
                //{
                //    lane.GetComponent<LaneScript>().LineActive = false;
                //    if (lane.transform.position.y != minlane.position.y)
                //    {
                //        lane.GetComponent<LaneScript>().allActive = false;
                //    }
                //    else if ((lane.transform.position.y == minlane.position.y))
                //    {
                //        lane.GetComponent<LaneScript>().allActive = true;
                //    }
                //}
                //DirLane = minlane;
                //minlane.GetComponent<LaneScript>().LineActive = true;
                //Debug.Log(minlane.position);
            }
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Lane")
        {
           // collider.transform.GetComponent<LaneScript>().LineActive = false;
            lanes.Remove(collider.transform);
        }
    }

    
    /*
    {
        Debug.Log("Collision");

        if (this.tag == "Player" && collider.transform.tag == "Bot")
        {
            Destroy(collider.transform);
            Debug.Break();
        }
    }
    */
    void OnTriggerEnter(Collider collider)
    {
        /*
        if (collider.gameObject.tag == "Bot" && this.tag == "Player")
        {
            
            Destroy(collider.transform.parent);
           // }
        }*/
        if (transform.tag == "Player" && collider.transform.tag == "Bot")
        {
            Debug.Log("I just hit " + collider.transform.parent.name + " bot!");
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                Handheld.Vibrate();
            }
            Camera.transform.parent.GetComponent<CameraMovementScript>().shake = true;
            Destroy(collider.transform.GetComponent<PlayerHitDetection>().Grid.transform.gameObject);
            Destroy(collider.transform.parent.gameObject);
            Destroy(collider.transform.gameObject);

        }
    

        if (collider.gameObject.tag == "Lane")
        {
            lanes.Add(collider.transform);
        }
        
        if (collider.gameObject.tag == "Road" && isRunning) // this string is your newly created tag
        {
            Vector3 roadPos = new Vector3(this.transform.InverseTransformPoint(collider.transform.position).x, this.transform.InverseTransformPoint(collider.transform.position).y, this.transform.InverseTransformPoint(collider.transform.position).z);

            float E, N, S, W;                                           // Find where we come from   
            if (roadPos.x > 0)                                          // 
            {                                                           // 
                W = roadPos.x;                                          // 
                E = 0;                                                  // 
            }                                                           // 
            else                                                        // 
            {                                                           // 
                W = 0;                                                  // 
                E = -roadPos.x;                                         // 
            }                                                           // 
                                                                        // 
            if (roadPos.z > 0)                                          // 
            {                                                           // 
                S = roadPos.z;                                          // 
                N = 0;                                                  // 
            }                                                           //    
            else                                                        //  
            {                                                           //                                                 
                S = 0;                                                  //
                N = -roadPos.z;                                         //
            }                                                           //
                                                                        //
            float temp = Mathf.Max(Mathf.Max(E, N), Mathf.Max(S, W));   //
                                                                        //
            if (temp == E)                                              //
            {                                                           //
                DirFrom = nameof(E);                                    //
            }                                                           //
            else if (temp == N)                                         //
            {                                                           //
                DirFrom = nameof(N);                                    //
            }                                                           //
            else if (temp == S)                                         //
            {                                                           //
                DirFrom = nameof(S);                                    //
            }                                                           //
            else if (temp == W)                                         //
            {                                                           //
                DirFrom = nameof(W);                                    //
            }                                                           //

            DirWhere = collider.GetComponent<RoadPieceScript>().Name;                   // Find where can we go and sort it relative to where we came from
            DirWhere = DirWhere.Replace("1", "N");
            DirWhere = DirWhere.Replace("2", "E");
            DirWhere = DirWhere.Replace("3", "S");
            DirWhere = DirWhere.Replace("4", "W");
            string relative0 = "NESWNES";
            string relative1 = "";

            int i = 0;
            while (relative0[i] != DirFrom[0])
            {
                i++;
            }
            for (int j = i; j < i + 4; j++)
            {
                foreach (char c in DirWhere)
                {
                    if (relative0[j] == c)
                    {
                        relative1 += relative0[j];
                    }
                }
            }
            //    relative1 = relative1.Remove(DirWhere.Length);

            DirWhere = relative1;
            DirWhere = DirWhere.Replace(DirFrom, "");






            float xValue = 0;
            float zValue = 0;

            switch (DirFrom)
            {
                case "S":
                    xValue = 0;
                    zValue = (collider.transform.position.z - Grid.transform.position.z) - (pieceSize / 2);
                    break;
                case "N":
                    xValue = 0;
                    zValue = (pieceSize / 2) + (collider.transform.position.z - Grid.transform.position.z);
                    break;
                case "W":
                    xValue = (collider.transform.position.x - Grid.transform.position.x) - (pieceSize / 2);
                    zValue = 0;
                    break;
                case "E":
                    xValue = (pieceSize / 2) + (collider.transform.position.x - Grid.transform.position.x);
                    zValue = 0;
                    break;
            }

            // Debug.Log(DirWhere + " has " + DirWhere.Length + " digits");
            // Debug.Log("Our Lane is " + DirLane);
            switch (DirWhere.Length)
            {
                case 1:
                    Grid.GetComponent<TestMovement>().Turn(xValue, zValue, DirFrom, DirWhere[0]);
                    break;
                case 2:
                    switch (DirLane.name)
                    {
                        case "L":
                            Grid.GetComponent<TestMovement>().Turn(xValue, zValue, DirFrom, DirWhere[0]);
                            break;
                        case "CL":
                            Grid.GetComponent<TestMovement>().Turn(xValue, zValue, DirFrom, DirWhere[0]);
                            break;
                        case "CR":
                            Grid.GetComponent<TestMovement>().Turn(xValue, zValue, DirFrom, DirWhere[1]);
                            break;
                        case "R":
                            Grid.GetComponent<TestMovement>().Turn(xValue, zValue, DirFrom, DirWhere[1]);
                            break;
                    }
                    break;
                case 3:
                    switch (DirLane.name)
                    {
                        case "L":
                            Grid.GetComponent<TestMovement>().Turn(xValue, zValue, DirFrom, DirWhere[0]);
                            break;
                        case "CL": //
                            break;
                        case "CR": //
                            break;
                        case "R":
                            Grid.GetComponent<TestMovement>().Turn(xValue, zValue, DirFrom, DirWhere[2]);
                            break;
                    }
                    break;
            }
        }
    }
}
