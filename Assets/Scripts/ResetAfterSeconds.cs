using System.Collections;
using UnityEngine;

// Component that resets a gameobject's position to it's starting position after a given number of seconds.
public class ResetAfterSeconds : MonoBehaviour
{
    [Tooltip("How many seconds until the GameObject resets to its starting position.")]
    public float seconds;

    // Starting position
    private Vector3 startingLocation;

    void Start()
    {
        startingLocation = transform.position;
        StartCoroutine("Reset");
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(seconds);
        transform.position = startingLocation;
        StartCoroutine("Reset");
    }
}
