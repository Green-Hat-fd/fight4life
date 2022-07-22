using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RilevaOggetto : MonoBehaviour
{ 
    [SerializeField]
    ManagerRisorse managerRisorse;

    //Il codice con le statistiche dell'Empty attaccato al personaggio
    //[SerializeField]
    CharacterStats statsDelPerson;


    private void Start()
    {
        statsDelPerson = GetComponentInParent<CharacterStats>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool cibo_disponibile = managerRisorse.LeggiCibo() > 0,
             acqua_disponibile = managerRisorse.LeggiAcqua() > 0,
             medicine_disponibili = managerRisorse.LeggiMedicine() > 0;

        bool hoFame = statsDelPerson.LeggiFamePercent() > 0f,
             hoSete = statsDelPerson.LeggiSetePercent() > 0f,
             sonoFerito = statsDelPerson.LeggiVitaPercent() > 0f;


        //Se trascina e rilascia il mouse con la risorsa
        if (!collision.GetComponent<DragAndDrop>().LeggiDrag())
        {
            //Se ha mangiato...
            if (collision.CompareTag("Food") && cibo_disponibile && hoFame)
            {
                GetComponentInParent<CharacterStats>().ScriviMangiato(true);
                managerRisorse.TogliCibo();
            }

            //Se ha bevuto...
            if(collision.CompareTag("Water") && acqua_disponibile && hoSete)
            {
                GetComponentInParent<CharacterStats>().ScriviBevuto(true);
                managerRisorse.TogliAcqua();
            }

            //Se ha ricevuto delle cure...
            if(collision.CompareTag("Medicine") && medicine_disponibili && sonoFerito)
            {
                GetComponentInParent<CharacterStats>().ScriviCurato(true);
                managerRisorse.TogliMedicine();
            }
        }
    }

    public CharacterStats LeggiStatsPerson()
    {
        return statsDelPerson;
    }
}
