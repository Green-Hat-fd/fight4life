using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CambiaTutorial : MonoBehaviour
{
    [SerializeField]
    Sprite[] immagini;
    [SerializeField]
    string[] spiegazioni;

    [Header("Oggetti da modificare")]
    [SerializeField]
    Image imgTutorial;
    [SerializeField]
    TMP_Text testoTutorial,
             indiceTutorial;

    int indice;


    void Update()
    {
        //Cambia ogni Update() la "slide" del tutorial
        imgTutorial.sprite = immagini[indice];  //L'immagine visualizzata
        testoTutorial.text = spiegazioni[indice];  //Il testo che spiega
        indiceTutorial.text = (indice + 1).ToString();  //Il numero della "slide"
        print(indice);
    }

    public void AvantiSlideTutorial()
    {
        if (indice >= spiegazioni.Length-1 || indice >= immagini.Length-1)
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
            indice = spiegazioni.Length - 1;
        else
            //Diminuisce la "slide" di 1
            indice--;
    }
}
