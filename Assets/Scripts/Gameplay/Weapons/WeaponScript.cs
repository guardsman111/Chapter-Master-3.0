using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem bullet;

    public void Fire()
    {
        muzzleFlash.Play();
        int rand = Random.Range(0, 100);

        if(rand > 20)
        {
            bullet.Play();
        }
    }
}
