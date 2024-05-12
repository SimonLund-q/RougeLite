using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    // It säkerhet är bra

    private float fireRate;
    private float bullets;
    private float spread;
    private float speed;
    private int damage;

    public Gun(float fireRate, float bullets, float spread, float speed, int damage)
    {
        this.fireRate = fireRate;
        this.bullets = bullets;
        this.spread = spread;
        this.speed = speed;
        this.damage = damage;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public float GetBullets()
    {
        return bullets;
    }

    public float GetSpread()
    {
        return spread;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public int GetDamage()
    {
        return damage;
    }

}
