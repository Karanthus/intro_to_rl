using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall")) // Assuming the walls have the "Wall" tag
        {
            Destroy(this.gameObject); // Destroy the bullet upon collision with the wall
        }
    }
}
