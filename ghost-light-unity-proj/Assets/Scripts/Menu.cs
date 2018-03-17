using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public Transform mainPanel;
    public Transform winPanel;
    public Transform losePanel;

    public bool Open
    {
        get
        {
            return (mainPanel.gameObject.activeInHierarchy 
                    || winPanel.gameObject.activeInHierarchy
                    || losePanel.gameObject.activeInHierarchy);
        }
    }

    public static Menu Instance;

	void Start () {
        Instance = this;
        mainPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
    }

    void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Open)
            {
                mainPanel.gameObject.SetActive(false);
            } else
            {
                mainPanel.gameObject.SetActive(true);
            }
        }
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        mainPanel.gameObject.SetActive(true);
    }

    public void Win()
    {
        winPanel.gameObject.SetActive(true);
    }

    public void Continue()
    {
        winPanel.gameObject.SetActive(false);
        ScriptManager.instance.NewScene();
    }

    public void Lose()
    {
        losePanel.gameObject.SetActive(true);
    }

    public void Retry()
    {
        losePanel.gameObject.SetActive(false);
        ScriptManager.instance.RetryScene();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        mainPanel.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
