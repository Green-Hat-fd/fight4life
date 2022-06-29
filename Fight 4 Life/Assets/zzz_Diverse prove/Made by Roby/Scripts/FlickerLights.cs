using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLights : MonoBehaviour
{
    private SpriteRenderer Renderer;
    private WaitForSeconds ThreeSecs = new WaitForSeconds(3f);

    private Color SpriteColor;
    private float OriginalAlpha;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Waiter());

        SpriteColor = Renderer.color;
        OriginalAlpha = SpriteColor.a;
    }

    IEnumerator Waiter()
    {
        while (true)
        {
            //Range di tempo tra una luce all'altra
            int WaitTime = Random.Range(1, 5);

            yield return new WaitForSecondsRealtime(WaitTime);

            //Qua la accende
            SpriteColor.a = 0f;
            Renderer.color = SpriteColor;

            yield return ThreeSecs;

            //Qua la porta come è impostata nell'inspector
            SpriteColor.a = OriginalAlpha;
            Renderer.color = SpriteColor;

        }
    }
}
