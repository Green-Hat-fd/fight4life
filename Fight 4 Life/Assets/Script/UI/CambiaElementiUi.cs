using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CambiaElementiUi : MonoBehaviour
{
    [SerializeField]
    GameObject gruppoPulsantiMandaInEspl;
    [SerializeField]
    Button pulsanteEsplora;
    [SerializeField]
    Button[] pulsMandaEspl;
    
    SalvataggiMainScript salvScript;


    private void Awake()
    {
        salvScript = FindObjectOfType<SalvataggiMainScript>();

        if(pulsMandaEspl.Length != 4)
            pulsMandaEspl = new Button[4];

        for (int i=0; i < pulsMandaEspl.Length; i++)
        {
            //Prende tutti i figli del gruppo dei personaggi e li assegna alla variabile
            pulsMandaEspl[i] = gruppoPulsantiMandaInEspl.transform.GetChild(i).GetComponent<Button>();
        }
    }

    void Update()
    {
        int quantiStanchi = 0;

        //Gira tra tutti i personaggi e controlla la loro Stanchezza
        for (int i=0; i < salvScript.LeggiPersonStatsScript().Length; i++)
        {
            var chStats = salvScript.LeggiPersonStatsScript()[i];

            //Se è troppo stanco
            if (chStats.LeggiStanchPercent() >= 0.9f)
            {
                //Disabilita il suo bottone per mandarlo/a in esplorazione
                pulsMandaEspl[i].interactable = false;

                quantiStanchi++;
            }
            else
            {
                pulsMandaEspl[i].interactable = true;
            }
        }

        //Se sono tutti stanchi, disabilita il pulsante per mandare in esplorazione
        pulsanteEsplora.interactable = quantiStanchi < 4;
    }
}
