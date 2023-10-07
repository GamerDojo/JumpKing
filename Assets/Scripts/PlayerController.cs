
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] RectTransform chargeBar;
    [SerializeField] Transform groundCheck;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float chargeRate = 0.3f; // per second 
    float jumpCharge;
    bool isGrounded;

    private void GroundCheck()
    {
        int excludePlayer = ~0 - LayerMask.GetMask("Player");
        var hit = Physics2D.BoxCast(groundCheck.transform.position, new Vector2(0.001f, 0.001f), 0, Vector2.down, 0.15f, excludePlayer);
        if (hit)
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        { // when button is pressed 
            jumpCharge = Mathf.Clamp01(jumpCharge + chargeRate * Time.deltaTime);
        }

        // jump when realeasing mouse button 
        if (Input.GetMouseButtonUp(0))
        {
            if (isGrounded)
                Jump();
        }

        // ground check 
        GroundCheck();
        if(isGrounded==false)
            jumpCharge = 0;

        // set chargebar ui
        var scale = chargeBar.localScale;
        scale.x = jumpCharge;
        chargeBar.localScale = scale;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce * jumpCharge, ForceMode2D.Impulse);
    }
}