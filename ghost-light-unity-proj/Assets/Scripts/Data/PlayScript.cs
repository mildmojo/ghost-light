using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Play", menuName = "New Play Script", order = 11)]
public class PlayScript : ScriptableObject
{
    public string Name;
    public List<PlayScene> Scenes = new List<PlayScene>();
}
