using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockerPlayerBehavior : MonoBehaviour
{
    private IAController iaController;
    private Rigidbody2D rigidbody;

    private readonly Vector2 crossX = new Vector2(0, 0.1f);
    private readonly Vector2 crossY = new Vector2(0.1f, 0);

    private void Start()
    {
        iaController = GetComponent<IAController>();
        rigidbody = GetComponent<Rigidbody2D>();
        iaController.AjustarRotacao();
    }

    private void FixedUpdate()
    {
        Vector2 steering = iaController.ObstacleAvoidance();
        if (steering == Vector2.zero) steering = iaController.PathFollowing();

        Vector2 v = rigidbody.velocity + steering;
        rigidbody.velocity = Vector2.ClampMagnitude(v, iaController.velocidadeMaxima);

        if (rigidbody.velocity.magnitude > iaController.stopVal) iaController.AjustarRotacao();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 p = Input.mousePosition;
            p = (Vector2) Camera.main.ScreenToWorldPoint(p);

            iaController.path.Enqueue(p);
        }

        foreach(Vector2 p in iaController.path)
        {
            Debug.DrawLine(p - crossX, p + crossX);
            Debug.DrawLine(p - crossY, p + crossY);
        }
    }
}
