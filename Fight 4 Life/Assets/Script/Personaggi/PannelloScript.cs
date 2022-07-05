using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelloScript : MonoBehaviour
{
    bool visibile;

    Vector2 posizMouse;

    RectTransform pannRT;
    Animator pannAnim;


    private void Start()
    {
        pannRT = GetComponent<RectTransform>();
        pannAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        posizMouse = Input.mousePosition;

        //Fa vedere o nasconde il pannello (tramite l'animazione)
        pannAnim.SetBool("Visibile", visibile);

        //Porta il pannello sul mouse (con un Lerp)
        if (visibile)
            pannRT.position = Vector3.Lerp(pannRT.position, posizMouse, 10f*Time.deltaTime);
    }

    public void ScriviVisibile(bool v)
    {
        visibile = v;
    }

    public void InvertiVisibile()
    {
        visibile = !visibile;
    }
}
