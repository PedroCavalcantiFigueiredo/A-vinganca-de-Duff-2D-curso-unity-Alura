using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private Transform detectPosition;
    [SerializeField] private Vector2 detectBoxSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float cooldownTimer;

    private bool canAttack = true;
    private Health playerHealth; // guardamos a referência

    protected override void Update()
    {
        if (!canAttack) return; // inimigo para se player morrer

        cooldownTimer += Time.deltaTime;
        VerifyCanAttack();
    }

    private void VerifyCanAttack()
    {
        if (cooldownTimer < attackCooldown)
            return;

        if (PlayerInSight())
        {
            animator.SetTrigger("attack");
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        cooldownTimer = 0;

        Collider2D playerCollider = CheckPlayerInDetectArea();
        if (playerCollider != null && playerCollider.TryGetComponent(out Health health))
        {
            // salvamos referência ao health do player
            if (playerHealth == null)
            {
                playerHealth = health;
                playerHealth.OnDead += OnPlayerDead;
            }

            if (health.CurrentHealth > 0)
            {
                Debug.Log("Making player take damage");
                health.TakeDamage();
            }
        }
    }

    private void OnPlayerDead()
    {
        canAttack = false;
        animator.ResetTrigger("attack");
    }

    private Collider2D CheckPlayerInDetectArea()
    {
        return Physics2D.OverlapBox(detectPosition.position, detectBoxSize, 0f, playerLayer);
    }

    private bool PlayerInSight()
    {
        Collider2D playerCollider = CheckPlayerInDetectArea();
        return playerCollider != null;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnDead -= OnPlayerDead;
    }

    private void OnDrawGizmos()
    {
        if (detectPosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detectPosition.position, detectBoxSize);
    }
}
