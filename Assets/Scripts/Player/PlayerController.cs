using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IEntityController
{
    [Header("Player Data")]
    public PlayerData playerData;

    [Header("References")]
    public CharacterController characterController;
    public PlayerMovement playerMovement;
    public Camera playerCamera;
    public Animator cameraAnimator;
    public PlayerHandAnimatorController handAnimatorController;
    public GameOverMenu gameOverMenu;

    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerCameraLook cameraLook;
    [HideInInspector] public PlayerShotgun shotgun;
    [HideInInspector] public HealthSystem health;

    [Header("Runtime State")]
    public bool isMovementLocked = false;
    public bool isAlive = true;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        cameraLook = GetComponent<PlayerCameraLook>();
        shotgun = GetComponent<PlayerShotgun>();
        health = GetComponent<HealthSystem>();
    }

    public EntityData GetEntityData() => playerData;

    public void OnEntityDeath()
    {        
        if (!isAlive) return;
        isAlive = false;

        Debug.Log("! ! ! Player is Dead ! ! !");
        
        Time.timeScale = 0f;

        if (gameOverMenu != null)
        {
            Cursor.lockState = CursorLockMode.None;

            gameOverMenu.gameObject.SetActive(true);
        }
        else
        {
           Debug.LogWarning("GameOverMenu reference is missing!"); 
        }
    }
}

