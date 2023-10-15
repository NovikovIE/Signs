using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] private string find_tag;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag(find_tag).Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }
}
