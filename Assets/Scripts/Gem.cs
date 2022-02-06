using UnityEngine;

// Component representing a gem collectible
public class Gem : MonoBehaviour
{
    [Tooltip("Score to give the player on collect.")]
    public int Score = 3;
    [Tooltip("Sound Effect to play on collect.")]
    public AudioClip gemSound;
 
    // Destroy this game object and give points if the player touches this object.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ply = collision.gameObject.GetComponent<Player>();
        if (ply != null)
        {
            GameManager.Instance.AddScore(Score);
            var source = ply.gameObject.GetComponent<AudioSource>();
            if (!GameManager.Instance.isSoundMuted)
            {
                source.pitch = Random.Range(0.5f, 1.5f);
                source.PlayOneShot(gemSound);
            }
            Destroy(gameObject);
        }
    }
}
