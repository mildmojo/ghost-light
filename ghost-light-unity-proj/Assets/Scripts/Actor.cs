using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : MonoBehaviour {
    public Role role;
    public SpriteRenderer FaceRenderer;
    
  public float moveSpeed;
  public float deadZone;
  public GameObject spotlight;

    public List<EmotionFace> EmotionMap;
    private Dictionary<Emotions, Sprite> facesByEmotion;

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
  private Rigidbody body;

  public void Start() {
    sqrMoveSpeed = moveSpeed * moveSpeed;
    sqrDeadzone = deadZone * deadZone;
    body = GetComponent<Rigidbody>();

        ScriptManager.OnLineChanged.AddListener(UpdateEmotion);

        facesByEmotion = EmotionMap.ToDictionary(t => t.Emotion, t => t.EmotionSprite);
   }

  public void Update() {
    if (isSelected) {
      if (StageManager.instance.CanActorEmote()) {
      }

      if (StageManager.instance.CanActorMove()) {
        handleInputs();
        body.velocity = motion;
        // gameObject.transform.position += motion * (float)MeterManager.instance.deltaTime;
      }
    } else {
      body.velocity = Vector3.zero;
    }
  }

  public void Select() {
    spotlight.SetActive(true);
    isSelected = true;
  }

  public void Unselect() {
    spotlight.SetActive(false);
    isSelected = false;
  }

    private void UpdateEmotion(PlayLine line)
    {
        if(facesByEmotion.ContainsKey(line.Emote))
        {
            FaceRenderer.sprite = facesByEmotion[line.Emote];
        }
    }

  private void handleInputs() {
    motion = Vector3.zero;

    var x = Input.GetAxis("Horizontal");
    motion += x * Vector3.right * moveSpeed;
    var y = Input.GetAxis("Vertical");
    motion += y * Vector3.up * moveSpeed;

    if (motion.sqrMagnitude < sqrDeadzone) {
      motion = Vector3.zero;
    }

    if (motion.sqrMagnitude > sqrMoveSpeed) {
      motion = motion.normalized * moveSpeed;
    }

    if (Input.GetButtonDown("LongSyllable")) {
      StageManager.instance.ActorSuccess();
    }

    if (Input.GetButtonDown("ShortSyllable")) {
      StageManager.instance.ActorSuccess();
    }
  }
}
