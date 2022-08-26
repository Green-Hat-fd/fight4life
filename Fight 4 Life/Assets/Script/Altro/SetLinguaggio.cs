using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetLinguaggio : MonoBehaviour
{
    [Tooltip("0: Italiano \n1: Inglese \n\nNON PIÙ DI 2!")]
    [SerializeField]
    string[] testoNelle2Lingue = new string[2];   //Le due stringhe che cambiano nel testo in base alla lingua (Ita & Eng)


    private void Update()
    {
        //Modifica il testo "sottotit" rispetto alla lingua selez.
        GetComponent<TMP_Text>().text = testoNelle2Lingue[OpzioniMainScript.linguaScelta];
    }
}
