using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public Transform Player;
    public Transform Grid;
    public Camera Camera;
    public float step;
    public Vector3 target;
    float RotationSpeed = 150.0f;
    public bool shake;
    public float timer;
    public float shakeVal;
    public Vector3 saveRotation;
    void Start()
    {
        shake = false;
        shakeVal = 0.7f;
        timer = 0;
        step = RotationSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        
        if (Grid.GetComponent<TestMovement>().SpeedUp && step != RotationSpeed * 2.5f * Time.deltaTime )
        {
            step = RotationSpeed * 2.5f * Time.deltaTime;
        }
        else if (Grid.GetComponent<TestMovement>().SpeedDown & step != RotationSpeed * 0.5f * Time.deltaTime)
        {
            step = RotationSpeed * 0.5f * Time.deltaTime;
        }
        else 
        {
            step = RotationSpeed * Time.deltaTime;
        }

        if (!shake)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, Grid.GetChild(0).eulerAngles.y, this.transform.eulerAngles.z);
            saveRotation = this.transform.eulerAngles;
        }
        else if (shake && timer<1.5f)
        {
            timer += Time.deltaTime;
            this.transform.eulerAngles += Random.insideUnitSphere;

        }
        else if (timer > 1.5f)
        {
            shake = false;
            this.transform.eulerAngles = saveRotation;
        }
        target = new Vector3(Grid.position.x, Grid.Find("XYmovement").position.y, Grid.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);
            
    }


}
