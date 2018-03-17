using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptDisplayer : MonoBehaviour {
    public Text display;

    private void Awake()
    {
        display.text = "";
        ScriptManager.OnLineChanged.AddListener(UpdateText);
    }

    void UpdateText(PlayLine newLine)
    {
        if(newLine == null)
        {
            display.text = "";
        } else
        {
            string newText = string.Format("{0}\n{1}", newLine.LineOneText, newLine.LineTwoText);
            display.text = newText;
        }
    }
}
