using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GiorniMainScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text counterGiorniTXT;

    [SerializeField, Space(20)]
    UnityEvent ev_NuovoGiorno_inMetro;

    [SerializeField]
    OpzioniMainScript opzScript;
    ManagerRisorse managRis;
    StazioniMainScript stazScript;
    SalvataggiMainScript salvScript;
    GestoreTesti txtsScript;
    CrossFadeMetroEsplora crossFadeMEScript;


    [Header("Curva/e per le stats"), Space(2.5f)]
    [Tooltip("Questa curva verrà usata 2 volte, \nper Fame & Sete, con i valori che verranno \nsommati e aggiunti alla Stanchezza")]
    [SerializeField]
    AnimationCurve curvaAumentoStanch;
    [Tooltip("Stessa cosa di quella sopra, \nma questa volta verrà usata \nsolo una volta (per diminuire la Vita)")]
    [SerializeField]
    AnimationCurve curvaDiminuizVita;

    int numGiorni = 0;

    //Coda per "storing" gli eventi che accadranno
    Queue<int> codaEventi = new Queue<int>();


    private void Awake()
    {
        opzScript = FindObjectOfType<OpzioniMainScript>();
        managRis = FindObjectOfType<ManagerRisorse>();
        stazScript = FindObjectOfType<StazioniMainScript>();
        salvScript = FindObjectOfType<SalvataggiMainScript>();
        txtsScript = FindObjectOfType<GestoreTesti>();
        crossFadeMEScript = FindObjectOfType<CrossFadeMetroEsplora>();

        //Notifica al GestoreTesti che non è in esplorazione
        txtsScript.ScriviSonoInEsplorazione(false);
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


        //Aggiorna il testo che indica numero del giorno
        counterGiorniTXT.text = numGiorni.ToString();
    }


    #region Funzioni Get personalizzate

    public int LeggiNumeroGiorni()
    {
        return numGiorni;
    }

    public int LeggiCodaEventi_Count()
    {
        return codaEventi.Count;
    }

    #endregion

    #region Funzioni Set personalizzate

    public void AddNumeroGiorni()
    {
        numGiorni++;
    }

    public void ScriviNumeroGiorni(int gg)
    {
        numGiorni = gg;
    }

    #endregion

    #region Funzioni per cambiare la giornata

    public void NuovoGiorno_inMetro()
    {
        //Aggiorna le stats di tutti (semi-)randomicamente
        foreach(CharacterStats chSt in salvScript.LeggiPersonStatsScript())
        {
             float ch_fame = chSt.LeggiFame(),
                   ch_sete = chSt.LeggiSete(),
                   ch_stanch = chSt.LeggiStanch();

            float daAgg_fame = Random.Range(10f, 20f),
                  daAgg_sete = Random.Range(15f, 25f),
                  daAgg_stanch = new float(),
                  daTogl_vita;

            //Aggiunge valori random alla Fame & Sete
            chSt.AggiungiAllaFame(daAgg_fame);
            chSt.AggiungiAllaSete(daAgg_sete);

            #region Stanchezza
            
            //Determina quale valore deve aggiungere (o togliere)
            //alla Stanchezza rispetto ai valori di Fame & Sete
            //(se sono alti la aggiunge, se sono bassi la toglie)
            daAgg_stanch += curvaAumentoStanch.Evaluate(ch_fame);  //--Ritorna il valore "value" della curva dandogli ch_fame come il "time"--//
            daAgg_stanch += curvaAumentoStanch.Evaluate(ch_sete);

            chSt.AggiungiAllaStanch(daAgg_stanch); //E l'aggiunge

            #endregion

            #region Vita

            float max_daTogl = curvaDiminuizVita.Evaluate(ch_stanch);

            //Prende a caso un valore da togliere alla Vita
            //con al max il valore della Stanchezza
            daTogl_vita = Random.Range(0f, max_daTogl);

            chSt.TogliAllaVita(daTogl_vita); //E la toglie

            #endregion
        }

        #region NON serve perché la salva nella timeline (vedi il ev.Invoke() qua dopo)
        //Salva la giornata
        //salvScript.SalvaPartita();
        #endregion

        ev_NuovoGiorno_inMetro.Invoke();
    }

    public void InizioNotte_Fuori()
    {
        #region Variabili di percentuale

        //Variabili usate per la % uscita nella scelta
        //(che determineranno se l'evento viene aggiunto o meno)
        int percCibo, percAcqua, percMedicine, percEnergia,
            percAmici, percLotta=0,
            percArmaC=0, percArmaP=0, percArmaF=0,
            percRadio;

        //Variabili usate per il range di % da usare nella scelta
        int min_percCibo=0, min_percAcqua=0, min_percMedicine=0, min_percEnergia=0, 
            min_percAmici=0, min_percLotta=0, 
            min_percArmaC=0, min_percArmaP=0, min_percArmaF=0,
            min_percRadio;

        #endregion

        #region Sceglie il minimo (del range della %) rispetto in quale zona si trova

        switch (stazScript.LeggiZonaDellaStazioneAttuale())
        {
            case 1:  //Piazza
                min_percCibo = 60;
                min_percAcqua = 60;
                min_percMedicine = 45;
                min_percEnergia = 45;
                 min_percAmici = 90;
                 min_percLotta = 10;
                min_percArmaC = 25;
                min_percArmaP = 10;
                min_percArmaF = 10;
                break;
            
            case 2:  //Mercato
                min_percCibo = 80;
                min_percAcqua = 60;
                min_percMedicine = 20;
                min_percEnergia = 20;
                 min_percAmici = 50;
                 min_percLotta = 50;
                min_percArmaC = 30;
                min_percArmaP = 10;
                min_percArmaF = 10;
                break;

            case 3:  //Zona Abitata
                min_percCibo = 60;
                min_percAcqua = 80;
                min_percMedicine = 35;
                min_percEnergia = 35;
                 min_percAmici = 50;
                 min_percLotta = 50;
                min_percArmaC = 35;
                min_percArmaP = 20;
                min_percArmaF = 10;
                break;

            case 4:  //Fabbrica
                min_percCibo = 30;
                min_percAcqua = 30;
                min_percMedicine = 10;
                min_percEnergia = 75;
                 min_percAmici = 60;
                 min_percLotta = 20;
                min_percArmaC = 80;
                min_percArmaP = 70;
                min_percArmaF = 60;
                break;

            case 5:  //Ospedale
                min_percCibo = 30;
                min_percAcqua = 30;
                min_percMedicine = 80;
                min_percEnergia = 25;
                 min_percAmici = 80;
                 min_percLotta = 15;
                min_percArmaC = 60;
                min_percArmaP = 50;
                min_percArmaF = 40;
                break;
        }

        min_percRadio = 10;
        
        /* NOTA:
         * Se si pensa che la percentuale minima
         * di trovare la radio sia troppo alta, 
         * l'idea originale era quella di metterla
         * al 5%, e non al 10%
         */

        #endregion

        #region Funzioni che scelgono random la percentuale di avvenuta dell'evento

        percCibo = Random.Range(0, 101);
        percAcqua = Random.Range(0, 101);
        percMedicine = Random.Range(0, 101);
        percEnergia = Random.Range(0, 101);
        percRadio = Random.Range(0, 101);
        if (managRis.LeggiHaUnArma())
        {
            percLotta = Random.Range(0, 101);
            percAmici = Random.Range(0, 101);
        }
        else
        {
            percAmici = Random.Range(0, 101);
            percArmaC = Random.Range(0, 101);
            percArmaP = Random.Range(0, 101);
            percArmaF = Random.Range(0, 101);
        }
        #endregion

        #region Aggiunge gli eventi e Mescola
        /* LEGENDA CODA/QUEUE:
         * 1 = Cibo
         * 2 = Acqua
         * 3 = Medicine
         * 4 = Energia
         * 5 = Pezzo Radio
         * 9 = Lotta (inizio)
         * 901 = Lotta (combatti)
         * 9010 = Lotta (avvia minigame)
         * 902 = Lotta (combatti-> vinci)
         * 903 = Lotta (combatti-> perdi)
         * 904 = Lotta (fuggi)
         * 909 = Lotta (morte del person.) [MAI USATA]
         * 0 = Amici
         * 91 = Coltello trovato
         * 92 = Pistola trovata
         * 93 = Fucile trovato
         * 7 = Si torna alla metro
         */

        //Se esce la percentuale giusta, aggiunge "l'evento"
        if (percCibo <= min_percCibo) codaEventi.Enqueue(1);
        if (percAcqua <= min_percAcqua) codaEventi.Enqueue(2);
        if (percMedicine <= min_percMedicine) codaEventi.Enqueue(3);
        if (percEnergia <= min_percEnergia) codaEventi.Enqueue(4);
        if (percRadio <= min_percRadio) codaEventi.Enqueue(5);
        if (managRis.LeggiHaUnArma())
        {
            int risultatoMaggiore = percAmici <= percLotta ? 9 : 0;

            switch (risultatoMaggiore)
            {
                //Se la percentuale degli Amici è maggiore
                case 0:
                    if (percAmici <= min_percAmici)
                    {
                        //Mette una risorsa a caso tra tutte e 4
                        int risACaso = Random.Range(1, 5);  //Tra 1 e 4
                        codaEventi.Enqueue(risACaso);
                    }
                    break;

                //Se la percentuale dei Nemici è maggiore
                case 9:
                    if (percLotta <= min_percLotta && percLotta >= percAmici)
                        codaEventi.Enqueue(9);
                    break;
            }

            #region OLD
            //Se esce la percentuale di un amico & è maggiore della lotta

            //Se esce la percentuale della lotta & è maggiore degli amici
            #endregion
        }
        else
        {
            //Se incontra degli amici
            if (percAmici <= min_percAmici)
            {
                int risorsaACaso = Random.Range(1, 5);  //Da 1 a 4, ovvero [1; 4)
                codaEventi.Enqueue(risorsaACaso);
            }


            #region Prende l'arma trovata con la percentuale maggiore            

            int[] percentuali_armi = { percArmaC, percArmaP, percArmaF };

            int percMaggioreArmi = 0;

            //Trova la maggiore
            for (int i = 0; i < percentuali_armi.Length; i++)
            {
                if (percentuali_armi[i] > percMaggioreArmi)
                    percMaggioreArmi = percentuali_armi[i];
            }

            //Aggiunge la maggiore alla lista
            switch (percMaggioreArmi)
            {
                default:
                case 1:
                    //Se trova il Coltello
                    if (percArmaC <= min_percArmaC)
                        codaEventi.Enqueue(91);
                    break;

                case 2:
                    //Se trova la Pistola
                    if (percArmaP <= min_percArmaP)
                        codaEventi.Enqueue(92);
                    break;

                case 3:
                    //Se trova il Fucile
                    if (percArmaF <= min_percArmaF)
                        codaEventi.Enqueue(93);
                    break;
            }
            #endregion
        }


        //Rende casuale la coda
        codaEventi = Mescola(codaEventi);

        //Ultimo punto da mettere nella coda per informare che si torna in metro
        codaEventi.Enqueue(7);


        #region ---DEBUG---
        
        string _ = "";
        foreach (var a in codaEventi)
            _ += a + " ";
        print("<b>Coda generata</b>: " + _);
        
        #endregion

        #endregion


        #region Passa all'esplorazione

        //Avvia il crossfade
        crossFadeMEScript.ScriviSiVaAdEsplorare(true);
        //crossFadeMEScript.ScriviIniziaTransizione(true);  //OLD_NON USATO

        //Notifica al GestoreTesti che è andato ad esplorare
        txtsScript.ScriviSonoInEsplorazione(true);

        //Inizia a scrivere
        txtsScript.IniziaAScrivere();
        //opzScript.ScenaSuccessiva();

        #endregion
    }

    Queue<int> Mescola(Queue<int> coda)
    {
        for (int i = 0; i < coda.Count; i++)
        {
            //int indiceRandom = Random.Range(0, coda.Count),
            //    temp = coda.ToArray()[indiceRandom];

            //coda.ToArray()[indiceRandom] = coda.ToArray()[i];
            //coda.ToArray()[i] = temp;
            int temp = coda.ToArray()[i],
                indiceRandom = Random.Range(0, coda.Count);

            coda.ToArray()[i] = coda.ToArray()[indiceRandom];
            coda.ToArray()[indiceRandom] = temp;
        }

        return coda;
    }

    public void FineNotte_Fuori()
    {
        //Torna alla metro
        crossFadeMEScript.ScriviSiVaAdEsplorare(false);
        //opzScript.ScenaScegliTu(1);

        //Notifica al GestoreTesti che non è più in esplorazione
        txtsScript.ScriviSonoInEsplorazione(false);

        //Salva la giornata
        salvScript.SalvaPartita();
        
        //Aggiorna il num dei giorni
        numGiorni++;
    }

    #endregion

    #region Funzioni per cambiare e leggere gli eventi

    public int VediProssimoEvento()
    {
        int avantiIlProssimo = codaEventi.Peek();

        return avantiIlProssimo;
    }

    public void TogliProssimoEvento()
    {
        codaEventi.Dequeue();
    }

    public void SostituisciEventoAttuale(int tipo_ev)
    {
        //Salva la coda in una temporanea
        int[] codaTemp;
        codaTemp = codaEventi.ToArray();

        //Cancella la coda vecchia
        for (int i=0; i < codaEventi.Count; i++)
            codaEventi.Dequeue();
        
        //Sostituisce nella coda temporanea il nuovo evento
        codaTemp[0] = tipo_ev;

        //Riprende ogni evento da quella temporanea
        codaEventi = new Queue<int>(codaTemp);
        
        //Avvia il testo
        txtsScript.ScriviTestoFinito(true);
    }

    #endregion
}
