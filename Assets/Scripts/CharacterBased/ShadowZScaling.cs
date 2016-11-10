using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShadowZScaling : MonoBehaviour {
    static float angle = 60;
    static float scaleRate = 0.1f;
    static float shadowScaleRate = 0.1f;
    static float moveRate = 1.5f;
    static float fadeRate = .1f;
    static float localAngleScaling = .000005f;

    public bool flip = false;
    public RectTransform shadowTransform, imageTransform;
    public Transform localTransform;
    public Image shadowImage;

    private int flipint;
    private float z;

    void Update () {
        Vector3 loc = Camera.main.WorldToScreenPoint(localTransform.position) - new Vector3(Screen.width,Screen.height,0);
        z = transform.localPosition.z;
        flipint = 1;

        if (flip)
            flipint = -1;

        //The magin number here is to scale arc tan to be in the right-ish range(1/~2.94)
        shadowTransform.localScale = new Vector2(Mathf.Max(1 - z * shadowScaleRate,0), Mathf.Max(1 - z * shadowScaleRate, 0));
        imageTransform.localScale = new Vector2(1 + z * scaleRate,1 + z * scaleRate);
        shadowImage.color = new Color(0, 0, 0,  .9f - z * fadeRate);

        shadowTransform.localPosition = moveRate * (loc * localAngleScaling +  new Vector3( -z * flipint *  Mathf.Cos(Mathf.Deg2Rad * angle), -z * Mathf.Sin(Mathf.Deg2Rad * angle), z));
        imageTransform.localPosition = moveRate * (-1* loc * localAngleScaling + new Vector3( z * flipint * Mathf.Cos(Mathf.Deg2Rad * angle), z * Mathf.Sin(Mathf.Deg2Rad * angle),z));
    }
}
