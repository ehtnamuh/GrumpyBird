using System;
using UnityEngine;

public class Grumpy : MonoBehaviour
{
    [SerializeField] private float birdGravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityMultiplier;

    [SerializeField] public RayCastObserver rayCastObserver;
    
    private float screenHeight;
    public event Action OnHitPipe;
    public event Action OnUpdateScore;
    
    private bool _willJump;


    private void Start()
    {
        screenHeight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;
        InputHandler.Instance.OnJump += Jump;
    }
    
    // Function to bind to OnJump Event
    private void Jump() => _willJump = true;

    #region Initialization


    #endregion
    
    private void FixedUpdate()
    {
        CalculateRelativeForceOnBird();
        rayCastObserver.CheckRayCasts();
        if (gameObject.transform.position.y > screenHeight) OnHitPipe?.Invoke();
    }

    #region Bird Physics

    // Bird Physics
    private void CalculateRelativeForceOnBird()
    {
        var velocityMultiplier = gravityMultiplier * Time.fixedDeltaTime;
        Vector3 velocity;
        if (_willJump)
        {
            velocity = velocityMultiplier * new Vector3(0, jumpForce + birdGravity, 0);
            _willJump = false;
        }
        else
            velocity = velocityMultiplier * new Vector3(0, birdGravity, 0);

        var o = gameObject;
        var newPosition = o.transform.position + velocity;
        o.transform.position = newPosition;
        _willJump = false;
    }



    #endregion

    #region Collision Detection

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Pipe")) OnHitPipe?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Score")) OnUpdateScore?.Invoke();
        if (other.CompareTag("Pipe"))
        {
            Debug.Log(other.ToString());
            OnHitPipe?.Invoke();
        }
    }
    
    #endregion
    

    private void OnDisable() => InputHandler.Instance.OnJump -= Jump;
    
}