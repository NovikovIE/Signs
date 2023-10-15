using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFrames : MonoBehaviour
{
    [SerializeField] public Sprite blue_layer, green_layer, gray_layer, red_layer, green_red_layer;
    public GameObject tile_under = null;
    public GameObject itself;
    public ButtonCreator.Objects type;
    public int value = -1;

    public bool is_destructible = false;
    public bool is_on_tile = false;
    public bool is_game_resets = false;
    public int i, j;
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

    public void switch_to_red()
    {
        GetComponent<SpriteRenderer>().sprite = red_layer;
    }

    public void switch_to_green_red()
    {
        GetComponent<SpriteRenderer>().sprite = green_red_layer;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (tile_under != null)
        {
            if (is_on_tile)
            {
                tile_under.SetActive(true);
                Tile temp = tile_under.GetComponent<Tile>();
                temp.bcr.prefabs[temp.i, temp.j] = tile_under;
                if (!is_game_resets) temp.bcr.substract_one_from_filled_tiles(temp.i, temp.j);
            }
            else
            {
                var es = GameObject.Find("EventSystem");
                if (es != null)
                {
                    var bcr = es.GetComponent<ButtonCreator>();
                    if (bcr != null) bcr.prefabs[i, j] = tile_under;
                }
            }
        }
        else
        {
            var es = GameObject.Find("EventSystem");
            if (es != null)
            {
                var bcr = es.GetComponent<ButtonCreator>();
                if (bcr != null) bcr.prefabs[i, j] = null;
            }
        }
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    private void OnMouseDown()
    {
        if (is_destructible) Destroy(itself);
    }
}
