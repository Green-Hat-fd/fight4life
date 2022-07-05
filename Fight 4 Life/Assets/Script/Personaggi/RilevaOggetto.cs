using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RilevaOggetto : MonoBehaviour
{
    [SerializeField]
    GameObject pannInfo;


    private void OnTriggerExit2D(Collider2D collision)
    {
        //Se trascina e rilascia il mouse con la risorsa
        if (!collision.GetComponent<DragAndDrop>().LeggiDrag())
        {
            //Se ha mangiato...
            if (collision.CompareTag("Food"))
                GetComponentInParent<CharacterStats>().ScriviMangiato(true);

            //Se ha bevuto...
            if(collision.CompareTag("Water"))
                GetComponentInParent<CharacterStats>().ScriviBevuto(true);
        }
    }

    #region Funz. per il mouse (Pannello)

    private void OnMouseDown()
    {
        //Se il mouse clicca dentro, mostra il pannello delle informazioni
        //pannInfo.GetComponent<PannelloScript>().ScriviVisibile(true);
        pannInfo.GetComponent<PannelloScript>().InvertiVisibile();

        /* Nota:
         * se vuoi tener premuto --> ScriviVisibile(true)
         * se vuoi clicchi una volta --> InvertiVisibile()
         */
    }

    private void OnMouseExit()
    {
        //Se il mouse esce, nasconde il pannello delle info
        pannInfo.GetComponent<PannelloScript>().ScriviVisibile(false);
    }

    //Se rilascia il mouse
    private void OnMouseUp()
    {
        //Se il mouse viene rilasciato, nasconde il pannello delle info
        //pannInfo.GetComponent<PannelloScript>().ScriviVisibile(false);

        /* Nota:
         * se vuoi tener premuto --> togli il commento
         * se vuoi clicchi una volta --> metti la riga sopra a commento
         */
    }

    #endregion
}
