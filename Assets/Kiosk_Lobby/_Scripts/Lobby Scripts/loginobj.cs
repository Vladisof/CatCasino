using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginobj : MonoBehaviour
{
    public int up, down;

    private void OnMouseDown()
    {
        if (up == 1)
        {
            Login.bar1 = 1;
            Login.bar2 = 0;
        }
        if (down == 1)
        {
            Login.bar2 = 1;
            Login.bar1 = 0;
        }
    }
}
