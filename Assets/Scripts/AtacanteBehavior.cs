using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AtacanteBehavior : MonoBehaviour {
    private IAController iaController;
    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        iaController = GetComponent<IAController>();
    }

	void FixedUpdate()
	{
		Vector2 steering = iaController.Arrival();
		Vector2 newVelocity = body.velocity + steering;
		body.velocity = Vector2.ClampMagnitude(newVelocity, iaController.velocidadeMaxima);

		if (body.velocity.magnitude > iaController.stopVal) iaController.AjustarRotacao();
	}

	void Update()
	{
		iaController.goal = iaController.bola.position;
	}
}