using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Material touchedAttack;
    [SerializeField] private Material missedAttack;
    [SerializeField] private float timeOfTheAttack = 2.0f;
    
    [SerializeField] private MeshRenderer playerRender;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float timeDamage = 0.05f;
    private Material playerMaterial;
    float cooldownDamage = 0f;
    
    private float currentTime = 0.0f;
    
    private GameObject enemy;

    private void Start()
    {
        playerMaterial = playerRender.material;
    }

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void Attack()
    {
        MeshRenderer MR = GetComponent<MeshRenderer>();
        if (enemy == null)
        {
            MR.material = missedAttack;
            return;
        }

        MR.material = touchedAttack;
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.health -= 10;
            playerRender.material = damageMaterial;

            cooldownDamage = timeDamage;
        }
    }

    private void Update()
    {
        if(cooldownDamage > 0f && playerRender.material != playerMaterial)
        {
            cooldownDamage -= Time.deltaTime;
        }
        else
        {
            playerRender.material = playerMaterial;
        }
        currentTime += Time.deltaTime;
        if (currentTime >= timeOfTheAttack)
        {
            currentTime = 0.0f;
            enemy = null;
            enabled = false;
        }
    }

    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
            enemy = collision.gameObject;
    }
}