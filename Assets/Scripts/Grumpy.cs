using System;
using UnityEngine;

public class Grumpy : MonoBehaviour
{
    [SerializeField] private float birdGravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityMultiplier;

    public event Action OnHitPipe;
    public event Action OnUpdateScore;
    
    private bool _willJump;

    private void Start()
    {
        InputHandler.Instance.OnJump += Jump;
    }

    private void Jump() => _willJump = true;

    private void FixedUpdate()
    {
        var velocityMultiplier = gravityMultiplier * Time.fixedDeltaTime;
        Vector3 velocity;
        if (_willJump)
        {
            velocity = velocityMultiplier * new Vector3(0, jumpForce + birdGravity, 0);
            _willJump = false;
        }
        else
        {
            velocity = velocityMultiplier * new Vector3(0, birdGravity, 0);
        }

        var o = gameObject;
        var newPosition = o.transform.position + velocity;
        o.transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pipe"))
        {
            OnHitPipe?.Invoke();
        }
        if (other.CompareTag("Score"))
        {
            OnUpdateScore?.Invoke();
        }
        
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnJump -= Jump;
    }
}
