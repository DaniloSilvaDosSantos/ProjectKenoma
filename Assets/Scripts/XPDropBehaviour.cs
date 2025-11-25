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
    [SerializeField] private float initialHorizontalSpeed = 4f;
    [SerializeField] private float horizontalDecaySpeed = 6f;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Attraction")]
    [SerializeField] private float maxAttractSpeed = 10f;
    [SerializeField] private float minAttractionPercent = 0.5f;
    [SerializeField] private float radiusPhysicalContact = 0.5f;

    private Vector3 velocity;
     private bool canAttract = false;

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
        velocity = direction.normalized * initialHorizontalSpeed;
        velocity.y = initialUpForce;
    }

    private void Update()
    {
        DecayInitialLaunch();
        AttractToPlayer();
        CheckToCollect();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        GroundCheck();
        transform.position += velocity * Time.deltaTime;
    }

    private void ApplyGravity()
    {
        velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask))
        {
            if (velocity.y < 0) velocity.y = 0;
        }
    }

    private void DecayInitialLaunch()
    {
        if (canAttract) return;

        Vector2 horizontal = new Vector2(velocity.x, velocity.z);

        if (horizontal.magnitude > 0.05f)
        {
            horizontal = Vector2.MoveTowards
            (
                horizontal,
                Vector2.zero,
                horizontalDecaySpeed * Time.deltaTime
            );

            velocity.x = horizontal.x;
            velocity.z = horizontal.y;
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
            canAttract = true;
        }
    }

    private void AttractToPlayer()
    {
        if (!canAttract) return;
        if (player == null) return;

        float range = playerController.playerData.dropCollectRange;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > range) return;

        float percent = Mathf.InverseLerp(range, range * minAttractionPercent, distance);
        float attractionSpeed = Mathf.Lerp(0, maxAttractSpeed, percent);

        Vector3 targetDir = (player.position - transform.position).normalized;
        Vector3 currentDir = new Vector3(velocity.x, 0, velocity.z);

        if (currentDir.sqrMagnitude < 0.001f)
        {
            currentDir = targetDir;
        }
        else
        {
            currentDir = Vector3.Lerp(currentDir, targetDir, Time.deltaTime * 50f).normalized;
        }

        velocity.x = currentDir.x * attractionSpeed;
        velocity.z = currentDir.z * attractionSpeed;
    }

    private void CheckToCollect()
    {
        if (player == null) return;

        if (Physics.CheckSphere(transform.position, radiusPhysicalContact, LayerMask.GetMask("Player")))
        {
            levelSystem.AddXP(xpAmount);
            Destroy(gameObject);
        }
    }
}
