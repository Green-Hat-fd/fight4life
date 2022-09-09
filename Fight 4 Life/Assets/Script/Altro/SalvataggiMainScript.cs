using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SalvataggiMainScript : MonoBehaviour
{
    [SerializeField]
    TextAsset txtSalvataggio;

    #region Variabili da dove prende le informazioni
    [Header("––Variabili delle informazioni––")]
    [SerializeField]
    ManagerRisorse risorseScript;
    [SerializeField]
    StazioniMainScript stazScript;
    [SerializeField]
    GiorniMainScript giorniScript;
    [SerializeField]
    GameObject gruppoPerson;
    [SerializeField]
    CharacterStats[] personStatsScript = new CharacterStats[4];
    #endregion

    [SerializeField]
    AudioSource sfxSalvaPartita, sfxCaricaPartita;
    
    [SerializeField]
    GameObject pausaTestoSalvato;

    /* #region Variabili aggiuntive per essere scritte
     * [Header("––Variabili aggiuntive per caricare il salvat.––")]
     * #endregion
     */

    string percorso_file;

    #region --DEBUG (nell'editor)--
    [SerializeField, Multiline(40), Space(15)]
    string fine;
    #endregion


    private void Awake()
    {
        gruppoPerson = GameObject.FindGameObjectWithTag("Elenco-dei-Personaggi");

        if (personStatsScript.Length != 4)
            personStatsScript = new CharacterStats[4];

        for (int i = 0; i < 4; i++)
        {
            //Prende tutti i figli del gruppo dei personaggi e li assegna alla variabile
            personStatsScript[i] = gruppoPerson.transform.GetChild(i).GetComponent<CharacterStats>();
        }




        //Prende la posizione del file
        percorso_file = Application.dataPath + "/salvFile.txt";

        //Da mettere le variabili che vengono messe all'inizio (come la var delle OpzioniMainScript)
        risorseScript = FindObjectOfType<ManagerRisorse>();
        stazScript = FindObjectOfType<StazioniMainScript>();
    }

    private void Update()
    {
        #region NON_USATO_Rileva se la scena è stata cambiata

        //GameObject cambioSc = GameObject.FindGameObjectWithTag("Cambio-scena");
        //if (cambioSc != null)
        //{
        //    Awake();

        //    Destroy(cambioSc);
        //}
        #endregion

        #region Rileva se c'è l'oggetto per creare una nuova partita

        GameObject segnalaNuovaPart = GameObject.FindGameObjectWithTag("Nuova-Partita");

        //Se trova l'oggetto "segnalatore", crea una nuova partita e distrugge l'oggetto
        if (segnalaNuovaPart != null)
        {
            GeneraNuovaPartita();

            Destroy(segnalaNuovaPart);
        }
        #endregion

        #region Rileva se c'è l'oggetto per caricare (load) la partita

        GameObject segnalaCaricaPart = GameObject.FindGameObjectWithTag("Carica-Salvataggio");

        //Se trova l'oggetto "segnalatore", crea una nuova partita e distrugge l'oggetto
        if (segnalaCaricaPart != null)
        {
            CaricaSalvataggio();

            Destroy(segnalaCaricaPart);
        }
        #endregion
    }

    #region Funzioni Get personalizzate

    public CharacterStats[] LeggiPersonStatsScript()
    {
        return personStatsScript;
    }

    #endregion

    public void SalvaPartita()
    {
        string finale;

        finale = "";


        #region --Personaggi--

        finale += "# PERSONAGGI #\n";

        finale += "4" + "\n";  //Quanti personaggi ci sono

        for (int i = 0; i < 4; i++)
        {
            finale += personStatsScript[i].LeggiNome() + "\n"
                   + personStatsScript[i].LeggiVita() + "\n"
                   + personStatsScript[i].LeggiFame() + "\n"
                   + personStatsScript[i].LeggiSete() + "\n"
                   + personStatsScript[i].LeggiStanch() + "\n";
        }
        #endregion

        #region --Risorse--

        finale += "\n# RISORSE #\n";

        finale += risorseScript.LeggiCibo() + "\n"
               + risorseScript.LeggiAcqua() + "\n"
               + risorseScript.LeggiMedicine() + "\n"
               + risorseScript.LeggiEnergia() + "\n"
               + risorseScript.LeggiTipoArma() + "\n"
               + risorseScript.LeggiPezziRadio() + "\n";

        finale += giorniScript.LeggiNumeroGiorni() + "\n";

        #endregion

        #region --Stazioni--

        finale += "\n# STAZIONI #\n";

        finale += stazScript.LeggiMaxStazioni() + "\n"
               + stazScript.LeggiStazioneAttuale() + "\n";

        foreach (int z in stazScript.LeggiTutteZoneDelleStazioni())
        {
            finale += z + "\n";  //Ogni zona all'esterno di tutte le stazioni
        }
        #endregion

        //Sovrascrive il file
        //(se non esiste, ne crea uno nuovo e ci scrive)
        //if (!File.Exists(percorso_file))
            File.WriteAllText(percorso_file, finale);

        sfxSalvaPartita.PlayDelayed(.5f);

        //Avvia l'animazione di pausa (se c'è il testo piccolo)
        if (pausaTestoSalvato != null)
            pausaTestoSalvato.GetComponent<Animator>().Play("AnimazioneTestoSalvat");


        //fine = finale; //OLD_DEBUG

        #region Prodotto finale
        /*  0:  ### PERSONAGGI ###
         *  1:  tot personaggi (4 o 5)
         *  2:  Nome (soldato)
         *  3:  - Vita
         *  4:  - Fame
         *  5:  - Sete
         *  6:  - Stanchezza
         *  7:  Nome (influencer-medico-cuoco)
         *  8:  - Vita
         *  9:  - Fame
         * 10:  - Sete
         * 11:  - Stanchezza
         * 12:  Nome (influencer-medico-cuoco)
         * 13:  - Vita
         * 14:  - Fame
         * 15:  - Sete
         * 16:  - Stanchezza
         * 17:  Nome (influencer-medico-cuoco)
         * 18:  - Vita
         * 19:  - Fame
         * 20:  - Sete
         * 21:  - Stanchezza
         * 22:  
         * 23:  ### RISORSE ###
         * 24:  Cibo (tot)
         * 25:  Acqua (tot)
         * 26:  Medicine (tot)
         * 27:  Energia (tot)
         * 28:  tipo dell'Arma raccolta (0= nulla, 1= coltello, 2= pistola, 3= fucile)
         * 29:  Pezzi della Radio (tot)
         * 30:  numero dei Giorni
         * 31:  
         * 32:  ### STAZIONI ###
         * 33:  num Max stazioni (4 o 5)
         * 34:  stazione Attuale (da 0 a 5)
         * 35:  num Zona esterno 1a stazione
         * 36:  num Zona esterno 2a stazione
         * 37:  num Zona esterno 3a stazione
         * 38:  num Zona esterno 4a stazione
         * 39:  num Zona esterno 5a stazione
         * 40:
         */
        #endregion
    }

    public void CaricaSalvataggio()
    {
        string[] letturaDelFile = new string[0];
        
        int i_personaggi=0, i_risorse=0, i_stazioni=0;


        //Legge il file di salvataggio
        if (File.Exists(percorso_file))
            letturaDelFile = File.ReadAllLines(percorso_file);
        else
            print("[!] Messaggio di errore");

        //Cerca nell'array i punti di inizio delle varie "regioni"
        for (int i= 0; i < letturaDelFile.Length; i++)
        {
            switch (letturaDelFile[i])
            {
                case "# PERSONAGGI #":
                    i_personaggi = i;
                    break;
                
                case "# RISORSE #":
                    i_risorse = i;
                    break;
                
                case "# STAZIONI #":
                    i_stazioni = i;
                    break;
            }
        }

        #region --Personaggi--

            /*Trasforma da string a float*/
        float vita_load = float.Parse(letturaDelFile[i_personaggi + 3]),
              fame_load = float.Parse(letturaDelFile[i_personaggi + 4]),
              sete_load = float.Parse(letturaDelFile[i_personaggi + 5]),
              stanch_load = float.Parse(letturaDelFile[i_personaggi + 6]);

        //Load del massimo dei personaggi (4 o 5)
        //.ScriviMaxPersonaggi(letturaDelFile[i_personaggi + 1]);

        //Load delle stats di tutt i personaggi
        for (int i = 0; i < 4; i++)
        {
            personStatsScript[i].ScriviVita(vita_load);
            personStatsScript[i].ScriviFame(fame_load);
            personStatsScript[i].ScriviSete(sete_load);
            personStatsScript[i].ScriviStanch(stanch_load);
        }
        #endregion

        #region --Risorse--

            /*Trasforma da string a int*/
        int cibo_load = int.Parse(letturaDelFile[i_risorse + 1]),
            acqua_load  = int.Parse(letturaDelFile[i_risorse + 2]),
            medicine_load = int.Parse(letturaDelFile[i_risorse + 3]),
            energia_load = int.Parse(letturaDelFile[i_risorse + 4]),
            tipoArma_load = int.Parse(letturaDelFile[i_risorse + 5]),
            pezziRadio_load = int.Parse(letturaDelFile[i_risorse + 6]),
            numGiorni_load = int.Parse(letturaDelFile[i_risorse + 7]);

        //Load di tutte i numeri delle risorse
        risorseScript.ScriviCibo(cibo_load);
        risorseScript.ScriviAcqua(acqua_load);
        risorseScript.ScriviMedicine(medicine_load);
        risorseScript.ScriviEnergia(energia_load);
        risorseScript.ScriviTipoArma(tipoArma_load);
        risorseScript.ScriviPezziRadio(pezziRadio_load);
        
        giorniScript.ScriviNumeroGiorni(numGiorni_load);
        #endregion

        #region --Stazioni--

            /*Trasforma da string a int*/
        int maxStaz_load = int.Parse(letturaDelFile[i_stazioni + 1]),
            stazAtt_load = int.Parse(letturaDelFile[i_stazioni + 2]);

        //Load delle stazioni
        stazScript.ScriviMaxStazioni(maxStaz_load);
        stazScript.ScriviStazioneAttuale(stazAtt_load);

        //Load dell'esterno di tutte le stazioni
        for (int i = 0; i < 5; i++)
        {
            int zonaFuori_load = int.Parse(letturaDelFile[i_stazioni + 3 + i]); //Prende tutte le zona una ad una

            stazScript.ScriviZonaDellaStazioneScelta(i, zonaFuori_load);  
        }
        #endregion


        sfxCaricaPartita.PlayDelayed(.5f);
    }

    public void GeneraNuovaPartita()
    {
        int[] zoneStazioni = new int[stazScript.LeggiMaxStazioni()];


        //Cancella il file di salvataggio
        CancellaFileSalvataggio();

        //Genera nuove risorse
        risorseScript.ScriviCibo(Random.Range(5, 10));
        risorseScript.ScriviAcqua(Random.Range(5, 10));
        risorseScript.ScriviMedicine(Random.Range(5, 10));
        risorseScript.ScriviEnergia(10);
        risorseScript.ScriviPezziRadio(0);

        //Genera stazioni a caso
        for (int i = 0; i < stazScript.LeggiMaxStazioni(); i++)
        {
            zoneStazioni[i] = Random.Range(1, 6);  //Random tra [1; 6)
        }

        #region OLD
        /*Prende una stazione a caso e la duplica*/
        //zoneStazioni[zoneStazioni.Length - 1] = zoneStazioni[Random.Range(0, zoneStazioni.Length)];
        #endregion

        /*Mescola (randomizza) le stazioni*/
        for (int i = 0; i < zoneStazioni.Length; i++)
        {
            int temp = zoneStazioni[i],
                indiceRandom = Random.Range(0, zoneStazioni.Length);
            zoneStazioni[i] = zoneStazioni[indiceRandom];
            zoneStazioni[indiceRandom] = temp;
        }

            /*Mette le scelte in quella*/
        for (int i = 0; i < zoneStazioni.Length; i++)
        {
            stazScript.ScriviZonaDellaStazioneScelta(i, zoneStazioni[i]);
        }

        //Mette le statistiche base
        for (int i = 0; i < 4; i++)
        {
            personStatsScript[i].ScriviVita(100);
            personStatsScript[i].ScriviFame(Random.Range(0f, 7.5f));
            personStatsScript[i].ScriviSete(Random.Range(0f, 7.5f));
            personStatsScript[i].ScriviStanch(Random.Range(0f, 7.5f));
        }

        //Salva in un nuovo file
        SalvaPartita();
    }

    public void CancellaFileSalvataggio()
    {
        //Se già esiste, lo elimina
        if (File.Exists(percorso_file))
            File.Delete(percorso_file);
    }
}
