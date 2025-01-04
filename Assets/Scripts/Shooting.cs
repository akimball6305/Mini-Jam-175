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
    [SerializeField] GameObject Player;

    public bool IsShooting => playerAnimator.GetBool("isShooting");
    public bool IsReloading => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("isReloading");


    private void Start()
    {
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
        playerAnimator.SetTrigger("ShootSingle");

        PerformRaycast();
    }

    void ShootContinuous()
    {
        if (currentammo <= 0 || isReloading) return;

        currentammo--;

        playerAnimator.SetBool("isShooting", true);

        PerformRaycast();
    }

    void StopShooting()
    {
        playerAnimator.SetBool("isShooting", false);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        StopShooting(); // Ensure shooting stops during reload

        ResetAnimationTriggers();
        playerAnimator.SetTrigger("isReloading");

        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        currentammo = maxammo;
        isReloading = false;

        // Reset Animator after reload
        ResetAnimationTriggers();
        playerAnimator.ResetTrigger("isReloading");
        Debug.Log("Reload Complete");
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
