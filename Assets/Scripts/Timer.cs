using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float time = 0;
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        time = PlayerPrefs.GetFloat("timer", 0);
        if (time >= 120) time = 80;
    }
    
    void Update()
    {
        time += Time.deltaTime;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("timer", time);
    }
}
