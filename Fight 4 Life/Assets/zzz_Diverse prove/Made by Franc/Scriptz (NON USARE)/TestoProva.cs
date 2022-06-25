using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestoProva : MonoBehaviour
{
    [SerializeField]
    string[,] matrice_str = new string[5, 3]; //5 righe di 3 frasi l'una


    void Start()
    {
        string dafare = "";

        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                dafare += c.ToString();

                matrice_str[r, c] = dafare;
            }
        }

        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                print(matrice_str[r, c]);
            }
        }
    }
}
