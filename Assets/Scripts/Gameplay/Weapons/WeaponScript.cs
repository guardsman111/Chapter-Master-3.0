using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private List<ParticleSystem> muzzleFlash;
    [SerializeField] private ParticleSystem bullet;

    public void Fire()
    {
        if (bullet != null)
        {
            foreach (ParticleSystem system in muzzleFlash)
            {
                system.Play();
            }
            int rand = Random.Range(0, 100);

            if (rand > 20)
            {
                bullet.Play();
            }
        }
    }
}
