using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{

    public Sprite blue_layer, green_layer;

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = green_layer;
    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = blue_layer;
    }
}
