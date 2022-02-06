using System.Collections;
using UnityEngine;

// Component representing a bullet that will destroy itself after the given lifetime.
public class Bullet : MonoBehaviour
{
    [Tooltip("How many seconds until this bullet is destroyed.")]
    public float LifeTime = 3;
    [Tooltip("GameObject that spawned this bullet.")]
    public GameObject source;

    void Start()
    {
        StartCoroutine("Kill");
    }

    // Destroy self after lifetime since spawn.
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }

    // Destroy self if it runs into an object.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null && collision.gameObject != source)
        {
            Destroy(gameObject);
        }
    }
}
