using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CambiaTutorial : MonoBehaviour
{
    [SerializeField]
    Sprite[] immagini;
    [SerializeField, TextArea]
    string[] spiegazioni_ITA, spiegazioni_ENG;

    [Header("Oggetti da modificare")]
    [SerializeField]
    Image imgTutorial;
    [SerializeField]
    TMP_Text testoTutorial,
             indiceTutorial;
    [SerializeField]
    GameObject bottonePrima;

    int indice;


    void Update()
    {
        //Cambia ogni Update() la "slide" del tutorial
        imgTutorial.sprite = immagini[indice];  //L'immagine visualizzata
        
        switch (OpzioniMainScript.linguaScelta)  //Il testo che spiega
        {
            case 0:
                testoTutorial.text = spiegazioni_ITA[indice];
                break;

            case 1:
            default:
                testoTutorial.text = spiegazioni_ENG[indice];
                break;
        }
        
        indiceTutorial.text = (indice + 1).ToString();  //Il numero della "slide"
        
        print(indice);
    }

    public void AvantiSlideTutorial()
    {
        bool oltreSpiegaz; 

        switch (OpzioniMainScript.linguaScelta)
        {
            case 0:
                oltreSpiegaz = indice >= spiegazioni_ITA.Length-1;
                break;

            case 1:
            default:
                oltreSpiegaz = indice >= spiegazioni_ENG.Length-1;
                break;
        }

        if (oltreSpiegaz /*|| indice >= immagini.Length*/)
            //Se supera la lunghezza massima, torna a 0
            indice = 0;
        else
            //Aumenta la "slide" di 1
            indice++;
    }

    public void IndietroSlideTutorial()
    {
        if (indice <= 0)
            //Se supera la lunghezza minima, torna al massimo
            switch (OpzioniMainScript.linguaScelta)
            {
                case 0:
                    indice = spiegazioni_ITA.Length - 1;
                    break;

                case 1:
                default:
                    indice = spiegazioni_ENG.Length - 1;
                    break;
            }
        else
            //Diminuisce la "slide" di 1
            indice--;
    }
}
