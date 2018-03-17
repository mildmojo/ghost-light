using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public Transform mainPanel;

    public bool Open
    {
        get { return mainPanel.gameObject.activeInHierarchy; }
    }

    public static Menu Instance;

	void Start () {
        Instance = this;
        mainPanel.gameObject.SetActive(true);
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
