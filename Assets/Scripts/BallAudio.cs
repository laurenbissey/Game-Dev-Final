using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAudio : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] golfSwings;
    [SerializeField] private AudioClip[] waterDrops;
    [SerializeField] private AudioClip[] bounces;

    private float previousWaterDropTimer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        previousWaterDropTimer += Time.deltaTime;
    }

    public void BallHit(float impact)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySFX(golfSwings[Random.Range(0, golfSwings.Length)], 
                .5f * Mathf.Clamp01(Mathf.Log10(Mathf.Abs(impact) + 1)));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        float impact = Vector2.Dot(rb.velocity, normal);

        if (impact >= .5f)
        {
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(bounces[Random.Range(0, bounces.Length)], 
                    .5f * Mathf.Clamp01(Mathf.Log10(Mathf.Abs(impact) + 1)));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && previousWaterDropTimer >= 1.0f)
        {
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(waterDrops[Random.Range(0, waterDrops.Length)], .125f);

            previousWaterDropTimer = 0.0f;
        }
    }
}
