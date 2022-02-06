using UnityEngine;

// Generic component to move a game obect by an offset every frame.
public class ConstantMovement : MonoBehaviour
{
    [Tooltip("How much we want to move this object by every frame.")]
    public Vector3 movement = new Vector3(-2, 0);

    // Move this object by offset every frame
    void Update()
    {
        transform.position += movement * Time.deltaTime;
    }
}
