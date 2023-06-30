using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCrosshair : MonoBehaviour
{
    private Transform Crosshair;
    private Vector3 crosshairOffset = new Vector3(0, 0.2f, 10f);
    private bool weaponDrawn;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Crosshair = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        weaponDrawn = player.GetComponent<PlayerController>().weaponDrawn;
        Cursor.visible = !weaponDrawn;
        Crosshair.GetComponent<SpriteRenderer>().enabled = weaponDrawn;
        Crosshair.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + crosshairOffset;
    }

}
