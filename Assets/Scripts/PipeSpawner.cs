using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private int numberOfPipes;
    [SerializeField] private GameObject pipe;
    [SerializeField] private float yOffsetRange;
    [SerializeField] private float pipeKillTime;
    
    private Queue<GameObject> _pipeQueue;
    private Grumpy _grumpy;

    private void Start()
    {
        _grumpy = FindObjectOfType<Grumpy>();
        _grumpy.OnHitPipe += StopPipeCoroutines;
        CreateAndQueuePipes();
        SpawnPipe();
    }

    private void StopPipeCoroutines() => StopAllCoroutines();

    private void CreateAndQueuePipes()
    {
        _pipeQueue = new Queue<GameObject>(numberOfPipes);
        for (var i = 0; i < numberOfPipes; i++)
        {
            var childPipe = Instantiate(pipe, gameObject.transform, true);
            childPipe.SetActive(false);
            _pipeQueue.Enqueue(childPipe);
        }
    }

    private void SpawnPipe() => StartCoroutine(WaitAndSpawnPipe());

    private IEnumerator WaitAndSpawnPipe()
    {
        yield return new WaitForSecondsRealtime(timeBetweenSpawns);
        var childPipe = _pipeQueue.Dequeue();
        var randomOffset = Random.Range(-yOffsetRange, yOffsetRange);
        var newPosition = gameObject.transform.position + new Vector3(0,randomOffset,0);
        childPipe.transform.position = newPosition;
        childPipe.SetActive(true);
        Pipe pipeScript = childPipe.GetComponent<Pipe>();
        /*Starts a coroutine that disables and enques the pipe to the _pipeQueue*/
        if (pipeScript) StartCoroutine(pipeScript.DeactivateAndReturnToQueue(pipeKillTime, _pipeQueue));
        SpawnPipe();
    }
}
