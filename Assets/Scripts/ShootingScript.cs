using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public GameObject bulletPrefab;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject firedBullet = Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
        firedBullet.layer = 11;

        if (animator)
        {
            animator.SetTrigger("Shoot");
        }
    }
}
