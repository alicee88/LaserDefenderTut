using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float shipSpeed = 10f;
    [SerializeField] int health = 200;
    [SerializeField] float padding = 0.5f;
    [SerializeField] GameObject laserPrefab;

    float minX;
    float maxX;
    float minY;
    float maxY;
    float projectileSpeed = 20f;
    float projectileFiringPeriod = 0.2f;
    Coroutine firingCoroutine;

    void Start()
    {
        SetMoveBoundaries();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * shipSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * shipSpeed;

        var newXPos = transform.position.x + deltaX;
        var newYPos = transform.position.y + deltaY;

        newXPos = Mathf.Clamp(newXPos, minX, maxX);
        newYPos = Mathf.Clamp(newYPos, minY, maxY);


        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (damageDealer)
        {
            health -= damageDealer.GetDamage();
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
