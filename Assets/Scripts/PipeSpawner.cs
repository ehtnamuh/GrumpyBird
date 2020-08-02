using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private int numberOfPipes;
    [SerializeField] private GameObject pipe;

    private Queue<GameObject> _pipeQueue; 
    
    private void Start()
    {
        _pipeQueue = new Queue<GameObject>(numberOfPipes);
        for (int i = 0; i < numberOfPipes; i++)
        {
            GameObject childPipe = Instantiate(pipe, gameObject.transform, true);
            childPipe.SetActive(false);
            _pipeQueue.Enqueue(childPipe);
        }
    }

    private void SpawnPipe()
    {
        StartCoroutine(WaitAndSpawnPipe());
    }

    private IEnumerator WaitAndSpawnPipe()
    {
        GameObject childPipe = _pipeQueue.Dequeue();
        childPipe.SetActive(true);
        yield return new WaitForSecondsRealtime(timeBetweenSpawns);
    }
}
