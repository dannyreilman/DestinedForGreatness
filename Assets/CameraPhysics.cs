using UnityEngine;
using System.Collections;

public class CameraPhysics : MonoBehaviour {
    public static float TERMINAL_VELOCITY = 50f;
    public static float GRAVITY = 1f;
    public static float SOFT_LAND_OFFSET = .5f;
    public static float SOFT_LAND_SPEED = .5f;
    public float finalZoom = 5f;
    public float speed;
    
    // Use this for initialization
	void Start () {
        speed = -TERMINAL_VELOCITY;   
	}
	
	// Update is called once per frame
	void Update () {
        float cameraZoom = GetComponent<Camera>().orthographicSize;
	    if(cameraZoom > finalZoom + SOFT_LAND_OFFSET)
        {
            if (speed < 0)
            {
                speed = Mathf.Max(speed - GRAVITY, -TERMINAL_VELOCITY);
            }
            else
            {
                speed = Mathf.Min(speed - GRAVITY, TERMINAL_VELOCITY);
            }
            cameraZoom += speed * Time.deltaTime;
        }
        else if(cameraZoom > finalZoom)
        {
            speed = -SOFT_LAND_SPEED;
            cameraZoom += speed * Time.deltaTime;
        }
        else
        {
            cameraZoom = finalZoom;
            speed = 0;
        }
        GetComponent<Camera>().orthographicSize = cameraZoom;
    }
}
