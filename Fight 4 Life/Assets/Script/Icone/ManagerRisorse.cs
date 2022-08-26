using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ManagerRisorse : MonoBehaviour
{
    //Totale delle risorse
    [SerializeField]
    int cibo, acqua, medicine, energia, pezziRadio;
    [SerializeField, Range(0, 3), Space(10)]
    int tipoArma;

    //Variabile che indica se un'arma è stata trovata
    [SerializeField]
    bool haUnArma, radioTrovata;

    //Testi da cambiare (counter delle risorse)
    [SerializeField]
    GameObject gruppoRisorse;
    TMP_Text[] counterRisorse = new TMP_Text[4];
    [SerializeField]
    TMP_Text counterPezziRadio;


    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            //Prende tutti i figli del gruppo del counter e li assegna alla variabile
            counterRisorse[i] = gruppoRisorse.transform.GetChild(i).GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        LimitaRisorse();
        CambiaCounterUi();

        //Mette a 0 se non abbiamo trovato nessun'arma
        if (!haUnArma)
            tipoArma = 0;

        if (radioTrovata)
            ;//FA LE SUE COSE
    }

    #region Funzioni cambio num risorse

    /// <summary>
    /// Toglie 1 da Cibo
    /// </summary>
    public void TogliCibo()
    {
        if(cibo > 0)
            cibo--;
    }
    public void AggiungiCibo(int daAgg)
    {
        if(cibo <= 99)
            cibo += daAgg;
    }

    /// <summary>
    /// Toglie 1 da Acqua
    /// </summary>
    public void TogliAcqua()
    {
        if(acqua > 0)
            acqua--;
    }
    public void AggiungiAcqua(int daAgg)
    {
        if(acqua <= 99)
            acqua += daAgg;
    }

    /// <summary>
    /// Toglie 1 da Medicine
    /// </summary>
    public void TogliMedicine()
    {
        if(medicine > 0)
            medicine--;
    }
    public void AggiungiMedicine(int daAgg)
    {
        if(medicine <= 99)
            medicine += daAgg;
    }

    /// <summary>
    /// Toglie 1 da Energia
    /// </summary>
    public void TogliEnergia()
    {
        if(energia > 0)
            energia--;
    }
    public void AggiungiEnergia(int daAgg)
    {
        if(energia <= 99)
            energia += daAgg;
    }

    /// <summary>
    /// Toglie 1 dai pezzi della Radio
    /// </summary>
    public void    TogliPezziRadio()
    {
        if(pezziRadio > 0)
            pezziRadio--;
    }
    public void AggiungiPezziRadio(int daAgg)
    {
        if(pezziRadio <= 99)
            pezziRadio += daAgg;
    }

    #endregion

    #region Funzioni Get personalizzate

    public int LeggiCibo()
    {
        return cibo;
    }

    public int LeggiAcqua()
    {
        return acqua;
    }

    public int LeggiMedicine()
    {
        return medicine;
    }

    public int LeggiEnergia()
    {
        return energia;
    }

    public bool LeggiHaUnArma()
    {
        return haUnArma;
    }

    public int LeggiTipoArma()
    {
        return tipoArma;
    }

    public int LeggiPezziRadio()
    {
        return pezziRadio;
    }

    #endregion

    #region Funzioni Set personalizzate

    public void ScriviCibo(int c)
    {
        cibo = c;
    }

    public void ScriviAcqua(int a)
    {
        acqua = a;
    }

    public void ScriviMedicine(int m)
    {
        medicine = m;
    }

    public void ScriviEnergia(int e)
    {
        energia = e;
    }

    public void ScriviHaUnArma(bool tf)
    {
        haUnArma = tf;
    }

    public void ScriviTipoArma(int tA)
    {
        tipoArma = tA;
    }

    public void ScriviPezziRadio(int pR)
    {
        pezziRadio = pR;
    }

    #endregion

    #region Limita le risorse & cambia counter risorse

    void LimitaRisorse()
    {
        cibo = Mathf.Clamp(cibo, 0, 99);
        acqua = Mathf.Clamp(acqua, 0, 99);
        medicine = Mathf.Clamp(medicine, 0, 99);
        energia = Mathf.Clamp(energia, 0, 99);
        pezziRadio = Mathf.Clamp(pezziRadio, 0, 99);
    }

    void CambiaCounterUi()
    {
        counterRisorse[0].text = cibo.ToString(); //del cibo
        counterRisorse[1].text = acqua.ToString(); //dell'acqua
        counterRisorse[2].text = medicine.ToString(); //delle medicine
        counterRisorse[3].text = energia.ToString(); //dell'energia
        counterPezziRadio.text = pezziRadio.ToString(); //dei pezzi della radio
    }

    #endregion
}
