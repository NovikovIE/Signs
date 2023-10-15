using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerSigns : MonoBehaviour
{
    const int buttons_number = 4;
    public int current_green_button = -1;
    // + = 0
    // - = 1
    // * = 2
    // / = 3

    public List<GameObject> signs = new List<GameObject>(buttons_number);
    public List<bool> is_sign_on = new List<bool>(buttons_number) { true, true, true, true };

    Dictionary<string, int> colors = new Dictionary<string, int> {
       {"Plus", 0},
       {"Minus", 1},
       {"Multiply", 2},
       {"Divide", 3}
    };

    private void Start()
    {
        for (int i = 0; i < buttons_number; ++i)
        {
            if (is_sign_on[i]) signs[i].GetComponent<SignsButtons>().switch_to_blue();
            else signs[i].GetComponent<SignsButtons>().switch_to_gray();
        }
    }

    /// make other buttons on the top blue
    public void make_other_blue(string name)
    {
        if (name == "") return;
        int new_green = colors[name];
        if (current_green_button == -1 || current_green_button == new_green)
        {
            current_green_button = new_green;
            return;
        }
        signs[current_green_button].GetComponent<SignsButtons>().switch_to_blue();
        current_green_button = new_green;
    }
}
