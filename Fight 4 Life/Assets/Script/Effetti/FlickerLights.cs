using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLights : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    [SerializeField]
    AudioSource luceSFX;


    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Waiter());
        
        myRenderer.enabled = false;
    }

    IEnumerator Waiter()
    {
        float waitTime, inScreenTime;

        while (true)
        {
            //Range di tempo tra una luce all'altra (in sec)
            waitTime = Random.Range(2.5f, 30f);
            yield return new WaitForSecondsRealtime(waitTime);

            //Qua spegne la luce
            myRenderer.enabled = true;


            //Range di quanto la luce si deve vedere (in sec)
            inScreenTime = Random.Range(0.1f, 1f);
            yield return new WaitForSeconds(inScreenTime);

            //Qua accende la luce (quindi l'img nera non si vede)
            myRenderer.enabled = false;
            luceSFX.PlayOneShot(luceSFX.clip);  //Con suono
        }
    }
}
