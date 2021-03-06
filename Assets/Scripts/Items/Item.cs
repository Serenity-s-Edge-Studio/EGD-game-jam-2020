using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Item", menuName = "Items/Base Item")]
public class Item : ScriptableObject
{
    public static Sprite Placeholder;
    public new string name;
    public int id;
    [TextArea]
    public string description;
    public float mass;
    public Sprite icon;


    public override int GetHashCode()
    {
        return id;
    }

    public override bool Equals(object obj)
    {
        if (obj is Item)
            return id == ((Item)obj).id;
        return false;
    }
}
