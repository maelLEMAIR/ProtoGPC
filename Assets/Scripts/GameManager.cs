using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject uiPostRound;
    
    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Transform playerSpawn;
    
    [Header("Enemy")]
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Health enemyHealth;
    [SerializeField] private Transform enemySpawn;


    public bool isInPostRound { get; set; }
    
    public static GameManager instance;
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        isInPostRound = false;
    }

    void Update()
    {
        if (enemyHealth.health <= 0 && !isInPostRound)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            playerHealth.GetComponent<PlayerInput>().enabled = false;
            isInPostRound = true;
            uiPostRound.SetActive(true);
        }
    }

    public void ReloadARound(int upgrade)
    {
        Debug.Log("ReloadARound");
        enemyHealth.health = enemyHealth.maxHealth;
        MeshRenderer enemyMR = enemyHealth.GetComponent<MeshRenderer>();
        switch (upgrade)
        {
            case 0:
                enemyMR.material.color = Color.white;
                break;
            case 1:
                enemyMR.material.color = Color.green;
                break;
            case 2:
                enemyMR.material.color = Color.purple;
                break;
        }
        uiPostRound.SetActive(false);
        
        enemyTransform.position = enemySpawn.position;
        enemyTransform.localRotation = enemySpawn.localRotation;
        
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        
        playerTransform.position = playerSpawn.position;
        playerTransform.localRotation = playerSpawn.localRotation;
        
        if (cc != null) cc.enabled = true;
        
        isInPostRound = false;
        
        playerHealth.GetComponent<PlayerInput>().enabled = true;
        enemyHealth.gameObject.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
