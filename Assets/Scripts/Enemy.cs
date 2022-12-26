using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private AudioClip explosionSound, collisionSound;
    [SerializeField] private float teleportBackDistance;
    private bool audioPlaying;
    [SerializeField] private Transform arrow;
    [SerializeField] private LevelManager levelManager;
    public float health;
    public float maxHealth;
    [SerializeField] private float regeneration;
    [SerializeField] private SpriteRenderer goalIcon, fuelIcon;
    private Rigidbody2D rb;
    [SerializeField] private Transform playerTransform;
    public float moveSpeed, maxSpeed;
    [SerializeField] private float maxITime;
    private float iTime;

    [Header("Healthbar")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    [SerializeField] private RectTransform healthSliderTransform;
    [SerializeField] private float offset;
    [SerializeField] private float damage = 30;
    [SerializeField] private Transform goal;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        health = maxHealth;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerTransform.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        RemoveHealth(Mathf.RoundToInt(-regeneration * Time.deltaTime));

        Vector2 moveDir = (playerTransform.position - GetComponent<Transform>().position);
        if (moveDir.magnitude > teleportBackDistance) GetComponent<Transform>().position = playerTransform.position;
        else
        {
            rb.AddForce(moveDir.normalized * moveSpeed);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        Vector2 toGoal = goal.position - GetComponent<Transform>().position;
        float arrowAngle = (Mathf.Atan2(toGoal.y, toGoal.x) * Mathf.Rad2Deg) - 90; //subtract 90 because arrow pointing up instead of right
        arrow.rotation = Quaternion.Euler(0, 0, arrowAngle);


        if (iTime > 0)
        {
            iTime -= Time.deltaTime;
        }

        healthSliderTransform.position = new Vector2(transform.position.x, transform.position.y + offset);
        healthSlider.value = health;
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Crashable" && iTime <= 0)
        {
            GetComponent<AudioSource>().clip = collisionSound;
            GetComponent<AudioSource>().Play();
            RemoveHealth(Mathf.RoundToInt(damage * rb.velocity.magnitude));
            iTime = maxITime;
        }
    }

    void RemoveHealth(int amnt)
    {
        health -= amnt;
        if (health <= 0)
        {
            StartCoroutine("Die");
        }
        else if (health > maxHealth) health = maxHealth;
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
        levelManager.YouDead();
    }

    public void UpdateGoal(Transform Goal, bool isGoal)
    {
        if (isGoal)
        {
            goalIcon.enabled = true;
            fuelIcon.enabled = false;
        } else
        {
            fuelIcon.enabled = true;
            goalIcon.enabled = false;
        }
        goal = Goal;
    }
}
