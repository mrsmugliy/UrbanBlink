using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    public List<Bot> BotList;
    public List<GameObject> SpawnposOption = new List<GameObject>();
    public GameObject modelPrefab;
    public GameObject gridPrefab;
    public GameObject emptyPoint;
    public Transform Player;
    public bool spawnPause;
    bool nextSpawn;
    IEnumerator StartRunning;
    int count;


    public class Bot
    {
        public GameObject modelBotu;
        public GameObject gridBotu;

        public Bot(GameObject mdlStatku, GameObject mdlGridu)
        {
            modelBotu = mdlStatku;
            gridBotu = mdlGridu;
        }
    }
    void Start()
    {
        nextSpawn = true;
        spawnPause = false;
        BotList = new List<Bot>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextSpawn)
        {
            StartRunning = WaitAndExecute();
            StartCoroutine(StartRunning);

        }
        if (count != GameObject.FindGameObjectsWithTag("Bot").Length)
        {
            //Debug.Log(GameObject.FindGameObjectsWithTag("Bot").Length);
            count = GameObject.FindGameObjectsWithTag("Bot").Length;
        }
    }
    IEnumerator WaitAndExecute()
    {
        nextSpawn = false;

        yield return new WaitForSeconds(0.5f); // нужена пауза для компилирования всех Start() в других скриптах
        int rand = 0;
        if (BotList.Count < 50)
            do
            {
                rand = Random.Range(0, SpawnposOption.Count);
                if (SpawnposOption[rand] != null && SpawnposOption[rand].GetComponent<RoadPieceScript>().CountCars <= 0 && Vector3.Distance(SpawnposOption[rand].transform.position, Player.position) > 36 )
                {
                    //Debug.Log(SpawnposOption[rand].transform.position);
                    // BotList.Add(new Bot(Instantiate(modelPrefab, SpawnposOption[rand].transform.position, Quaternion.identity), Instantiate(modelPrefab, SpawnposOption[rand].transform.position, Quaternion.identity)));
                    if (SpawnposOption[rand].GetComponent<RoadPieceScript>().Name == "13")
                    {
                        
                        GameObject model = Instantiate(modelPrefab, SpawnposOption[rand].transform.position, Quaternion.identity); // добавить случайную линию появления бота
                        // model.transform.rotation =
                        model.transform.eulerAngles = new Vector3(0, 0, 0);
                        model.transform.position += new Vector3(1.5f, 0, 0);

                        //model.GetComponent<PlayerHitDetection>().enabled = false;
                        GameObject grid = Instantiate(gridPrefab, SpawnposOption[rand].transform.position + new Vector3(0, 0, 4), Quaternion.identity);
                        grid.transform.eulerAngles = new Vector3(0, 0, 0);

                        grid.GetComponent<ReycastScript>().enabled = true;
                        grid.GetComponent<ReycastScript>().enabled = true;
                        grid.GetComponent<ReycastScript>().Bot = model.transform;

                        model.GetComponent<PlayerHitDetection>().Grid = grid.transform;
                        grid.GetComponent<TestMovement>().Player = model.transform;
                        grid.GetComponent<TestMovement>().Player3DModel = model.transform.GetChild(0);


                        GameObject botParent= Instantiate(emptyPoint, SpawnposOption[rand].transform.position, Quaternion.identity);
                        botParent.name = "BotNum" + BotList.Count;
                        grid.transform.parent = botParent.transform;
                        model.transform.parent = botParent.transform;

                        if (BotList.Count < 100)
                            BotList.Add(new Bot(model, grid));
                    }

                }
                else rand = 0;
                //if(rand!=0)
                //Debug.Break();
            } while (rand == 0);
        yield return new WaitForSeconds(0.25f);
        nextSpawn = true;
    }
}
