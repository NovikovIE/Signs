using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    private void OnMouseDown()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1) {
            GameObject.Find("ClickAudio").GetComponent<AudioSource>().Play();
        }
    }
}
