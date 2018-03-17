using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayScene contains all the lines used in a single scene of the play.
/// We won't enforce the line count here, but if there's more we should clamp it elsewhere.
/// </summary>
[System.Serializable]
public class PlayScene
{
    public string Name;
    public AudioClip MusicTrack;

    [Tooltip("What non-chorus actors are in this scene?")]
    public List<Role> Roles = new List<Role>();
    public List<PlayLine> Lines = new List<PlayLine>();
    public List<PlayLine> ChorusLines = new List<PlayLine>();
}
