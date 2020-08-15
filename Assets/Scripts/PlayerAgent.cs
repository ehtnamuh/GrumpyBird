using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerAgent : Agent
{
    private GameManager _gameManager;
    private Grumpy _grumpy;
    
    public override void Initialize()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _grumpy = gameObject.GetComponentInChildren<Grumpy>();
        _gameManager.OnGameEnd += ResetGame;
    }

    private void ResetGame()
    {
        AddReward(-10);
        _gameManager.Replay();
    }


    public override void OnEpisodeBegin()
    {
        SetReward(0);
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        // Debug.Log("Collecting observations");
        var observations = _grumpy.GetObservations();
        foreach (var obs in observations)
            sensor.AddObservation(obs);
    }

    
    public override void OnActionReceived(float[] vectorAction)
    {
        Debug.Log(vectorAction[0]);
        // 1 = Flap |  2 = Don't Flap
        var x = vectorAction[0];
        if (x > 0.0f) InputHandler.Instance.InvokeOnJump();
        SetReward(_gameManager.GetScore());
        if (_gameManager.HasGameEnded()) _gameManager.Replay();
    }

    public override void Heuristic(float[] actionsOut)
    {
        // Allows normal control without neural network
        base.Heuristic(actionsOut);
    }
    
}
