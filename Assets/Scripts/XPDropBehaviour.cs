using UnityEngine;

public class XPDropBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerLevelSystem levelSystem;
    [SerializeField] private LayerMask groundMask;

    [Header("XP")]
    [SerializeField] private int xpAmount;

    [Header("Physics")]
    [SerializeField] private float initialUpForce = 5f;
    [SerializeField] private float horizontalSpeed = 4f;
    [SerializeField] private float groundDrag = 8f;
    [SerializeField] private float airDrag = 1f;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private Vector3 velocity;
    private bool hasLanded = false;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        levelSystem = FindAnyObjectByType<PlayerLevelSystem>();

        if (playerController != null) player = playerController.transform;

        velocity = new Vector3(velocity.x, initialUpForce, velocity.z);
    }

    public void Initialize(int xp, Vector3 direction)
    {
        xpAmount = xp;
        velocity = direction.normalized * horizontalSpeed;
        velocity.y = initialUpForce;
    }

    void Update()
    {
        CheckCollect();
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
    }

    private void ApplyPhysics()
    {
        velocity.y += Physics.gravity.y * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        GroundCheck();

        if (hasLanded)
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, groundDrag * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, 0, groundDrag * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, airDrag * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, 0, airDrag * Time.deltaTime);
        }

        if (transform.position.y < 0)
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
            velocity.y = 0;
            hasLanded = true;
        }
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask))
        {
            hasLanded = true;
            velocity.y = 0f;
        }
        else
        {
            hasLanded = false;
        }
    }

    private void CheckCollect()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= playerController.playerData.dropCollectRange)
        {
            levelSystem.AddXP(xpAmount);
            Destroy(gameObject);
        }
    }
}
