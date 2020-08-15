using System;
using System.Collections.Generic;
using UnityEngine;

public class Grumpy : MonoBehaviour
{
    [SerializeField] private float birdGravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private GameObject castPoint;
    [SerializeField] private float rayCastLength;
    [SerializeField] private int rayCastCount;
    [SerializeField] private float rayCastAngleRangeDegrees;
    public event Action OnHitPipe;
    public event Action OnUpdateScore;
    
    private bool _willJump;
    private List<Vector3> _rayCastDirections;
    private List<float> _rayCastObservations;

    private void Start()
    {
        InputHandler.Instance.OnJump += Jump;
        CalculateRayDirections();
        InitRayCastObservations();
    }
    
    // Function to bind to OnJump Event
    private void Jump() => _willJump = true;

    #region Initialization

    // Populate RayCastObeservation List
    private void InitRayCastObservations()
    {
        _rayCastObservations = new List<float>();
        for (int i = 0; i < rayCastCount; i++) _rayCastObservations.Add(0.0f);
    }
    
    // Populate RayCastDirections with Vector Directions for the rays
    private void CalculateRayDirections()
    {
        _rayCastDirections = new List<Vector3>();
        var baseAngle = gameObject.transform.rotation.z;
        var angleStep = rayCastAngleRangeDegrees / rayCastCount;
        angleStep = (float) ((Math.PI / 180.0f) * angleStep);
        var xComponent = (float) Math.Cos(baseAngle);
        var yComponent = (float) Math.Sin(baseAngle);
        float yMultiplier = 1;
        for (var i = 0; i < rayCastCount; i++)
        {
            _rayCastDirections.Add(new Vector3(xComponent, yComponent, 0));
            if (i % 2 == 0) baseAngle += angleStep;
            yMultiplier *= -1;
            xComponent = (float) Math.Cos(baseAngle);
            yComponent = (float) Math.Sin(baseAngle) * yMultiplier;
        }
    }

    #endregion
    
    private void Update()
    {
        CalculateRelativeForceOnBird();
        CheckRayCasts(rayCastLength);
    }

    #region Running In Update loop

    // Bird Physics
    private void CalculateRelativeForceOnBird()
    {
        var velocityMultiplier = gravityMultiplier * Time.deltaTime;
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
        _willJump = false;
    }

    // Make and check Ray Casts for hits and store observations in RayCastObservations
    private void CheckRayCasts(float distance)
    {
        var castPointPosition = this.castPoint.transform.position;
        for (int i = 0; i < rayCastCount; i++)
        {
            var direction = _rayCastDirections[i];
            var endPosition = castPointPosition + (direction * distance);
            var layerMask = 1 << LayerMask.NameToLayer("Action");
            var hit = Physics2D.Linecast(castPointPosition, endPosition, layerMask);

            _rayCastObservations[i] = -2.0f;
            if (!hit.collider)
                Debug.DrawLine(castPointPosition, endPosition, Color.green);
            else
            { 
                if (!hit.collider.CompareTag("Pipe")) continue;
                Debug.DrawLine(castPointPosition, endPosition, Color.red);
                _rayCastObservations[i] = hit.distance;
                // Debug.Log($"Ray Number: {i}  Distance: {_rayCastObservations[i]}");
            }
        }
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
    }
    
    #endregion

    public List<float> GetObservations() => _rayCastObservations;

    private void OnDisable() => InputHandler.Instance.OnJump -= Jump;
    
}
