using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject resetTip;
    private bool resetTipShown = false;

    private bool space;
    [SerializeField] public float maxGas;
    public float gas;
    private GameObject rt;

    [SerializeField] private AudioSource thrusterAudio;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject lThrust, rThrust;
    [SerializeField] private float moveSpeed, maxSpeed;
    [SerializeField] private Sprite idleSprite, activeSprite;
    private AudioSource source;
    private bool onLandingSpot, onSpawn;

    [Header("Landing")]
    [SerializeField] private Transform landingSpot;
    [SerializeField] private float landingRadius;
    [SerializeField] private LayerMask whatIsLandable;
    [SerializeField] private LayerMask spawnLayer;
    // Start is called before the first frame update
    void Start()
    {
        gas = maxGas;
        source = thrusterAudio;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        onLandingSpot = Physics2D.OverlapCircle(landingSpot.position, landingRadius, whatIsLandable);
        onSpawn = Physics2D.OverlapCircle(landingSpot.position, landingRadius, spawnLayer);
        GetInput();
        PointToMouse();
        if (onLandingSpot) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (gas <= 0 && !resetTipShown) {
            resetTipShown = true;
            showResetTip();
        } else if (gas > 0 && resetTipShown){
            resetTipShown = false;
            Destroy(rt);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        space = Input.GetKey(KeyCode.Space);
    }

    void Move()
    {
        if (space && gas > 0)
        {
            gas -= Time.deltaTime;
            GetComponent<SpriteRenderer>().sprite = activeSprite;
            lThrust.SetActive(true);
            rThrust.SetActive(true);
            rb.AddForce((transform.up).normalized * moveSpeed, ForceMode2D.Force);
            source.volume = 0.2f;
        }
        else
        {
            lThrust.SetActive(false);
            rThrust.SetActive(false);
            GetComponent<SpriteRenderer>().sprite = idleSprite;
            source.volume = 0;
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void showResetTip()
    {
        rt = Instantiate(resetTip, GetComponent<Transform>().position + Vector3.up * 3, Quaternion.identity, GetComponent<Transform>());
    }

    void PointToMouse()
    {
        if (!onSpawn)
        {
            Vector2 pointVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GetComponent<Transform>().position;
            float angle = Mathf.Atan2(pointVector.y, pointVector.x) * Mathf.Rad2Deg;
            GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
    }

    public void SetGas(float amnt)
    {
        gas = amnt;
        if (gas > maxGas) gas = maxGas;
    }
}
