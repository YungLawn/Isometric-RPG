using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    private Collider2D collider;
    [SerializeField]
    private ContactFilter2D filter;
    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

    // Start is called before the first frame update
    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        collider.OverlapCollider(filter, collidedObjects);
        foreach(var o in collidedObjects) {
            Debug.Log(o.name);
        }
    }
}
