using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;           // When we add Diagnostics library there is a problem. Debug is used in both Diagnostics and UnityEngine...
using Debug = UnityEngine.Debug;    //...that's why we need this line. To clarify wich library runs Debug

public class CitySpawner : MonoBehaviour
{
    public GameObject RoadPiece;
    public GameObject WallPiece;
    public GameObject Grid;
    Transform Map;
    public Canvas UserInterface;
    public GameObject WorldMapParent;
    public GameObject EdgeMapParent;
    public GameObject emptyPoint;
    public GameObject[,] city;
    public Vector3 spawnPoint;
    Stopwatch sw = new Stopwatch(); // Use it to Debug time
    int[,] testMap;

    int delete;
    int randMax;
    public int mapSize = 25;
    public int mapCount = 4;
    public int ranMax = 20;
    public int ranMin = 5;
    public float pieceSizeX;
    public float pieceSizeZ;

    public int[,] worldMapl;



    void Start()
    {
        WorldMapParent.transform.position = Grid.transform.position;
        pieceSizeX = RoadPiece.GetComponent<Renderer>().bounds.size.x;
        pieceSizeZ = RoadPiece.GetComponent<Renderer>().bounds.size.z;
        spawnPoint = Grid.transform.position - new Vector3(((mapCount / 2 * mapSize * pieceSizeX) + (pieceSizeX / 2) - (mapCount / 2 * pieceSizeX)), 0, (mapCount / 2 * mapSize * pieceSizeX) + (pieceSizeX / 2) - (mapCount * pieceSizeX)); //obowiązkowo kratne 2
        mapSize = 25;
        mapCount = 4;

        testMap = new int[mapSize, mapSize];
        worldMapl = new int[mapSize * mapCount, mapSize * mapCount];
        city = new GameObject[mapCount, mapCount];
        for (int i = 0; i < mapCount; i++)
        {
            for (int j = 0; j < mapCount; j++)
            {
                city[i, j] = WorldMapParent.transform.GetChild(i * mapCount + j).gameObject;
                if (city[i, j].transform.childCount != 0)
                {
                    //city[i, j].transform.GetChild(0).gameObject.SetActive(false); // выключаем предметы для просчёта culling
                }
                
            }
        }

         SpawnMapEdges(spawnPoint); // <<----------------------------



    }
    
    public void SpawnMapEdges(Vector3 edgePos) // Шиучер создаем край карты
    {
        Debug.Log(edgePos);
        edgePos += new Vector3(pieceSizeX / 2, 0, 0);
        EdgeMapParent = Instantiate(emptyPoint, Grid.transform.position, Quaternion.identity);
        EdgeMapParent.name = "EdgeMapParent";
        for (int i = 0; i < mapSize * mapCount; i++)
        {
            //Debug.Log(i % (mapSize));
            for (int j = 0; j < mapSize * mapCount; j++)
            {

                
                if ( i==0 && j < mapSize * mapCount - 3)
                {
                    GameObject instLane = Instantiate(RoadPiece, edgePos, Quaternion.identity);
                    instLane.transform.parent = EdgeMapParent.transform;
                }
                
                if ( j==0 && i < mapSize * mapCount - 3)
                {
                    GameObject instLane = Instantiate(RoadPiece, edgePos, Quaternion.identity);
                    instLane.transform.parent = EdgeMapParent.transform;
                }
                
                if (i == mapSize * mapCount - 4 && j < mapSize * mapCount - 3)
                {
                    GameObject instLane = Instantiate(RoadPiece, edgePos, Quaternion.identity);
                    instLane.transform.parent = EdgeMapParent.transform;
                }
                if (j == mapSize * mapCount - 4 && i < mapSize * mapCount - 3) 
                {
                    GameObject instLane = Instantiate(RoadPiece, edgePos, Quaternion.identity);
                    instLane.transform.parent = EdgeMapParent.transform;
                }
                /*
                if (i % (mapSize) == 0 && i != 0 && j==0)
                {
                    edgePos += new Vector3(0, 0, -pieceSizeX);

                }
                if (j % (mapSize) == 0 && j != 0 && i == 0)
                {
                    edgePos += new Vector3(-pieceSizeX, 0, 0);

                }*/

                edgePos += new Vector3(pieceSizeX, 0, 0);



            }
            edgePos += new Vector3(-mapSize * mapCount * pieceSizeX, 0, pieceSizeZ);
        }
        //testMap[0, 0] = 1;
        // testMap[mapSize - 1, mapSize - 1] = 1;
        //testMap[mapSize / 2, mapSize - 1] = 0;
        //testMap[mapSize - 1, mapSize / 2] = 0;
    }
    

