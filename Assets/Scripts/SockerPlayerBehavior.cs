using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockerPlayerBehavior : MonoBehaviour
{
    public Vector2 goal;
    public float velocidade;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        GoTowardsGoal();
    }

    void GoTowardsGoal()
    {
        Vector2 direcao = goal - rigidbody.position;
        rigidbody.velocity = direcao.normalized * velocidade;
    }

    void AjustarRotacao()
    {
        float angulo = Vector2.Angle(rigidbody.velocity, Vector2.right);

        if (rigidbody.velocity.y >= 0) rigidbody.rotation = angulo;
        else rigidbody.rotation = 360 - angulo;
    }

    private void FixedUpdate()
    {
        GoTowardsGoal();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            goal = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GoTowardsGoal();
            AjustarRotacao();
        }
    }
}
