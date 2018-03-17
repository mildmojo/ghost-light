using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Establish rhythm with audio
// On meter beats, advance selected actor

public class MeterManager : MonoBehaviour {
  public int bpm;
  public AudioClip musicClip;
  public AudioSource musicSource;
  public List<Actor> actors;

  private Actor selectedActor;
  private int actorIdx;
  private double lastTick;
  private double elapsedTime;
  private int beatCount;

    public UnityEvent OnActorChanged = new UnityEvent();
  public void Start() {
    lastTick = AudioSettings.dspTime;

    actors.ForEach(actor => actor.Unselect());
    selectedActor = actors[0];
    selectedActor.Select();

    musicSource.clip = musicClip;
    musicSource.Play();
  }

  public void Update() {
    elapsedTime += AudioSettings.dspTime - lastTick;
    lastTick = AudioSettings.dspTime;

    // One beat elapsed?
    if (elapsedTime > 1 / (bpm / 60f)) {
      beatCount++;
      elapsedTime = 0f;
      if (beatCount > 0 && beatCount % 5 == 0) {
        nextActor();
      }
    }
  }

  private void nextActor() {
    selectedActor.Unselect();
    actorIdx = (actorIdx + 1) % actors.Count;
    selectedActor = actors[actorIdx];
    selectedActor.Select();

        OnActorChanged.Invoke();
  }
}
