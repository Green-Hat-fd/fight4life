using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miragame_delleArmi : MonoBehaviour
{
    Slider sliderMG;

    [SerializeField]
    float velocColt;
    
    //Velocità delle armi
    int velocPist = 40,
        velocFucLati = 4,
        velocFucCentro = 15;

    [SerializeField, Range(1, 3)]
    int tipoArma;
    bool oscillazYN;


    #region Scelta casuale iniziale

    private void Start()
    {
        sliderMG = GetComponent<Slider>();

        switch (Random.Range(1, 3)) //int a caso in [1, 3)
        {
            case 1:
                
                break;
            case 2:
                
                break;
        }
    }
    #endregion

    void FixedUpdate()
    {
        bool siMuove = GetComponent<Animator>().GetBool("Muoversi");


        /*if (siMuove)
        {
            //Controlla se è arrivato nella posizione (e le scambia)
            if (transform.position == pos1.position)
                devoArrivareQui = pos2;

            if (transform.position == pos2.position)
                devoArrivareQui = pos1;*/

        //Sceglie il tipo di movimento rispetto all'arma selezionata
        switch (tipoArma)
        {
            case 1: MovimentoColtello(); break;
            case 2: MovimentoPistola(); break;
            case 3: MovimentoFucile(); break;
        }
    }


    #region Funz. -> Movimento del Coltello
    //-------------------------DA FINIRE-------------------------//
    void MovimentoColtello()
    {
        
    }

    #endregion

    #region Funz. -> Movimento della Pistola
    void MovimentoPistola()
    {
        //Movimento in sè
        sliderMG.value += 1f * velocPist;
    }
    #endregion

    #region Funz. -> Movimento del Fucile
    void MovimentoFucile()
    {
        float valore = sliderMG.value;


        //Se si trova al centro, accellera...
        if (valore <= 0.5f && valore >= -0.5f)
            sliderMG.value += 1f * velocFucCentro;
        else
            //...se no, va lento
            sliderMG.value += 1f * velocFucLati;

    }
    #endregion
}
