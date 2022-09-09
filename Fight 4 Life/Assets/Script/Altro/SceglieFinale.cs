using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SceglieFinale : MonoBehaviour
{
    [SerializeField]
    GameObject gruppoPerson;
    CharacterStats[] personStatsScript = new CharacterStats[4];

    ManagerRisorse managRisScript;
    OpzioniMainScript opzScript;
    CreaOggetti creaObjScript;
    SalvataggiMainScript salvScript;


    private void Awake()
    {
        managRisScript = FindObjectOfType<ManagerRisorse>();
        opzScript = FindObjectOfType<OpzioniMainScript>();
        creaObjScript = FindObjectOfType<CreaOggetti>();
        salvScript = FindObjectOfType<SalvataggiMainScript>();


        gruppoPerson = GameObject.FindGameObjectWithTag("Elenco-dei-Personaggi");

        if (personStatsScript.Length != 4)
            personStatsScript = new CharacterStats[4];

        for (int i = 0; i < 4; i++)
        {
            //Prende tutti i figli del gruppo dei personaggi e li assegna alla variabile
            personStatsScript[i] = gruppoPerson.transform.GetChild(i).GetComponent<CharacterStats>();
        }
    }

    void Update()
    {
        #region NON_USATO_Rileva se la scena è stata cambiata

        //GameObject cambioSc = GameObject.FindGameObjectWithTag("Cambio-scena");
        //if (cambioSc != null)
        //{
        //    Awake();

        //    Destroy(cambioSc);
        //}
        #endregion

        int mortiInTutto = 0;


        foreach(CharacterStats cS in personStatsScript)
        {
            //Controlla chi è morto e aggiunge 1 alla sommatoria
            if (cS.LeggiSonoMorto())
                mortiInTutto++;
        }

        //Se abbiamo trovato la radio
        if (managRisScript.LeggiRadioTrovata())
            creaObjScript.CreaOggettoSegnalatoreGoodEnd();

        //Se sono morti tutti
        if (mortiInTutto >= 4)
            creaObjScript.CreaOggettoSegnalatoreBadEnd();


        #region Rileva se c'è l'oggetto per il Finale felice

        GameObject segnalaGoodEnd = GameObject.FindGameObjectWithTag("Good-Ending");

        //Se trova l'oggetto "segnalatore", sposta nel Good End e distrugge l'oggetto
        if (segnalaGoodEnd != null)
        {
            opzScript.ScenaScegliTu(4);  //___Finale felice - Good ending___//
            salvScript.CancellaFileSalvataggio();

            gameObject.SetActive(false);
            Destroy(segnalaGoodEnd);
        }
        #endregion

        #region Rileva se c'è l'oggetto per il Finale cattivo

        GameObject segnalaBadEnd = GameObject.FindGameObjectWithTag("Bad-Ending");

        //Se trova l'oggetto "segnalatore", sposta nel Bad End e distrugge l'oggetto
        if (segnalaBadEnd != null)
        {
            opzScript.ScenaScegliTu(3);  //___Finale cattivo - Bad ending___//
            salvScript.CancellaFileSalvataggio();

            gameObject.SetActive(false);
            Destroy(segnalaBadEnd);
        }
        #endregion
    }
}
