using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    private void Update()
    {
        GetComponent<Transform>().position += Vector3.up * scrollSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetFloat("hasWon", 1);
        SceneManager.LoadScene(0);
    }
}
