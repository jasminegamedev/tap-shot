using System.Collections;
using UnityEngine;

// Component that represents an enemy spaceship
public class EnemyShip : MonoBehaviour
{
    [Tooltip("How fast te enemy should move.")]
    public float speed = 1;
    [Tooltip("How fast the enemy should rotate.")]
    public float rotspeed = 10;

    [Tooltip("How often we want to shoot.")]
    public float shootInterval = 2;
    [Tooltip("How fast the bullet we spawn should move.")]
    public float shootSpeed = -750;
    [Tooltip("GameObject to spawn on Shoot.")]
    public GameObject bulletPrefab;

    // How long the enemy has been alive.
    private float life;

    void Start()
    {
        StartCoroutine("Shoot");
        life = Random.Range(0.0f, 10.0f);
    }

    void Update()
    {
        life += Time.deltaTime;
        if (rotspeed > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(life) * rotspeed);
        }
        transform.position += Vector3.right * -Time.deltaTime * speed;
    }

    // Spawn a bullet, and set it's values
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(shootInterval);
        if (!GameManager.Instance.isGameOver)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.right * shootSpeed / Mathf.Abs(shootSpeed), transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * shootSpeed);
            bullet.GetComponent<Bullet>().source = gameObject;
        }
        StartCoroutine("Shoot");
    }
}
