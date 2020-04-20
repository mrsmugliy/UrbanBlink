using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchManager : MonoBehaviour
{
    public int stickId;
    public Vector2 startPos;
    public Vector2 curPos = Vector2.zero;
    public Vector2 difference;
    public float Horizontal = 0;
    public float Vertical= 0;
    Transform hit_object;
    RaycastHit hit;
    Touch touch;
    public Transform Grid;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    PointerEventData StickData;
    EventSystem m_EventSystem;
    public bool useStick = false;
    public bool endStick = false;
    public float ScreenWidthInch;
    public float ScreenHeightInch;



    public Text Text;
    string message;
    int touch_id;

    void Start()
    {
        endStick = false;
        ScreenWidthInch = Screen.width;
        ScreenHeightInch = Screen.height;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Application.targetFrameRate = 30;
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene

    }
    void Update()
    {
        int n = Input.touchCount;
        if (n > 0) for (int i = 0; i < n; i++)
            {
                Touch touch = Input.GetTouch(i);                            // количевство прикосновений
                TouchPhase phase = touch.phase;                             // фаза прикосновения
                m_PointerEventData = new PointerEventData(m_EventSystem);   // событие, в которое мы сохроним нужную информацию
                List<RaycastResult> results = new List<RaycastResult>();    // список лучей
                switch (phase)
                {
                    case TouchPhase.Began: // когда палец ТОЛЬКО ПРИКОСАЕТСЯ

                        m_PointerEventData.position = touch.position;       // сохраняем позицию в наше событие
                        m_Raycaster.Raycast(m_PointerEventData, results);   // проводим луч 
                        foreach (RaycastResult result in results)           // проверяем или лучь проходит сквозь нужную часть UI
                            if (result.gameObject.tag == "Stick" && useStick == false)               // если лучь попадает в нужное место
                            {
                                useStick = true;            // используем стик
                                startPos = touch.position;  // сохраняем позицию начала
                                curPos = startPos;          // сохраняем позицыю действительную
                                stickId = touch.fingerId;   // сохраняем номер пальца
                            }
                        break;

                    case TouchPhase.Moved:  // когда палец ДВИЖЕТЬСЯ
                        
                        if (touch.fingerId == stickId) // если номер пальца совпадает
                        {
                            curPos += touch.deltaPosition;  // меняем действительную позиц0ыю
                            if (SystemInfo.deviceType != DeviceType.Desktop)
                            {
                                Grid.GetComponent<TestMovement>().xValue = touch.deltaPosition.x;
                                Grid.GetComponent<TestMovement>().yValue = touch.deltaPosition.y;
                            }
                                
                        }

                        //  float Horizontal = CanvasTransform.GetComponent<TouchManager>().Horizontal;
                        // float Vertical = CanvasTransform.GetComponent<TouchManager>().Vertical;
                        
                        //if (difference.x > 100 || difference.y > 100 || difference.x < -100 || difference.y < -100) touch.phase = TouchPhase.Ended;

                        break;


                    case TouchPhase.Stationary: // когда палец НЕ ДВИЖЕТЬСЯ

                        break;

                    case TouchPhase.Ended: // когда палец ЗАКАНЧИВАЕТ ПРИКОСНОВЕНИЕ 
                        if (touch.fingerId == stickId)  // если номер пальца совпадает
                            endStick = true;            // фиксируем конец прикосновения к стику

                        break;

                    case TouchPhase.Canceled: // когда палец НЕ ДВИЖЕТЬСЯ

                        if (touch.fingerId == stickId)  // если номер пальца совпадает
                            endStick = true;            // фиксируем конец прикосновения к стику
                        break;
                }
            }
        // if (useStick)
        //   FollowStick();

        if (useStick == true)   // если мы используем стик
            FollowStick();      //запускаем функцию   FollowStick()
    }

    void FollowStick()
    {
        difference = startPos - curPos;                 // считаем разницу между начальной и действительной позицыей 

        if (difference.x > 100) difference.x = 100;     // держим её в пределах 100....-100
        if (difference.y > 100) difference.y = 100;     //
        if (difference.x < -100) difference.x = -100;   //
        if (difference.y < -100) difference.y = -100;   //

        Horizontal = difference.x * -0.01f;             // сохраняем Горизонталь в предел 1...-1
        Vertical = difference.y * -0.01f;               // сохраняем Вертикаль в предел 1...-1       

        if (endStick == true)                           // если закачивается прикосновение пальца
        {
            startPos = Vector2.zero;                    // все обнуляем
            curPos = Vector2.zero;                      //
            endStick = false;                           //
            stickId = -1;                               //
            Horizontal = 0;                             //
            Vertical = 0;                               //
            useStick = false;                           //

        }

    }
}
