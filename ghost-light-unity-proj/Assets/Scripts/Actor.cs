using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Actor : MonoBehaviour {
  public float moveSpeed;
  public float deadZone;
  public GameObject spotlight;

  public enum Direction {
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 4
  }

  private Vector3 motion;
  private float sqrMoveSpeed;
  private float sqrDeadzone;
  private bool isSelected;

  public void Start() {
    sqrMoveSpeed = moveSpeed * moveSpeed;
    sqrDeadzone = deadZone * deadZone;
  }

  public void Update() {
    if (isSelected) {
      handleInputs();
    }
    gameObject.transform.position += motion;
  }

  public void Select() {
    spotlight.SetActive(true);
    isSelected = true;
  }

  public void Unselect() {
    spotlight.SetActive(false);
    isSelected = false;
  }

  private void handleInputs() {
    motion = Vector3.zero;

    if (Input.GetButton("LeftArrow")) {
      motion += Vector3.left * moveSpeed;
    }
    if (Input.GetButton("RightArrow")) {
      motion += Vector3.right * moveSpeed;
    }
    if (Input.GetButton("UpArrow")) {
      motion += Vector3.up * moveSpeed;
    }
    if (Input.GetButton("DownArrow")) {
      motion += Vector3.down * moveSpeed;
    }

    if (motion.sqrMagnitude < sqrDeadzone) {
      motion = Vector3.zero;
    }

    if (motion.sqrMagnitude > sqrMoveSpeed) {
      motion = motion.normalized * moveSpeed;
    }
  }
}
