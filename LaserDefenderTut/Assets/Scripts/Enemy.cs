
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 2f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathVolume = 0.5f;
    [SerializeField] AudioClip laserSound;
    [SerializeField] float laserVolume = 0.5f;
    [SerializeField] int score = 100;
    [SerializeField] float projectileSpeed = 10.0f;

    float shotCounter;
    GameSession gs;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        gs = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        
        if(shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }

        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathVolume);
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 0.5f);
        gs.AddToScore(score);
        Destroy(gameObject);
    }
}
