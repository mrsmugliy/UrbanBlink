using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [Range (1, 10)]
    [Header ("Conrollers")]
    public int panCount;
    [Range(0, 500)]
    public int panOffset;
    [Range(0, 500)]
    public int panOffsetDecreas;
    public int selectedPanID;

    [Header("Other Objects")]
    [Range(0f, 5f)]
    public float scaleOffset;
    [Range(0f, 50f)]
    public float snapSpeed;
    [Range(0f, 20f)]
    public float scaleSpeed;

    public ScrollRect scrollRect;
    


    public GameObject panPrefab;
    private GameObject[] instPans;

    private Vector2[] pansPos;
    private Vector2 contentVector;
    private Vector2[] pansScale;
 
    private RectTransform contentRect;
    private Transform mainPan;

    public bool isScrolling;
    public string[] buttonText;
    void Start()
    {
        buttonText = new string[] { "CREDITS", "CONTINUE", "NEW GAME", "STORE", "MODIFY", "OPTIONS", "EXIT" }; ;
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];
        for (int i = 0; i < panCount; i++)
        {
            instPans[i] = Instantiate(panPrefab, transform, false);
            //instPans[i].transform.GetChild(0).gameObject.AddComponent<ButtonClick>();
            instPans[i].transform.SetAsFirstSibling();
            instPans[i].transform.GetChild(0).GetComponentInChildren<Text>().text = buttonText[i];

            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i -1].transform.localPosition.x + panOffsetDecreas + panOffset,
                instPans[i].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition;
        }
        StartCenter();
    }
    public void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x && !isScrolling) scrollRect.inertia = false;
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
                
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            instPans[i].transform.localScale = pansScale[i];
            
        }
        mainPan = instPans[selectedPanID].transform; // Cast it to RectTransform
        mainPan.SetAsLastSibling(); // Make the panel show on top.

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }
    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
    public void StartCenter()
    {
        int select = 2;
        float nearestPos = float.MaxValue;
        for (int i = 0; i<panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (select == i)
            {
                nearestPos = distance;
                selectedPanID = i;
                
            }
            float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
            if (isScrolling || scrollVelocity > 400) return;
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, 1);
            contentRect.anchoredPosition = contentVector;

        }
    }
    public void Click()
    {

    }

}
