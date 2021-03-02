using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * FOV_Camera erstellt zwei Meshes, welche das Sichtfeld der Kamera visualisieren.
 * 
 * Hilfe zur erstellung dieses Scripts: https://www.youtube.com/watch?v=73Dc5JTCmKI&t=382s
 **/

public class FOV_Camera : MonoBehaviour
{
    public float viewRadius;
    [Range(0f, 360f)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    public Transform playerTarget;

    public GameObject uiController;

    public float meshResolution;
    public int edgeResolveIteration;

    public MeshFilter viewMeshFilterUpside;
    Mesh viewMeshUpside;

    public MeshFilter viewMeshFilterDownside;
    Mesh viewMeshDownside;

    public destroyCamera camera;

    public LevelChanger lvlChanger;

    public string loadLevelWhenCaught;

    public bool setPlayerPositionBack;


    void Start()
    {
        viewMeshUpside = new Mesh();
        viewMeshUpside.name = "View Mesh Upside";

        viewMeshFilterUpside.mesh = viewMeshUpside;

        viewMeshDownside = new Mesh();
        viewMeshDownside.name = "View Mesh Downside";

        viewMeshFilterDownside.mesh = viewMeshDownside;

        //Prüft ob Spieler sich im Sichtfeld der Kamera befindet. Der Erkennung wird mit
        //einer Verzögerung von 0,2 Sekunden ausgeführt.
        StartCoroutine("FindTargetsWithDelay", 0.2f);

    }

    // Update is called once per frame
    void Update()
    {
        DrawFOVUpside();
        DrawFOVDownside();
    }

    /**
     * Führt die Methode "findVisibleTargets()" aus solange die Kamera aktiv ist.
     */
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (!camera.camDestroyed)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }

    /**
    * Greift auf den LevelChanger zu wenn ein Spieler erkannt wird. Die Methode wird nach der
    * eingestellten Verzögerung ausgeführt.
    */
    IEnumerator PauseAndResetLevel(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            if (setPlayerPositionBack)
            {
                playerTarget.position = new Vector3(7,3.2f,-20);
                break;
            }
            else
            {
                lvlChanger.FadeToLevel(loadLevelWhenCaught);
            }
            
        }
    }



    /**
    * Bestimmt ob sich der Spieler im Sichtfeld befindet. Überprüft ob die Kamera den Spieler
    * sehen kann.
    */
    void FindVisibleTarget()
    {
        // Alle Objekte die sich im Layer "Target" befinden und durch "OverlapSphere" gefunden wurden werden in
        // einer Liste gespeichert.
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            //Es wird der Richtungsvektor zwischen Ziel und Kamera bestimmt.
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
 
            // Wenn der Winkel zwischen Ziel und Blickrichtung kleiner ist als die hälfte des Sichtfeldes,
            // dann befindet sich das Ziel innerhalb des Sichtfeldes.
            if (Vector3.Angle(transform.localPosition, dirToTarget) < viewAngle / 2)
            {
                // Wenn eine Linie zwischen Ziel und Kamera gezogen werden kann ohen das diese Linie
                // etwas anderes Trifft, wird ein der Spieler als erkannt angesehen.
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    //uiController.GetComponent<UIController>().onDetect();
                    StartCoroutine("PauseAndResetLevel", 1f);
                }
            }
        }
    }

    /*
     * Zeichent die Oberseite des Sichtfeldes
     */
    void DrawFOVUpside()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = (transform.eulerAngles.y) - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                if (oldViewCast.hit != newViewCast.hit)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[((vertexCount - 2) * 3) * 2];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMeshUpside.Clear();
        viewMeshUpside.vertices = vertices;
        viewMeshUpside.triangles = triangles;
        viewMeshUpside.RecalculateNormals();
    }

    /*
     * Zeichnet die Unterseite des Sichtfeldes
     */
    void DrawFOVDownside()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                if (oldViewCast.hit != newViewCast.hit)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[((vertexCount - 2) * 3) * 2];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }

        viewMeshDownside.Clear();
        viewMeshDownside.vertices = vertices;
        viewMeshDownside.triangles = triangles;
        viewMeshDownside.RecalculateNormals();
    }



    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = GetVectorFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIteration; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (newViewCast.hit == minViewCast.hit)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    public Vector3 GetVectorFromAngle(float angle, bool globalAngle)
    {
        if (!globalAngle)
        {
            angle += transform.eulerAngles.y;
        }
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }

    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
