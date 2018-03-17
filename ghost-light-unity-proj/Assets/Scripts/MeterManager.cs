using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Establish rhythm with audio
// On meter beats, advance selected actor

public class MeterManager : MonoBehaviour {
  public static MeterManager instance;

  public int bpm;
  public AudioClip musicClip;
  public AudioSource musicSource;
  public UnityEvent onMeasureFinish = new UnityEvent();
  public UnityEvent OnActorChanged = new UnityEvent();
  public List<Actor> actors;

  [HideInInspector]
  public double deltaTime;
  [HideInInspector]
  public int beatsPerActor = 10;

  private Actor selectedActor;
  private int actorIdx;
  private int lineIdx;
  private double lastTick;
  private double elapsedTime;
  private int beatCount;


  public void Awake() {
    instance = this;
  }

  public void Start() {
    lastTick = AudioSettings.dspTime;

    actors.ForEach(actor => actor.Unselect());
    selectedActor = actors[0];
    selectedActor.Select();

    UpdateBeatsPerActor();
    onMeasureFinish.AddListener(NextActor);

    musicSource.clip = musicClip;
    musicSource.Play();
  }

  public void Update() {
    elapsedTime += AudioSettings.dspTime - lastTick;
    deltaTime = AudioSettings.dspTime - lastTick;
    lastTick = AudioSettings.dspTime;

    // One beat elapsed?
    if (elapsedTime > 1 / (bpm / 60f)) {
      beatCount++;
      elapsedTime = 0f;
      if (beatCount > 0 && beatCount % beatsPerActor == 0) {
        onMeasureFinish.Invoke();
      }
    }

    UpdateBeatsPerActor();
  }

  public void UpdateBeatsPerActor() {
    var line = ScriptManager.instance.CurrentLine;
    beatsPerActor = line.Syllables;
// Debug.Log(beatsPerActor);
  }

  public void NextActor() {
Debug.Log("next actor!");
    selectedActor.Unselect();
    actorIdx = (actorIdx + 1) % actors.Count;
    selectedActor = actors[actorIdx];
    selectedActor.Select();
    OnActorChanged.Invoke();
  }
}
