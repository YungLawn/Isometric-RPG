using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<string> Inventory = new List<string>();
    private Transform player;

    void Start() {
        player = transform.parent;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "ItemPickup") {
            Inventory.Add(collider.gameObject.name);
            Destroy(collider.gameObject);
            // foreach(KeyValuePair<string, GameObject> items in Inventory)
            // {
            //     print(items.Value + " " + items.Key);
            // }
        }
    }
}
