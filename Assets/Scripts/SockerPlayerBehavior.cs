using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockerPlayerBehavior : MonoBehaviour
{
    public Vector2 goal;
    public float velocidadeMaxima;
    public float maxSteeringForce; // Força maxima de navegacao / Quanto maior mais rapido vira para o destino
    public float stopVal; // Limite de velocidade para considerar como parado

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        AjustarRotacao();
    }

    void AjustarRotacao()
    {
        float angulo = Vector2.Angle(rigidbody.velocity, Vector2.right);

        if (rigidbody.velocity.y >= 0) rigidbody.rotation = angulo;
        else rigidbody.rotation = 360 - angulo;
    }

    Vector2 Seek()
    {
        Vector2 v = (goal - rigidbody.position).normalized * velocidadeMaxima;
        Vector2 steering = v - rigidbody.velocity;

        // Limita o valor máximo da força do steering
        return Vector2.ClampMagnitude(steering, maxSteeringForce);
    }

    private void FixedUpdate()
    {
        Vector2 steering = Seek();
        Vector2 v = rigidbody.velocity + steering;
        rigidbody.velocity = Vector2.ClampMagnitude(v, velocidadeMaxima);

        if (rigidbody.velocity.magnitude > stopVal) AjustarRotacao();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            goal = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
