using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{

    public void OnButtonClick(string name)
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);

        switch (EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text)
        {
            case "NEW GAME":
                SceneManager.LoadScene("MapGenerator", LoadSceneMode.Single);
                break;
            case "CONTINUE":
                SceneManager.LoadScene("MapGenerator", LoadSceneMode.Single);
                break;
            case "EXIT":
                Application.Quit();
                break;
        };
    }
}
