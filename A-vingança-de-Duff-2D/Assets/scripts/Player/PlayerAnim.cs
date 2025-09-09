using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;
    private IsGroundedChecker groundedChecker;
    private Health playerHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        groundedChecker = GetComponent<IsGroundedChecker>();
        playerHealth = GetComponent<Health>();

        playerHealth.OnHurt += PlayHurtAnim;
        playerHealth.OnDead += PlayDeadAnim;
    }

    // Adicionado o método Start para a inscrição de eventos que dependem de outros Managers
    private void Start()
    {
        // Movido do Awake para o Start para garantir que o InputManager já exista
        GameManager.Instance.InputManager.OnAttack += PlayAttackAnim;
    }

    private void Update()
    {
        bool isMoving = GameManager.Instance.InputManager.Movement != 0;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumping", !groundedChecker.IsGrounded());
    }

    private void PlayHurtAnim()
    {
        animator.SetTrigger("hurt");
    }

    private void PlayDeadAnim()
    {
        animator.SetTrigger("dead");
        GameManager.Instance.InputManager.DisableInputs(); //
    }

    private void PlayAttackAnim()
    {
        animator.SetTrigger("attack"); //
    }

    // Boa prática: remover a inscrição do evento ao destruir o objeto
    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.InputManager != null)
        {
            GameManager.Instance.InputManager.OnAttack -= PlayAttackAnim;
        }

        if (playerHealth != null)
        {
            playerHealth.OnHurt -= PlayHurtAnim;
            playerHealth.OnDead -= PlayDeadAnim;
        }
    }
}