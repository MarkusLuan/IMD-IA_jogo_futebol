using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {
	public float speed;
     private Rigidbody2D body;
     // Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
        	body.velocity = Random.onUnitSphere * speed;
	}

	void OnTriggerEnter2D(Collider2D other)    	{
		Vector2  velocidade = body.velocity;

        if (other.name == "ladoA" || other.name == "ladoB")  velocidade.Set(velocidade.x * (-1), velocidade.y);
		else if (other.name == "cima" || other.name == "baixo")  velocidade.Set(velocidade.x, velocidade.y * (-1));

        body.velocity = velocidade;
	}

     // Update is called once per frame
	void Update () {}
}
