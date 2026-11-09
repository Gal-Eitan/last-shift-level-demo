using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// Drop-in controller for the Nurse player prefab.
/// Move: A/D or Left/Right (and W/S or Up/Down if allowVerticalMovement). Interact: E.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class NursePlayerControllerOld : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool allowVerticalMovement = true;
    [Tooltip("Seconds the interact animation locks movement")]
    public float interactDuration = 0.6f;
    [Tooltip("Seconds into the animation when the action fires (injection moment)")]
    public float interactFireTime = 0.3f;
    public UnityEvent onInteract;

    

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

    void Update()
    {
        if (lockTimer > 0f) { lockTimer -= Time.deltaTime; move = Vector2.zero; }
        else
        {
            move = ReadMove();
            //Debug.Log(move);
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
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        var k = Keyboard.current;
        if (k == null) return Vector2.zero;
        float x = (k.dKey.isPressed || k.rightArrowKey.isPressed ? 1 : 0) - (k.aKey.isPressed || k.leftArrowKey.isPressed ? 1 : 0);
        float y = allowVerticalMovement ? (k.wKey.isPressed || k.upArrowKey.isPressed ? 1 : 0) - (k.sKey.isPressed || k.downArrowKey.isPressed ? 1 : 0) : 0;
        return new Vector2(x, y);

#else
        return new Vector2(Input.GetAxisRaw("Horizontal"), allowVerticalMovement ? Input.GetAxisRaw("Vertical") : 0f);
#endif
    }

    bool InteractPressed()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        return Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.E);
#endif
    }
}
