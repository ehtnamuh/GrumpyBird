using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    private GameManager _gameManager;
    private GameObject _grumpy;
    
    public override void Initialize()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _grumpy = gameObject;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Feed Raycast sensor data to Neural Network here
        sensor.AddObservation(_gameManager.GetScore());
        // TODO: Add raycast sensors to agent script
        // Will fetch a list of Floats
        // For each unique RayCast Object from _grumpy
        // Each hit will be an float from 0 - 2
        // representing the objects they are hitting,
        // 0 = Nothing |  1 = Pipe | 2 = Ground 
    }

    
    public override void OnActionReceived(float[] vectorAction)
    {
        // 1 = Flap |  2 = Don't Flap
        base.OnActionReceived(vectorAction);
    }

    public override void Heuristic(float[] actionsOut)
    {
        // Allows normal control without neural network
        base.Heuristic(actionsOut);
    }
    
}
