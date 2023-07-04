using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    void Awake() {
        Destroy(gameObject, 3f);
    }
    void OnCollisionEnter2D(Collision2D collision) {
        // Debug.Log("Hit");
        Destroy(gameObject);
    }
}
