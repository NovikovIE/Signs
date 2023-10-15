using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private ButtonControllerSigns bcos;
    private ButtonControllerNumbers bcon;
    public ButtonCreator bcr;
    [SerializeField] private GameObject itself;

    public int i, j;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Start()
    {
        bcr = GameObject.Find("EventSystem").GetComponent<ButtonCreator>();
        if (bcr.is_signs_mode) bcos = GameObject.Find("EventSystem").GetComponent<ButtonControllerSigns>();
        else bcon = GameObject.Find("EventSystem").GetComponent<ButtonControllerNumbers>();
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    private void OnMouseDown()
    {
        if (bcr.is_signs_mode)
        {
            if (bcos.current_green_button == -1) return;
            ButtonCreator.Objects type;
            if (bcos.current_green_button == 0) type = ButtonCreator.Objects.PLUS;
            else if (bcos.current_green_button == 1) type = ButtonCreator.Objects.MINUS;
            else if (bcos.current_green_button == 2) type = ButtonCreator.Objects.MULTIPLY;
            else type = ButtonCreator.Objects.DIVIDE;

            FieldFrames temp = bcr.instantiate_operation(type, transform.position, i, j).GetComponent<FieldFrames>();
            temp.tile_under = itself;
            temp.type = type;
            itself.SetActive(false);
            temp.is_destructible = true;
            temp.is_on_tile = true;
            bcr.add_one_to_filled_tiles(i, j);
        }
        else
        {
            if (bcon.current_green_button == -1) return;
            FieldFrames temp = bcr.instantiate_number(bcon.current_green_button, transform.position, i, j).GetComponent<FieldFrames>();
            temp.value = bcon.current_green_button;
            temp.tile_under = itself;
            temp.type = ButtonCreator.Objects.NUMBER;
            itself.SetActive(false);
            temp.is_destructible = true;
            temp.is_on_tile = true;
            bcr.add_one_to_filled_tiles(i, j);
        }
    }
}
