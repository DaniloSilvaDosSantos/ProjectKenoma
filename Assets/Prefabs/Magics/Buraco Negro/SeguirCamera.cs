using UnityEngine;

public class SeguirCamera : MonoBehaviour
{
    Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    
    void Update()
    {
       transform.forward = _cam.transform.position - transform.position; 
    }
}
