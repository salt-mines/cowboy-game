using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip hurtSound;
    public AudioClip deathSound;
    public AudioClip shootSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurt()
    {
        audioSource.PlayOneShot(hurtSound);
    }

    public void PlayDeath()
    {
        audioSource.PlayOneShot(deathSound);
    }

    public void PlayShoot()
    {
        audioSource.PlayOneShot(shootSound);
    }
}
