using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    private float myTimer = 0;
    public float waitingTime = 2;
    private bool useDoor = false;

    private Animator animator;
    private bool doorClosed = true;
    public bool requiresKey;
    private GameObject hands;
    public bool doorOpensToRight;

    private laserPointer laserPointer;
    private GameObject player;
    private MeshRenderer meshRenderer;

    /**
     * gazeAt wird aufgerufen, wenn man dieses Objekt (this.gameObject) anguckt
     * zeigt Laserstrahl
     * <param name="gazing">ob der Spieler auf dieses Objekt guckt</param>
     */
    public void gazeAt(bool gazing)
    {
        if (gazing)
        {
            laserPointer.setLaserLength(transform.position, false);
            laserPointer.setLaserVisible(GetComponent<MeshRenderer>(), transform.position, 0.5f, Color.yellow, 0.1f);
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
        hands = player.transform.Find("Main Camera/Hands").gameObject;
        laserPointer = player.GetComponent<laserPointer>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    /**
    * überprüft, ob die Tür geschlossen ist
    * überprüft, ob die Tür abgeschlossen ist
    * Türanimation wird abgespielt, wenn die Tür nicht abgeschlossen ist
    * oder der Player einen Schlüssel hat
    */
    void Update()
    {
        if (useDoor)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
            {
                if (doorClosed == true)
                {
                    if (requiresKey == true)
                    {
                        if (hands.transform.Find("Key"))
                        {
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
                            doorClosed = false;
                            requiresKey = false;
                            Destroy(GameObject.Find("/Player/Main Camera/Hands/Key"));
                        }
                        else
                        {
                            SoundManager.PlayDoorLockedSound();
                        }
                    }
                    else
                    {
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
                        doorClosed = false;
                    }
                }
                else
                {
                    if (doorOpensToRight == true)
                    {
                        animator.SetBool("openDoorRight", false);
                    }
                    else
                    {
                        animator.SetBool("openDoorLeft", false);
                    }
                    doorClosed = true;
                }

                useDoor = false;

            }

        }
    }
}
