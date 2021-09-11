using System;
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
    public float distDesvio; // Distancia para desviar dos obstaculos
    public float distTouch;

    public Rigidbody2D bola;

    private Rigidbody2D rigidbody;
    private Queue<Vector2> path;

    private readonly Vector2 crossX = new Vector2(0, 0.1f);
    private readonly Vector2 crossY = new Vector2(0.1f, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        path = new Queue<Vector2>();
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
    {
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

    // Algoritmo de desvio de obstaculo
    Vector2 ObstacleAvoidance()
    {
        Vector2 steer = Vector2.zero;

        float ahead = distDesvio * (rigidbody.velocity.magnitude / velocidadeMaxima);
        RaycastHit2D[] obs = Physics2D.RaycastAll(rigidbody.position, rigidbody.velocity, ahead);

        if (obs.Length > 1) // A propria AI sempre estará na lista de obstaculos
        {
            Array.Sort(obs, delegate (RaycastHit2D h1, RaycastHit2D h2)
            {
                return h1.CompareTo(h2);
            });
            RaycastHit2D hit = obs[1];

            Vector2 to = (Vector2) hit.transform.position - rigidbody.position;
            Vector2 direcao = SetMagnetude(rigidbody.velocity, to.magnitude) - to;
            steer = SetMagnetude(direcao, maxSteeringForce);
        }

        return steer;
    }

    // Busca por caminho
    Vector2 PathFollowing()
    {
        if (path.Count > 0)
        {
            goal = path.Peek();

            if (path.Count > 1)
            {
                if (Vector2.Distance(rigidbody.position, path.Peek()) < distTouch)
                {
                    path.Dequeue();
                }

                return Seek();
            }

            return Arrival();
        }
        
        return Vector2.zero;
    }

    private void FixedUpdate()
    {
        Vector2 steering = ObstacleAvoidance();
        if (steering == Vector2.zero) steering = PathFollowing();

        Vector2 v = rigidbody.velocity + steering;
        rigidbody.velocity = Vector2.ClampMagnitude(v, velocidadeMaxima);

        if (rigidbody.velocity.magnitude > stopVal) AjustarRotacao();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 p = Input.mousePosition;
            p = (Vector2) Camera.main.ScreenToWorldPoint(p);

            path.Enqueue(p);
        }

        foreach(Vector2 p in path)
        {
            Debug.DrawLine(p - crossX, p + crossX);
            Debug.DrawLine(p - crossY, p + crossY);
        }
    }
}
