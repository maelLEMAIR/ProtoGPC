using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    void Start()
    {
        health = 100;
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
