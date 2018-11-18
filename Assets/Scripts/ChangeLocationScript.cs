using UnityEngine;
using System.Collections;

//Script to change the location of character models (usually in animation). 
//This is a simple wrapper to handle offsets and such
public class ChangeLocationScript : MonoBehaviour 
{
	public static float[] layerPositions = {25.0f, 45.0f, 65.0f};
	public static float[] layerVerticalOffsets = {1.0f, 10.0f, 15.0f};

	public static float[] outOfSightPos = {33.0f, 33.0f, 50.0f};

	private float layerVertical = 25.0f;
	
	private float layerPos = 1.0f;

	private static float LAYER_V = 90.0f;	


	public static float OFFSET_ROTATION = 0;
	
	public Vector2 animationLocation;

	public float rotx;
	public float roty;
	public float rotz; 

	public Vector2 movementLocation;
	public float attackRotx;
	public float attackRoty;
	public float attackRotz;
	public Vector2 attackLocation;
	public int layer;
	
	void Start()
	{
		animationLocation = new Vector2(0,-100);
		rotx = 0;
		roty = 0;
		rotz = 0;
		movementLocation = new Vector2(0,-100);
		attackLocation = new Vector2(0,0);
		transform.parent.position = GetActualPosition();

		transform.SetPositionAndRotation(GetActualPosition(), Quaternion.Euler(rotx, roty, rotz));
		transform.Rotate(OFFSET_ROTATION, 0, 0);
	}

	public Vector3 GetMovementLocation()
	{
		return new Vector3(movementLocation.x, movementLocation.y + layerVertical, layerPos);
	}
	
	public Vector3 GetActualPosition()
	{
		Vector2 sum = animationLocation + attackLocation + movementLocation;
		return new Vector3(sum.x, sum.y + layerVertical, layerPos);
	}

	// Update is called once per frame
	void Update () {
		if(layerPos < layerPositions[layer])
		{
			layerPos = Mathf.Min(layerPos + LAYER_V * Time.deltaTime, layerPositions[layer]);
		}
		else if(layerPos > layerPositions[layer])
		{
			layerPos = Mathf.Max(layerPos - LAYER_V * Time.deltaTime, layerPositions[layer]);
		}

		if(layerVertical < layerVerticalOffsets[layer])
		{
			layerVertical = Mathf.Min(layerVertical + LAYER_V * Time.deltaTime, layerVerticalOffsets[layer]);
		}
		else if(layerVertical > layerVerticalOffsets[layer])
		{
			layerVertical = Mathf.Max(layerVertical - LAYER_V * Time.deltaTime, layerVerticalOffsets[layer]);
		}

		transform.SetPositionAndRotation(GetActualPosition(), Quaternion.Euler(rotx + attackRotx, roty + attackRoty, rotz + attackRotz));
		transform.Rotate(OFFSET_ROTATION, 0, 0);
	}


}
