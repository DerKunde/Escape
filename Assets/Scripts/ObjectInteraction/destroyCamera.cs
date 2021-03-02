using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyCamera : MonoBehaviour
{
    public Color inactive;
    public Color gazedAt;
    public Color destroyed;
    private MeshRenderer cameraRenderer;
    private float myTimer = 0;
    public float waitingTime = 2;
    private GameObject throwObject;
    private GameObject hands;
    public bool destroyCam = false;
    public bool camDestroyed = false;
    private GameObject player;

    private Rigidbody throwObjectRb;

    public bool destroyMO = false;

    private Animator animator;

    private bool isLerping;
    private float startTime;
    private float lerpTime;

    private Vector3 startPos;
    private Vector3 endPos;

    private pickUp pickUp;

    private laserPointer laserPointer;
    private BoxCollider boxCollider;

    /**
     * gazeAt wird aufgerufen, wenn man dieses Objekt (this.gameObject) anguckt
     * zeigt Laserstrahl
     * überprüft, ob in der Hand ein Objekt und die Kamera intakt ist
     * <param name="gazing">ob der Spieler auf dieses Objekt guckt</param>
     */
    public void gazeAt(bool gazing)
    {
        if (gazing)
        {
            laserPointer.setLaserLength(transform.position, true);
            laserPointer.setLaserVisible(transform.GetChild(0).GetComponent<MeshRenderer>(), transform.position, 0.5f, Color.red, 0.1f);
            laserPointer.setLaserColor(Color.red);
            if (hands.transform.childCount == 1 && !camDestroyed)
            {
                throwObject = hands.transform.GetChild(0).gameObject;
                throwObjectRb = throwObject.GetComponent<Rigidbody>();
                pickUp = throwObject.GetComponent<pickUp>();
                destroyCam = true;
            }

        }
        else if (!camDestroyed)
        {
            myTimer = 0;
            destroyCam = false;
            laserPointer.setLaserInvisible();
        }
        else
        {
            laserPointer.setLaserInvisible();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        cameraRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        cameraRenderer.material.color = inactive;
        player = GameObject.FindWithTag("Player");
        hands = player.transform.Find("Main Camera/Hands").gameObject;
        animator = GetComponent<Animator>();
        laserPointer = player.GetComponent<laserPointer>();
        boxCollider = transform.GetChild(0).GetComponent<BoxCollider>();
        boxCollider = gameObject.AddComponent<BoxCollider>();
    }

    // Update is called once per frame
    /**
     * Objekt aus der Hand wird gegen die Kamera geworfen
     */
    void Update()
    {

        if (destroyCam)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
                {
                    throwObject.transform.SetParent(null);
                    throwObjectRb.detectCollisions = true;
                    StartLerping();
                    destroyCam = false;
            }
            
        }

    }

    /**
     * wenn das geworfene Objekt mit der Kamera kollidiert und das Objekt die Kamera beschädiegen kann,
     * wird die Kamera zerstört und ist nicht mehr aktiv
     * Geräusch wird abgespielt
     * <param name="collision">collision</param>
     */
    private void OnCollisionEnter(Collision collision)
    {
        if(pickUp.canDestroyCam)
        {
            SoundManager.PlayDestroyCamSound();
            cameraRenderer.material.color = destroyed;
            camDestroyed = true;
            animator.enabled = false;
            GetComponent<FOV_Camera>().enabled = false;
            throwObjectRb.useGravity = true;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
                if (destroyMO == true)
            {
                Destroy(throwObject);
            }
        }
    }

    /**
     * setzt Daten zum Bewegen des Objekts fest
     */
    void StartLerping()
    {
        isLerping = true;
        startTime = Time.time;

        startPos = throwObject.transform.position;
        endPos = transform.position;

        throwObjectRb.useGravity = false;

        float dist = Vector3.Distance(startPos, endPos);

        lerpTime = dist * 0.05f;
    }

    /**
     * Objekt wird gegen die Kamera geworfen
     */
    private void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / lerpTime;

            throwObject.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);
            if (percentageComplete >= 1.0f)
            {
                throwObjectRb.useGravity = true;
                isLerping = false;
            }
        }
    }
}
