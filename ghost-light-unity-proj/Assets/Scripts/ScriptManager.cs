using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

//Handle setting the currently playing VO and text from a script file
public class ScriptManager : MonoBehaviour {
    public static ScriptManager instance;

    public PlayScript scriptToPerform;
    public Actor[] actorPrefabs;

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

    private List<ActorPosition> actorPositions;

    private int currentSceneIndex;
    private int currentLineIndex;

    private MeterManager meterManager;

    void Awake() {
        instance = this;
        actorPositions = FindObjectsOfType<ActorPosition>().ToList();
    }

    private IEnumerator Start()
    {
        meterManager = MeterManager.instance;
        meterManager.OnActorChanged.AddListener(NextLine);
        UpdateActors();

        yield return null;

        OnLineChanged.Invoke(CurrentLine);
    }

    void NextLine()
    {
        currentLineIndex++;

        if(CurrentLine == null)
        {
            NewScene();
        }
        OnLineChanged.Invoke(CurrentLine);
    }

    void NewScene()
    {
        OnSceneEnded.Invoke();

        currentSceneIndex++;
        currentLineIndex = 0;

        UpdateActors();
    }

    void UpdateActors()
    {
        foreach(ActorPosition ap in actorPositions)
        {
            if(ap.transform.childCount > 0)
            {
                Destroy(ap.transform.GetChild(0));
            }
        }
        int actorCount = 0;
        foreach(Role role in CurrentScene.Roles)
        {
            Actor prefab = actorPrefabs.First(a => a.role == role);
            if (prefab != null && actorPositions.Count > actorCount)
            {
                Actor newActor = GameObject.Instantiate(prefab, actorPositions[actorCount].transform);
                newActor.transform.position = actorPositions[actorCount].transform.position;
                newActor.transform.rotation = actorPositions[actorCount].transform.rotation;
            }
            actorCount++;
        }
    }
}
