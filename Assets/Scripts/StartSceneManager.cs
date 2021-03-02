using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool endScene;
    public static int itemCount;
    public GameObject goat;

    void Start()
    {
        if (endScene)
        {
            goat.transform.Rotate(90, 0, 0);
           // goat.transform.position = new Vector3(0, 2, 0);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
