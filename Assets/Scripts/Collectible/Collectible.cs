using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Collectible
{
    public static int MAX_STACK = 999;

    public enum CollectibleType
    {
        None,
        Energy,
        Other
    }

    public enum CollectibleClassification
    { 
        Consumable,
        Storeable
    }

    [SerializeField] private bool stackable;
    [SerializeField] private CollectibleType collectibleType;
    [SerializeField] private string collectibleName;
    [SerializeField] private int amount;

    public bool IsStackable()
    {
        return stackable;
    }

    public string GetName()
    {
        return collectibleName;
    }

    public int GetAmount()
    {
        return amount;
    }

    public CollectibleType GetCollectibleType()
    {
        return collectibleType;
    }
}
