using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeMetroEsplora : MonoBehaviour
{
    [SerializeField]
    GameObject canvasMetro, canvasEsplora;
    [SerializeField]
    GameObject gruppoAudioMetro, gruppoAudioEsplora;
    [SerializeField]
    AudioSource musDentroMetro, musEsplora;

    [SerializeField]
    bool siVaAdEsplorare = new bool();


    void Update()
    {

        int cambioMus = new int();

        CanvasGroup cG_cMetro = canvasMetro.GetComponent<CanvasGroup>(),
                    cG_cEsplora = canvasEsplora.GetComponentInChildren<CanvasGroup>();

        //Cambia canvas in quella dell'esterno
        if (siVaAdEsplorare)
        {
            //Attiva quella dell'esterno
            canvasEsplora.SetActive(true);

            //Crossfade
            if (cG_cMetro.alpha >= 0.01f && cG_cEsplora.alpha <= 0.99f)
            {
                cG_cMetro.alpha = Mathf.MoveTowards(cG_cMetro.alpha, 0f, 1.5f * Time.deltaTime);
                cG_cEsplora.alpha = Mathf.MoveTowards(cG_cEsplora.alpha, 1f, 1.5f * Time.deltaTime);
            }
            else
            {
                //Se la metro non si vede, la disattiva e rompe il ciclo
                canvasMetro.SetActive(false);
            }

            //Cambia la musica
            cambioMus = 0;
        }
        //Cambia canvas in quella della metro
        else
        {
            //Attiva quella della metro
            canvasMetro.SetActive(true);

            //Crossfade
            if (cG_cEsplora.alpha >= 0.01f && cG_cMetro.alpha <= 0.99f)
            {
                cG_cEsplora.alpha = Mathf.MoveTowards(cG_cEsplora.alpha, 0f, 1.5f * Time.deltaTime);
                cG_cMetro.alpha = Mathf.MoveTowards(cG_cMetro.alpha, 1f, 1.5f * Time.deltaTime);
            }
            else
            {
                //Se l'esterno non si vede, la disattiva e rompe il ciclo
                canvasEsplora.SetActive(false);
            }

            //Cambia la musica
            cambioMus = 1;
        }


        #region Crossfade della musica

        switch (cambioMus)
        {
            //Musica metro --> esterno
            case 0:
                if (musDentroMetro.volume >= 0.01f)
                {
                    gruppoAudioEsplora.SetActive(true);

                    musDentroMetro.volume = Mathf.Lerp(musDentroMetro.volume, 0f, 1.5f * Time.deltaTime);
                    musEsplora.volume = Mathf.Lerp(musEsplora.volume, 1f, 1.5f * Time.deltaTime);
                }
                else
                    gruppoAudioMetro.SetActive(false);

                break;

            //Musica esterno --> metro
            case 1:
                if (musEsplora.volume >= 0.01f)
                {
                    gruppoAudioMetro.SetActive(true);

                    musEsplora.volume = Mathf.Lerp(musEsplora.volume, 0f, 1.5f * Time.deltaTime);
                    musDentroMetro.volume = Mathf.Lerp(musDentroMetro.volume, 0.75f, 1.5f * Time.deltaTime);
                }
                else
                    gruppoAudioEsplora.SetActive(false);

                break;
        }
        #endregion
    }

    #region Funzioni Set personalizzate

    public void ScriviSiVaAdEsplorare(bool v_f)
    {
        siVaAdEsplorare = v_f;
    }

    #endregion
}
