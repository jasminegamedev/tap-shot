using UnityEngine;

// Generic Component that will kill the player if the player enters this object's trigger
public class KillPlayerOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ply = collision.gameObject.GetComponent<Player>();
        if (ply != null)
        {
            ply.Kill();
        }
    }
}
