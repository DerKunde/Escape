using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject screen;

    private void Start()
    {
        screen.SetActive(false);
    }

    public void onDetect()
    {
        screen.SetActive(true);
    }
}
