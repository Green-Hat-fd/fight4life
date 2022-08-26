using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class StazioniMainScript : MonoBehaviour
{
    [SerializeField, Range(4, 5)]
    int max_stazioni = 4;
    [SerializeField]
    int stazione_attuale = 3;

    [SerializeField, Space(15)]
    ManagerRisorse managerRisorse_script;
    [SerializeField, Space(10)]
    Button[] bottoniStazioni = new Button[5];
    [SerializeField]
    TMP_Text[] txtStazioni = new TMP_Text[5];

    [SerializeField]
    UnityEvent viaggia, noMoreEnergia;

    /// <summary>
    /// Var. della zona esterna alle stazioni:
    /// <br></br> 1. Piazza
    /// <br></br> 2. Mercato
    /// <br></br> 3. Zona abitata
    /// <br></br> 4. Fabbrica
    /// <br></br> 5. Ospedale
    /// </summary>
    int[] zoneStazioni = { 1, 2, 3, 4, 5 };
    string[] nomiStazioni = {"", "", "", "", "[CLOSED]"};

        
    private void Update()
    {
        for (int i=0; i<5; i++)
        {
            txtStazioni[i].text = nomiStazioni[i];
        }

        #region Assegna il nome a ciascuna stazione + aggiunge (QUI)/(HERE)

        for (int i = 0; i < 5; i++)
        {
            switch (zoneStazioni[i])
            {
                case 1:  //Piazza
                    CambiaNomeStazione(i, "Piazza", "Square");
                    break;

                case 2:  //Mercato
                    CambiaNomeStazione(i, "Mercato", "Market");
                    break;

                case 3:  //Zona abitata
                    CambiaNomeStazione(i, "Zona abitativa", "Housing area");
                    break;

                case 4:  //Fabbrica
                    CambiaNomeStazione(i, "Fabbrica", "Factory");
                    break;

                case 5:  //Ospedale
                    CambiaNomeStazione(i, "Ospedale", "Hospital");
                    break;
            }
        }
        
        //Se ci sono 4 stazioni, toglie la 5a (rendendola "inagibile")
        if (max_stazioni < 5)
            CambiaNomeStazione(4, "[INAGIBILE]", "[CLOSED]");

        
        //Indica al giocatore dove si trova 
        //mettendo (QUI) o (HERE) rispetto alla lingua scelta nelle opzioni
        switch (OpzioniMainScript.linguaScelta)
        {
            case 0:
                txtStazioni[stazione_attuale].text += " (QUI)";
                break;

            case 1:
            default:
                txtStazioni[stazione_attuale].text += " (HERE)";
                break;
        }
        #endregion

        //Limita il range della stazione attuale così non va fuori dagli array
        stazione_attuale = Mathf.Clamp(stazione_attuale, 0, 4);

        #region Attiva le stazioni agibili

        //Disattiva tutte (rendendo attive le compon. Image e Button, ma col Button non-interagibile)
        foreach (Button b in bottoniStazioni)
        {
            b.enabled = true;
            b.gameObject.GetComponent<Image>().enabled = true;

            b.interactable = false;
        }


        //Toglie la possibilità al giocatore di interagire con quella in cui è
        bottoniStazioni[stazione_attuale].gameObject.GetComponent<Image>().enabled = false;
        bottoniStazioni[stazione_attuale].enabled = false;

        Button buttPrev, buttNext;

        /* Assegna le variabili se esistono */
        buttPrev = stazione_attuale - 1 >= 0 ? bottoniStazioni[stazione_attuale - 1] : null;
        buttNext = stazione_attuale + 1 < max_stazioni ? bottoniStazioni[stazione_attuale + 1] : null;


        //Controllo per attivare le stazioni prima e dopo
        switch (stazione_attuale)
        {
            //Attiva quello dopo se è alla prima fermata
            case 0:
                buttNext.interactable = true;
                break;

            //Attiva quello prima e dopo se è alla penultima (4a) fermata
            case 3:

                buttPrev.interactable = true;
                
                //Viene attivato solo se il max è maggiore o uguale a 5
                if(max_stazioni >= 5)
                    buttNext.interactable = true;

                break;

            //Attiva quello prima se è all'ultima (5a) fermata
            case 4:

                //Viene attivato solo se il max è maggiore o uguale a 5
                if (max_stazioni >= 5)
                    buttPrev.interactable = true;
                //Casomai dovesse essere all'ultima (5a) stazione MA il limite è a 4,
                //riporta la stazione_attuale a quella prima (ovvero la 4a)
                else
                    stazione_attuale = 3;

                break;
            
            default:
                buttPrev.interactable = true;
                buttNext.interactable = true;
                break;
        }

        #endregion
    }


    //Funzione per viaggiare tra le stazioni (usata nelle stazioni-bottoni)
    public void PrendiNumStazione(int num)
    {
        num--;

        //Si assicura che puoi viaggiare (ovvero che hai energia)
        if (managerRisorse_script.LeggiEnergia() > 0)
        {
            viaggia.Invoke();

            if (num < stazione_attuale)
                ViaggiaStazionePrecedente();
            if (num > stazione_attuale)
                ViaggiaStazioneSuccessiva();

            //Toglie 1 di energia 
            managerRisorse_script.TogliEnergia();
        }
        else
        {
            //Se energia = 0, allora mostra al giocatore il messaggio che non ha più energia
            noMoreEnergia.Invoke();
        }
    }

    void CambiaNomeStazione(int indiceNome, string nomeITA, string nomeENG)
    {
        switch (OpzioniMainScript.linguaScelta)
        {
            case 0:
                nomiStazioni[indiceNome] = nomeITA;
                break;

            case 1:
            default:
                nomiStazioni[indiceNome] = nomeENG;
                break;
        }
    }

    #region Funzioni Get personalizzate

    public int LeggiStazioneAttuale()
    {
        return stazione_attuale;
    }

    public int LeggiMaxStazioni()
    {
        return max_stazioni;
    }

    public int[] LeggiTutteZoneDelleStazioni()
    {
        return zoneStazioni;
    }

    /// <summary>
    /// Questa funzione legge (get) la zona esterna della stazione attuale
    /// <br></br><br></br>--Ricordo che:
    /// <br></br> 1. Piazza
    /// <br></br> 2. Mercato
    /// <br></br> 3. Zona abitata
    /// <br></br> 4. Fabbrica
    /// <br></br> 5. Ospedale
    /// </summary>
    public int LeggiZonaDellaStazioneAttuale()
    {
        return zoneStazioni[stazione_attuale];
    }

    #endregion

    #region Funzioni Set personalizzate

    public void ScriviStazioneAttuale(int sa)
    {
        stazione_attuale = sa;
    }

    public void ScriviMaxStazioni(int ms)
    {
        max_stazioni = ms;
    }

    /// <summary>
    /// Questa funzione scrive (set) la zona esterna <br></br>della stazione scelta, la quale "staz"
    /// <br></br><br></br>--Ricordo che:
    /// <br></br> 1. Piazza
    /// <br></br> 2. Mercato
    /// <br></br> 3. Zona abitata
    /// <br></br> 4. Fabbrica
    /// <br></br> 5. Ospedale
    /// </summary>
    public void ScriviZonaDellaStazioneScelta(int staz, int zona)
    {
        zoneStazioni[staz] = zona;
    }

    #endregion

    #region Funzioni per viaggiare tra le stazioni

    //Sucessiva (verso destra)
    void ViaggiaStazioneSuccessiva()
    {
        //Controlla se è possibile viaggiare nella prossima stazione
        if (stazione_attuale != max_stazioni)
            stazione_attuale++;
    }

    //Precedente (verso sinistra)
    void ViaggiaStazionePrecedente()
    {
        //Controlla se è possibile viaggiare nella precedente stazione
        if (stazione_attuale != 0)
            stazione_attuale--;
    }

    #endregion
}
