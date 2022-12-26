using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private bool mouse1, mouse1down;
    private RaycastHit2D shotHit;

    [SerializeField] public float range;
    [SerializeField] private LayerMask whatCanShoot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Pew();
    }

    void GetInput()
    {
        mouse1 = Input.GetMouseButton(0);
        mouse1down = Input.GetMouseButtonDown(0); ;
    }

    void Pew()
    {
        if (mouse1down)
        {
            shotHit = (Physics2D.Raycast(GetComponent<Transform>().position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - GetComponent<Transform>().position, range, whatCanShoot));
            if (shotHit)
            {
                Debug.Log("hit");
            }
        }
    }
}
