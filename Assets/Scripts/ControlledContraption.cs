using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlledContraption : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);
    [SerializeField] private Vector3 groundCheckOffset;

    private Rigidbody2D _rigidbody2D;
    [SerializeField] bool _isGrounded;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
        HandleInput();
    }

    private bool IsGrounded()
    {
        return true;

        float extraDistance = 0.1f;
        Vector3 boxPosition = transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.BoxCast(boxPosition, groundCheckSize, 0f, Vector2.down, groundCheckSize.y / 2 + extraDistance, groundLayer);

        return hit.collider != null;  // This always returns false somehow
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float extraDistance = 0.1f;
        Vector3 boxPosition = transform.position + groundCheckOffset;
        float totalDistance = groundCheckSize.y / 2 + extraDistance;
        Vector3 endBoxPosition = boxPosition + Vector3.down * totalDistance;

        Gizmos.DrawWireCube(boxPosition, groundCheckSize);
        Gizmos.DrawWireCube(endBoxPosition, groundCheckSize);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Player1_Horizontal");

        if (Input.GetButtonDown("Player1_Jump") && _isGrounded)
        {
            Jump();
        }

        Vector2 inputVector = new Vector2(horizontalInput, 0);
        if (inputVector != Vector2.zero)
        {
            ApplyInput(inputVector);
        } 
    }

    private void ApplyInput(Vector2 inputVector)
    {
        _rigidbody2D.velocity = new Vector2(inputVector.x * moveSpeed, _rigidbody2D.velocity.y);
    }

    private void HandleMovement()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        Vector2 force = new Vector2(0, jumpForce);
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
}
