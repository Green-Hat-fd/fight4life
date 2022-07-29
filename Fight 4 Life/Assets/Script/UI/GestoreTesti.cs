using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoreTesti : MonoBehaviour
{
    [SerializeField]
    TextAsset fileTesto_Ita, fileTesto_Eng;
    //[SerializeField]
    //TextAsset fileTesto_Ita, fileTesto_Eng;

    string[][] dialoghi_Ita, dialoghi_Eng;


    void Start()
    {
        
    }

    
    //void ScriviTesto()
    //{


    //    //Se è un prefisso
    //    if (i == "[")
    //    {
    //        //Controlla se c'è un prefisso
    //        if (i + 1 == "/" && i + 3 == "]")
    //        {
    //            switch (i + 2)
    //            {
    //                case "p":
    //                    /* ---NOTA:
    //                     * Scrive un personaggio al posto del testo
    //                     * & salta i caratteri
    //                     */
    //                    i += 4;
    //                    break;

    //                case "r":
    //                    /* ---NOTA:
    //                     * Scrive un numero + risorsa al posto del testo
    //                     * & salta i caratteri
    //                     */
    //                    break;

    //                case "f":
    //                    /* ---NOTA:
    //                     * Fine della scelta
    //                     * & salta i caratteri
    //                     */
    //                    break;
    //            }
    //        }
    //        else
    //            //Controlla se è per la formattazione del testo
    //            switch (i + 1)
    //            {
    //                case "*":
    //                    break;
    //            }
    //    }


    //    //Se non è un prefisso
    //    switch (i)
    //    {
    //        case ",":
    //        case ";":
    //            /* ---NOTA:
    //             * Scrivi il carattere ma aspetta con pausa della virgola
    //             */
    //            break;

    //        case ".":
    //        case "!":
    //        case "?":
    //            /* ---NOTA:
    //             * Scrivi il carattere ma aspetta con pausa del punto
    //             */
    //            break;

    //        case " ":
    //            /* ---NOTA:
    //             * Scrivi il carattere ma aspetta con pausa dello spazio
    //             */
    //            break;

    //        case "...":
    //            /* ---NOTA:
    //             * Scrivi il carattere ma aspetta per i 3 punti
    //             */
    //            break;

    //        //Se il carattere non è uno "speciale" (es. alfanumerico)
    //        default:
    //            /* ---NOTA:
    //             * Scrivi il carattere
    //             */
    //            break;

    //    }
    //}
}
