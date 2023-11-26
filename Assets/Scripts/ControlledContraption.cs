using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlledContraption : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] LayerMask groundLayer;
    
    float width = 1f;
    float height = 1f;
    float halfHeight;
    [SerializeField] float groundCheckExtraDistance = 0.125f;
    
    Rigidbody2D _rigidbody2D;
    [SerializeField] bool _isGrounded;
    
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        halfHeight = height / 2;
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
        HandleInput();
    }

    private (Vector2 groundCheckPosition, Vector2 groundCheckBoxSize, float groundCheckDistance) CalculateGroundCheckParameters()
    {
        Vector2 groundCheckPosition = transform.position;
        Vector2 groundCheckBoxSize = new Vector2(1f, groundCheckExtraDistance);
        float groundCheckDistance = halfHeight;

        return (groundCheckPosition, groundCheckBoxSize, groundCheckDistance);
    }

    private bool IsGrounded()
    {
        var (groundCheckPosition, groundCheckBoxSize, groundCheckDistance) = CalculateGroundCheckParameters();

        RaycastHit2D hit = Physics2D.BoxCast(groundCheckPosition, groundCheckBoxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        var (groundCheckPosition, groundCheckBoxSize, groundCheckDistance) = CalculateGroundCheckParameters();

        Gizmos.color = Color.cyan;
        Vector2 bottomCenter = groundCheckPosition + Vector2.down * (groundCheckBoxSize.y / 2 + groundCheckDistance);
        Gizmos.DrawWireCube(bottomCenter, new Vector3(groundCheckBoxSize.x, groundCheckBoxSize.y, 0));
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
