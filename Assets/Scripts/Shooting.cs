using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public float damage = 10f;
    public Camera fpsCam;

    public int maxammo = 3;
    private int currentammo;
    public float reloadTime;
    private bool isReloading = false;

    public float fireRate; // Automatically set using GetAnimationLength

    private float nextFireTime = 0f;

    Animator playerAnimator;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject Player;
    AudioSource gunShot;
    [SerializeField] AudioSource reload;

    public bool IsShooting => playerAnimator.GetBool("isShooting");
    public bool IsReloading => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("isReloading");

    [Header("Audio Fade Settings")]
    public float fadeOutDuration = 0.3f;
    private Coroutine fadeOutCoroutine;


    private void Start()
    {
        gunShot = GetComponent<AudioSource>();
        currentammo = maxammo;
        playerAnimator = Player.GetComponent<Animator>();

        fireRate = GetAnimationLength("Armature|Arms_FPS_Anim_Shoot");
        Debug.Log("Fire Rate set to: " + fireRate);

        reloadTime = GetAnimationLength("Armature|Arms_FPS_Anim_Reload_Fast");
        Debug.Log("Reload Time set to: " + reloadTime);
    }

    void Update()
    {
        if (isReloading) return;

        if (currentammo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Single Shot
        if (Input.GetButtonDown("Fire1"))
        {
            ShootSingle();
        }

        // Continuous Shooting
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootContinuous();
        }

        // Stop continuous shooting animation when button is released
        if (Input.GetButtonUp("Fire1"))
        {
            StopShooting();
        }
    }

    void ShootSingle()
    {
        if (currentammo <= 0 || isReloading) return;

        currentammo--;

        ResetAnimationTriggers();
        reload.Stop();
        playerAnimator.SetTrigger("ShootSingle");
        muzzleFlash.Play();
        gunShot.Play();
        PerformRaycast();
    }

    void ShootContinuous()
    {
        if (currentammo <= 0 || isReloading) return;

        currentammo--;

        playerAnimator.SetBool("isShooting", true);
        reload.Stop();
        muzzleFlash.Play();
        gunShot.Play();
        PerformRaycast();
    }

    void StopShooting()
    {
        playerAnimator.SetBool("isShooting", false);
        muzzleFlash.Stop();

        // Start fading out the audio instead of stopping abruptly
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutAudio());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        StopShooting(); // Ensure shooting stops during reload

        ResetAnimationTriggers();
        playerAnimator.SetTrigger("isReloading");
        reload.Play();
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        currentammo = maxammo;
        isReloading = false;

        // Reset Animator after reload
        ResetAnimationTriggers();
        playerAnimator.ResetTrigger("isReloading");
        Debug.Log("Reload Complete");
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = gunShot.volume;

        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            gunShot.volume = Mathf.Lerp(startVolume, 0, t / fadeOutDuration);
            yield return null;
        }

        gunShot.volume = 0;
        gunShot.Stop();
        gunShot.volume = startVolume; // Reset the volume for the next play
    }

    private void ResetAnimationTriggers()
    {
        playerAnimator.ResetTrigger("ShootSingle");
        playerAnimator.SetBool("isShooting", false);
    }

    private void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    private float GetAnimationLength(string animationName)
    {
        RuntimeAnimatorController controller = playerAnimator.runtimeAnimatorController;
        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning("Animation " + animationName + " not found!");
        return 0.5f;
    }
}
