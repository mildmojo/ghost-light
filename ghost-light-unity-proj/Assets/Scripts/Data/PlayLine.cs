using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayLine is a the smallest gameplay 'unit'
/// </summary>
[System.Serializable]
public class PlayLine
{
    public string LineText = "";
    public AudioClip LineAudio;
    public Emotions Emote;
    public Directions Direction;
}
