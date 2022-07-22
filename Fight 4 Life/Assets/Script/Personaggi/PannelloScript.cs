using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelloScript : MonoBehaviour
{
    bool visibile;

    Vector2 posizMouse;

    RectTransform pannRT;
    Animator pannAnim;
    Vector3[] pannAngoli = new Vector3[4];


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

    #region Controlla e sistema il pannello, rendendolo sempre visibile a schermo

    private void LateUpdate()
    {
        float lungh, altez, distX, distY;

        pannRT.GetWorldCorners(pannAngoli);


        lungh = pannAngoli[2].x - pannAngoli[0].x;
        altez = pannAngoli[1].y - pannAngoli[0].y;

        if (visibile)
        {
            //Rileva se il pannello mette "il piede" fuori schermo...
            //...calcolando la distanza dai bordi del pannello ai bordi dello schermo
            distX = posizMouse.x - lungh; 
            distY = posizMouse.y - altez;

            #region Nota -- per switchare i controlli del bordo
            /* In DistX, rimpiazza il meno con un più & aggiungi "+ Screen.width" per renderlo a sx
             * In DistY, rimpiazza il meno con un più & aggiungi "+ Screen.height" per renderlo in alto
             */
            #endregion


            #region Se si trova fuori dello schermo (a sx) porta il pannello a dx
            
            if (distX < 0)
                pannRT.pivot = CambiaPivot(pannRT.pivot, 0, pannRT.pivot.y);  //Si vede a destra
            else
                pannRT.pivot = CambiaPivot(pannRT.pivot, 1, pannRT.pivot.y);  //Si vede a sinistra (default)

            #endregion

            #region Se si trova fuori dello schermo (in basso) porta il pannello in alto

            if (distY < 0)
                pannRT.pivot = CambiaPivot(pannRT.pivot, pannRT.pivot.x, 0);  //Si vede in alto
            else
                pannRT.pivot = CambiaPivot(pannRT.pivot, pannRT.pivot.x, 1);  //Si vede in basso (default)

            #endregion
        }
    }

    Vector2 CambiaPivot(Vector2 posIniz, float x, float y)
    {
        return Vector2.LerpUnclamped(posIniz, new Vector2(x, y), 10f * Time.deltaTime);
    }

    #endregion 

    public void InvertiVisibile()
    {
        visibile = !visibile;
    }

    public void ScriviVisibile(bool v)
    {
        visibile = v;
    }

    public bool LeggiVisibile()
    {
        return visibile;
    }
}
