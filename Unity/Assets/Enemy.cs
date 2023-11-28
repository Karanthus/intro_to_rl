using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;

public class Enemy : MonoBehaviour
{
    public PlayerAgent playerAgent;
    public GameObject enemyProjectile; // Reference to the enemy projectile prefab
    public float minInterval = 1f; // Minimum time between shots
    public float maxInterval = 3f; // Maximum time between shots
    public float projectileSpeed = 10f; // Initial speed of the projectile
    public float projectileLifetime = 5f; // Time before the projectile disappears

    

    IEnumerator StartShooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Get the forward direction of the enemy
            Vector3 forwardDirection = transform.forward;

            // Calculate the spawn position in the forward direction
            Vector3 spawnPosition = transform.position + forwardDirection * 1.0f;

            // Instantiate the enemy's projectile at the calculated position and facing forward
            GameObject newProjectile = Instantiate(enemyProjectile, spawnPosition, Quaternion.LookRotation(forwardDirection));

            playerAgent.RegisterBullet(newProjectile);

            // Access the projectile's Rigidbody
            Rigidbody projectileRb = newProjectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                // Give the projectile an initial velocity in the forward direction
                projectileRb.velocity = forwardDirection * projectileSpeed;

                // Destroy the projectile after projectileLifetime seconds
                Destroy(newProjectile, projectileLifetime);
            }
        }
    }

    void Start()
    {
        StartCoroutine(StartShooting());
    }
    

    
    
}
