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

    [SerializeField]
    CharacterStats persInEsploraz;
    [SerializeField]
    GameObject[] frecceAvanti;
    [SerializeField]
    GameObject gruppoBottoniSceltaTesti;

    GiorniMainScript giorniScript;
    OpzioniMainScript opzScript;
    ManagerRisorse managRisScript;
    SalvataggiMainScript salvScript;
    [SerializeField]
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
        i_Combatti,
        i_trovaColt, i_trovaPist, i_trovaFuc,
        i_siTornaInMetro;
    int i_addLotta;

    int risorsaPresa, risorsaPresa_inLotta;
    string tipoRisorsaPresa_s = "";

    bool testoFinito, possoIniziare, sonoInEplorazione;


    private void Awake()
    {
        giorniScript = FindObjectOfType<GiorniMainScript>();
        opzScript = FindObjectOfType<OpzioniMainScript>();
        managRisScript = FindObjectOfType<ManagerRisorse>();
        salvScript = FindObjectOfType<SalvataggiMainScript>();
        //miragameScript = FindObjectOfType<Miragame_delleArmi>();

        if (casellaDiTesto_Metro == null)
        {
            //Se non è assegnata, trova l'oggetto con la tag
            //e prendi il componente di quell'oggetto
            casellaDiTesto_Metro = GameObject.FindGameObjectWithTag("Casella-di-testo").GetComponent<TMP_Text>();
        }

        #region NON USATO
        //frecciaAvanti_Metro = null;

        //if (frecciaAvanti == null)
        //{
        //frecciaAvanti_Metro = GameObject.FindGameObjectWithTag("Freccia-manda-avanti-testo");
        //}
        #endregion

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

        bool codaVuota = giorniScript.LeggiCodaEventi_Count() <= 0;
        

        if (testoFinito)
        {
            bool comando = Input.GetMouseButtonDown(0);

            //Animazione che serve al giocatore per capire che può andare avanti
            foreach (var freccia in frecceAvanti)
            {
                freccia.SetActive(true);
            }

            //Se il testo ha finito e si è dato il comando,
            //inizia a scrivere il testo
            possoIniziare = comando ? true : false;
        }
        else
        {
            //Disattiva l'animazione
            foreach (var freccia in frecceAvanti)
            {
                freccia.SetActive(false);
            }

            possoIniziare = false;
        }

        if (possoIniziare)
        {
            string totTesto;

            //Se ha finito le cose da scrivere
            if (codaVuota && sonoInEplorazione)
            {
                //Aumenta al massimo la stanchezza del personaggio che è "ritornato in metro"
                persInEsploraz.ScriviStanch(999);

                //Cambia scena (interrompe tutto) e torna in metro
                giorniScript.FineNotte_Fuori();
                
                possoIniziare = false;
            }
            //Se no, continua con il testo
            else
            {
                //Serve per dire a questo script quale "evento" deve scrivere
                PassamiTestoEsplorazione(giorniScript.VediProssimoEvento());

                //Passa a "totTesto" tutto il testo nella lingua scelta
                totTesto = CambiaFileInBaseAllaLinguaScelta()[i_daDoveIniziare][0];


                //Sostituisce i prefissi (es. [/p])
                totTesto = totTesto.Replace("[/p]", persInEsploraz.LeggiNome());
                totTesto = totTesto.Replace("[/x]", risorsaPresa.ToString());
                totTesto = totTesto.Replace("[/lx]", risorsaPresa_inLotta.ToString());
                totTesto = totTesto.Replace("[/r]", tipoRisorsaPresa_s);


                //Per sicurezza, termina tutte le coroutine
                StopAllCoroutines();

                //Inizia a scrivere il testo dall'inizio
                StartCoroutine(ScriviTesto(totTesto, casellaDiTesto_Esterno));
            }
        }
    }

    #region Funzioni sulla lingua

    string[][] CambiaFileInBaseAllaLinguaScelta()
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

    void CambiaRisorsaInBaseAllaLingua(string s_ita, string s_eng)
    {
        switch (OpzioniMainScript.linguaScelta)
        {
            case 0:
                tipoRisorsaPresa_s = s_ita;
                break;

            case 1:
            default:
                tipoRisorsaPresa_s = s_eng;
                break;
        }
    }

    #endregion

    public void ScriviBigliettoMetro()
    {
        //Passa a "testoFinale" un testo a caso nella lingua scelta
        int indiceScelto = i_testoInMetro + UnityEngine.Random.Range(1, 9);
        string testoFinale = CambiaFileInBaseAllaLinguaScelta()[indiceScelto][0];

        #region --Non utilizzato--
        //Prende un personaggio a caso
        int lunghChSt = salvScript.LeggiPersonStatsScript().Length,
            numeroRandom = UnityEngine.Random.Range(0, lunghChSt);
        CharacterStats chSt_aCaso = salvScript.LeggiPersonStatsScript()[numeroRandom];
        #endregion

        //Sostituisce i prefissi (es. [/p])
        testoFinale = testoFinale.Replace("[/p]", chSt_aCaso.LeggiNome());

        testoFinito = false;
        sonoInEplorazione = false;

        StopAllCoroutines();


        //Inizia a scrivere il testo dall'inizio (se non è in esplorazione)
        if (!sonoInEplorazione)
        {
            StartCoroutine(ScriviTestoVeloce(testoFinale, casellaDiTesto_Metro));
        }
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

            if (r.Contains("# ENERGIA #"))
                i_Energia = i;

            if (r.Contains("# COMBATTIMENTO #"))
                i_Combatti = i;

            if (r.Contains("# PEZZO DELLA RADIO #"))
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

            if (r.Contains("# SI TORNA IN METRO #"))
                i_siTornaInMetro = i;
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
        float sec = new float();

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

        testoFinito = false;

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
                //Salta questo carattere (serve per finire il testo)
                case '$':
                    testoFinito = true;
                    break;

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

                        i += 2;
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

            //if (i == fraseDaScrivere.Length - 1 || txtDaScrivere.text == fraseDaScrivere)
            //    testoFinito = true;
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

        if (txtDaScrivere.text == frase && !sonoInEplorazione)
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

        if (txtDaScrivere.text == frase && !sonoInEplorazione)
            testoFinito = true;
    }
    #endregion

    #region Funzione per andare avanti nel testo in eplorazione

    public void PassamiTestoEsplorazione(int tipo_ev)
    {
        int i_addRisorse = UnityEngine.Random.Range(1, 3),  //tra 1 e 2
            i_addRadio = UnityEngine.Random.Range(1, 5),  //tra 1 e 4
            i_addArmi = UnityEngine.Random.Range(1, 3),  //tra 1 e 2
            i_addMorte = UnityEngine.Random.Range(1, 5),  //tra 1 e 4
            i_addRientro = UnityEngine.Random.Range(1, 5);  //tra 1 e 4


        //Rispetto a che tipo di risorsa mi sta passando, mi comporto di conseguenza
        //(vedi GiorniMainScript.cs) per più informazioni su questi numeri
        switch (tipo_ev)
        {
            #region Risorse

            case 1:  //Cibo
                i_daDoveIniziare = i_Cibo + i_addRisorse;
                
                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiCibo(risorsaPresa);
                giorniScript.TogliProssimoEvento();
                break;

            case 2:  //Acqua
                i_daDoveIniziare = i_Acqua + i_addRisorse;

                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiAcqua(risorsaPresa);
                giorniScript.TogliProssimoEvento();
                break;

            case 3:  //Medicine
                i_daDoveIniziare = i_Medicine + i_addRisorse;

                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiMedicine(risorsaPresa);
                giorniScript.TogliProssimoEvento();
                break;

            case 4:  //Energia
                i_daDoveIniziare = i_Energia + i_addRisorse;

                risorsaPresa = UnityEngine.Random.Range(1, 4);
                managRisScript.AggiungiEnergia(risorsaPresa);
                giorniScript.TogliProssimoEvento();
                break;

            case 5:  //Pezzi radio
                i_daDoveIniziare = i_pezziRadio + i_addRadio;

                managRisScript.AggiungiPezziRadio(1);
                giorniScript.TogliProssimoEvento();
                break;

            #endregion

            #region Lotta

            case 9:  //Lotta (inizio)
                //Sceglie un indice a caso (tra 0 e 4)
                i_addLotta = UnityEngine.Random.Range(0, 5);

                //Prende il primo testo della regione "COMBATTIMENTO" a caso
                i_daDoveIniziare = i_Combatti + (i_addLotta * 5) + 1;

                gruppoBottoniSceltaTesti.SetActive(true);
                gruppoBottoniSceltaTesti.GetComponent<Animator>().Play("Apre"); //Attiva la scelta

                /* NOTA:
                 * Questo è un sistema dove si sceglie se lottare o meno,
                 * 
                 * per maggiori informazioni, vai in:
                 * GiorniMainScript.cs
                 *   > InizioNotte_Fuori()
                 *      > #region [Se esce la percentuale giusta, aggiunge "l'evento"]
                 */
                
                possoIniziare = true;
                break;

            case 901:  //Lotta (combatti)
                //Prende il secondo testo della regione "COMBATTIMENTO" a caso
                i_daDoveIniziare = i_Combatti + (i_addLotta * 5) + 2;

                testoFinito = true;
                break;

            case 9010:  //Lotta (combatti, inizio minigame)
                i_daDoveIniziare = i_Combatti - 1;

                gruppoBottoniSceltaTesti.SetActive(false);

                miragameScript.ScriviAperturaMinigioco(true); //Apre il minigame
                break;

            case 902:  //Lotta (combatti -> vinci)
                //Prende il terzo testo della regione "COMBATTIMENTO" a caso
                i_daDoveIniziare = i_Combatti + (i_addLotta * 5) + 3;

                risorsaPresa_inLotta = UnityEngine.Random.Range(1, 4); //Tra 1 e 3
                
                int tipoRisorsaPresa = UnityEngine.Random.Range(1, 5); //Tra 1 e 4
                switch (tipoRisorsaPresa)
                {
                    case 1: //Cibo
                        managRisScript.AggiungiCibo(risorsaPresa_inLotta);
                        CambiaRisorsaInBaseAllaLingua("cibo", "food");
                        break;
                        
                    case 2: //Acqua
                        managRisScript.AggiungiAcqua(risorsaPresa_inLotta);
                        CambiaRisorsaInBaseAllaLingua("acqua", "water");
                        break;
                        
                    case 3: //Medicine
                        managRisScript.AggiungiMedicine(risorsaPresa_inLotta);
                        CambiaRisorsaInBaseAllaLingua("medicine", "medicines");
                        break;
                        
                    case 4: //Energia
                        managRisScript.AggiungiEnergia(risorsaPresa_inLotta);
                        CambiaRisorsaInBaseAllaLingua("batterie", "water");
                        break;
                }

                miragameScript.ScriviAperturaMinigioco(false); //Chiude il minigame

                managRisScript.TogliUsiArma(); //Consuma l'arma di 1

                giorniScript.TogliProssimoEvento();
                break;

            case 903:  //Lotta (combatti -> perdi)
                //Prende il quarto testo della regione "COMBATTIMENTO" a caso
                i_daDoveIniziare = i_Combatti + (i_addLotta * 5) + 4;

                miragameScript.ScriviAperturaMinigioco(false); //Chiude il minigame
                
                managRisScript.TogliUsiArma(); //Consuma l'arma di 1

                giorniScript.TogliProssimoEvento();
                break;

            case 904:  //Lotta (fuga)
                //Prende il quinto testo della regione "COMBATTIMENTO" a caso
                i_daDoveIniziare = i_Combatti + (i_addLotta * 5) + 5;

                gruppoBottoniSceltaTesti.SetActive(false);

                testoFinito = true;
                giorniScript.TogliProssimoEvento();
                break;

            case 909:  //Lotta (morte del personaggio)
                i_daDoveIniziare = i_morteDiUnPers + i_addMorte;

                persInEsploraz.TogliAllaVita(1000);
                giorniScript.TogliProssimoEvento();
                break;

            #endregion

            #region Armi trovate / Amici

            case 91:  //Coltello trovato
                i_daDoveIniziare = i_trovaColt + i_addArmi;

                managRisScript.ScriviUsiArma(UnityEngine.Random.Range(7, 11)); //A caso tra 7 e 10
                managRisScript.ScriviTipoArma(1);
                giorniScript.TogliProssimoEvento();
                break;

            case 92:  //Pistola trovata
                i_daDoveIniziare = i_trovaPist + i_addArmi;

                managRisScript.ScriviUsiArma(UnityEngine.Random.Range(5, 7)); //A caso tra 5 e 6
                managRisScript.ScriviTipoArma(2);
                giorniScript.TogliProssimoEvento();
                break;

            case 93:  //Fucile trovato
                i_daDoveIniziare = i_trovaFuc + i_addArmi;
                
                managRisScript.ScriviUsiArma(UnityEngine.Random.Range(4, 9)); //A caso tra 4 e 8
                managRisScript.ScriviTipoArma(3);
                giorniScript.TogliProssimoEvento();
                break;

            case 0:  //Amici
                print("AMICI"); //---DEBUG
                break;

            #endregion

            #region Torna in metro
            
            case 7:  //Si torna!
                i_daDoveIniziare = i_siTornaInMetro + i_addRientro;

                giorniScript.TogliProssimoEvento();
                break;

            #endregion
        }

        testoFinito = false;
    }

    #endregion

    public void IniziaAScrivere()
    {
        switch (OpzioniMainScript.linguaScelta)
        {
            case 0:
                casellaDiTesto_Esterno.text = "(Clicca per continuare)";
                break;
                
            case 1:
            default:
                casellaDiTesto_Esterno.text = "(Click to continue)";
                break;
        }

        testoFinito = true;
    }

    #region Funzioni Set personalizzate

    public void PassaPersonaggio(CharacterStats statsPersonScript)
    {
        persInEsploraz = statsPersonScript;
    }

    public CharacterStats LeggiPersonaggio()
    {
        return persInEsploraz;
    }

    public void ScriviSonoInEsplorazione(bool v_f)
    {
        sonoInEplorazione = v_f;
    }

    public void ScriviTestoFinito(bool v_f)
    {
        testoFinito = v_f;
    }

    #endregion
}
