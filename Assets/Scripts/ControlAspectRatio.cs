using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlAspectRatio : MonoBehaviour {
    public float goalRatio;
    public Image target;
    [Range(0f,1f)]
    public float transitionAmount;
    public float speed = 0.1f;
    public Image background;
    public float opacity;

    public float aspectRatio = 1;
    public float currentRatio = 1;
    public void StartTransitionUp()
    {
        //aspectRatio = (float)Screen.width / Screen.height;
        transitionAmount = 0;
        StopAllCoroutines();
        StartCoroutine(TransitionUp());
    }

    private IEnumerator TransitionUp()
    {
        while(transitionAmount < 1)
        {
            transitionAmount += speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transitionAmount = 1;
    }

    public void StartTransitionDown()
    {
        //aspectRatio = (float)Screen.width / Screen.height;
        transitionAmount = 1;
        StopAllCoroutines();
        StartCoroutine(TransitionDown());
    }

    private IEnumerator TransitionDown()
    {
        while (transitionAmount > 0)
        {
            transitionAmount -= speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transitionAmount = 0;
    }

    // Update is called once per frame
    void Update () {
        //currentRatio = aspectRatio + (goalRatio - aspectRatio) * transitionAmount;
        //target.GetComponent<AspectRatioFitter>().aspectRatio = currentRatio;
        background.color = new Color(background.color.r, background.color.g, background.color.b, transitionAmount * opacity);
        target.color = new Color(target.color.r, target.color.g, target.color.b, transitionAmount * opacity);

    }
}
