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
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Input.GetAxisRaw("Vertical") == 1 && Input.GetAxisRaw("Horizontal") == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && Input.GetAxisRaw("Horizontal") == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        else if (Input.GetAxisRaw("Vertical") == 1 && Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -45);
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -135);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        GameObject firedBullet = Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
        firedBullet.layer = 11;

        if (animator)
        {
            animator.SetTrigger("Shoot");
        }
    }
}
