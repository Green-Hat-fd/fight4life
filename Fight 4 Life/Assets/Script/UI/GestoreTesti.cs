using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GestoreTesti : MonoBehaviour
{
    [SerializeField]
    TextAsset fileTesto_Ita, fileTesto_Eng;
    [SerializeField]
    TextAsset risorseFile_Ita, risorseFile_Eng;
    [SerializeField]
    TMP_Text abcTesto;//--------------------------------DEBUG

    [SerializeField]
    CharacterStats persInEsploraz;

    //Il separatori delle stringhe & vettori temporanei
    string[] separat = new [] { "[/f]" };

    //Variabili matrici contenitori dei testi di gioco
    //(vedi la funzione Start() per capire perché sono matrici)
    string[][] dialoghi_Ita, dialoghi_Eng,
               risorse_Ita, risorse_Eng;


    void Start()
    {
        dialoghi_Ita = LeggiEDividiTesto(fileTesto_Ita);
        dialoghi_Eng = LeggiEDividiTesto(fileTesto_Eng);

        risorse_Ita = LeggiEDividiTesto(risorseFile_Ita);
        risorse_Eng = LeggiEDividiTesto(risorseFile_Eng);

        /* RISULTATO FINALE SIMILE A:
         *   [d1.a] [d2.a] [d3.a]
         *   [d1.b] [d2.b] [d3.b]
         *   [d1.c]        [d3.c]
         *                 [d3.d] 
         */

        #region DEBUG
        abcTesto.text = "";

        //for (int i=0; i < dialoghi_Eng.Length; i++)
        //{
        //    for (int j=0; j < dialoghi_Eng[i].Length; j++)
        //    {
        //        abcTesto.text += dialoghi_Eng[i][j] + " - ";
        //    }

        //    abcTesto.text += '\n';
        //}
        #endregion
    }

    void Update()
    {
        #region DEBUG
        bool avanti = Input.GetKeyDown(KeyCode.Space),
             inizio = Input.GetKeyDown(KeyCode.O),
             possoIniziare;

        int d = 0;


        if (inizio)
            possoIniziare = true;
        else
            possoIniziare = false;

        if (possoIniziare)
        {
            StartCoroutine(ScriviTesto(dialoghi_Eng[0][0], abcTesto));  //Inizia a scrivere il testo dall'inizio
            print("Sono il testo e ho <i><b>finito</b></i> di scrivere");
        }

        if(/*abcTesto.text != dialoghi_Eng[0][0] &&*/ avanti)
        {
            d++;
            StartCoroutine(ScriviTesto(dialoghi_Eng[0][d], abcTesto));  //Inizia a scrivere il testo
            print("Sono il testo e ho <i><b>finito</b></i> di scrivere");
        }
        #endregion
    }

    #region Funz. che divide e restituisce il testo in una matrice (array 2D di stringhe)

    string[][] LeggiEDividiTesto(TextAsset fileTXT)
    {
        string[][] contenitore = null;
        
        for (int i = 0; i < fileTXT.text.Length; i++)
        {
            //Divide i testi ad ogni "a capo" e li mette nelle variabili temporanee
            string[] temp = fileTXT.text.Split('\n');

            //Mette il limite di righe lo stesso di quello delle var. temp.
            contenitore = new string[temp.Length][];

            //Divide le ogni "[/f]" e li mette nella matrice dialoghi
            for (int j = 0; j < temp.Length; j++)
                contenitore[j] = temp[j].Split(separat, StringSplitOptions.None);
        }

        return contenitore;
    }

    #endregion

    IEnumerator ScriviTesto(string fraseDaScrivere, TMP_Text txtDaScrivere)
    {
        float sec;

        #region DEBUG
        //Questi sono per evitare che il testo si scrivesse in maniera "brutta"
        //txtDaScrivere.text = "<color=#00000000>" + frase + "</color>";
        //txtDaScrivere.SetText("<color=#00000000>" + frase + "</color>");
        #endregion

        //Cancella tutto il testo prima di scrivere la frase
        txtDaScrivere.text = "";

        //Controlla il testo
        for (int i = 0; i < fraseDaScrivere.Length; i++)
        {
            char c = fraseDaScrivere[i];

            //Se è un prefisso
            if (c == '[')
            {
                //Controlla se c'è un prefisso
                if (fraseDaScrivere[i + 1] == '/' && fraseDaScrivere[i + 3] == ']')
                {
                    switch (fraseDaScrivere[i + 2])
                    {
                        case 'p': //  [/p]
                            print(fraseDaScrivere);
                            /* ---NOTA:
                             * Scrive un personaggio al posto del testo
                             * & salta i caratteri
                             */

                            //Scrive un personaggio al posto del testo
                            fraseDaScrivere = fraseDaScrivere.Insert(i, persInEsploraz.LeggiNome());
                            fraseDaScrivere.Remove(i, 4);
                            print(fraseDaScrivere);

                            //Salta i caratteri
                            i += 3;
                            break;

                        case 'r': //  [/r]
                            /* ---NOTA:
                             * Scrive un numero + risorsa al posto del testo
                             * & salta i caratteri
                             */
                            i += 4;
                            break;

                        case 'f': //  [/f]
                            /* ---NOTA:
                             * Fine della scelta
                             * & salta i caratteri
                             */
                            i += 4;
                            break;
                    }
                }
                //else
                //    //Controlla se è per la formattazione del testo
                //    switch (c + 1)
                //    {
                //        case '*':
                //            break;
                //    }
            }
        }

        //Scrive il testo
        for (int i = 0; i < fraseDaScrivere.Length; i++)
        {
            char c = fraseDaScrivere[i];


            switch (c) //Scrittura
            {
                //Scrive il carattere, ma aspetta con la pausa della virgola
                case ',':
                case ';':
                    txtDaScrivere.text += c;

                    sec = .1f;
                    break;
                
                //Scrive il carattere, ma aspetta con una pausa più lunga (del punto)
                case '.':
                case '!':
                case '?':
                    if(c == '.' && fraseDaScrivere[i+1] == '.' && fraseDaScrivere[i+2] == '.') //Controlla se ci sono 3 punti "..."
                    {
                        //Scrive ". . .", aspettando con unapausa ogni punto
                        yield return new WaitForSeconds(.5f);
                            txtDaScrivere.text += c;
                            txtDaScrivere.text += " ";
                        yield return new WaitForSeconds(.5f);
                            txtDaScrivere.text += c;
                            txtDaScrivere.text += " ";
                        yield return new WaitForSeconds(.5f);
                            txtDaScrivere.text += c;
                            txtDaScrivere.text += " ";

                        sec = .5f;

                        i += 3;
                    }
                    else //Se invece sono i caratteri nei "case '':"
                    {
                        txtDaScrivere.text += c;

                        sec = .5f;
                    }
                    break;


                //Se il carattere non è uno "speciale" (es. alfanumerico)
                //scrive il carattere e aspetta un tempo standard
                default:
                    txtDaScrivere.text += c;

                    sec = .05f;
                    break;
            }

            yield return new WaitForSeconds(sec);
        }
    }

    #region Funz. che scrive il testo, ma senza pause
    
    /// <summary>
    /// Scrive il testo senza aspettare pause
    /// <br></br>(PS. la velocità è quella standard: 0.05 sec)
    /// </summary>
    IEnumerator ScriviTestoVeloce(string frase, TMP_Text txtDaScrivere)
    {
        foreach (char c in frase)
        {
            txtDaScrivere.text += c;

            yield return new WaitForSeconds(.05f);
        }
    }
    
    IEnumerator ScriviTestoVeloce(string frase, TMP_Text txtDaScrivere, float velScritt)
    {
        foreach (char c in frase)
        {
            txtDaScrivere.text += c;

            yield return new WaitForSeconds(velScritt);
        }
    }

    #endregion

    public void PassaPersonaggio(CharacterStats statsPersonScript)
    {
        persInEsploraz = statsPersonScript;
    }

    public CharacterStats LeggiPersonaggio()
    {
        return persInEsploraz;
    }
}
