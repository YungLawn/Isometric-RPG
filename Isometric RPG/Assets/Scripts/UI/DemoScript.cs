using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id) {
        bool result = inventoryManager.addItem(itemsToPickup[id]);
        if(result) {
            Debug.Log("added");
        }
        else
            Debug.Log("not added");
    }
}
