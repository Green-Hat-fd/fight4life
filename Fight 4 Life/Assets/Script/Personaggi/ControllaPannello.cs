using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ControllaPannello : MonoBehaviour
{
    RilevaOggetto scriptGenitore;
    
    [SerializeField]
    GameObject pannInfo;

    //Testo dove mettere il nome del personaggio
    TMP_Text nomeTxt;
    //Slider delle statistiche
    Slider vitaSl, fameSl, seteSl, stamSl;


    private void Start()
    {
        scriptGenitore = GetComponent<RilevaOggetto>();

        //Prende tutti i figli dal pannello e li assegna alle rispettive variabili
        PrendiNelPannello();
    }

    #region Funz. per il mouse (Rileva Pannello)

    private void OnMouseDown()
    {
        //Se il mouse clicca (non sulla UI) mostra il pannello delle informazioni
        if (!IsMouseSopraUI())
        {
            pannInfo.GetComponent<PannelloScript>().InvertiVisibile();

            //pannInfo.GetComponent<PannelloScript>().ScriviVisibile(true);

            /* Nota:
             * se vuoi tener premuto --> ScriviVisibile(true)
             * se vuoi clicchi una volta --> InvertiVisibile()
             */

        }

        //Cambia il pannello rispetto al personaggio sul quale hai cliccato
        CambiaPannello();
    }

    private void OnMouseExit()
    {
        //Se il mouse esce, nasconde il pannello delle info
        pannInfo.GetComponent<PannelloScript>().ScriviVisibile(false);
    }

    private void OnMouseUp()
    {
        //Se il mouse viene rilasciato, nasconde il pannello delle info
        //pannInfo.GetComponent<PannelloScript>().ScriviVisibile(false);

        /* Nota:
         * se vuoi tener premuto --> togli il commento
         * se vuoi clicchi una volta --> metti la riga sopra a commento
         */
    }

    //Controlla se il mouse si trova sopra la UI
    bool IsMouseSopraUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    #endregion

    #region Funz. per scrivere sul pannello

    void CambiaPannello()
    {
        bool nonHoVita = scriptGenitore.LeggiStatsPerson().LeggiVitaPercent() <= 0,
             nonHoFame = scriptGenitore.LeggiStatsPerson().LeggiFamePercent() <= 0,
             nonHoSete = scriptGenitore.LeggiStatsPerson().LeggiSetePercent() <= 0,
             nonHoStam = scriptGenitore.LeggiStatsPerson().LeggiStanchPercent() <= 0;

        //Fa vedere il nome del personaggio nel pannello
        nomeTxt.text = scriptGenitore.LeggiStatsPerson().LeggiNome();

        //Toglie gli slider se sono arrivati a 0
        vitaSl.fillRect.GetComponent<Image>().enabled = nonHoVita ? false : true;
        fameSl.fillRect.GetComponent<Image>().enabled = nonHoFame ? false : true;
        seteSl.fillRect.GetComponent<Image>().enabled = nonHoSete ? false : true;
        stamSl.fillRect.GetComponent<Image>().enabled = nonHoStam ? false : true;

        //Indica a che valore si trova ogni stat (negli slider del pannello)
        vitaSl.value = scriptGenitore.LeggiStatsPerson().LeggiVitaPercent();
        fameSl.value = scriptGenitore.LeggiStatsPerson().LeggiFamePercent();
        seteSl.value = scriptGenitore.LeggiStatsPerson().LeggiSetePercent();
        stamSl.value = scriptGenitore.LeggiStatsPerson().LeggiStanchPercent();
    }

    void PrendiNelPannello()
    {
        //Testo (il nome del personaggio)
        nomeTxt = pannInfo.transform.GetChild(0).GetComponent<TMP_Text>();

        //Slider (delle statistiche)
        Transform gruppoSlider = pannInfo.transform.GetChild(1);

        vitaSl = gruppoSlider.GetChild(0).GetComponent<Slider>();
        fameSl = gruppoSlider.GetChild(1).GetComponent<Slider>();
        seteSl = gruppoSlider.GetChild(2).GetComponent<Slider>();
        stamSl = gruppoSlider.GetChild(3).GetComponent<Slider>();
    }

    #endregion
}
