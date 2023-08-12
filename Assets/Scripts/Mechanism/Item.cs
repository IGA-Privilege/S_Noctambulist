using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Item/New Item", fileName = "New Item")]
public class Item : ScriptableObject
{
    public string name;
    public Sprite icon;
}
