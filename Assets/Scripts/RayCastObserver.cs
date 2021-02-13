using System;
using System.Collections.Generic;
using UnityEngine;

public class RayCastObserver : MonoBehaviour
{
    [SerializeField] private int rayCastCount;
    [SerializeField] private float rayCastAngleRangeDegrees;
    [SerializeField] private float rayCastLength;
    private GameObject castPoint;

    private List<Vector3> _rayCastDirections;
    private List<float> _rayCastObservations;

    private void Start()
    {
        CalculateRayDirections();
        InitRayCastObservations();
        castPoint = gameObject;
    }

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
    
    // Make and check Ray Casts for hits and store observations in RayCastObservations
    public void CheckRayCasts()
    {
        var distance = rayCastLength;
        var castPointPosition = castPoint.transform.position;
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
    public List<float> GetObservations() => _rayCastObservations;
}