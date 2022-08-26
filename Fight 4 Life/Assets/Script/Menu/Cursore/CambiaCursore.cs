using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiaCursore : MonoBehaviour
{
    enum TCursore
    {
        Default,
        Collegamento,
        Prendi,
        Caricamento
    }

    [SerializeField, Space(10)]
    TCursore mouseQuandoEntra, mouseQuandoTracina, mouseQuandoRilascia, mouseQuandoEsce;


    #region Funzioni di quando il mouse fa cose

    //Il mouse ENTRA
    private void OnMouseEnter()
    {
        #region OLD_DEBUG
        //print("<color=green>Cursore mouse entrato</color>");
        #endregion
        RichiamaIlCambioCursore(mouseQuandoEntra);
    }

    //Il mouse TRASCINA
    private void OnMouseDrag()
    {
        RichiamaIlCambioCursore(mouseQuandoTracina);
    }

    //Il mouse viene RILASCIATO
    private void OnMouseUp()
    {
        RichiamaIlCambioCursore(mouseQuandoRilascia);
    }

    //Il mouse ESCE
    private void OnMouseExit()
    {
        RichiamaIlCambioCursore(mouseQuandoEsce);
    }

    #endregion


    void RichiamaIlCambioCursore(TCursore mQ)
    {
        GestoreCursoriMouse.CambiaCursoreScelto((int)mQ);
    }
}
