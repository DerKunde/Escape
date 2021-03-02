using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Script für die Bewegung der Zahlenräder des Zahlenschloss
 * wird durch den "Down"-Button ausgelöst 
 */
public class RotateDown: MonoBehaviour
{
    //Variablen fuer die Blicksteuerung
    private Color inactive;
    //private Color gazedAt; // durch "Laser" visualisiert
    private MeshRenderer myRenderer;

    private bool activated = false; //eigentlich gazedAt
    private float myTimer = 0;
    private float waitingTime = 1; // Dauer bis ein Event ausgelöst wird
    private GameObject player; // Verweis auf den Spieler

    //Variablen fuer das Zahlenschloss
    private bool coroutineAllowed;
    private int numberShown;

    private laserPointer laserPointer;
    public float offset = 0.5f;
    public float size = 1;


    public LockControl lockControl; // zur Überprüfung des gesamten Codes
    public GameObject numberWheel; // das Rad das gesteuert wird



    void Start()
    {
        player = lockControl.getPlayer();
        inactive = new Color(1, 1, 1, 0);//transparent
        //gazedAt = new Color(0, 1, 0, 1); //grün

        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material.color = inactive;

        coroutineAllowed = true;
        numberShown = 1;

        player = GameObject.FindWithTag("Player");
        laserPointer = player.GetComponent<laserPointer>();
    }

    public void gazeAt(bool gazing)
    {

        if (gazing)
        {
            laserPointer.setLaserLength(transform.position, true);
            laserPointer.setLaserVisible(GetComponent<MeshRenderer>(), transform.position, size, Color.yellow, offset);
            laserPointer.setLaserColor(Color.yellow);
            activated = true;
           // myRenderer.material.color = gazedAt;
        }
        else
        {
            myTimer = 0;
           // myRenderer.material.color = inactive;
            activated = false;
            laserPointer.setLaserInvisible();
        }

    }
    /**
     * Code Beruht auf dem Tutorial von Alexander Zotov, vgl: https://youtu.be/SFwz9JBl9Bc
     */
    private IEnumerator RotateWheel()
    {
        //es kann sich immer nur ein Rad drehen
        coroutineAllowed = false;

        for (int i = 0; i <= 11; i++)
        {//insgesamt dreht es sich um 36 Grad
            numberWheel.transform.Rotate(0f, 0f, 3f);
            yield return new WaitForSeconds(0.01f);
        }
        numberShown = lockControl.GetNumber(numberWheel.name);
        coroutineAllowed = true;// jetzt darf wieder neu gedreht werden
        numberShown -= 1;
        
        if (numberShown < 0)
        {
            numberShown = 9;
        }
        
        //ueberpruefe, ob die richtige Kombination erreicht wurde
        lockControl.CheckResults(numberWheel.name, numberShown);

    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            myTimer += Time.deltaTime;
            //linearer Farbwechsel
            //myRenderer.material.color = Color.Lerp(gazedAt, inactive, (waitingTime - myTimer) / waitingTime);

            if (myTimer >= waitingTime)
            {
                if (coroutineAllowed)
                {
                    StartCoroutine("RotateWheel");
                    SoundManager.PlayRotateCogwheelSound();
                }

                myRenderer.material.color = inactive;
                myTimer = 0;
                activated = false;
            }
        }
    }

}
