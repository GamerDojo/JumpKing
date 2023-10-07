using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    public bool isGrounded;
    public GameObject groundcheck;
    public LayerMask groundLayer;
    void Start()
    {
        
    }
    void Update()
    {
        var hit = Physics2D.BoxCast(groundcheck.transform.position, new Vector2(0.001f, 0.001f), 0, Vector2.down, 0.15f, groundLayer);
        if (hit)
        {
            isGrounded = true;
            print(hit.transform.name);
        }
        else
            isGrounded = false;
        print(isGrounded);
    }
}
