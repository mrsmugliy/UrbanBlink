
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserInterfaceScript : MonoBehaviour
{
    public GameObject MapSpawner;
    public Image Pixel;
    public GameObject PausePanel;
    public GameObject FullMapPanel;
    public GameObject ExitPanel;
    public GameObject RestartPanel;
    public GameObject SpeedUp;
    public GameObject SpeedDown;
    public GameObject Grid;
    public GameObject Player;

    public float plyrIdX;
    public float plyrIdY;


    public float ScreenWidthInch;
    public float ScreenHeightInch;

    Color clrRoad;
    Color clrPoint;
    int mapSize;
    dots[,] FullMap;

    public class dots
    {
        public int index;
        public Image image;
        public Color color;

        //public int i;
        // public int j;

        public dots(int id, Image img, Color clr)
        {
            index = id;
            image = img;
            color = clr;
        }
    }
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            transform.GetComponent<TouchManager>().enabled = false;   
        }
    }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ScreenWidthInch = Screen.width;
        ScreenHeightInch = Screen.height;
        PausePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidthInch * 0.1f, ScreenHeightInch * 0.1f); // так указывать размер элементов

        RestartPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidthInch * 0.1f, ScreenHeightInch * 0.1f);
        RestartPanel.GetComponent<RectTransform>().localPosition += new Vector3(ScreenWidthInch * 0.1f, 0, 0);

        ExitPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidthInch * 0.1f, ScreenHeightInch * 0.1f);
        ExitPanel.GetComponent<RectTransform>().localPosition += new Vector3(ScreenWidthInch * 0.2f, 0, 0);

        SpeedUp.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidthInch * 0.15f, ScreenHeightInch * 0.3f);
        SpeedUp.GetComponent<RectTransform>().localPosition = new Vector3(-ScreenWidthInch * 0.45f, -ScreenHeightInch * 0.25f, 0);

        SpeedDown.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenWidthInch * 0.15f, ScreenHeightInch * 0.15f);
        SpeedDown.GetComponent<RectTransform>().localPosition = new Vector3(-ScreenWidthInch * 0.45f, -ScreenHeightInch * 0.45f, 0);

        FullMapPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenHeightInch * 0.9f, ScreenHeightInch * 0.9f);

        mapSize = MapSpawner.GetComponent<CitySpawner>().mapSize;
        FullMap = new dots[mapSize * 4, mapSize * 4];
        clrRoad = new Color(0, 0.271f, 0.212f);
        clrPoint = new Color(0, 0.98f, 0.761f);

        for (int i = 0; i < mapSize * 4; i++)
        {
            for (int j = 0; j < mapSize * 4; j++)
            {
                FullMap[i, j] = new dots(0, null, Color.clear);

            }
        }
        Pixel.rectTransform.sizeDelta = new Vector2(FullMapPanel.GetComponent<RectTransform>().sizeDelta.x / (mapSize * 4 - 3), FullMapPanel.GetComponent<RectTransform>().sizeDelta.y / (mapSize * 4 - 3));
        //RectTransform rt = UICanvas.GetComponent(typeof(RectTransform)) as RectTransform;
        // rt.sizeDelta = new Vector2(100, 100);
    }

    void Update()
    {
        UpdatePlayerPos();
    }

    public void Pause()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
            FullMapPanel.SetActive(true);
            ExitPanel.SetActive(true);
            RestartPanel.SetActive(true);
            SpeedUp.SetActive(true);
            SpeedDown.SetActive(true);

        }
        else
        {
            FullMapPanel.SetActive(false);
            ExitPanel.SetActive(false);
            RestartPanel.SetActive(false);
            SpeedUp.SetActive(false);
            SpeedDown.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MapGenerator");
        
    }

    public void Exit()
    {
        Application.Quit();
        //SceneManager.LoadScene("MenuSrolling", LoadSceneMode.Single);

    }

    public void PlusSpeed()
    {
        Grid.GetComponent<TestMovement>().SpeedDown = false;

        Grid.GetComponent<TestMovement>().SpeedUp = true;
    }
    public void PlusCancle()
    {

        Grid.GetComponent<TestMovement>().SpeedUp = false;
    }
    public void MinusSpeed()
    {
        Grid.GetComponent<TestMovement>().SpeedUp = false;

        Grid.GetComponent<TestMovement>().SpeedDown = true;
    }
    public void MinusCancle()
    {

        Grid.GetComponent<TestMovement>().SpeedDown = false;
    }

    public void UpdateFullMap()
    {
        Debug.Log("UpdateFullMap");
        for (int i = 0; i < mapSize * 4; i++)
        {
            for (int j = 0; j < mapSize * 4; j++)
            {
                FullMap[i, j].index = MapSpawner.GetComponent<CitySpawner>().worldMapl[i, j];
            }
        }
        InstFullMap();
    }
    public void UpdatePlayerPos()
    {
        FullMap[(int)plyrIdX, (int)plyrIdY].color = clrRoad;
        plyrIdX = Player.GetComponent<PlayerHitDetection>().plyrIdX;
        plyrIdY = Player.GetComponent<PlayerHitDetection>().plyrIdX;
        FullMap[(int)plyrIdX, (int)plyrIdY].color = Color.red;
    }

    void InstFullMap()
    {
        foreach (Transform child in FullMapPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Debug.Log("InstFullMap");
        float pieceSizeX = Pixel.rectTransform.sizeDelta.x;
        float pieceSizeY = Pixel.rectTransform.sizeDelta.y;
        float mapSizeX = pieceSizeX * mapSize * 4;
        float mapSizeZ = pieceSizeY * mapSize * 4;
        Vector3 spawnPos = new Vector3(FullMapPanel.transform.position.x, FullMapPanel.transform.position.y, 0);
        for (int i = 0; i < mapSize * 4; i++)
        {
            for (int j = 0; j < mapSize * 4; j++)
            {
                if (FullMap[i, j].index == 1)
                {
                    Image creainstPixel = Instantiate(Pixel, spawnPos, Quaternion.identity, FullMapPanel.transform);
                    creainstPixel.color = clrRoad;
                    FullMap[i, j].image = creainstPixel;
                    FullMap[i, j].color = creainstPixel.color;
                    // creainstPixel.transform.parent = FullMapPanel.transform;
                }
                spawnPos += new Vector3(pieceSizeX, 0, 0);
            }
            spawnPos += new Vector3(-mapSizeX, pieceSizeY, 0);
        }
    }
}

