using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignsButtons : MonoBehaviour
{

    public Sprite blue_layer, green_layer, gray_layer;
    public string sign = "";
    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    private void OnMouseDown()
    {
        if (GetComponent<SpriteRenderer>().sprite == gray_layer) return;
        if (GetComponent<SpriteRenderer>().sprite == green_layer)
        {
            switch_to_blue();
            GameObject.Find("EventSystem").GetComponent<ButtonControllerSigns>().current_green_button = -1;
        }
        else
        {
            GameObject.Find("EventSystem").GetComponent<ButtonControllerSigns>().make_other_blue(sign);
            switch_to_green();
        }
    }


    public void switch_to_green()
    {
        GetComponent<SpriteRenderer>().sprite = green_layer;
    }

    public void switch_to_gray()
    {
        GetComponent<SpriteRenderer>().sprite = gray_layer;
    }

    public void switch_to_blue()
    {
        GetComponent<SpriteRenderer>().sprite = blue_layer;
    }
}
