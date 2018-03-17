using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : MonoBehaviour {
  public Role role;
  public SpriteRenderer FaceRenderer;

  public float moveSpeed;
  public float deadZone;
  public float maxMarkDistance;
  public GameObject spotlight;
  public List<EmotionFace> EmotionMap;
  private Dictionary<Emotions, Sprite> facesByEmotion;
  public Transform mark;

  public enum Direction {
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 4
  }

  private StageManager stageManager;
  private MeterManager meterManager;
  private Vector3 motion;
  private float sqrMoveSpeed;
  private float sqrDeadzone;
  private bool isSelected;
  private bool hasHitMark;
  private Rigidbody body;

    private void Awake()
    {
        spotlight.SetActive(false);
    }
  public void Start() {
    stageManager = StageManager.instance;
    meterManager = MeterManager.instance;
    sqrMoveSpeed = moveSpeed * moveSpeed;
    sqrDeadzone = deadZone * deadZone;
    body = GetComponent<Rigidbody>();

        if(role != Role.Chorus)
        {
            ScriptManager.OnLineChanged.AddListener(UpdateMainActorStance);
        }
        ScriptManager.OnLineChanged.AddListener(UpdateEmotion);

    facesByEmotion = EmotionMap.ToDictionary(t => t.Emotion, t => t.EmotionSprite);
   }

  public void Update() {
    if (isSelected) {
      handleInputs();

      if (StageManager.instance.CanActorEmote()) {
        // no-op
      }

      if (stageManager.CanActorMove()) {
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
        if(mark != null)
        {
            hasHitMark = false;
            throwMark();
        }
  }

  public void Unselect() {
    spotlight.SetActive(false);
    isSelected = false;
    }

    private void UpdateMainActorStance(PlayLine line)
    {
        if(line.Speaker == role)
        {
            spotlight.SetActive(true);
        } else
        {
            spotlight.SetActive(false);
        }
    }

    private void UpdateEmotion(PlayLine line)
    {
        if(role == Role.Chorus || line.Speaker == role)
        {
            if (facesByEmotion.ContainsKey(line.Emote))
            {
                FaceRenderer.sprite = facesByEmotion[line.Emote];
            }
        } else
        {
            if (facesByEmotion.ContainsKey(Emotions.None))
            {
                FaceRenderer.sprite = facesByEmotion[Emotions.None];
            }
        }
        if(mark != null)
        {
            mark.gameObject.SetActive(false);
        }
    }

  void OnTriggerEnter(Collider c) {
    if (c.gameObject == mark.gameObject) {
      stageManager.ActorSuccess(1.5f);
    }
  }

  void OnTriggerEnter2D(Collider2D c) {
    if (c.gameObject == mark.gameObject) {
      stageManager.ActorSuccess(1.5f);
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
      if (meterManager.IsOnLongSyllable()) {
        stageManager.ActorSuccess();
      } else {
        stageManager.ActorFail();
      }
    }

    if (Input.GetButtonDown("ShortSyllable")) {
      if (meterManager.IsOnShortSyllable()) {
        stageManager.ActorSuccess();
      } else {
        stageManager.ActorFail();
      }
    }
  }

  private void throwMark() {
        if (!StageManager.instance.CanActorMove()) return;

    hasHitMark = false;
    var offset = Random.insideUnitCircle * maxMarkDistance;
    var newPos = transform.position + new Vector3(offset.x, offset.y, 0);
    mark.position = transform.position;
    mark.gameObject.SetActive(true);
    LeanTween.move(mark.gameObject, newPos, 0.25f).setEaseOutCubic();
  }
}
