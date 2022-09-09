using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GestoreTesti : MonoBehaviour
{
    [SerializeField]
    TextAsset fileTesto_Ita, fileTesto_Eng;
    [SerializeField]
    TMP_Text casellaDiTesto_Metro,
             casellaDiTesto_Esterno;

    //[SerializeField]
    public static CharacterStats persInEsploraz;
    [SerializeField]
    GameObject frecciaAvanti;

    GiorniMainScript giorniScript;
    OpzioniMainScript opzScript;
    ManagerRisorse managRisScript;
    SalvataggiMainScript salvScript;
    Miragame_delleArmi miragameScript;

    //Il separatori delle stringhe & vettori temporanei
    readonly string[] separat = new [] { "[/f]" };

    //Variabili matrici contenitori dei testi di gioco
    //(vedi la funzione Start() per capire perché sono matrici)
    string[][] dialoghi_Ita, dialoghi_Eng;

    //Variabili dei punti di inizio delle varie "regioni"
    //delle due lingue
    int i_daDoveIniziare,
        i_testoInMetro, i_morteDiUnPers,
        i_Cibo, i_Acqua, i_Medicine, i_Energia,
        i_pezziRadio,
        i_trovaColt, i_trovaPist, i_trovaFuc;

    int risorsaPresa, tipoRisorsaPresa;

    bool testoFinito, possoIniziare;


    private void Awake()
    {
        giorniScript = FindObjectOfType<GiorniMainScript>();
        opzScript = FindObjectOfType<OpzioniMainScript>();
        managRisScript = FindObjectOfType<ManagerRisorse>();
        salvScript = FindObjectOfType<SalvataggiMainScript>();
        miragameScript = FindObjectOfType<Miragame_delleArmi>();

        if (casellaDiTesto_Metro == null)
        {
            //Se non è assegnata, trova l'oggetto con la tag
            //e prendi il componente di quell'oggetto
            casellaDiTesto_Metro = GameObject.FindGameObjectWithTag("Casella-di-testo").GetComponent<TMP_Text>();
        }

        frecciaAvanti = null;

        //if (frecciaAvanti == null)
        //{
            frecciaAvanti = GameObject.FindGameObjectWithTag("Freccia-manda-avanti-testo");
        //}

        dialoghi_Ita = LeggiEDividiTesto(fileTesto_Ita);
        dialoghi_Eng = LeggiEDividiTesto(fileTesto_Eng);

        /* RISULTATO FINALE SIMILE A:
         *   [d1.a] [d2.a] [d3.a]
         *   [d1.b] [d2.b] [d3.b]
         *   [d1.c]        [d3.c]
         *                 [d3.d] 
         */

        casellaDiTesto_Metro.text = "";
    }

    void Update()
    {
        #region NON_USATO_Rileva se la scena è stata cambiata

        //GameObject cambioSc = GameObject.FindGameObjectWithTag("Cambio-scena");
        //if (cambioSc != null)
        //{
        //    Awake();

        //    Destroy(cambioSc);
        //}
        #endregion

        bool comando = Input.GetMouseButtonDown(0);
        //if (frecciaAvanti)
        if (testoFinito)
        {
            //Animazione che serve al giocatore per capire che può andare avanti
            frecciaAvanti.SetActive(true);

            //Se il testo ha finito e si è dato il comando,
            //inizia a scrivere il testo
            possoIniziare = comando ? true : false;
        }
        else
        {
            //Disattiva l'animazione
            frecciaAvanti.SetActive(false);
        }

        if (possoIniziare)
        {
            string totTesto;

            //Se ha finito le cose da scrivere
            if (giorniScript.LeggiCodaEventi_Count() <= 0)
            {
                //Cambia scena (interrompe tutto) e torna in metro
                opzScript.ScenaScegliTu(1);
            }
            //Se no, continua con il testo
            else
            {
                //Serve per dire a questo script quale "evento" deve scrivere
                PassamiTestoEsplorazione(giorniScript.ProssimoEvento());

                //Passa a "totTesto" tutto il testo nella lingua scelta
                totTesto = CambiaTestoInBaseAllaLinguaScelta()[i_daDoveIniziare][0];


                //Sostituisce i prefissi (es. [/p])
                totTesto = totTesto.Replace("[/p]", persInEsploraz.LeggiNome());
                totTesto = totTesto.Replace("[/x]", risorsaPresa.ToString());


                //Per sicurezza, termina tutte le coroutine
                //StopAllCoroutines();

                //Inizia a scrivere il testo dall'inizio
                StartCoroutine(ScriviTesto(totTesto, casellaDiTesto_Esterno));
            }
        }
    }

    string[][] CambiaTestoInBaseAllaLinguaScelta()
    {
        switch (OpzioniMainScript.linguaScelta)
        {
            case 0:
                return dialoghi_Ita;

            case 1:
            default:
                return dialoghi_Eng;
        }
    }

    public void ScriviBigliettoMetro()
    {
        //Passa a "testoFinale" un testo a caso nella lingua scelta
        int indiceScelto = i_testoInMetro + UnityEngine.Random.Range(1, 9);
        string testoFinale = CambiaTestoInBaseAllaLinguaScelta()[indiceScelto][0];

        #region --Non utilizzato--
        //Prende un personaggio a caso
        int lunghChSt = salvScript.LeggiPersonStatsScript().Length,
            numeroRandom = UnityEngine.Random.Range(0, lunghChSt);
        CharacterStats chSt_aCaso = salvScript.LeggiPersonStatsScript()[numeroRandom];
        #endregion

        //Sostituisce i prefissi (es. [/p])
        testoFinale = testoFinale.Replace("[/p]", chSt_aCaso.LeggiNome());

        testoFinito = false;

        StopAllCoroutines();


        //Inizia a scrivere il testo dall'inizio
        StartCoroutine(ScriviTestoVeloce(testoFinale, casellaDiTesto_Metro));
    }


    #region Funz. che divide e restituisce il testo in una matrice (array 2D di stringhe)

    string[][] LeggiEDividiTesto(TextAsset fileTXT)
    {
        string[][] contenitore = null;

        //Divide i testi ad ogni "a capo" e li mette nelle variabili temporanee
        string[] temp = fileTXT.text.Split('\n');

        /*
         * Adesso abbiamo questo:
         *   [d1] [d2] [d3]
         *    []   []   []
         *    []        []
         *              [] 
         */

        #region Cerca le "regioni"

        //Cerca nell'array i punti di inizio delle varie "regioni"
        //(stesso di SalvataggiMainScript.cs)
        for (int i = 0; i < temp.Length; i++)
        {
            string r = temp[i];

            if (r.Contains("# CIBO #"))
                i_Cibo = i;

            if (r.Contains("# ACQUA #"))
                i_Acqua = i;

            if (r.Contains("# MEDICINE #"))
                i_Medicine = i;

            if (r.Contains("# ENERGIE #"))
                i_Energia = i;

            if (r.Contains("# PEZZI DELLA RADIO #"))
                i_pezziRadio = i;

            if (r.Contains("# COLTELLO TROVATO #"))
                i_trovaColt = i;

            if (r.Contains("# PISTOLA TROVATA #"))
                i_trovaPist = i;

            if (r.Contains("# FUCILE TROVATO #"))
                i_trovaFuc = i;

            if (r.Contains("# TESTO IN METRO #"))
                i_testoInMetro = i;

            if (r.Contains("# MORTE DEI PERSONAGGI #"))
                i_morteDiUnPers = i;
        }
        #endregion


        //Mette come numero di righe lo stesso di quello delle var. temp.
        contenitore = new string[temp.Length][];

        //Divide le ogni "[/f]" e li mette nella matrice dialoghi
        for (int j = 0; j < temp.Length; j++)
            contenitore[j] = temp[j].Split(separat, StringSplitOptions.None);
        
        return contenitore;
    }
    #endregion

    #region Funz. che scrive il testo (con pause)

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

        #region OLD
        //Controlla il testo
        //for (int i = 0; i < fraseDaScrivere.Length; i++)
        //{
        //    char c = fraseDaScrivere[i];

        //    //Se è un prefisso
        //    if (c == '[')
        //    {
        //        //Controlla se c'è un prefisso
        //        if (fraseDaScrivere[i + 1] == '/' && fraseDaScrivere[i + 3] == ']')
        //        {
        //            switch (fraseDaScrivere[i + 2])
        //            {
        //                case 'p': //  [/p]
        //                    print("Prima> " + fraseDaScrivere);
        //                    /* ---NOTA:
        //                     * Scrive un personaggio al posto del testo
        //                     * & salta i caratteri
        //                     */

        //                    //Scrive un personaggio al posto del testo
        //                    fraseDaScrivere = fraseDaScrivere.Insert(i, persInEsploraz.LeggiNome());
        //                    //fraseDaScrivere.Insert(i, persInEsploraz.LeggiNome());
        //                    fraseDaScrivere.Remove(i, 4);
        //                    print("Dopo> " + fraseDaScrivere);

        //                    //Salta i caratteri
        //                    i += 3;
        //                    break;

        //                case 'r': //  [/r]
        //                    /* ---NOTA:
        //                     * Scrive un numero + risorsa al posto del testo
        //                     * & salta i caratteri
        //                     */
        //                    i += 4;
        //                    break;

        //                case 'f': //  [/f]
        //                    /* ---NOTA:
        //                     * Fine della scelta
        //                     * & salta i caratteri
        //                     */
        //                    i += 4;
        //                    break;
        //            }
        //        }
        //        //else
        //        //    //Controlla se è per la formattazione del testo
        //        //    switch (c + 1)
        //        //    {
        //        //        case '*':
        //        //            break;
        //        //    }
        //    }
        //}
        #endregion

        //Scrive il testo
        for (int i = 0; i < fraseDaScrivere.Length; i++)
        {
            char c = fraseDaScrivere[i];


            #region Evita i caratteri speciali (come <i></i> o <br>)

            //if (fraseDaScrivere[i+1] == '<')
            //{

            //    string cSpeciale_prima = fraseDaScrivere[i+2].ToString() + fraseDaScrivere[i+3].ToString(),
            //           cSpeciale_dopo  = fraseDaScrivere[i+2].ToString() + fraseDaScrivere[i+3].ToString() + fraseDaScrivere[i+4].ToString();

            //    //Se è <i> o <b>
            //    if (cSpeciale_prima == "i>" || cSpeciale_prima == "b>")
            //    {
            //        i += 3;
            //    }

            //    //Se è </i> o </b> o <br>
            //    if (cSpeciale_dopo == "/i>" || cSpeciale_dopo == "/b>" || cSpeciale_dopo == "br>")
            //    {
            //        i += 4;
            //    }
            //}
            #endregion

            #region Scrittura del testo

            switch (c)
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
            #endregion

            yield return new WaitForSeconds(sec);

            if (i == fraseDaScrivere.Length - 1 && txtDaScrivere.text == fraseDaScrivere)
                testoFinito = true;
        }

    }
    #endregion

    #region Funz. che scrive il testo, ma senza pause

    /// <summary>
    /// Scrive il testo senza aspettare pause
    /// <br></br>(PS. la velocità è quella standard: 0.05 sec)
    /// </summary>
    IEnumerator ScriviTestoVeloce(string frase, TMP_Text txtDaScrivere)
    {
        txtDaScrivere.text = "";

        foreach (char c in frase)
        {
            txtDaScrivere.text += c;

            yield return new WaitForSeconds(.05f);

        }

        if (txtDaScrivere.text == frase)
            testoFinito = true;
    }
    
    IEnumerator ScriviTestoVeloce(string frase, TMP_Text txtDaScrivere, float velScritt)
    {
        txtDaScrivere.text = "";

        foreach (char c in frase)
        {
            txtDaScrivere.text += c;

            yield return new WaitForSeconds(velScritt);
        }

        if (txtDaScrivere.text == frase)
            testoFinito = true;
    }
    #endregion

    #region Funzione per andare avanti nel testo

    public void PassamiTestoEsplorazione(int tipo_ev)
    {
        int i_addRisorse = UnityEngine.Random.Range(1, 3),  //tra 1 e 2
            i_addRadio = UnityEngine.Random.Range(1, 5),  //tra 1 e 4
            i_addArmi = UnityEngine.Random.Range(1, 3),  //tra 1 e 2
            i_addMorte = UnityEngine.Random.Range(1, 5);  //tra 1 e 4


        //Rispetto a che tipo di risorsa mi sta passando, mi comporto di conseguenza
        //(vedi GiorniMainScript.cs) per più informazioni su questi numeri
        switch (tipo_ev)
        {
            case 1:  //Cibo
                i_daDoveIniziare = i_Cibo + i_addRisorse;
                
                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiCibo(risorsaPresa);
                break;

            case 2:  //Acqua
                i_daDoveIniziare = i_Acqua + i_addRisorse;
                
                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiAcqua(risorsaPresa);
                break;

            case 3:  //Medicine
                i_daDoveIniziare = i_Medicine + i_addRisorse;
                
                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiMedicine(risorsaPresa);
                break;

            case 4:  //Energia
                i_daDoveIniziare = i_Energia + i_addRisorse;
                
                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiEnergia(risorsaPresa);
                break;

            case 5:  //Pezzi radio
                i_daDoveIniziare = i_pezziRadio + i_addRadio;

                managRisScript.AggiungiPezziRadio(1);
                break;

            case 9:  //Lotta (inizio)
                //bool sistemaSceltaMultipla_ovveroAttivaIPulsati;

                giorniScript.SostituisciEventoAttuale(901); //Va alla lotta
                /*
                 * Nota: Originariamente ci doveva essere un sistema
                 * dove sceglievi se lottare o meno
                 * (vedi il bool poche righe sopra usato come promemoria),
                 * mentre ora si va direttamente al combattimento
                 * - ovvero apre il minigame direttamente
                 */

                //testoFinito = false;
                break;

            case 901:  //Lotta (combatti, inizio minigame)

                miragameScript.ScriviAperturaMinigioco(true);
                break;

            case 902:  //Lotta (combatti -> vinci)

                miragameScript.ScriviAperturaMinigioco(false);
                break;

            case 903:  //Lotta (combatti -> perdi)

                miragameScript.ScriviAperturaMinigioco(false);
                break;

            case 904:  //Lotta (fuga)
                break;

            case 909:  //Lotta (morte del personaggio)
                i_daDoveIniziare = i_morteDiUnPers + i_addMorte;

                persInEsploraz.TogliAllaVita(1000);
                break;

            case 91:  //Coltello trovato
                i_daDoveIniziare = i_trovaColt + i_addArmi;

                managRisScript.ScriviTipoArma(1);
                break;

            case 92:  //Pistola trovata
                i_daDoveIniziare = i_trovaPist + i_addArmi;

                managRisScript.ScriviTipoArma(2);
                break;

            case 93:  //Fucile trovato
                i_daDoveIniziare = i_trovaFuc + i_addArmi;
                
                managRisScript.ScriviTipoArma(3);
                break;
        }

        testoFinito = false;
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
