using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using GD.MinMaxSlider;

public class Miragame_delleArmi : MonoBehaviour
{
    [SerializeField]
    ManagerRisorse risorseScript;
    GestoreTesti testiScript;

    [SerializeField, Tooltip("Se non metti lo slider, lo prende da sé stesso \noppure lo prende dai figli")]
    Slider sliderMG;

    [SerializeField, Range(0, 3)]
    int arma;

    //Velocità delle armi
    float velocColt;
    [SerializeField]
    float velocPist = 40,
          velocFucLati = 4,
          velocFucCentro = 15;
    
    //Il numero di arrivo per ogni arma (vedi "Funz. -->" più sotto)
    float numArrivo,
          numArrivo_random;
    
    /*
    [SerializeField, MinMaxSlider(1, 10)]  //Il range di velocità rispettivamente del coltello
    Vector2 range_velC;     [3, 5.5]
    [SerializeField, MinMaxSlider(-1f, 1f)]  //Il range di quando il fucile deve rallentare
    Vector2 range_rallentaFuc;      [-0.15, 0.15]
    */

    bool siMuove,
         aperto = false;

    [Header("\"Animazione\" della barretta")]
    [SerializeField]
    Image barretta;
    [SerializeField]
    Sprite spr_barrChiara;
    [SerializeField]
    AudioSource musEsplCalma, musEsplNervosa;

    [Header("Risultato del minigame")]
    [SerializeField]
    AnimationCurve curvaRisultatoMinigame;

    public static float percentualeUscita;


    private void Start()
    {
        //Se non trova il component Slider, allora prendi quello dei figli,
        //altrimenti (lo ha trovato) mette il suo
        sliderMG = GetComponent<Slider>() != null ? GetComponent<Slider>() : GetComponentInChildren<Slider>();
        
        risorseScript = FindObjectOfType<ManagerRisorse>();
        testiScript = FindObjectOfType<GestoreTesti>();
        
        if (risorseScript == null)
            risorseScript = FindObjectOfType<ManagerRisorse>();

        siMuove = true;

        #region Scelta casuale iniziale

        switch (Random.Range(1, 3)) //int a caso in [1, 2]
        {
            case 1: numArrivo = -1; break;
            case 2: numArrivo = 1; break;
        }
        numArrivo_random = Random.Range(-1f, 1f);
        velocColt = Random.Range(3f, 5.5f);
        #endregion
    }

    void Update()
    {
          //Se non trova il component Animator, allora prendi quello dei figli,
          //altrimenti (lo ha trovato) mette il suo
        Animator animSlider = GetComponent<Animator>() != null ? GetComponent<Animator>() : GetComponentInChildren<Animator>();
        int cambioMus = new int();
        
        arma = risorseScript.LeggiTipoArma();


        #region Animazione di apertura e chiusura

        if (aperto)
        {
            animSlider.SetBool("Aperto", true);
            cambioMus = 1;
        }
        else
        {
            animSlider.SetBool("Aperto", false);
        }
        #endregion

        //Se si preme spazio, si ferma
        if (Input.GetKeyDown(KeyCode.Space))
            siMuove = false;

        #region Cosa fa quando si muove e quando si ferma

        if (siMuove)
        {
            //Sceglie il tipo di movimento rispetto all'arma selezionata
            switch (arma)
            {
                case 1: MovimentoColtello(); break;
                case 2: MovimentoPistola(); break;
                case 3: MovimentoFucile(); break;
            }
        }
        else
        {
            //Avverte che si è premuto [Spazio]
            barretta.sprite = spr_barrChiara;
            cambioMus = 0;

            //Ritorna il valore dove si è fermato lo slider
            percentualeUscita = curvaRisultatoMinigame.Evaluate(Mathf.Abs(sliderMG.value));

            //Appena finisce l'animazione, chiude il minigame
            StartCoroutine(FineMinigame());
        }
        #endregion

        #region Crossfade della musica

        switch (cambioMus)
        {
            //Musica nervosa --> calma
            case 0:
                musEsplCalma.volume   = Mathf.Lerp(musEsplCalma.volume,   1f, 1.5f * Time.deltaTime);
                musEsplNervosa.volume = Mathf.Lerp(musEsplNervosa.volume, 0f, 1.5f * Time.deltaTime);
                break;

            //Musica calma --> nervosa
            case 1:
                musEsplCalma.volume   = Mathf.Lerp(musEsplCalma.volume,   0f, 1.5f * Time.deltaTime);
                musEsplNervosa.volume = Mathf.Lerp(musEsplNervosa.volume, 1f, 1.5f * Time.deltaTime);
                break;
        }
        #endregion
    }


    #region Funz. -> Movimento del Coltello
    void MovimentoColtello()
    {
        float valore = sliderMG.value;

        //Se la barretta si avvicina al valore preso a caso
        if(numArrivo_random-.1f <= valore  &&  valore <= numArrivo_random+.1f)
        {
            //Prende un nuovo numero & una nuova velocità
            numArrivo_random = Random.Range(-1f, 1f);
            velocColt = Random.Range(3f, 5.5f);
        }
        else
        {
            //Se no, continua a muoversi verso quel numero
            sliderMG.value = Mathf.MoveTowards(sliderMG.value, numArrivo_random, velocColt*Time.deltaTime);
        }
    }

    #endregion

    #region Funz. -> Movimento della Pistola
    void MovimentoPistola()
    {
        float valore = sliderMG.value;


        //Controlla se è arrivato nella alla fine (e la scambia)
        if (valore >= 1f)
            numArrivo = -1f;
        if (valore <= -1f)
            numArrivo = 1f;

        //Movimento in sè
        sliderMG.value = Mathf.MoveTowards(sliderMG.value, numArrivo, velocPist*Time.deltaTime);
    }
    #endregion

    #region Funz. -> Movimento del Fucile
    void MovimentoFucile()
    {
        float valore = sliderMG.value;


        //Controlla se è arrivato nella alla fine (e la scambia)
        if (valore >= 1f)
            numArrivo = -1f;
        if (valore <= -1f)
            numArrivo = 1f;

        //Se si trova al centro, accellera...
        if (valore <= 0.15f && valore >= -0.15f)
            sliderMG.value = Mathf.MoveTowards(sliderMG.value, numArrivo, velocFucCentro*Time.deltaTime);
        else
            //...se no, va lento
            sliderMG.value = Mathf.MoveTowards(sliderMG.value, numArrivo, velocFucLati*Time.deltaTime);

    }
    #endregion

    IEnumerator FineMinigame()
    {
        yield return new WaitForSeconds(2.5f);

        //Chiude il minigame
        aperto = false;

        //Va avanti nel testo

    }

    #region Funzioni Set personalizzate

    public void ScriviAperturaMinigioco(bool vf)
    {
        aperto = vf;
    }

    #endregion
}
