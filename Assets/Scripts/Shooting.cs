using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{

    public float damage = 10f;
    public Camera fpsCam;

    public int maxammo = 3;
    private int currentammo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    private void Start()
    {
        currentammo = maxammo;
    }
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentammo <= 0f)
        {
           StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        currentammo--;

        RaycastHit hit;
       if( Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) 
            {
                target.TakeDamage(damage);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime);
        currentammo = maxammo;
        isReloading = false;
    }

}
