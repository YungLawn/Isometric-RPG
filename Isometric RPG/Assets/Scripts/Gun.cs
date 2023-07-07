using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Transform GunPosition;
    private Transform shootPointRifle;
    private Transform shootPointPistol;

    // Start is called before the first frame update
    void Start()
    {
        GunPosition = GetComponent<Transform>();
        shootPointPistol = transform.Find("ShootPointPistol");
        shootPointRifle = transform.Find("ShootPointRifle");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void handleGun(bool weaponDrawn, float lookAngle, Vector2 lookInput) {
        GunPosition.GetComponent<SpriteRenderer>().enabled = weaponDrawn;
        GunPosition.eulerAngles = new Vector3(0,0,lookAngle);
        GunPosition.GetComponent<SpriteRenderer>().flipY = lookInput.x < 0 ? true : false;
        GunPosition.GetComponent<SpriteRenderer>().sortingOrder = lookInput.y > 0 ? 0 : 1;
    }
}
