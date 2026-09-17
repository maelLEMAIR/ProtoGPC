using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Material damageMaterial;
    public Material enemyMaterial;

    public MeshRenderer enemyRenderer;

    float cooldown = 0f;
    public float time = 0.05f;

    GameObject enemy;


    private void Update()
    {
        if(cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            enemyRenderer.material = enemyMaterial;
        }
    }

    public void Attack()
    {
        if (enemy == null)
            return;

        Health enemyHealth = enemy.GetComponent<Health>();

        if (enemyHealth != null)
        {
            enemyHealth.health -= 10;
            Debug.Log("attack");


            enemyRenderer.material = damageMaterial;

            cooldown = time;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject;
            Debug.Log("collision");
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemy = null;
    }

}
