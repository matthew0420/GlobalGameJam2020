using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitScript : MonoBehaviour
{
    void OnMouseDown()
    {
        Invoke("QuitGame", 1f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    }
