using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;

    public void OpenMenuUI()
    {
        open = true;
        gameObject.SetActive(true);
    }

    public void CloseMenuUI()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
