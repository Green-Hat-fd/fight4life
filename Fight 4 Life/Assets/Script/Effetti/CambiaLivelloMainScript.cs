using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CambiaLivelloMainScript : MonoBehaviour
{
    [SerializeField]
    Animator cambioScena_anim;
    [SerializeField]
    GameObject bloccaInteraz;

    //Variabile dummy per testare in quale scena sei
    Scene scenaAttuale;

    CreaOggetti creaObjScript;


    private void Start()
    {
        scenaAttuale = SceneManager.GetActiveScene();
        creaObjScript = GameObject.FindObjectOfType<CreaOggetti>();
    }

    void Update()
    {
    //----VECCHIO METODO PER CAPIRE SE L'ANIMATORE AVESSE FINITO----
    //   if (cambioScena_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f)
    //       for (int i = 0; i < gameObject.transform.childCount; i++)
    //           gameObject.transform.GetChild(i).gameObject.SetActive(false);
    //   else
    //       for (int i = 0; i < gameObject.transform.childCount; i++)
    //           gameObject.transform.GetChild(i).gameObject.SetActive(true);

        //Non appena si cambia la scena
        if (scenaAttuale != SceneManager.GetActiveScene())
        {
            //Crea l'oggetto cambioscena
            //creaObjScript.CreaOggettoSegnalatoreCambioScena();

            //Riproduce l'animazione di far sparire l'img nera...
            cambioScena_anim.SetBool("Scena in cambio", false);

            //Rimette le interazioni per gli sprite
            bloccaInteraz.SetActive(false);

            //...e si aggiorna il counter della scena
            scenaAttuale = SceneManager.GetActiveScene();
        }


    }
}
