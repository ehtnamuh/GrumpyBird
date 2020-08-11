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

    private void Start()
    {
        InputHandler.Instance.OnJump += Jump;
        CalculateRayDirections(rayCastCount, rayCastAngleRangeDegrees);
    }

    private void Jump() => _willJump = true;

    private void Update()
    {
        CalculateRelativeForceOnBird();
        CheckRayCasts(rayCastLength);
    }

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
    }

    private void CalculateRayDirections(int rayCastCount, float rayCastAngleRangeDegrees)
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
    
    private void CheckRayCasts(float distance)
    {
        // TODO: make list of RayCasts 
        // List<RaycastHit2D> hits = new List<RaycastHit2D>();
        
        var castPointPosition = this.castPoint.transform.position;
        for (var i = 0; i < rayCastCount; i++)
        {
            var direction = _rayCastDirections[i];
            var endPosition = castPointPosition + (direction * distance);
            var layerMask = 1 << LayerMask.NameToLayer("Action");
            var hit = Physics2D.Linecast(castPointPosition, endPosition, layerMask);
            // hit.distance;
            if (!hit.collider)
                Debug.DrawLine(castPointPosition, endPosition, Color.green);
            else
            {
                if (hit.collider.CompareTag("Pipe"))
                    Debug.DrawLine(castPointPosition, endPosition, Color.red);
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Pipe")) OnHitPipe?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Score")) OnUpdateScore?.Invoke();
    }

    private void OnDisable() => InputHandler.Instance.OnJump -= Jump;
    
}
