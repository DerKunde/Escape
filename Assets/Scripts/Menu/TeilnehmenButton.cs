using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeilnehmenButton : MonoBehaviour
{
    public GameObject teilnehmenButton;
    private float myTimer = 0;
    public float waitingTime = 1;
    private float showButton = 63;
    private bool play = false;
    public InteractiveButton iB;

    // Start is called before the first frame update
    void Start()
    {
        teilnehmenButton.SetActive(false);
    }

    // Update is called once per frame
    /**
     * überprüft, ob das Video abgespielt wird
     */
    void Update()
    {
        play = iB.play;
        ShowButton();
    }


    /**
     * zeigt Button um mit dem Spiel fortzufahren
     */
    private void ShowButton()
    {
        if (play)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= showButton)
            {
                teilnehmenButton.SetActive(true);
                play = false;
            }
        }

    }
}
