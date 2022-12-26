using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health;
    public int maxHealth;
    [SerializeField] private AudioClip explosionSound, collisionSound;
    private bool audioPlaying;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    [SerializeField] private RectTransform healthSliderTransform;
    [SerializeField] private float offset;
    [SerializeField] private float damageMult;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        UpdateHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        healthSliderTransform.position = new Vector2(transform.position.x, transform.position.y + offset);
    }

    public void UpdateHealth(int hlth)
    {
        health = hlth;
        if (health <= 0)
        {
            StartCoroutine("Die");
        }
        healthSlider.value = health;
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Crashable")
        {
            GetComponent<AudioSource>().clip = collisionSound;
            GetComponent<AudioSource>().Play();
            UpdateHealth(Mathf.RoundToInt(health - damageMult * rb.velocity.magnitude));
        }
        else if (collision.collider.tag == "Gas")
        {
            GetComponent<MovePlayer>().gas = GetComponent<MovePlayer>().maxGas;
        }
    }

    IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(explosionParticles, GetComponent<Transform>().position, Quaternion.identity, GetComponent<Transform>());
        if (!audioPlaying)
        {
            GetComponent<AudioSource>().clip = explosionSound;
            GetComponent<AudioSource>().Play();
            audioPlaying = true;
        }
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
