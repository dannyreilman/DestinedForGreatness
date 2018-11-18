using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchableInstance : MonoBehaviour {
	public delegate void EndOfFrame();
	public static EndOfFrame frame_end;

	void LateUpdate () {
		if(frame_end != null)
			frame_end();		
	}
}
