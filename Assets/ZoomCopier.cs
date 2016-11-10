using UnityEngine;
using System.Collections;

public class ZoomCopier : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        GetComponent<Camera>().orthographicSize = GetComponentInParent<Camera>().orthographicSize;
	}
}
