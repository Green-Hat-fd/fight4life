using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CambiaCursoreUI : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    enum TCursore
    {
        Default,
        Collegamento,
        Prendi,
        Caricamento
    }

    [SerializeField, Space(10)]
    TCursore mouseQuandoEntra, mouseQuandoClicca, mouseQuandoTrascina, mouseQuandoEsce;


    #region Funzioni di quando il mouse fa cose (UI)

    //Il mouse ENTRA
    public void OnPointerEnter(PointerEventData eventData)
    {
        #region OLD_DEBUG
        //print("<color=maroon>Cursore mouse entrato</color>");
        #endregion
        RichiamaIlCambioCursore(mouseQuandoEntra);
    }

    //Il mouse CLICCA
    public void OnPointerClick(PointerEventData eventData)
    {
        RichiamaIlCambioCursore(mouseQuandoClicca);
    }

    //Il mouse TRASCINA
    public void OnDrag(PointerEventData eventData)
    {
        RichiamaIlCambioCursore(mouseQuandoTrascina);
    }

    //Il mouse ESCE
    public void OnPointerExit(PointerEventData eventData)
    {
        RichiamaIlCambioCursore(mouseQuandoEsce);
    }

    #endregion


    void RichiamaIlCambioCursore(TCursore mQ)
    {
        GestoreCursoriMouse.CambiaCursoreScelto((int)mQ);
    }
}
