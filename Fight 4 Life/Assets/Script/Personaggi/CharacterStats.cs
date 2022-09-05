using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    string nome;

    [SerializeField, Range(0f, 100f)]
    float vita, fame, sete, stanchezza;

    float MAX_vita = 100,
          MAX_fame = 100,
          MAX_sete = 100,
          MAX_stam = 100;

    [SerializeField]
    bool sonoMorto = false,
         inEsplorazione = false;

    //Variabili per cambio di statistiche
    [SerializeField]
    bool mangiato = false,
         bevuto = false,
         curato = false;

    //Variabili del personaggio
    SpriteRenderer sprPers;
    GameObject ombraObj;


    private void Start()
    {
        sprPers = GetComponentInChildren<SpriteRenderer>();
        ombraObj = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        //Il personaggio mangia (Diminuisce la fame se ha mangiato)
        if (mangiato && !sonoMorto)
        {
            if (fame >= 0 || fame < MAX_fame)
                fame -= 25f;

            fame = Mathf.Clamp(fame, 0f, MAX_fame);

            #region --DEBUG--
            print(gameObject.name + " ha mangiato");
            #endregion

            mangiato = false;
        }

        //Il personaggio beve (Diminuisce la sete se ha bevuto)
        if (bevuto && !sonoMorto)
        {
            if (sete >= 0 || sete < MAX_sete)
                sete -= 25f;

            sete = Mathf.Clamp(sete, 0f, MAX_sete);

            #region --DEBUG--
            print(gameObject.name + " ha bevuto");
            #endregion

            bevuto = false;
        }

        //Il personaggio viene curato (Aumenta la vita se ha ricevuto cure)
        if (curato && !sonoMorto)
        {
            if (vita >= 0 || vita < MAX_vita)
                vita += 45f;

            vita = Mathf.Clamp(vita, 0f, MAX_vita);

            #region --DEBUG--
            print(gameObject.name + " ha preso le medicine");
            #endregion

            curato = false;
        }

        //Muore se la vita arriva a 0
        if (vita <= 0)
            sonoMorto = true;

        //Rende o meno trasparente il personaggio se è in esplorazione
        sprPers.color = inEsplorazione ? new Color(.75f, .75f, .75f, .4f) : Color.white;
        ombraObj.SetActive(inEsplorazione || sonoMorto ? false : true);

        //Toglie il personaggio se è morto
        sprPers.gameObject.SetActive(sonoMorto ? false : true);
    }

    #region Funzioni Set personalizzate

    public void ScriviMangiato(bool m)
    {
        mangiato = m;
    }

    public void ScriviBevuto(bool b)
    {
        bevuto = b;
    }

    public void ScriviCurato(bool c)
    {
        curato = c;
    }

    public void ScriviVita(float v)
    {
        vita = v;
    }

    public void ScriviFame(float f)
    {
        fame = f;
    }

    public void ScriviSete(float s)
    {
        sete = s;
    }

    public void ScriviStanch(float s)
    {
        stanchezza = s;
    }

    public void TogliAllaVita(float v_toSubt)
    {
        vita -= v_toSubt;
        
        vita = Mathf.Clamp(vita, 0f, MAX_vita);
    }

    public void AggiungiAllaFame(float f_toAdd)
    {
        fame += f_toAdd;
        
        fame = Mathf.Clamp(fame, 0f, MAX_fame);
    }

    public void AggiungiAllaSete(float s_toAdd)
    {
        sete += s_toAdd;
        
        sete = Mathf.Clamp(sete, 0f, MAX_sete);
    }

    public void AggiungiAllaStanch(float s_toAdd)
    {
        stanchezza += s_toAdd;

        stanchezza = Mathf.Clamp(stanchezza, 0f, MAX_stam);
    }

    #endregion

    #region Funzioni Get personalizzate

    public string LeggiNome()
    {
        return nome;
    }

    public float LeggiVitaPercent()
    {
        return vita / MAX_vita;
    }
    public float LeggiVita()
    {
        return vita;
    }

    public float LeggiFamePercent()
    {
        return fame / MAX_fame;
    }
    public float LeggiFame()
    {
        return fame;
    }

    public float LeggiSetePercent()
    {
        return sete / MAX_sete;
    }
    public float LeggiSete()
    {
        return sete;
    }

    public float LeggiStanchPercent()
    {
        return stanchezza / MAX_stam;
    }
    public float LeggiStanch()
    {
        return stanchezza;
    }

    public bool LeggiSonoMorto()
    {
        return sonoMorto;
    }

    public bool LeggiInEsplorazione()
    {
        return inEsplorazione;
    }

    #endregion
}
