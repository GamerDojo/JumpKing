using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contor : MonoBehaviour
{
    public bool jumpPressed;
    public PlayerGrounded playerdata;
    private float power;
    public Image PowerMeter;
    public Rigidbody2D rb;
    public float powerMultiplier = 250f;
    void Start()
    {
        StartCoroutine(HandlePower());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
    }
    void FixedUpdate()
    {
        if (jumpPressed = true && playerdata.isGrounded == true)
        {
            rb.AddForce(new Vector2(0, power * powerMultiplier));
            jumpPressed = false;
        }
    }
    public IEnumerator HandlePower()
    {
        while (true)
        {
            for (float i = 0; i <= 1; i += 0.05f)
            {
                power = i;
                PowerMeter.fillAmount = power;
                yield return new WaitForSeconds(0.05f);
            }

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                power = i;
                PowerMeter.fillAmount = power;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
