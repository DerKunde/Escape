using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class SecurityNPC : MonoBehaviour
{
    NavMeshAgent nmA;
    Rigidbody rb;

    private Transform Target;
    private Transform[] WayPoints;
    public Transform WayPointParent;
    public int Cur_WayPoint;
    public float speed, stop_distance;
    public float PauseTimer;
    public bool hasFixedPosition;

    [SerializeField]
    private float Cur_timer;


    // Start is called before the first frame update
    void Start()
    {
        nmA = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        WayPoints = WayPointParent.GetComponentsInChildren<Transform>();
        Target = WayPoints[Cur_WayPoint];
        Cur_timer = PauseTimer;
    }

    // Update is called once per frame
    void Update()
    {

        if (hasFixedPosition) return;
        // Dem NavMesh Agent werden die die eingestellten Ergebnisse übergeben.
        nmA.acceleration = speed;
        nmA.stoppingDistance = stop_distance;

        if (Cur_WayPoint < 0) return;
        

        //Die Distanz zwischen NPC und Wegpunkt wird berechnet.
        float distance = Vector3.Distance(transform.position, Target.position);

        //NPC läuft weiter auf Punkt zu, solange die Stop Distanz kleiner ist als die aktuelle Distanz.
        if(distance > stop_distance && WayPoints.Length > 0)
        {
            Target = WayPoints[Cur_WayPoint];
        }

        //Stop Distanz ist kleiner als aktuelle Distanz
        else if(distance <= stop_distance && WayPoints.Length > 0)
        {
            //NPC pausiert am erreichten Punkt bis der Timer bei 0 ist.
            if (Cur_timer > 0)
            {
                Cur_timer -= 0.01f;
            }

            //Timer erreicht 0, Wegpunkt wird um eins erhöht. Das Ziel wird auf den nächsten Wegpunkt eingestellt.
            //Ist das Ziel das letzte Elment der List, wird der erste Wegpunkt der Liste angesteuert.
           if(Cur_timer <= 0)
            {
                Cur_WayPoint++;
                if(Cur_WayPoint >= WayPoints.Length)
                {
                    Cur_WayPoint = 0;
                }
                Target = WayPoints[Cur_WayPoint];
                Cur_timer = PauseTimer;
            }
        }

        // NavMesh Agent bewegt den NPC zu der unter Target gespeicherten Position.
        nmA.SetDestination(Target.position);
    }
}
