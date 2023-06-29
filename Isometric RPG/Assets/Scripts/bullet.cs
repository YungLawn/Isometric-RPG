using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    const float speed = 10f;

    private Vector3 direction;

    public void Shoot(Vector3 shootDirection) {
        direction = shootDirection;
    }

    void Update() {
        transform.position += direction * speed * Time.deltaTime;
    }
}
