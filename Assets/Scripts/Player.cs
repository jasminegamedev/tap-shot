using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Main Player Controller component
public class Player : MonoBehaviour
{
    [Tooltip("Prefab of bullet to spawn on shoot.")]
    public GameObject bulletPrefab;
    [Tooltip("Prefab of particles to spawn on death.")]
    public GameObject deathParticlesPrefab;

    [Tooltip("Reference to flame effect game object.")]
    public GameObject flame;

    [Tooltip("Sound Effect to play on Jump.")]
    public AudioClip JumpSound;

    // Main Sound Effects AudioSource reference
    protected AudioSource aud;
    // Rigidbody Reference
    protected Rigidbody2D rbody;

    // Cache components
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        aud = GetComponent<AudioSource>();
    }

    // Launch ship up on touch/click
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && !GameManager.Instance.isPaused && !IsPointerOverUIObject()))
        {
            if (!GameManager.Instance.isStarted)
            {
                GameManager.Instance.isStarted = true;
                rbody.gravityScale = 1;
                GameManager.Instance.PauseButton.SetActive(true);
            }
            rbody.AddForce(new Vector2(0, 260));
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.1f), transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.rotation * Vector2.right * 750);
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 260));
            bullet.GetComponent<Bullet>().source = gameObject;

            StopCoroutine("ShowFlame");
            StartCoroutine("ShowFlame");
            if (!GameManager.Instance.isSoundMuted)
            {
                aud.pitch = Random.Range(0.5f, 1.2f);
                aud.PlayOneShot(JumpSound);
            }
        }

        if (!GameManager.Instance.isStarted)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.timeSinceLevelLoad) * 0.5f);
        }
    }

    // Check if mouse cursor is currently over a UI object
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count(r => r.gameObject.GetComponent<Button>() != null) > 0;
    }

    // Set player's velocity and rotation
    void FixedUpdate()
    {
        rbody.velocity = new Vector2(0, Mathf.Clamp(rbody.velocity.y, -100, 100));
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.AngleAxis(90, Vector3.forward) * (new Vector2(10, 0) + rbody.velocity));
    }

    // Show then hide flame trail
    IEnumerator ShowFlame()
    {
        flame.SetActive(true);
        yield return new WaitForSeconds(1);
        flame.SetActive(false);
    }

    // Spawn Death Particles, and destroy player gameobject. Call Game Manager death event.
    public void Kill()
    {
        var deathParticle = Instantiate(deathParticlesPrefab, transform.position, transform.rotation);
        if (GameManager.Instance.isSoundMuted)
        {
            deathParticle.GetComponent<AudioSource>().volume = 0;
        }
        GameManager.Instance.OnDeath();
        Destroy(gameObject);
    }

    // Kill the player if they run into an enemy bullet.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null && bullet.source!= null && bullet.source.GetComponent<EnemyShip>() != null)
        {
            Kill();
            Destroy(bullet.gameObject);
        }
    }
}