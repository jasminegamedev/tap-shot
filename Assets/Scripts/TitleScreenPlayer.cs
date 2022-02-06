using UnityEngine;

// Game Object to move and rotate the ship on the title screen.
public class TitleScreenPlayer : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.timeSinceLevelLoad) * 0.5f);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.timeSinceLevelLoad * 0.25f) * 45);
    }
}