    void Update()
    {
        /*
        if (Input.GetKeyDown("space"))
        {
            sw.Start();
            NullWorld();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    GenerateMap();
                    SpawnMap(spawnPoint, i, j);
                    SavePiece(i, j);
                    NullMap();
                    spawnPoint += new Vector3((mapSize - 1) * RoadPiece.GetComponent<Renderer>().bounds.size.x, 0, 0);
                }
                spawnPoint += new Vector3(-4 * (mapSize - 1) * RoadPiece.GetComponent<Renderer>().bounds.size.x, 0, (mapSize - 1) * RoadPiece.GetComponent<Renderer>().bounds.size.z);
            }
           // SpawnWorldMap();
            sw.Stop();
            // Debug.ClearDeveloperConsole();
            Debug.Log("Generating this map took only" + sw.Elapsed.TotalSeconds + " seconds. PogChamp!");
            sw.Reset();
            spawnPoint = new Vector3(0, 0, 0);
            // spawnPoint += new Vector3(mapSize * RoadPiece.GetComponent<Renderer>().bounds.size.x, 0, 0);

        }
        */

    }

    public void InitialSpawn()
    {
        sw.Start();
        NullWorld();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                GenerateMap();
                SpawnMap(spawnPoint, i, j);
                SavePiece(i, j);
                NullMap();
                spawnPoint += new Vector3((mapSize - 1) * RoadPiece.GetComponent<Renderer>().bounds.size.x, 0, 0);
            }
            spawnPoint += new Vector3(-4 * (mapSize - 1) * RoadPiece.GetComponent<Renderer>().bounds.size.x, 0, (mapSize - 1) * RoadPiece.GetComponent<Renderer>().bounds.size.z);
        }
        //SpawnWorldMap();
        UpdateWorldMap();
        // Player.gameObject.GetComponent<GeneracjaZamowienia>().UpdateNavigationMap();
        sw.Stop();
        // Debug.ClearDeveloperConsole();
        Debug.Log("Generating this map took only" + sw.Elapsed.TotalSeconds + " seconds. PogChamp!");
        sw.Reset();
        spawnPoint = new Vector3(0, 0, 0);
        // spawnPoint += new Vector3(mapSize * RoadPiece.GetComponent<Renderer>().bounds.size.x, 0, 0);

    }

    void SavePiece(int indexI, int indexJ)
    {
        //    Debug.Log(indexI + " " + indexJ);
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (worldMapl[i + (indexJ * mapSize) - indexJ, j + (indexI * mapSize) - indexI] != 1)
                {
                    worldMapl[i + (indexJ * mapSize) - indexJ, j + (indexI * mapSize) - indexI] = testMap[j, i];
                }

            }
        }

    }

    void UpdateWorldMap()
    {
        UserInterface.GetComponent<UserInterfaceScript>().UpdateFullMap();
    }

    void SpawnWorldMap()
    {
        for (int i = 0; i < mapSize * mapCount; i++)
        {
            for (int j = 0; j < mapSize * mapCount; j++)
            {
                if (worldMapl[i, j] == 1)
                {

                    GameObject instLane = Instantiate(RoadPiece, new Vector3(i * RoadPiece.GetComponent<Renderer>().bounds.size.x, 100, RoadPiece.GetComponent<Renderer>().bounds.size.z * j), Quaternion.identity);
                    instLane.GetComponent<Renderer>().material.color = Color.red;
                    instLane.transform.parent = WorldMapParent.transform;

                }
            }
        }
    }

    void NullWorld()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in WorldMapParent.transform)
        {
            foreach (Transform subchild in child)
            {
                GameObject.Destroy(subchild.gameObject);
            }
        }
        for (int i = 0; i < mapSize * mapCount; i++)
        {
            for (int j = 0; j < mapSize * mapCount; j++)
            {
                worldMapl[i, j] = 0;
            }
        }

    }




    void NullMap()
    {
        testMap = new int[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                testMap[i, j] = 0;
            }
        }

    }

    void GenerateMap()
    {
        int randomNum = Random.Range(ranMin, ranMax);
        bool randomIs = false;

        for (int i = 0; i < mapSize; i++)
        {
            if (randomIs == true)
            {

                randomNum = i + Random.Range(ranMin, ranMax);
                randomIs = false;
                if (randomNum + ranMin >= mapSize)
                {
                    randomNum = mapSize;
                }
            }
            for (int j = 0; j < mapSize; j++)
            {
                if (i == 0 || j == 0 || i == mapSize - 1 || j == mapSize - 1)
                {
                    testMap[i, j] = 1;
                }
                if (i == randomNum)
                {
                    testMap[i, j] = 1;
                    randomIs = true;
                }
            }
        }

        randomNum = Random.Range(ranMin, ranMax);

        for (int i = 0; i < mapSize; i++)
        {
            if (randomIs == true)
            {
                randomNum = i + Random.Range(ranMin, ranMax);
                randomIs = false;
                if (randomNum + ranMin >= mapSize)
                {
                    randomNum = mapSize;
                }
            }
            for (int j = 0; j < mapSize; j++)
            {

                if (i == randomNum)
                {
                    testMap[j, i] = 1;
                    randomIs = true;
                }
            }
        }

        // Удаляем рандомные отрезки

        for (int i = 0; i < mapSize - 1; i++)
        {
            if (testMap[i, 1] == 1)
            {
                int cross = 0;
                int count = 0;
                for (int j = 1; j < mapSize - 1; j++)
                {
                    if (i == 0)
                    {
                        if (testMap[i + 1, j] == 1)
                        {
                            count++;
                        }
                        if (j == mapSize - 2)
                        {
                            randMax = count + 1;
                            delete = Random.Range(0, randMax);
                        }
                    }
                    else
                    {
                        count = 0;
                        if (testMap[i + 1, j] == 1) count++;
                        if (testMap[i, j + 1] == 1) count++;
                        if (testMap[i - 1, j] == 1) count++;
                        if (testMap[i, j - 1] == 1) count++;
                        if (count > 2)
                        {
                            cross++;
                        }
                        if (cross == delete && count < 3)       // We need count < 3 to delet everything EXEPT the crossroad
                        {
                            testMap[i, j] = 0;
                        }
                    }
                }
                delete = Random.Range(0, randMax);
            }

        }
        delete = 0;
        randMax = 0;
        // Удаляем рандомные отрезки, но уже вертикально

        for (int i = 1; i < mapSize - 1; i++)
        {
            int cross = 0;
            int count = 0;
            for (int j = 1; j < mapSize - 1; j++)
            {
                count = 0;
                if (testMap[j + 1, i] == 1) count++;
                if (testMap[j, i + 1] == 1) count++;
                if (testMap[j - 1, i] == 1) count++;
                if (testMap[j, i - 1] == 1) count++;
                if (count > 2)
                {
                    cross++;
                }
            }
            randMax = cross + 1;
            cross = 0;
            delete = Random.Range(0, randMax);
            if (randMax != 1)
            {
                //  Debug.Log(delete + 1 + " from " + randMax);
                for (int j = 1; j < mapSize - 1; j++)
                {
                    count = 0;
                    if (testMap[j + 1, i] == 1) count++;
                    if (testMap[j, i + 1] == 1) count++;
                    if (testMap[j, i - 1] == 1) count++;
                    if (count > 1)
                    {
                        cross++;
                    }
                    if (cross == delete && count < 2)       // We need count < 3 to delet everything EXEPT the crossroad
                    {
                        testMap[j, i] = 0;
                    }
                }
            }

            // delete = Random.Range(0, randMax);


        }

        // Удаляем часть крайних елементов для соединения с соседними массивами

        for (int i = 0; i < mapSize; i++)
        {

            for (int j = 0; j < mapSize; j++)
            {

                if (i == 0 && j < mapSize / 2)
                {
                    testMap[i, j] = 0;
                }
                if (i > mapSize / 2 && j == 0)
                {
                    testMap[i, j] = 0;
                }
                if (i == mapSize - 1 && j > mapSize / 2)
                {
                    testMap[i, j] = 0;
                }
                if (i < mapSize / 2 && j == mapSize - 1)
                {
                    testMap[i, j] = 0;
                }

            }
        }
        testMap[0, 0] = 1;
        // testMap[mapSize - 1, mapSize - 1] = 1;
        testMap[mapSize / 2, mapSize - 1] = 0;
        testMap[mapSize - 1, mapSize / 2] = 0;
    }
    void SpawnMap(Vector3 spawnpoint, int indexI, int indexJ)
    {
        //Vector3 spawnpoint = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);


        float mapSizeX = pieceSizeX * mapSize;
        float mapSizeZ = pieceSizeZ * mapSize;

        Vector3 spawnPos = new Vector3(spawnpoint.x /*- mapSizeX / 2*/ + pieceSizeX / 2, 0, spawnpoint.z /*- mapSizeZ / 2*/);

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (testMap[i, j] == 1)
                {
                    GameObject instLane = Instantiate(RoadPiece, spawnPos, Quaternion.identity);
                    instLane.transform.parent = city[indexI, indexJ].transform;
                }
                else if (testMap[i, j] == 0 && i != 0 && j != 0 && i != mapSize - 1 && j != mapSize - 1)
                {
                    
                    GameObject instWall = Instantiate(WallPiece, spawnPos - new Vector3(0, 3 * 9, 0), Quaternion.identity);
                    instWall.transform.parent = city[indexI, indexJ].transform;
                    instWall.GetComponent<BuildingScript>().IndexH = (int)(i + indexI * mapSize - indexI);
                    instWall.GetComponent<BuildingScript>().IndexW = (int)(j + indexJ * mapSize - indexJ);
                    
                }
                spawnPos += new Vector3(pieceSizeX, 0, 0);
            }
            spawnPos += new Vector3(-mapSizeX, 0, pieceSizeZ);
        }
    }
}