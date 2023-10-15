using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerNumbers : MonoBehaviour
{
    const int buttons_number = 10;
    public int current_green_button = -1;

    public List<GameObject> numbers = new List<GameObject>(buttons_number);
    public List<bool> is_sign_on = new List<bool>(buttons_number) { true, true, true, true, true, true, true, true, true, true };

    private void Start()
    {
        for (int i = 0; i < buttons_number; ++i)
        {
            if (is_sign_on[i]) numbers[i].GetComponent<NumbersButtons>().switch_to_blue();
            else numbers[i].GetComponent<NumbersButtons>().switch_to_gray();
        }
    }

    /// make other buttons on the top blue
    public void make_other_blue(int number)
    {
        if (number == -1) return;
        if (current_green_button == -1 || current_green_button == number)
        {
            current_green_button = number;
            return;
        }
        numbers[current_green_button].GetComponent<NumbersButtons>().switch_to_blue();
        current_green_button = number;
    }
}
