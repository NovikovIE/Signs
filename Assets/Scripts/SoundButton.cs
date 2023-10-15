using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    [SerializeField] public GameObject sound_sign_on;
    [SerializeField] public GameObject sound_sign_off;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1) {
            sound_sign_on.SetActive(true);
            sound_sign_off.SetActive(false);
        } else {
            sound_sign_on.SetActive(false);
            sound_sign_off.SetActive(true);
        }
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    private void OnMouseDown()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1) {
            PlayerPrefs.SetInt("sound", 0);
        } else {
            PlayerPrefs.SetInt("sound", 1);
        }
        sound_sign_on.SetActive(!sound_sign_on.activeSelf);
        sound_sign_off.SetActive(!sound_sign_off.activeSelf);
    }
}
