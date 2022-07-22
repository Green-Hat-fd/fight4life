using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ControllaPannello : RilevaOggetto
{
    [SerializeField]
    GameObject pannInfo;

    //Testo dove mettere il nome del personaggio
    TMP_Text nomeTxt;
    //Slider delle statistiche
    Slider vitaSl, fameSl, seteSl, stamSl;


    private void Start()
    {
        
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
        //Fa vedere il nome del personaggio nel pannello
        nomeTxt.text = LeggiStatsPerson().LeggiNome();

        //Indica a che valore si trova ogni stat (negli slider del pannello)
        vitaSl.value = LeggiStatsPerson().LeggiVitaPercent();
        fameSl.value = LeggiStatsPerson().LeggiFamePercent();
        seteSl.value = LeggiStatsPerson().LeggiSetePercent();
        stamSl.value = LeggiStatsPerson().LeggiStaminaPercent();
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
