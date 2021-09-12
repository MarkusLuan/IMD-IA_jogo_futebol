using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoleiroBehavior : MonoBehaviour {
	private IAController iaController;
	private Rigidbody2D body;

	private Vector3 inicial_pos;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
		iaController = GetComponent<IAController>();
		inicial_pos = transform.position;
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
		float dist = Mathf.Abs(iaController.bola.transform.position.x - inicial_pos.x);

		if (dist < 2) iaController.goal = iaController.bola.position;
		else iaController.goal = inicial_pos;
	}
}