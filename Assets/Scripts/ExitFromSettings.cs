using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFromSettings : MonoBehaviour
{
    public GameObject settings;
    public GameObject event_system;

    private void OnMouseDown()
    {
        Debug.Log("ExitFromSettings");
        settings.SetActive(false);
        event_system.GetComponent<ButtonCreator>().settings_exit();
    }
}
