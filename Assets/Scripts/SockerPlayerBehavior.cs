using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockerPlayerBehavior : MonoBehaviour
{
    public Vector2 goal;
    public float velocidadeMaxima;
    public float maxSteeringForce; // Força maxima de navegacao / Quanto maior mais rapido vira para o destino
    public float stopVal; // Limite de velocidade para considerar como parado
    public float distChegada;

    public Rigidbody2D bola;

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

    // Normaliza a velocidade
    Vector2 SetMagnetude (Vector2 v, float max)
    {   // Multiplica com a velocidade maxima
        return v.normalized * max;
    }

    // Algoritmo de busca
    Vector2 Seek()
    {   // Calcula a velocidade desejada através da subtração dos vetores do destino e da posição da IA
        Vector2 v = SetMagnetude((goal - rigidbody.position), velocidadeMaxima);
        Vector2 steering = v - rigidbody.velocity; // Subtrai a velocidade desejada da atual

        // Limita/Trunca o valor máximo da força do steering
        return Vector2.ClampMagnitude(steering, maxSteeringForce);
    }

    // Algoritmo de chegada ao destino
    Vector2 Arrival()
    {
        float dist = Vector2.Distance(rigidbody.position, goal);

        if (dist < stopVal) // Encontra-se muito proximo ao alvo
        { // Força contraria ao jogador para fazê-lo parar
            return -rigidbody.velocity;
        }
        else if (dist < distChegada) // Encontra-se na area de freio (onde deve começar a parar)
        {
            Vector2 v = SetMagnetude((goal - rigidbody.position), velocidadeMaxima);
            v *= dist / distChegada;
            Vector2 steering = v - rigidbody.velocity;

            return Vector2.ClampMagnitude(steering, maxSteeringForce);
        }
        else return Seek(); // Ainda não encontrou o destino
    }

    private void FixedUpdate()
    {
        Vector2 steering = Arrival();
        Vector2 v = rigidbody.velocity + steering;
        rigidbody.velocity = Vector2.ClampMagnitude(v, velocidadeMaxima);

        if (rigidbody.velocity.magnitude > stopVal) AjustarRotacao();
    }

    // Update is called once per frame
    void Update()
    {
        goal = bola.position;
    }
}
