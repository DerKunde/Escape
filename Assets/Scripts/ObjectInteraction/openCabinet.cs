using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openCabinet : MonoBehaviour
{
    private float myTimer = 0;
    public float waitingTime = 2;
    private bool useDoor = false;

    private Animator animator;
    private bool cabinetClosedLeft = true;
    private bool cabinetClosedRight = true;

    private GameObject player;
    private laserPointer laserPointer;
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
            laserPointer.setLaserVisible(GetComponent<MeshRenderer>(), transform.position, 0.01f, Color.yellow, 0.001f);
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
        laserPointer = player.GetComponent<laserPointer>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    /**
     * Türanimation wird abgespielt je nach Name der Tür
     */
    void Update()
    {
        if (useDoor)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
            {
                if (gameObject.name == "doorLeft" && cabinetClosedLeft == true)
                {
                    animator.SetBool("openleft", true);
                    cabinetClosedLeft = false;
                }else if(gameObject.name == "doorLeft" && cabinetClosedLeft == false)
                {
                    animator.SetBool("openleft", false);
                    cabinetClosedLeft = true;
                }

                if (gameObject.name == "doorRight" && cabinetClosedRight == true)
                {
                    animator.SetBool("openright", true);
                    cabinetClosedRight = false;
                }
                else if (gameObject.name == "doorRight" && cabinetClosedRight == false)
                {
                    animator.SetBool("openright", false);
                    cabinetClosedRight = true;
                }

                useDoor = false;

            }

        }
    }
}
