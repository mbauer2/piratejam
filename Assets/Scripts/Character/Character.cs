using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum StatusEffect
    { 
        Stunned,
        Slowed,
        Hidden,
        Cloaked
    }

    [SerializeField] private string characterName;
    
    protected Vector3 velocity = Vector3.zero;
    protected int energy = 0;

    private Dictionary<Collectible.CollectibleType, int> inventory;
    private Dictionary<StatusEffect, float> statusEffects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetEnergy()
    {
        return energy;
    }

    public bool Collect(GameObject collectibleObject)
    {
        Collectible collectible = collectibleObject.gameObject.GetComponent<CollectibleObject>().GetCollectible();

        bool success = false;

        if (collectible.GetCollectibleType() == Collectible.CollectibleType.Energy)
        {
            energy += collectible.GetAmount();

            success = true;
        }
        else
        {
            success = AddToInventory(collectible);
        }
        

        if (success)
        {
            Destroy(collectibleObject);
        }

        return success;
    }

    public bool AddToInventory( Collectible collectible )
    {
        bool added = false;
        Collectible.CollectibleType cType = collectible.GetCollectibleType();
        int newValue = collectible.GetAmount();
        if (inventory.ContainsKey( cType ))
        {
            if (collectible.IsStackable())
            {
                newValue = inventory[cType] + collectible.GetAmount();
            }
            else
            {
                newValue = 1;
                added = false;
            }

            if (newValue == 0)
            {
                inventory.Remove(cType);
                added = true;
            }
            else if (newValue < 0)
            {
                added = false;
            }
            else
            {
                inventory[cType] = newValue;
            }
        }
        else
        {
            if ( newValue > 0)
            {
                inventory.Add(cType, newValue);
            }
        }

        return added;
    }


    public void InflictStatus( StatusEffect status, float amount, bool infinite, bool additive )
    {
        CheckStatusApplicable(status);

        float newValue = infinite ? int.MaxValue : amount;
        if (statusEffects.ContainsKey(status))
        {
            if (additive)
            {
                newValue = statusEffects[status] + amount;
            }

            if (newValue <= 0)
            {
                statusEffects.Remove(status);
            }
            else
            {
                statusEffects[status] = newValue;
            }
        }
        else
        {
            statusEffects.Add(status, newValue);
        }
    }

    public bool CheckStatusApplicable( StatusEffect status )
    {
        bool isVulnerable = true;
        return isVulnerable;
    }

    public void RemoveStatus( StatusEffect status )
    {
        statusEffects.Remove(status);
    }
}
