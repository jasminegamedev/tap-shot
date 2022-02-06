using UnityEngine;

// Player Subclass that represents an alternate UFO player
public class PlayerUFO : Player
{
    // Move the player upwards as long as the screen is being touched.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && !GameManager.Instance.isPaused && !IsPointerOverUIObject()))
        {
            if (!GameManager.Instance.isStarted)
            {
                GameManager.Instance.isStarted = true;
                rbody.gravityScale = 0.6f;
                GameManager.Instance.PauseButton.SetActive(true);
            }
            rbody.AddForce(new Vector2(0, 7));
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.1f), transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.rotation * Vector2.right * 750);
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 7));
            bullet.GetComponent<Bullet>().source = gameObject;
            rbody.velocity = new Vector2(0, Mathf.Clamp(rbody.velocity.y, -30, 30));
            if (!GameManager.Instance.isSoundMuted)
            {
                aud.pitch = Random.Range(0.5f, 1.2f);
                aud.PlayOneShot(JumpSound);
            }
        }
        else if (Input.GetKey(KeyCode.Space) || (Input.GetMouseButton(0) && !GameManager.Instance.isPaused && !IsPointerOverUIObject()))
        {
            rbody.AddForce(new Vector2(0, 10 * Time.deltaTime * 60));
        }

        if (!GameManager.Instance.isStarted)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.timeSinceLevelLoad) * 0.5f);
        }
    }

    // Set the players Velocity and rotation
    void FixedUpdate()
    {
        rbody.velocity = new Vector2(0, Mathf.Clamp(rbody.velocity.y, -30, 30));
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.AngleAxis(90, Vector3.forward) * (new Vector2(10, 0) + rbody.velocity));
    }
}
