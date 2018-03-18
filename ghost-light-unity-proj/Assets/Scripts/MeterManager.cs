using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Establish rhythm with audio
// On meter beats, advance selected actor

public class MeterManager : MonoBehaviour {
  public static MeterManager instance;

  public int bpm;
  public float windowForInputSecs;
  public AudioClip musicClip;
  public AudioSource musicSource;
  public UnityEvent onMeasureFinish = new UnityEvent();
  public UnityEvent OnBeat = new UnityEvent();
  public UnityEvent OnExitSyllableWindow = new UnityEvent();
  public UnityEvent OnActorChanged = new UnityEvent();
  public GameObject flasher;
  public List<Actor> actors;

  [HideInInspector]
  public double deltaTime;
  [HideInInspector]
  public int beatsPerActor = 10;

  private ScriptManager scriptManager;
  private Actor selectedActor;
  private int actorIdx;
  private int lineIdx;
  private double gameStartTick;
  private double nextTickAt;
  private double lastTick;
  private double elapsedTime;
  private int beatCount;
  private List<int> longBeats = new List<int>() {0, 3, 5, 8};

  public void Awake() {
    instance = this;
  }

  public void Start() {
    scriptManager = ScriptManager.instance;

    actors.ForEach(actor => actor.Unselect());
    selectedActor = actors[0];
    selectedActor.Select();

    UpdateBeatsPerActor();
    onMeasureFinish.AddListener(NextActor);

    lastTick = AudioSettings.dspTime;
    nextTickAt = AudioSettings.dspTime;
    gameStartTick = AudioSettings.dspTime;
  }

  public void Update() {
    if(Menu.Instance.Open)
    {
        return;
    }

    deltaTime = AudioSettings.dspTime - lastTick;
    lastTick = AudioSettings.dspTime;

    // One beat elapsed?
    var dspNow = AudioSettings.dspTime;
    if (dspNow > nextTickAt) {
      nextTickAt = dspNow + beatLength();
      beatCount++;
      OnBeat.Invoke();
      if (beatCount > 0 && beatCount % beatsPerActor == 0) {
        onMeasureFinish.Invoke();
      }
    }

    if (dspNow > nextTickAt - beatLength() + windowForInputSecs / 2f) {
      OnExitSyllableWindow.Invoke();
    }

    UpdateBeatsPerActor();
  }

  public void Reset() {
    deltaTime = 0;
    beatCount = 0;
    lineIdx = 0;
    actorIdx = 0;
    actors.ForEach(actor => actor.Unselect());
    selectedActor = actors[0];
    selectedActor.Select();

    UpdateBeatsPerActor();

    musicSource.Stop();
    musicSource.clip = scriptManager.CurrentScene.MusicTrack;
    musicSource.Play();

    lastTick = AudioSettings.dspTime;
    nextTickAt = AudioSettings.dspTime;
    gameStartTick = AudioSettings.dspTime;
  }

  public void UpdateBeatsPerActor() {
    var line = scriptManager.CurrentLine;
    beatsPerActor = line.Syllables > 0 ? line.Syllables : 10;
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

  public bool IsOnShortSyllable() {
Debug.Log("Short syllable? just before: " + isJustBeforeShortSyllable() + ", just after: " + isJustAfterShortSyllable());
    return isJustBeforeShortSyllable() || isJustAfterShortSyllable();
  }

  public bool IsOnLongSyllable() {
Debug.Log("Long syllable? just before: " + isJustBeforeLongSyllable() + ", just after: " + isJustAfterLongSyllable());
    return isJustBeforeLongSyllable() || isJustAfterLongSyllable();
  }

  private bool isJustBeforeLongSyllable() {
    var syllableCount = scriptManager.CurrentLine.Syllables;
    var isLongSyllableNext = longBeats.Contains((beatCount + 1) % syllableCount);
    var isNearBeat = nextTickAt - AudioSettings.dspTime < windowForInputSecs / 2f;
    return isLongSyllableNext && isNearBeat;
  }

  private bool isJustAfterLongSyllable() {
    var syllableCount = scriptManager.CurrentLine.Syllables;
    var isLongSyllable = longBeats.Contains(beatCount % syllableCount);
    var isNearBeat = AudioSettings.dspTime - (nextTickAt - beatLength()) < windowForInputSecs / 2f;
    return isLongSyllable && isNearBeat;
  }

  private bool isJustBeforeShortSyllable() {
    var syllableCount = scriptManager.CurrentLine.Syllables;
    var isShortSyllableNext = !longBeats.Contains((beatCount + 1) % syllableCount);
    var isNearBeat = nextTickAt - AudioSettings.dspTime < windowForInputSecs / 2f;
    return isShortSyllableNext && isNearBeat;
  }

  private bool isJustAfterShortSyllable() {
    var syllableCount = scriptManager.CurrentLine.Syllables;
    var isShortSyllable = !longBeats.Contains(beatCount % syllableCount);
    var isNearBeat = AudioSettings.dspTime - (nextTickAt - beatLength()) < windowForInputSecs / 2f;
    return isShortSyllable && isNearBeat;
  }

  private float beatLength() {
    return 1 / (bpm / 60f);
  }
}
