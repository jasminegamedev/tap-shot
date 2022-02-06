using UnityEngine;

// Generic Component that will be destroyed if it is shot with enough bullets.
public class Breakable : MonoBehaviour
{
    [Tooltip("How many times this can get hit before being destroyed.")]
    public int health;
    [Tooltip("What to scale this too after it is hit.")]
    public float resizeOnHit = 1;
    [Tooltip("How many points to give after this is destroyed.")]
    public int score = 1;

    [Tooltip("Particle effect to spawn on Destroy.")]
    public GameObject DestroyParticle;

    // Remove health if this is hit by a bullet. Destroy if health is less than or equal to zero.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null && bullet.source != null && bullet.source.GetComponent<Player>() != null)
            {
                gameObject.transform.localScale *= resizeOnHit;
                health--;
                if (health <= 0)
                {
                    GameManager.Instance.AddScore(score);

                    if (DestroyParticle != null)
                    {
                        var deathParticle = Instantiate(DestroyParticle, transform.position, transform.rotation);
                        if (GameManager.Instance.isSoundMuted)
                        {
                            deathParticle.GetComponent<AudioSource>().volume = 0;
                        }
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
