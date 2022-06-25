using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuoviBarra : MonoBehaviour
{
    [SerializeField]
    Transform pos1, pos2;
    [SerializeField]
    float velocColt;

    Transform devoArrivareQui;

    //Velocità delle armi
    int velocPist = 40,
        velocFucLati = 4, 
        velocFucCentro = 15;

    [SerializeField, Range(1, 3)]
    /*public static*/ int tipoArma = 3;
    bool oscillazYN;


    #region Scelta casuale iniziale

    private void Start()
    {
        switch(Random.Range(1, 3)) //int a caso in [1, 3)
        {
            case 1:
                devoArrivareQui = pos1;
                break;
            case 2:
                devoArrivareQui = pos2;
                break;
        }
    }
    #endregion

    void FixedUpdate()
    {
        bool siMuove = GetComponent<Animator>().GetBool("Muoversi");


        if (siMuove)
        {
            //Controlla se è arrivato nella posizione (e le scambia)
            if (transform.position == pos1.position)
                devoArrivareQui = pos2;

            if (transform.position == pos2.position)
                devoArrivareQui = pos1;

            //Sceglie il tipo di movimento rispetto all'arma selezionata
            switch (tipoArma)
            {
                case 1: MovimentoColtello();  break;
                case 2: MovimentoPistola();  break;
                case 3: MovimentoFucile();  break;
            }
        }
        else
            StopAllCoroutines();
    }


    #region Funz. -> Movimento del Coltello
    //-------------------------DA FINIRE-------------------------//
    void MovimentoColtello()
    {
        InvokeRepeating("MovimCasualeColt", 0, Random.Range(0.75f, 1f));

        transform.position = Vector2.MoveTowards(transform.position,
                                                 devoArrivareQui.position,
                                                 velocColt * Time.deltaTime);
    }

    void MovimCasualeColt()
    {
        int indCas = Random.Range(10, 26);

        for (int i = 0; i < indCas; i++)
        {

        }
    }
    #endregion

    #region Funz. -> Movimento della Pistola
    void MovimentoPistola()
    {
        //Movimento in sè
        transform.position = Vector2.MoveTowards(transform.position,
                                                 devoArrivareQui.position,
                                                 velocPist * Time.deltaTime);
    }
    #endregion

    #region Funz. -> Movimento del Fucile
    void MovimentoFucile()
    {
        Vector3 posiz = transform.localPosition;


        //Se si trova al centro, accellera...
        if (posiz.x <= 0.5f && posiz.x >= -0.5f)
            transform.position = Vector2.MoveTowards(transform.position,
                                                     devoArrivareQui.position,
                                                     velocFucCentro*Time.deltaTime);
        else
            //...se no, va lento
            transform.position = Vector2.MoveTowards(transform.position,
                                                     devoArrivareQui.position,
                                                     velocFucLati *Time.deltaTime);
            
    }
    #endregion
}
