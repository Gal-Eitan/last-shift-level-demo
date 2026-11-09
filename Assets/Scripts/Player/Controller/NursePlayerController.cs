using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// Drop-in controller for the Nurse player prefab.
/// Move and Interact are now driven by the Input System's InputActionReference.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class NursePlayerController : MonoBehaviour
{
    [Header("Movement & Actions")]
    public float moveSpeed = 3f;
    public bool allowVerticalMovement = true;
    
    [Tooltip("Seconds the interact animation locks movement")]
    public float interactDuration = 0.6f;
    [Tooltip("Seconds into the animation when the action fires (injection moment)")]
    public float interactFireTime = 0.3f;
    public UnityEvent onInteract;

    [Header("Input Actions")]
    [Tooltip("Reference to the Move Action (Value -> Vector2)")]
    public InputActionReference moveAction;
    [Tooltip("Reference to the Interact Action (Button)")]
    public InputActionReference interactAction;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Vector2 move;
    float lockTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // Enable inputs when the script is active
        if (moveAction != null) moveAction.action.Enable();
        if (interactAction != null) interactAction.action.Enable();
    }

    void OnDisable()
    {
        // Disable inputs when the script is inactive to prevent errors
        if (moveAction != null) moveAction.action.Disable();
        if (interactAction != null) interactAction.action.Disable();
    }

    void Update()
    {
        if (lockTimer > 0f) 
        { 
            lockTimer -= Time.deltaTime; 
            move = Vector2.zero; 
        }
        else
        {
            move = ReadMove();
            
            if (move.sqrMagnitude > 1f) move.Normalize();
            
            if (move.x > 0.01f) sr.flipX = false;
            else if (move.x < -0.01f) sr.flipX = true;
            
            if (InteractPressed()) StartInteract();
        }
        anim.SetFloat("Speed", move.magnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    public void StartInteract()
    {
        lockTimer = interactDuration;
        anim.SetTrigger("Interact");
        Invoke(nameof(FireInteract), interactFireTime);
        onInteract.Invoke();
    }

    void FireInteract() { onInteract?.Invoke(); }

    Vector2 ReadMove()
    {
        if (moveAction == null || moveAction.action == null) return Vector2.zero;

        // Read the Vector2 directly from the Input Action
        Vector2 inputMove = moveAction.action.ReadValue<Vector2>();

        // Zero out the Y axis if vertical movement is not allowed
        if (!allowVerticalMovement)
        {
            inputMove.y = 0f;
        }

        return inputMove;
    }

    bool InteractPressed()
    {
        if (interactAction == null || interactAction.action == null) return false;

        // WasPressedThisFrame replaces Input.GetKeyDown
        return interactAction.action.WasPressedThisFrame();
    }
}