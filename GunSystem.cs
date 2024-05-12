using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public Gun gun;
    public Transform firePoint;

    protected bool readyToFire = true;
    protected Color dColor;
    public GameObject bullet;
    float timeToShoot;

    //      public Gun(float fireRate, float bullets, float spread, float speed, int damage)
    //      MyScript script = new Script();
    //      MyScript script = obj.AddComponent<MyScript>();

    public Gun pistol = new Gun(1f, 100f, 0.15f, 9f, 9);
    public Gun revolver = new Gun(1f, 60f, 0, 10f, 15);

    public Gun shotgun = new Gun(1f, 6, 0.25f, 14f, 1);
    public Gun sawedoffShotgun = new Gun(1f, 8, 0.5f, 14f, 1);

    public static List<Gun> guns;

    private void Awake()
    {
        gun = gameObject.GetComponent<Gun>();

        guns = new List<Gun>();
        guns.Add(pistol); 
        guns.Add(revolver);
        guns.Add(shotgun);
        guns.Add(sawedoffShotgun); 
    
    }

    public Gun GetGun(int n)
    {
        return guns[n];
    }




    public void Fire(GameObject firePoint)
    {
        bool readyToShoot = false;
        if (readyToShoot == false)
        {
            timeToShoot = gun.GetFireRate();
            readyToShoot = true;
            Debug.Log(timeToShoot);
        }
        timeToShoot = timeToShoot - Time.deltaTime;

        if (timeToShoot <= 0f)
        {
            Instantiate(bullet, firePoint.transform.position, transform.rotation);
            readyToShoot = false;
        }
    }
}
