using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] ParticleSystem BloodSplash;

    public float health = 10f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            die();
        }
    }

    void die()
    {
        if (BloodSplash != null)
        {
            ParticleSystem splash = Instantiate(BloodSplash, transform.position, Quaternion.identity);      //plays bloodsplash when target dies
            splash.Play();
            Destroy(splash.gameObject, splash.main.duration); // Destroy after particle finishes
        }
        Destroy(gameObject);
    }
}
