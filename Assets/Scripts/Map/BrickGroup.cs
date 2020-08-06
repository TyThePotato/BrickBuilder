using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BrickGroup
{
    public string Name; // name of the group
    public int ID; // id of the group

    public List<int> Children = new List<int>(); // ids of the group's children
    public BrickGroup Parent; // parent
}
