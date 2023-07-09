using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    [Header("Gameplay Only")]
    public ItemType type;
    // public ActionType actionType;
    public Vector2Int range = new Vector2Int(5,4);

    [Header("UI Only")]
    public bool stackable = true;

    [Header("Gameplay and UI")]
    public Sprite image;

    public enum ItemType {
        Consumable,
        Weapon
    }
}
