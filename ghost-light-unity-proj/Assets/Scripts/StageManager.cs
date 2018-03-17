using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
  public static StageManager instance;

  public PlayScript script;

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

  private SecondaryAction currentSecondaryAction;
  private State currentState;

  void Awake() {
    instance = this;
    currentState = State.INTRO;
  }

  void Update() {
    // Placeholder; do this in response to input.
    currentState = State.PLAY;
    currentSecondaryAction = SecondaryAction.MOVE;
  }

  public bool CanActorMove() {
    return currentSecondaryAction == SecondaryAction.MOVE;
  }

}
