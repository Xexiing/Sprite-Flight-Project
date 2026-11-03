using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float maxSpinSpeed = 50f;
    public float minSize = 1f;
    public float maxSize = 3f;
    public float minSpeed = 250f;
    public float maxSpeed = 500f;
    public GameObject bounceEffectPrefab;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        
        float randomSpeed = Random.Range(minSpeed, maxSpeed)/randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
        {
            // 1. Get the contact point and spawn the bounce effect there
            Vector2 contactPoint = collision.GetContact(0).point;
            GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);
            Destroy(bounceEffect, 1f);
        }
}
