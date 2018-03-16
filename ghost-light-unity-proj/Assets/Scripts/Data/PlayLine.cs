using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayLine is a the smallest gameplay 'unit'
/// </summary>
[System.Serializable]
public class PlayLine
{
    public string LineOneText = "";
    public string LineTwoText = "";
    public AudioClip LineOneAudio;
    public AudioClip LineTwoAudio;
    public Emotions Emote;
    public Directions Direction;
}
