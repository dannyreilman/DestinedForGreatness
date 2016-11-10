using UnityEngine;
using System.Collections;
using UnityEditor;

public class Shaking : MonoBehaviour {
    public RectTransform shakeObject, shadowObject;
    public float duration;
    public float speed = 1;
    public float angleRange;

    public float randAngle;
    float convertedBaseAngle;
    public float baseAngle;
    private float soFar = 0;
    public Character character;

    void Start()
    {
        float angle = Random.Range(-angleRange, angleRange);
        baseAngle = 0;
    }

    // Update is called once per frame
	public void CallableUpdate (float angle) {
        baseAngle = angle;
        convertedBaseAngle = -baseAngle + randAngle;
        if (gameObject.GetComponent<Character>().IsAlive())
        {
            soFar += Time.deltaTime;
            
            if (soFar >= (duration/speed))
            {
                soFar -= duration/speed;
                Shake();
            }

            shakeObject.eulerAngles = new Vector3(0, 0, convertedBaseAngle);
            shadowObject.eulerAngles = new Vector3(0, 0, convertedBaseAngle);
        }
        else
        {
            shakeObject.eulerAngles = new Vector3(0, 0, 0);
            shadowObject.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void Shake()
    {
        randAngle = Random.Range(-angleRange, angleRange);
    }

    public float GetBaseAngle()
    {
        return baseAngle;
    }
}
