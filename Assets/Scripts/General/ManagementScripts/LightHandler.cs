using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LightHandler : MonoBehaviour {
	public bool brightStageLights = true;
	
	public float stageBrightness = 0.0f;

	private GameObject[] stageLights;

	
	private Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
		SceneManager.sceneLoaded += UpdateLights;
		stageBrightness = 0.0f;
	}

	public void UpdateLights(Scene scene, LoadSceneMode mode)
	{
		stageLights = GameObject.FindGameObjectsWithTag("StageLights");
	}

	// Update is called once per frame
	void Update () 
	{
		anim.SetBool("Bright", brightStageLights);
		foreach(GameObject o in stageLights)
		{
			o.GetComponent<Light>().intensity = stageBrightness;
		}
	}
}
