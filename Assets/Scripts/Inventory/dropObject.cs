using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropObject : MonoBehaviour
{
    private float myTimer = 0;
    public float waitingTime = 2;
    private GameObject hands;
    private bool dropOb = false;
    public GameObject cam;

    private GameObject obToDrop;
    private Rigidbody obToDropRB;

    private GameObject player;

    private laserPointer laserPointer;

    /**
     * gazeAt wird aufgerufen, wenn man dieses Objekt (this.gameObject) anguckt
     * zeigt Laserstrahl
     * überprüft, ob in der Hand ein Objekt ist
     * <param name="gazing">ob der Spieler auf dieses Objekt guckt</param>
     */
    public void gazeAt(bool gazing)
    {
        if (gazing)
        {
            laserPointer.setLaserLength(transform.position, false);
            laserPointer.setLaserVisible(GetComponent<MeshRenderer>(), transform.position, 0.5f, Color.white, 0.1f);
            laserPointer.setLaserColor(Color.white);
            if (hands.transform.childCount > 0)
            {
                obToDrop = hands.transform.GetChild(0).gameObject;
                obToDropRB = hands.transform.GetComponentInChildren<Rigidbody>();
                dropOb = true;
            }
        }
        else
        {
            myTimer = 0;
            dropOb = false;
            laserPointer.setLaserInvisible();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        hands = player.transform.Find("Main Camera/Hands").gameObject;
        laserPointer = player.GetComponent<laserPointer>();
    }

    // Update is called once per frame
    /**
     * Objekt in der Hand wird aus der Hand geworfen
     */
    void Update()
    {

        if (dropOb)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
            {
                obToDrop.transform.SetParent(null);
                obToDropRB.useGravity = true;
                obToDropRB.detectCollisions = true;
                obToDropRB.velocity = cam.transform.rotation * Vector3.forward * 8;
                dropOb = false;
            }

        }
    }
}
