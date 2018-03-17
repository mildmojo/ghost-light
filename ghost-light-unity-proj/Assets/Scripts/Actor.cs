using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Actor : MonoBehaviour {
  public GameObject spotlight;

  public enum Direction {
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 4
  }


  public void Start() {
  }

  public void Update() {
    handleInputs();
  }

  public void Select() {
    spotlight.SetActive(true);
  }

  public void Unselect() {
    spotlight.SetActive(false);
  }

  private void handleInputs() {
    var inputDevice = InputManager.ActiveDevice;

    if (inputDevice.LeftStickX < 0) {

    }
  }
}
