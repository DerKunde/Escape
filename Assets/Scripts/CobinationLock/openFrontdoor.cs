using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Script zur Animation der Ausgangstür. Wurde zuvor das Zahlenschloss gelöst, kann die Tür geöffnet werden.
 * Ansonsten wird der Sound für die "Verschlossene Tür" abgespielt.
 * 
 */
public class openFrontdoor : MonoBehaviour
{
    private float myTimer = 0;
    public float waitingTime = 2;
    
    private bool useDoor = false;

    private Animator animator;
    public bool doorClosed { get; set; }
   
    public bool doorOpensToRight;

    private laserPointer laserPointer;
    private GameObject player;

    public void gazeAt(bool gazing)
    {
        if (gazing)
        {
            laserPointer.setLaserLength(transform.position, false);
            laserPointer.setLaserVisible(GetComponent<MeshRenderer>(), transform.position, 0.5f, Color.yellow, 0.5f);
            laserPointer.setLaserColor(Color.yellow);
            useDoor = true;
        }
        else
        {
            myTimer = 0;
            useDoor = false;
            laserPointer.setLaserInvisible();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        player = GameObject.FindWithTag("Player");
        doorClosed = true;
        laserPointer = player.GetComponent<laserPointer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (useDoor)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
            {
                if (!doorClosed)
                {
                    Debug.Log("Tür ist offen");
                    
                    if (doorOpensToRight == true)
                    {
                        animator.SetBool("openDoorRight", true);
                        SoundManager.PlayOpenDoorSound();
                    }
                    else
                    {
                        animator.SetBool("openDoorLeft", true);
                        SoundManager.PlayOpenDoorSound();
                    }
                  
                           
                }
                else
                {
                    SoundManager.PlayDoorLockedSound();
                }
                useDoor = false;
            }
            

        }

    }
}

