using System;
using UnityEngine;

public class Grumpy : MonoBehaviour
{
    [SerializeField] private float birdGravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityMultiplier;

    [SerializeField] public RayCastObserver rayCastObserver;
    private float screenTop;
    private float screenBottom;
    public event Action OnHitPipe;
    public event Action OnUpdateScore;
    
    private bool _willJump;
    private Rigidbody2D grumpyRigidbody;

    private void Start()
    {
        grumpyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        screenTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).y;
        gameObject.transform.position = new Vector3(-5, 0, 0);
        InputHandler.Instance.OnJump += Jump;
    }
    
    // Function to bind to OnJump Event
    private void Jump()
    {
        _willJump = true;
    }


    #region Initialization


    #endregion
    
    private void FixedUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     Debug.Log("here aight");
        //     grumpyRigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        // }
        CalculateRelativeForceOnBird();
        rayCastObserver.CheckRayCasts();
        if (gameObject.transform.position.y > screenTop) OnHitPipe?.Invoke();
        if (gameObject.transform.position.y < screenBottom) OnHitPipe?.Invoke();
    }

    #region Bird Physics

    // Bird Physics
    private void CalculateRelativeForceOnBird()
    {
        var velocityMultiplier = gravityMultiplier * Time.unscaledDeltaTime;
        Vector3 velocity;
        if (_willJump)
        {
            velocity =  new Vector3(0, jumpForce + birdGravity, 0) * velocityMultiplier;
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