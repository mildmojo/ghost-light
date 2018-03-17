using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Handle setting the currently playing VO and text from a script file
public class ScriptManager : MonoBehaviour {
    public PlayScript scriptToPerform;

    public PlayScene CurrentScene
    {
        get
        {
            if(scriptToPerform == null
                || currentSceneIndex >= scriptToPerform.Scenes.Count)
            {
                return null;
            }
            return scriptToPerform.Scenes[currentSceneIndex];
        }
    }

    //it's probably unwise to have this index go across two lists but here we are :)
    //I blame Ruby Canyon, personally
    public PlayLine CurrentLine
    {
        get
        {
            if(CurrentScene == null)
            {
                return null;
            }

            if(currentLineIndex >= CurrentScene.Lines.Count)
            {
                if(currentLineIndex < CurrentScene.Lines.Count + CurrentScene.ChorusLines.Count - 2)
                {
                    if(currentLineIndex == CurrentScene.Lines.Count)
                    {
                        OnChorusStarted.Invoke();
                    }
                    return CurrentScene.ChorusLines[currentLineIndex - CurrentScene.Lines.Count];
                } else
                {
                    return null;
                }
            }

            return CurrentScene.Lines[currentLineIndex];
        }
    }

    public static UnityEvent OnSceneEnded = new UnityEvent();
    public static UnityEvent OnChorusStarted = new UnityEvent();
    public static NewLineEvent OnLineChanged = new NewLineEvent();

    private int currentSceneIndex;
    private int currentLineIndex;

    private MeterManager meterManager;

    void Awake() {
        meterManager = FindObjectOfType<MeterManager>();
        meterManager.OnActorChanged.AddListener(NextLine);
    }

    private void Start()
    {
        OnLineChanged.Invoke(CurrentLine);
    }

    void NextLine()
    {
        currentLineIndex++;

        if(CurrentLine == null)
        {
            OnSceneEnded.Invoke();

            currentSceneIndex++;
            currentLineIndex = 0;
        }
        OnLineChanged.Invoke(CurrentLine);
    }
}
