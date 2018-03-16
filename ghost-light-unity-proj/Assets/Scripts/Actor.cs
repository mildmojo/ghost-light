using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {
  public GameObject spotlight;

  public void Start() {
  }

  public void Update() {

  }

  public void Select() {
    spotlight.SetActive(true);
  }

  public void Unselect() {
    spotlight.SetActive(false);
  }
}
