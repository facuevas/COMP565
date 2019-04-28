using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType
    {
        Semi,
        Burst,
        Auto
    }

    //Handler variables
    public GunType gunType;
    public Transform projectile;
    public float rpm;
    public int damage;
    public float shotDistance;

    //System variables
    private float secondsBetweenShots;
    private float nextPossibleShootTime;
    private AudioSource gunSound;

    //Components
    private LineRenderer tracer;

    void Start()
    {
        secondsBetweenShots = 60 / rpm;
        gunSound = GetComponent<AudioSource>();
        if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();
        }
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            float shotEnd = shotDistance;
            Ray ray = new Ray(projectile.position, projectile.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, shotEnd))
            {
                shotEnd = hit.distance;
                Debug.Log(hit.transform.name);
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<EnemyHealth>().HurtEnemy(damage);
                }
            }


            nextPossibleShootTime = Time.time + secondsBetweenShots;
            gunSound.Play();

            if (tracer)
            {
                StartCoroutine("RenderTracer", ray.direction * shotEnd);
            }

            Debug.DrawRay(ray.origin, ray.direction * shotEnd, Color.red, 1);
        }
    }

    public void ShootContinuous()
    {
        if (gunType == GunType.Auto)
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        bool canShoot = true;

        if (Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }

        return canShoot;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, projectile.position);
        tracer.SetPosition(1, projectile.position + hitPoint);
        yield return null;
        tracer.enabled = false;
    }
}
