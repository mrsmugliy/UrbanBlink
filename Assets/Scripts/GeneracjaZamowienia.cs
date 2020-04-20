
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneracjaZamowienia : MonoBehaviour
{
    public GameObject Dist;
    public GameObject MapSpawner;
    public int[,] navigationMap;
    public int mapSize = 0;

    void Start()
    {
        mapSize = MapSpawner.GetComponent<CitySpawner>().mapSize;
        navigationMap = new int[mapSize * 4, mapSize * 4];

    }
    
    void Update()
    {
        
    }
    public void UpdateNavigationMap()
    {
        Debug.Log(mapSize + "HUI");
        for (int i = 0; i < mapSize * 4; i++)
        {
            for (int j = 0; j < mapSize * 4; j++)
            {

                navigationMap[i, j] = MapSpawner.GetComponent<CitySpawner>().worldMapl[i,j];
            }
        }
    }

}