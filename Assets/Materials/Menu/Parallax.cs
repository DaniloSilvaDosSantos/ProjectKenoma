using UnityEngine;



public class Parallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = .3f;
   private Vector3 startPosition;
   private Vector3 velocity = Vector3.zero;

    void Start()
    {
       startPosition = transform.position; 
    }

   
    void Update()
    {
       Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
       Vector3 target = startPosition + new Vector3(offset.x, offset.y, 0f) * offsetMultiplier;
       transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);

    }
}
