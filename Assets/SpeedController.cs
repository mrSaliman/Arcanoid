using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{

    public float speed;
    public Rigidbody2D rb;

    private void OnCollisionExit2D(Collision2D other)
    {
        rb.velocity = rb.velocity.normalized * speed;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        rb.velocity = rb.velocity.normalized * speed;
    }

    
}
