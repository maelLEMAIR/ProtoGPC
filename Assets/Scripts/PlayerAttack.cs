using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Material touchedAttack;
    [SerializeField] private Material missedAttack;
    [SerializeField] private float timeOfTheAttack = 2.0f;
    
    [SerializeField] private Material damageMaterial;
    [SerializeField] private MeshRenderer enemyRenderer;
    private Material enemyMaterial;

    float cooldown = 0f;
    public float time = 0.05f;
    
    private float currentTime = 0.0f;
    
    private GameObject enemy;

    private void Start()
    {
        enemyMaterial = enemyRenderer.material;
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
            Debug.Log("attack");


            enemyRenderer.material = damageMaterial;

            cooldown = time;
        }

    }

    private void Update()
    {
        if(cooldown > 0f && enemyRenderer.material != enemyMaterial)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            enemyRenderer.material = enemyMaterial;
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
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject;
            Debug.Log("collision");
        }
    }
}
