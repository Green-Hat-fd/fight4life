using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OpzioniMainScript : MonoBehaviour
{
    //Menu principale
    #region Cambia scena
    public static void ScenaScegliTu_Trigger(int numScena)
    {
        SceneManager.LoadSceneAsync(numScena);
    }
    public void ScenaScegliTu(int numScena)
    {
        StartCoroutine(AnimazioneCambioScena(numScena));
    }
    public void ScenaSuccessiva()
    {
        int scenaOra = SceneManager.GetActiveScene().buildIndex;

        StartCoroutine(AnimazioneCambioScena(++scenaOra));
    }
    public void ScenaPrecedente()
    {
        int scenaOra = SceneManager.GetActiveScene().buildIndex;

        StartCoroutine(AnimazioneCambioScena(--scenaOra));
    }

    #endregion


    #region Esci

    public void EsciDalGioco()
    {
        Application.Quit();
    }

    #endregion


    //Opzioni
    #region Testo da cambiare

    [Header("##  Testo da sostituire  ##")]
    [SerializeField]
    TMP_Text TestoDaCamb;

    [SerializeField]
    string inPiu;


    //Cambia il testo quando il valore cambia
    public void CambiaTesto_Stringa(string t)
    {
        TestoDaCamb.text = t + inPiu;
    }
    public void CambiaTesto_FloatIntero(float t) //Bleh
    {
        TestoDaCamb.text = t + inPiu;
    }
    public void CambiaTesto_daFloatAInt(float t)
    {
        TestoDaCamb.text = Mathf.RoundToInt(t) + inPiu;
    }

    public void CambiaTestoAppross(float t)
    {
        TestoDaCamb.text = t/100f + inPiu;
    }

    #endregion


    #region Selezione Lingua

    public static int linguaScelta;

    public void CambiaLingua(int l)
    {
        linguaScelta = l;
        //0: Italiano
        //1: English (default negli switch)
    }

    #endregion


    #region Volume

    public static float volumeMusica = 1f,
                        volumeSuoni = 1f;

    public void CambiaVolumeMusica(float v)
    {
        volumeMusica = v/100f;
    }

    public void CambiaVolumeSuoni(float v)
    {
        volumeSuoni = v/100f;
    }

    #endregion


    #region Schermo intero

    public void SchermoIntero_OnOff(bool yn)
    {
        Screen.fullScreen = yn;
    }

    #endregion


    //Altro
    #region Altre funzioni

        // Animazione per il cambio della scena //
    [SerializeField, Space(20)]
    GameObject cambiaLivelloObj;
    [SerializeField]
    Animator cambioScena_anim;

    IEnumerator AnimazioneCambioScena(int s)
    {
        cambiaLivelloObj.SetActive(true);

        for (int i = 0; i < cambiaLivelloObj.transform.childCount; i++)
        {
            cambiaLivelloObj.transform.GetChild(i).gameObject.SetActive(true);
        }

        cambioScena_anim.SetBool("Scena in cambio", true);


            yield return new WaitForSeconds(1f);


        SceneManager.LoadSceneAsync(s);
    }


        // Altre funzioni qui //

    #endregion


    //Unity
    private void Update()
    {
        //Se non trova il "CambioScena", allora prende quello che trova
        if (cambioScena_anim == null)
            cambioScena_anim = GameObject.FindGameObjectWithTag("CambioScena-Anim").GetComponent<Animator>();
        if (cambiaLivelloObj == null)
            cambiaLivelloObj = GameObject.FindGameObjectWithTag("CambioScena-Obj");
    }
}
