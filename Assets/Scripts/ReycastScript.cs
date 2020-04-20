
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ReycastScript : MonoBehaviour
{
    public class Line
    {

        public bool lineActive;
        public float distForward;
        public float distMinusForward;
        public Vector3 linePosition;
        public float distForBot;
        public bool isBlocked;


        public Line(bool _lineAct, float _distFwd, float _distMinusFwd, Vector3 _linePst, float _distFB, bool _isBlck)
        {
            lineActive = _lineAct;
            distForward = _distFwd;
            distMinusForward = _distMinusFwd;
            linePosition = _linePst;
            distForBot = _distFB;
            isBlocked = _isBlck;
        }
    }

    public Line line1 = new Line(false, 0, 0, Vector3.zero, 0, false);
    public Line[] allLines;

    public Transform[] trLine;
    public Transform Bot;


    public int minNum;

    public float min;
    public float minBlink;
    public float hitDist;
    public float botBlink;

    public bool AllTrriger = true;
    int botMove = -1;


    void Start()
    {
        botBlink = 0;
        minNum = -1;
        minBlink = int.MaxValue;
        min = int.MaxValue;
        trLine = Array.FindAll(GetComponentsInChildren<Transform>(), child => child != this.transform && child.tag == "Lane"); // tag Line dla paskow
        int i = trLine.Length;
        allLines = new Line[i];
        for (i = 0; i < trLine.Length; i++)
        {
            allLines[i] = new Line(false, 0, 0, Vector3.zero, 0, false);
            allLines[i].linePosition = trLine[i].position;
            // Debug.Log(allLines[i].linePosition);
        }

    }
    void Update()
    {
        if (botMove != -1)
        {
            MoveBot(botMove);
        }
        min = int.MaxValue;
        // Debug.Log(minNum);
        for (int i = 0; i < trLine.Length; i++)
        {

            allLines[i].linePosition = trLine[i].position;
            allLines[i].distForBot = Vector3.Distance(allLines[i].linePosition, Bot.position);
            if (allLines[i].distForBot < min)
            {

                min = allLines[i].distForBot;
                if (minNum != -1) { allLines[minNum].lineActive = false; }

                minNum = i;
                allLines[minNum].lineActive = true;
            }
        }
        for (int a = 0; a < trLine.Length; a++)
        {
            botBlink = 0;
            if (minNum != -1)
            {
                allLines[a].isBlocked = false;
                RaycastHit hit;
                //RaycastHit hittwo;
                //int layerMask = 1 << LayerMask.NameToLayer("Bot");
                int layerMask = LayerMask.GetMask("Bot");
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                Vector3 fwdminus = transform.TransformDirection(-Vector3.forward);



                if (Physics.Raycast(allLines[minNum].linePosition, fwd, out hit, 10f, layerMask))
                {
                    Debug.DrawRay(allLines[minNum].linePosition, fwd * hit.distance, Color.yellow);
                    //Debug.Break();
                    allLines[minNum].isBlocked = true;
                    for (int i = 0; i < trLine.Length; i++)
                    {
                        if (i != minNum)
                        {
                            //  Debug.Log("cout minNum: " + i);
                            if (Physics.Raycast(allLines[i].linePosition, transform.TransformDirection(Vector3.forward), out hit, 10f, layerMask))
                            {
                                Debug.DrawRay(allLines[i].linePosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                                allLines[i].isBlocked = true;

                            }
                        }
                    }
                    hitDist = hit.distance;
                    minBlink = int.MaxValue;
                    for (a = 0; a < trLine.Length; a++)
                    {
                        if (allLines[a].isBlocked == true)
                        {

                            // Debug.Log("cout falseIsBl: " + a);
                            for (a = 0; a < trLine.Length; a++)
                            {

                                allLines[a].distForBot = Vector3.Distance(allLines[a].linePosition, Bot.position);
                                if (allLines[a].distForBot < minBlink && (allLines[a].isBlocked == false ))
                                {
                                    Debug.Log("Found botMove A: " + a);

                                    minBlink = allLines[a].distForBot;
                                    botMove = a;
                                }
                            }
                        }
                    }
                    //  Bot.transform.position += Vector3.left * Time.deltaTime * botBlink;
                }

                else
                {
                    allLines[a].isBlocked = false;
                    Physics.Raycast(allLines[minNum].linePosition, fwd, 10f);
                    Debug.DrawRay(allLines[minNum].linePosition, fwd * 10, Color.red);
                }



            }
            /*
            Debug.Log(allLines[0].isBlocked);
            Debug.Log(allLines[1].isBlocked);
            Debug.Log(allLines[2].isBlocked);
            Debug.Log(allLines[3].isBlocked);
            Debug.Log(allLines[4].isBlocked);
            Debug.Log(allLines[5].isBlocked);
            Debug.Log(allLines[6].isBlocked);
            Debug.Log(allLines[7].isBlocked);
            Debug.Log(allLines[8].isBlocked);
            Debug.Log(allLines[9].isBlocked);
            Debug.Log(allLines[10].isBlocked);
            Debug.Log(allLines[11].isBlocked);*/


        }


    }
    void MoveBot(int a)
    {
        //Debug.Log("Move Bot executed!");
        //Bot.transform.position += new Vector3((allLines[a].linePosition.x) - (Bot.position.x), (allLines[a].linePosition.y) - (Bot.position.y), (allLines[a].linePosition.z) - (Bot.position.z)) * Time.deltaTime;
        Bot.transform.position = allLines[a].linePosition;


    }
}




