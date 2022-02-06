using UnityEngine;

// Generic component to rotate a game obect every frame.
public class ConstantRotator : MonoBehaviour
{
    [Tooltip("Speed to rotate the game object.")]
    public float rotSpeed = 30;

    // Rotate this object every frame
    void Update()
    {
        transform.Rotate(transform.forward, rotSpeed * Time.deltaTime);
    }
}
