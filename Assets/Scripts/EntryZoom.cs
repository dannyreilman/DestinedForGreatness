using UnityEngine;
using System.Collections;

public class EntryZoom : MonoBehaviour {
    public float finalZoom;
    private float speed = 50;
	

	// Update is called once per frame
	void Update () {
        if (GetComponent<Camera>().orthographicSize > finalZoom)
        {
            GetComponent<Camera>().orthographicSize -= speed * Time.deltaTime;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = finalZoom;
            Destroy(this);
        }

    }
}
