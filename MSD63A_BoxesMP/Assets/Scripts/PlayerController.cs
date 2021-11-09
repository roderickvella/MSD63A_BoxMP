using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;

    float horizontal;
    float vertical;

    public float runSpeed = 5.0f;

    public FixedJoystick fixedJoystick;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        fixedJoystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
    }

    void Update()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        horizontal = fixedJoystick.Horizontal;
        vertical = fixedJoystick.Vertical;
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
