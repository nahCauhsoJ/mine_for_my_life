using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCore : MonoBehaviour
{
    public static LavaCore main;
    void Awake() { main = this; }

    public Collider2D col;
    public float speed;

    public bool start;

    public void StartFlow()
    {
        start = true;
        Flow(Vector2.right * speed);
    }

    public void Flow(Vector2 vel)
    {
        col.attachedRigidbody.velocity = vel;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!start) return;

        if (c.tag == "Player")
        {
            Flow(Vector2.zero);
            PlayerCore.main.Dead();
        }
    }
}
