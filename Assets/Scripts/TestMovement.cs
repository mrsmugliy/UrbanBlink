using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMovement : MonoBehaviour
{
    public GameObject EmptyPoint;
    public GameObject Lane;
    Transform ParentPoint;


    public Transform Player;
    public Transform Player3DModel;
    public Transform UpRight;
    public Transform UpLeft;
    public Transform DownRight;
    public Transform DownLeft;
    public Transform TurnRight;
    public Transform TurnLeft;
    public Transform XYmovement;
    public Transform CanvasTransform;
    public Transform tempParent;

    public bool SpeedUp;
    public bool SpeedDown;

    public float xValue;
    public float yValue;
    public bool horizontalEdge;
    public bool verticalEdge;
      float Speed;
    float RotationSpeed;
    float turnSpeed;
    float moveSpeed;

    float laneSize;

    float timeCount;
    public Vector3 playerVector;
    public bool MoveForward;
    public bool TurnR;
    public bool TurnL;
    public bool afterTurn;
    public float afterTurnTimer;


    void Start()
    {
        UpRight = transform.GetChild(0);
        UpLeft = transform.GetChild(1);
        DownRight = transform.GetChild(2);
        DownLeft = transform.GetChild(3);
        TurnRight = transform.GetChild(4);
        TurnLeft = transform.GetChild(5);
        XYmovement = transform.GetChild(6);
        horizontalEdge = false;
        verticalEdge = false;
        afterTurn = false;
        afterTurnTimer = 0;
        xValue = 0;
        yValue = 0;
          Speed = 0.5f;
        RotationSpeed = 10;
        turnSpeed = 2.0f;
        moveSpeed = 25.0f;
        MoveForward = false;
        laneSize = Lane.GetComponent<Renderer>().bounds.size.x;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject instLane = Instantiate(Lane, new Vector3(this.transform.position.x + (-1.5f * laneSize + i * laneSize), this.transform.position.y + (-laneSize + j * laneSize), this.transform.position.z), Quaternion.identity);
                instLane.transform.parent = this.transform;
                switch (i)
                {
                    case 0:
                        instLane.name = "L";
                        break;
                    case 1:
                        instLane.name = "CL";
                        break;
                    case 2:
                        instLane.name = "CR";
                        break;
                    case 3:
                        instLane.name = "R";
                        break;
                }
            }
        }

    }

    void Update()
    {
       /* if (Player == null)
        {
            Destroy(this);
        }*/
        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKeyDown(KeyCode.LeftShift))
        {
            SpeedUp = true;
        }
        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKeyUp(KeyCode.LeftShift))

        {
            SpeedUp = false;
        }
        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKeyDown(KeyCode.LeftControl))
        {
            SpeedDown = true;
        }
        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKeyUp(KeyCode.LeftControl))

        {
            SpeedDown = false;
        }


        if (SpeedUp && Player.tag == "Player")
        {
            if (moveSpeed < 40.0f)
            {
                moveSpeed += Time.deltaTime * 10;
            }
        }
        else if (SpeedDown && Player.tag == "Player")
        {
            if (moveSpeed > 15.0f)
            {
                moveSpeed -= Time.deltaTime * 10;
            }
        }
        else
        {
            if (moveSpeed < 24.0f && Player.tag == "Player")
            {
                moveSpeed += Time.deltaTime * 10;
            }
            if (moveSpeed > 25.0f && Player.tag == "Player")
            {
                moveSpeed -= Time.deltaTime * 10;
            }
        }


        if (afterTurn)
        {
            afterTurnTimer += Time.deltaTime;
            if (afterTurnTimer > 0.1f)
            {
                //здесь добавить РейкастОлл для подбора индикаторов 
                afterTurn = false;
                
            }
        }
        else
        {
            afterTurnTimer = 0;
        }

        if (this.transform.parent == null && !afterTurn)
        {
           // Debug.Log("Before " + Player3DModel.eulerAngles);
            float temp = Player3DModel.eulerAngles.y;
            Player3DModel.LookAt(XYmovement);
            switch (Player.GetComponent<PlayerHitDetection>().DirFrom)
            {
                case "N":
                   // Player3DModel.eulerAngles = new Vector3(Player3DModel.eulerAngles.x, Player3DModel.eulerAngles.y, Player3DModel.eulerAngles.y - 180); // working
                    Player3DModel.eulerAngles = new Vector3(Player3DModel.eulerAngles.x, temp, Player3DModel.eulerAngles.y - 180);
                   // Debug.Log(temp);
                    break;
                case "S":
                    Player3DModel.eulerAngles = new Vector3(Player3DModel.eulerAngles.x, temp, 0 - Player3DModel.eulerAngles.y); 
                    break;
                case "E":
                    Player3DModel.eulerAngles = new Vector3(Player3DModel.eulerAngles.x, temp, -Player3DModel.eulerAngles.y - 90);
                    break;
                case "W":
                    Player3DModel.eulerAngles = new Vector3(Player3DModel.eulerAngles.x, temp, -Player3DModel.eulerAngles.y + 90);
                    break;
            }
           // Debug.Log("After " + Player3DModel.eulerAngles);
            //Debug.Log("NotTurning");
            //Debug.Log(Player.GetComponent<PlayerHitDetection>().DirWhere);
        }
        else //if (TurnR || TurnL)
        {
            //Player3DModel.LookAt(XYmovement);
            Player3DModel.eulerAngles = XYmovement.eulerAngles;
        }

        if (SystemInfo.deviceType == DeviceType.Desktop && Player.tag == "Player")
        {
            //Debug.Log("Desktop detected!");
            xValue = Speed * Input.GetAxis("Horizontal"); // from -0.5 to 0.5
            yValue = Speed * Input.GetAxis("Vertical"); // from -0.5 to 0.5
           // Debug.Log("You are playing on a PC");
        }
        else if (CanvasTransform != null && CanvasTransform.GetComponent<TouchManager>().useStick == true && Time.timeScale != 0 && SystemInfo.deviceType != DeviceType.Desktop && Player.tag == "Player")
        {
            //Debug.Log("You are playing on a phone");
            xValue = xValue * 0.015f;
            yValue = yValue * 0.010f;
        }
            

        //float Horizontal = CanvasTransform.GetComponent<TouchManager>().Horizontal;
        //float Vertical = CanvasTransform.GetComponent<TouchManager>().Vertical;


        //if (CanvasTransform != null && CanvasTransform.GetComponent<TouchManager>().useStick == true && Time.deltaTime != 0 && SystemInfo.deviceType != DeviceType.Desktop)
        //{

        //    xValue = xValue * 0.015f;
        //    yValue = yValue * 0.010f;
        //}

        /*if (Time.timeScale == 0)
        {
            xValue = 0;
            yValue = 0;
            if (Player != null && XYmovement != null && Player.GetComponent<PlayerHitDetection>().DirLane != null)
            {
                XYmovement.transform.position = Player.GetComponent<PlayerHitDetection>().DirLane.transform.position;
            }
            // XYmovement.transform.Translate(Player.GetComponent<PlayerHitDetection>().DirLane.transform.position * Time.deltaTime, Space.World);
        }
        */
        if ((xValue == 0 && yValue == 0) || CanvasTransform.GetComponent<TouchManager>().useStick == false)
        {
            if (Player != null && XYmovement != null && Player.GetComponent<PlayerHitDetection>().minlane != null) // Player.GetComponent<PlayerHitDetection>().DirLane != null &&
            {
            //    XYmovement.transform.position = Player.GetComponent<PlayerHitDetection>().minlane.position;
            }
            // XYmovement.transform.Translate(Player.GetComponent<PlayerHitDetection>().DirLane.transform.position * Time.deltaTime, Space.World);
        }
        


        //Debug.Log(Input.GetAxis("Horizontal"));
        if (Player.GetComponent<BoxCollider>() == null) // <---- самоуничтожение при фатальной ошибке
        {
            //Destroy(Player);
            //Destroy(this);
        }
        else 
        if (xValue + XYmovement.localPosition.x > UpRight.localPosition.x - Player.GetComponent<BoxCollider>().size.x * 0.75f || XYmovement.localPosition.x + xValue < UpLeft.localPosition.x + Player.GetComponent<BoxCollider>().size.x * 0.75f)
        {
            horizontalEdge = true;
        }
        else
        {
            horizontalEdge = false;
        }
        if (Player.GetComponent<BoxCollider>() == null) // <---- самоуничтожение при фатальной ошибке
        {
            //Destroy(Player);
           //Destroy(this);
        }
        else
        if (yValue + XYmovement.localPosition.y > UpRight.localPosition.y - Player.GetComponent<BoxCollider>().size.y * 0.75f || XYmovement.localPosition.y + yValue < DownRight.localPosition.y + Player.GetComponent<BoxCollider>().size.y * 0.75f)
        {
            verticalEdge = true;
        }
        else
        {
            verticalEdge = false;
        }
        if (!horizontalEdge)
        {
            //XYmovement.Translate(xValue * Time.deltaTime, 0, 0, Space.Self);
            XYmovement.localPosition += new Vector3(xValue, 0, 0);


        }
        if (!verticalEdge)
        {
            // XYmovement.Translate(0, yValue * Time.deltaTime, 0,   Space.Self);
            XYmovement.localPosition += new Vector3(0, yValue, 0);

        }

        TurnRight.Translate(0, yValue, 0, Space.Self);
        TurnLeft.Translate(0, yValue, 0, Space.Self);

        playerVector = XYmovement.position - Player.position;

        if (Player.GetComponent<PlayerHitDetection>().isRunning)
        {
            Player.Translate(playerVector * Time.deltaTime * RotationSpeed);

        }

        if (TurnR)
        {
            ParentPoint.rotation = Quaternion.Slerp(ParentPoint.rotation, Quaternion.Euler(0, Mathf.Round(ParentPoint.rotation.y + 90), 0), timeCount);

            timeCount += Time.deltaTime * turnSpeed;
        }

        if (TurnR && Quaternion.Angle(transform.parent.rotation, Quaternion.Euler(0, ParentPoint.rotation.y + 90, 0)) < 0.4f)
        {

            transform.parent = tempParent;
            Destroy(ParentPoint.gameObject); ;
            transform.Rotate(0, -transform.eulerAngles.y % 90, 0, Space.Self);
            TurnR = false;
            MoveForward = true;
            afterTurn = true;
        }

        if (TurnL)
        {
            ParentPoint.rotation = Quaternion.Slerp(ParentPoint.rotation, Quaternion.Euler(0, Mathf.Round(ParentPoint.rotation.y - 90), 0), timeCount);
            timeCount += Time.deltaTime * turnSpeed;
        }

        if (TurnL && Quaternion.Angle(transform.parent.rotation, Quaternion.Euler(0, ParentPoint.rotation.y - 90, 0)) < 0.4f)
        {
            this.transform.parent = tempParent;
            Destroy(ParentPoint.gameObject);
            transform.Rotate(0, 90 - transform.eulerAngles.y % 90, 0, Space.Self);
            TurnL = false;
            MoveForward = true;
            afterTurn = true;
        }

        if (MoveForward)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }

    public void Turn(float xValue, float zValue, string DirFrom, char DirWhere)
    {
        string where = "";
        where += DirWhere;
        switch (DirFrom)
        {
            case "S":
                switch (where)
                {
                    case "W":
                        where = "left";
                        break;
                    case "E":
                        where = "right";
                        break;
                }
                break;

            case "W":
                switch (where)
                {
                    case "S":
                        where = "right";
                        break;
                    case "N":
                        where = "left";
                        break;
                }
                break;

            case "N":
                switch (where)
                {
                    case "W":
                        where = "right";
                        break;
                    case "E":
                        where = "left";
                        break;
                }
                break;

            case "E":
                switch (where)
                {
                    case "S":
                        where = "left";
                        break;
                    case "N":
                        where = "right";
                        break;
                }
                break;
        }
        switch (where)
        {
            case "right":
                TurnR = true;
                MoveForward = false;
                this.transform.Translate(xValue, 0, zValue, Space.World);
                ParentPoint = Instantiate(EmptyPoint, TurnRight.position, Quaternion.identity).transform;
                tempParent = this.transform.parent;
                ParentPoint.parent = tempParent;
                this.transform.parent = ParentPoint;
                timeCount = 0;
                break;

            case "left":
                TurnL = true;
                MoveForward = false;
                this.transform.Translate(xValue, 0, zValue, Space.World);
                ParentPoint = Instantiate(EmptyPoint, TurnLeft.position, Quaternion.identity).transform;
                tempParent = this.transform.parent;
                ParentPoint.parent = tempParent;
                this.transform.parent = ParentPoint;
                timeCount = 0;
                break;
        }
    }
    /*
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        if (this.tag == "Player" && collision.transform.tag == "Bot")
        {
            Destroy(collision.transform);
            Debug.Break();
        }


    }*/
}