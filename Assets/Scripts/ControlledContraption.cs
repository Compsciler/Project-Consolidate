using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ControlledContraption : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForcePerMass = 10f;
    float JumpForce => jumpForcePerMass * _rigidbody2D.mass;
    [SerializeField] LayerMask groundLayer;
    
    float width;
    float height;
    float halfHeight;
    [SerializeField] float groundCheckExtraDistance = 0.125f;
    
    Rigidbody2D _rigidbody2D;
    [SerializeField] bool _isGrounded;
    
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        width = transform.localScale.x;
        height = transform.localScale.y;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        
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
        Vector2 force = new Vector2(0, JumpForce);
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
