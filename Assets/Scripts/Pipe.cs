using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        transform.position += Vector3.left * (speed * Time.fixedDeltaTime);
    }

    public IEnumerator DeactivateAndReturnToQueue(float time, Queue<GameObject> queue)
    {
        yield return new WaitForSecondsRealtime(time);
        var pipe = gameObject;
        pipe.SetActive(false);
        queue.Enqueue(pipe);
    }
}
