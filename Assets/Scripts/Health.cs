using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health { get; set; }
    public int maxHealth = 100;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
