using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Side", menuName = "Game/Side Settings", order = 1)]
public class SideSettings : ScriptableObject
{
    public Side side;
    public Sprite icon;

    public Color healthColor;
    public Color middleHealthColor;
    public Color lowHealthColor;
}
