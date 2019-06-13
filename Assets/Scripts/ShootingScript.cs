using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject barrel;
    private SpriteRenderer gunSprite;

    private Animator animator;
    private PlayerSounds playerSounds;
    private PlayerInput playerInput;

    private float timeElapsed;
    public float gunShowtime = 0.7f;

    public float rateOfFire = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        gunSprite = GetComponentInChildren<SpriteRenderer>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerSounds = GetComponentInParent<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > gunShowtime)
        {
            gunSprite.enabled = false;
        }
        if (Input.GetButton("Fire1"))
        {
            if (timeElapsed > rateOfFire)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -45);
        }
        else if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -135);
        }
        else if (transform.parent.localScale.x == 1 && Input.GetAxisRaw("Vertical") == 0 && (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") == 0))
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        else if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 135);
        }
        else if (transform.parent.localScale.x == -1 && Input.GetAxisRaw("Vertical") == 0 && (Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") == 0))
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        GameObject firedBullet = Instantiate(bulletPrefab, barrel.transform.position, gameObject.transform.rotation);
        firedBullet.layer = 11;

        if (animator)
        {
            gunSprite.enabled = true;
            animator.SetTrigger("Shoot");
            timeElapsed = 0;
        }

        if (playerSounds)
        {
            playerSounds.PlayShoot();
        }
    }
}
