using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 3;

    [Header("Propriedades de ataque")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask attackLayer;


    private float moveDirection;

    private Rigidbody2D Rigidbody;
    private IsGroundedChecker isGroundedChecker;


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        isGroundedChecker = GetComponent<IsGroundedChecker>();
    }
    
    private void Start()
    {
        GameManager.Instance.InputManager.OnJump += HandleJump;
    }
    // Atualizado a cada frame
    private void Update()
    {
        float moveDirection = 
            GameManager.Instance.InputManager.Movement;
        transform.Translate(moveDirection * Time.deltaTime * moveSpeed, 0, 0);

        if (moveDirection > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDirection < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
    }
    

    private void HandleJump()
    {
        if (isGroundedChecker.IsGrounded() == false) 
            return;
        Rigidbody.linearVelocity += Vector2.up * jumpForce;
    }

    private void HandlePlayerDeath()
    {
        GetComponent<Collider2D>().enabled = false;
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.Instance.InputManager.DisableInputs();
    }

    private void Attack()
    {
        Collider2D[] hittedEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, attackLayer);
        print("Fazendo inimigo tomar dano");
        print(hittedEnemies.Length);

        foreach (Collider2D hittedEnemy in hittedEnemies)
            {
                print("Checando inimigo");
                if (hittedEnemy.TryGetComponent(out Health enemyHealth))
                {
                    print("Dando dano ");
                    enemyHealth.TakeDamage();
                }
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

}
