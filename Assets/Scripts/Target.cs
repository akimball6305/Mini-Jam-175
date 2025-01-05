using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] ParticleSystem BloodSplash;  // Assign the prefab in Inspector

    ScoreKeeper scoreKeeper;

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
            // Instantiate at object position with slight upward offset
            Vector3 spawnOffset = Vector3.up * 1f;
            ParticleSystem splash = Instantiate(BloodSplash, transform.position + spawnOffset, Quaternion.identity);

            // Force the particle system to emit immediately
            splash.transform.localScale = Vector3.one * 2;  // Scale up effect if necessary
            splash.Play();
            splash.Emit(100);  // Forces instant emission of 10 particles
            Debug.Log("Blood effect instantiated and emitting.");

            // Destroy the particle system after its duration
            Destroy(splash.gameObject, splash.main.duration);
        }
        else
        {
            Debug.LogWarning("BloodSplash particle system not assigned!");
        }

        if (ScoreKeeper.instance != null)
        {
            ScoreKeeper.instance.AddScore(100);
        }

        // Destroy the target object
        Destroy(gameObject);
    }
}
