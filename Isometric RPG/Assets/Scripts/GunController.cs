using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Transform Gun;
    private Transform shootPointRifle;
    private Transform shootPointPistol;

    public GameObject bulletPF;
    public GameObject muzzleFlashPF;

    public float bulletForce = 20f;

    public Transform Crosshair;

    public Sprite[] Rifles;
    private Dictionary<string, Sprite> riflesDic = new Dictionary<string, Sprite>();

    public Sprite[] Pistols;
    private Dictionary<string, Sprite> pistolsDic = new Dictionary<string, Sprite>();

    void Start()
    {
        Gun = GetComponent<Transform>();
        shootPointPistol = transform.Find("ShootPointPistol");
        shootPointRifle = transform.Find("ShootPointRifle");

        foreach(Sprite sprite in Pistols) {
            pistolsDic.Add(sprite.name, sprite);
        }
        foreach(Sprite sprite in Rifles) {
            riflesDic.Add(sprite.name, sprite);
        }
        Gun.GetComponent<SpriteRenderer>().sprite = pistolsDic["HandGun"];
    }

    public void handleGun(bool weaponDrawn, float lookAngle, Vector2 lookInput) {
        Gun.GetComponent<SpriteRenderer>().enabled = weaponDrawn;
        Gun.eulerAngles = new Vector3(0,0,lookAngle);
        Gun.GetComponent<SpriteRenderer>().flipY = lookInput.x < 0 ? true : false;
        Gun.GetComponent<SpriteRenderer>().sortingOrder = lookInput.y > 0 ? 0 : 1;
    }

    public void Shoot() {
        GameObject muzzleFlash = Instantiate(muzzleFlashPF, shootPointPistol.position, Gun.rotation);
        Destroy(muzzleFlash, 0.025f);
        GameObject bullet = Instantiate(bulletPF, shootPointPistol.position, Gun.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootPointPistol.right * bulletForce, ForceMode2D.Impulse);
    }
}
