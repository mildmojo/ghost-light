using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {
  public static StageManager instance;

  [Tooltip("Decay rate per second")]
  public float momentumDecayRate;
  [Tooltip("Amount to add to momentum on successful action")]
  public float momentumIncrement;
  public float momentumMax;
  public Slider momentumMeter;
  public PlayScript script;

  [HideInInspector]
  public PlayScene currentScene;

  public enum SecondaryAction {
    MOVE = 0,
    EMOTE = 1
  }

  public enum State {
    INTRO = 0,
    SCENE_BEGIN = 1,
    PLAY = 2,
    SCENE_FINISH = 3
  }

  private SecondaryAction[] secondaryActions = new[] { SecondaryAction.MOVE, SecondaryAction.EMOTE };
  private SecondaryAction currentSecondaryAction;
  private State currentState;
  private float momentum;

  void Awake() {
    instance = this;
    currentState = State.INTRO;
    currentScene = script.Scenes[Random.Range(0, script.Scenes.Count)];
    momentum = momentumMax;
  }

  void Start() {
    MeterManager.instance.OnActorChanged.AddListener(() => {
      var nextActionIdx = Random.Range(0,2);
Debug.Log("changing secondary action: " + nextActionIdx);

      currentSecondaryAction = secondaryActions[nextActionIdx];
    });
  }

  void Update() {
    // Placeholder; do this in response to input.
    currentState = State.PLAY;
    momentum -= momentumDecayRate * (float)MeterManager.instance.deltaTime;
    momentumMeter.value = momentum;
    // currentSecondaryAction = SecondaryAction.MOVE;
  }

  public bool CanActorMove() {
    return currentSecondaryAction == SecondaryAction.MOVE;
  }

  public bool CanActorEmote() {
    return currentSecondaryAction == SecondaryAction.EMOTE;
  }

  public void ActorSuccess(float factor = 1f) {
    momentum += momentumIncrement * factor;
  }
}
