using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayLine is a the smallest gameplay 'unit'
/// </summary>
[System.Serializable]
public class PlayLine
{
    public Role Speaker = Role.None;
    public string LineOneText = "";
    public string LineTwoText = "";
    public int Syllables;
    public AudioClip LineOneAudio;
    public AudioClip LineTwoAudio;
    public Emotions Emote;
    public Directions Direction;
}
