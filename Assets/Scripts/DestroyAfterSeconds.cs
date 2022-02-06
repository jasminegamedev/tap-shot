using System.Collections;
using UnityEngine;

// Generic component to destroy a Game Object after a given amount of seconds.
public class DestroyAfterSeconds : MonoBehaviour
{
    [Tooltip("How many seconds until this Game Object is destroyed.")]
    public float LifeTime = 3;

    void Start()
    {
        StartCoroutine("Kill");
    }

    // Destroy the game object after a given amount of seconds
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}
