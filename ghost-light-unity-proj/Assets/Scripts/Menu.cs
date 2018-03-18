using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public static Menu Instance;
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

    private CanvasGroup mainCanvas;

	void Start () {
        Instance = this;
        mainPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        mainCanvas = GetComponent<CanvasGroup>();
        mainCanvas.alpha = 1f;
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
        StageManager.instance.SetGameState(StageManager.State.SCENE_FINISH);
    }

    public void Continue()
    {
        winPanel.gameObject.SetActive(false);
        ScriptManager.instance.NewScene();
        MeterManager.instance.Reset();
        StageManager.instance.SetGameState(StageManager.State.PLAY);
    }

    public void Lose()
    {
        losePanel.gameObject.SetActive(true);
        StageManager.instance.SetGameState(StageManager.State.SCENE_FINISH);
    }

    public void Retry()
    {
        losePanel.gameObject.SetActive(false);
        MeterManager.instance.Reset();
        ScriptManager.instance.RetryScene();
        StageManager.instance.SetGameState(StageManager.State.PLAY);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        mainPanel.gameObject.SetActive(false);
        StageManager.instance.SetGameState(StageManager.State.PLAY);
        MeterManager.instance.Reset();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
