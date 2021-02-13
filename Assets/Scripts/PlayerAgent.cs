using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    private GameManager _gameManager;
    private Grumpy _grumpy;
    
    public override void Initialize()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _grumpy = gameObject.GetComponentInChildren<Grumpy>();
        _gameManager.OnGameEnd += ResetGame;
        _grumpy.OnHitPipe += KillBird;
    }

    private void KillBird() => EndEpisode();

    private void ResetGame()
    {
        AddReward(-10);
        _gameManager.Replay();
    }


    public override void OnEpisodeBegin()
    {
        
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        var observations = _grumpy.rayCastObserver.GetObservations();
        if(observations == null) return; 
        foreach (var obs in observations)
            sensor.AddObservation(obs);
    }

    
    public override void OnActionReceived(float[] vectorAction)
    {
        // Debug.Log(vectorAction[0]);
        // 1 = Flap |  2 = Don't Flap
        var x = vectorAction[0];
        if (x > 0.0f) InputHandler.Instance.InvokeOnJump();
        AddReward(0.02f);
        AddReward(_gameManager.GetScore());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _gameManager.OnGameEnd -= ResetGame;
    }
}
