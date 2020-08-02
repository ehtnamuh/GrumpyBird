using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    public event Action OnJump;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            OnJump?.Invoke();
    }
}
