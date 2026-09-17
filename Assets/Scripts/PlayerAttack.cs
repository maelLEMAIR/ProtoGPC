using Unity.VisualScripting;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    GameObject enemy;

    private void Start()
    {
    }

    public void Attack()
    {
        if (enemy == null)
        {
            Debug.Log("null");
            return;
        }

        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.health -= 10;
            Debug.Log("Attack Done");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject;
            Debug.Log("go recup");
        }
        
    }

}
