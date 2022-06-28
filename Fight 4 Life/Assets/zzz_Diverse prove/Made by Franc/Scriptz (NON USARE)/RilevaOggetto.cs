using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RilevaOggetto : MonoBehaviour
{
    //Variabili per statistiche
    bool mangiato = false,
         bevuto = false;


    private void OnTriggerExit2D(Collider2D collision)
    {
        //Se trascina e rilascia il mouse
        if (!collision.GetComponent<DragAndDrop>().isDragging)
        {
            if(collision.CompareTag("Food"))
                print($"<color=brown>Mangiato {gameObject.name}</color>");
            if(collision.CompareTag("Water"))
                print($"<color=blue>Bevuto {gameObject.name}</color>");
        }
        /*
         * ----Nota: mettere ancora il sistema 
         */
    }
}
