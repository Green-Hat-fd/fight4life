using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AnimCompareTesto : MonoBehaviour
{
    CanvasGroup gruppoCanv;

    [Tooltip("La velocità con cui compare il testo/parte della UI\n(Deve avere un CanvasGroup)")]
    [SerializeField]
    float velCompare = .4f;


    void Awake()
    {
        gruppoCanv = GetComponent<CanvasGroup>();

        gruppoCanv.alpha = 0;
    }

    void Update()
    {
        //Aumenta l'opacità con la velocità "vel compare" (default: pian piano)
        if (gruppoCanv.alpha < 1f)
            gruppoCanv.alpha = Mathf.MoveTowards(gruppoCanv.alpha, 1f, velCompare * Time.deltaTime);
    }
}
