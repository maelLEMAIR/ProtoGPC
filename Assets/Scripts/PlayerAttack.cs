using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    GameObject enemy;

    public void Attack()
    {
        if (enemy == null)
            return;

        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
            enemyHealth.health -= 10;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemy = collision.gameObject;
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemy = null;
    }

}
