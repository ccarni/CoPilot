using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine;

public class BounceAroundScreen : MonoBehaviour
{
    Vector2 v = Vector2.zero;
    [SerializeField] private float SPEED;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("hasWon") == 1) { GetComponent<SpriteRenderer>().color = new Color(189f, 134f, 255f); }

        GetComponent<Transform>().position = new Vector2(Random.Range(-10.1f, 10.1f), Random.Range(-4.4f, 4.4f));
        while (Mathf.Approximately(v.y, 0) || Mathf.Approximately(v.x, 0)) v = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * SPEED;
    }

    private void FixedUpdate()
    {
        CheckBounds();
        GetComponent<Transform>().position += new Vector3(v.x, v.y, 0) * Time.deltaTime;
    }

    private void CheckBounds()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(GetComponent<Transform>().position);
        Vector2 scale = new Vector2(50, 50);
        if (screenPos.x + scale.x / 2> Screen.width) v = Vector2.Reflect(v, Vector2.left);
        if (screenPos.y - scale.y / 2 < 0) v = Vector2.Reflect(v, Vector2.up);
        if (screenPos.x - scale.x / 2 < 0 ) v = Vector2.Reflect(v, Vector2.right);
        if (screenPos.y + scale.y / 2> Screen.height ) v = Vector2.Reflect(v, Vector2.down);

    }
}
