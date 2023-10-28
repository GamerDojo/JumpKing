
using System;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] RectTransform chargeBar;
    [SerializeField] Transform groundCheck;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float chargeRate = 0.3f; // per second 
    [SerializeField] float deathY = -10; // height at which player dies

    private float chargingSign = 1;

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
            // the platform contains the score as a int.ToString() in the tag 
            scoreText.text = "Score: " + hit.transform.GetComponent<Platform>().score;
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
        animator = GetComponentInChildren<Animator>();
    }
    private float zoomVel;
    private void Update()
    {
        bool isCharging = false;

        if (Input.GetMouseButtonDown(0))
        { // first time the jump is pressed
            chargingSign = 1;
        }

        if (Input.GetMouseButton(0))
        { // when button is pressed (runs every frame)
            isCharging = true;

            if (jumpCharge == 1)
            {
                chargingSign = 0.5f;
                chargingSign = -chargingSign;
            }

            jumpCharge = Mathf.Clamp01(jumpCharge + chargeRate * Time.deltaTime * chargingSign);

            if (jumpCharge == 0)
            {
                chargingSign = -chargingSign;
            }
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
        if (transform.position.y < deathY)
        {
            transform.position = initialPosition;
        }

        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, Mathf.Lerp(5, 10, rb.velocity.magnitude / jumpForce), ref zoomVel, .2f);

        animator.SetBool("isFalling", isGrounded == false);
        animator.SetBool("isCharging", isCharging);
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