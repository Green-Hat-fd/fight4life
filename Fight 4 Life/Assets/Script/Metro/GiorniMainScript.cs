using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    int numGiorni = 0;

    //Coda per "storing" gli eventi che accadranno
    Queue<int> codaEventi = new Queue<int>();


    private void Awake()
    {
        opzScript = FindObjectOfType<OpzioniMainScript>();
        managRis = FindObjectOfType<ManagerRisorse>();
        stazScript = FindObjectOfType<StazioniMainScript>();
        salvScript = FindObjectOfType<SalvataggiMainScript>();
    }

    void Update()
    {
        if (codaEventi.Count == 0)
             //o qualcosa simile che interrompe tutto e torna in metro


        //Aggiorna il testo che indica numero del giorno
        counterGiorniTXT.text = numGiorni.ToString();
    }


    #region Funzioni Get personalizzate

    public int LeggiNumeroGiorni()
    {
        return numGiorni;
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
        //Salva la giornata
        salvScript.SalvaPartita();

        ev_NuovoGiorno_inMetro.Invoke();
    }

    public void InizioNotte_Fuori()
    {
        //Variabili usate per la % uscita nella scelta
        //(che determineranno se l'evento viene aggiunto o meno)
        int percCibo, percAcqua, percMedicine, percEnergia,
            percAmici, percLotta=0,
            percArmaC=0, percArmaP=0, percArmaF=0;

        //Variabili usate per il range di % da usare nella scelta
        int min_percCibo=0, min_percAcqua=0, min_percMedicine=0, min_percEnergia=0, 
            min_percAmici=0, min_percLotta=0, 
            min_percArmaC=0, min_percArmaP=0, min_percArmaF=0;

        //Sceglie il minimo (del range della %) rispetto in quale zona si trova
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

        #region Funzioni che scelgono random la percentuale di avvenuta dell'evento
        percCibo = Random.Range(0, 101);
        percAcqua = Random.Range(0, 101);
        percMedicine = Random.Range(0, 101);
        percEnergia = Random.Range(0, 101);
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

        #region Se esce la percentuale giusta, aggiunge "l'evento"
        /* LEGENDA CODA/QUEUE:
         * 1 = Cibo
         * 2 = Acqua
         * 3 = Medicine
         * 4 = Energia
         * 9 = Lotta (con o senza minigame)
         * 0 = Amici
         * 91 = Coltello trovato
         * 92 = Pistola trovata
         * 93 = Fucile trovato
         */
        if (percCibo >= min_percCibo) codaEventi.Enqueue(1);
        if (percAcqua >= min_percAcqua) codaEventi.Enqueue(2);
        if (percMedicine >= min_percMedicine) codaEventi.Enqueue(3);
        if (percEnergia >= min_percEnergia) codaEventi.Enqueue(4);
        if (managRis.LeggiHaUnArma())
        {
            //Se esce la percentuale della di un amico & è maggiore della lotta
            if (percAmici >= min_percAmici && percAmici >= percLotta)
                codaEventi.Enqueue(0);

            //Se esce la percentuale della lotta & è maggiore degli amici
            if (percLotta >= min_percLotta && percLotta >= percAmici)
                codaEventi.Enqueue(9);
        }
        else
        {
            //Se incontra degli amici
            if (percAmici >= min_percAmici)
                codaEventi.Enqueue(0);

            //Se trova il Coltello
            if (min_percArmaC <= percArmaC)
                codaEventi.Enqueue(91);

            //Se trova la Pistola
            if (min_percArmaP <= percArmaP)
                codaEventi.Enqueue(92);

            //Se trova il Fucile
            if (min_percArmaF <= percArmaF)
                codaEventi.Enqueue(93);
        }

        codaEventi = Mescola(codaEventi);

        #endregion

        //Passa alla scena successiva (dell'esplorazione)
        opzScript.ScenaScegliTu(2);
    }

    Queue<int> Mescola(Queue<int> coda)
    {
        for (int i = 0; i < coda.Count; i++)
        {
            int temp = coda.ToArray()[i],
                indiceRandom = Random.Range(0, coda.Count);
            coda.ToArray()[i] = coda.ToArray()[indiceRandom];
            coda.ToArray()[indiceRandom] = temp;
        }

        return coda;
    }

    public void FineNotte_Fuori()
    {
        //Salva la giornata
        salvScript.SalvaPartita();
        
        //Torna alla scena della metro
        opzScript.ScenaScegliTu(1);

        //Aggiorna il num dei giorni
        numGiorni++;
    }

    #endregion

    void ProssimoEvento()
    {

        /*
         * NOTA: vedi cosa metti in Update() e comportati di conseguenza
         *       es. fai che questa funzione fa solo un dequeue, oppure dequeue-a e ritorna quello dopo…
         */
    }
}
