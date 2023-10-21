
using System;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] RectTransform chargeBar;
    [SerializeField] Transform groundCheck;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float chargeRate = 0.3f; // per second 
    [SerializeField] float deathY = -10; // height at which player dies
    
    Vector3 initialPosition; // this is used for respawning 

    Camera mainCamera;
    float jumpCharge;
    bool isGrounded;

    private void GroundCheck()
    {
        int excludePlayer = ~0 - LayerMask.GetMask("Player");
        var hit = Physics2D.BoxCast(groundCheck.transform.position, new Vector2(1.1f, 1), 0, Vector2.down, 0.15f, excludePlayer);
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
        mainCamera = Camera.main;
        initialPosition = transform.position;
    }
    private float zoomVel;
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
        if (isGrounded == false)
            jumpCharge = 0;

        // set chargebar ui
        var scale = chargeBar.localScale;
        scale.x = jumpCharge;
        chargeBar.localScale = scale;

        // respawn 
        if(transform.position.y < deathY)
        {
            transform.position = initialPosition;
        }

        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, Mathf.Lerp(5, 10, rb.velocity.magnitude / jumpForce), ref zoomVel, .2f);
    }

    private void Jump()
    {
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var dif = mousePos - transform.position;
        dif.z = 0; 

        rb.AddForce(dif.normalized * jumpForce * jumpCharge, ForceMode2D.Impulse);
        jumpCharge = 0;
    } 
}