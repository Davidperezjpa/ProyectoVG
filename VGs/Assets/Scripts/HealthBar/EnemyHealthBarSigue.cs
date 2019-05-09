using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarSigue : MonoBehaviour
{
    public GameObject enemy;
    private RawImage barRawImage;
    private RectTransform barMaskRectTransform;
    private RectTransform edgeRectTransform;
    private float barMaskWidth;
    private EnemigoSigue scriptEnemigo;

    private void Awake()
    {
        //Visualizacion de la barra
        barMaskRectTransform = transform.Find("BarMask").GetComponent<RectTransform>();
        barRawImage = transform.Find("BarMask").Find("Bar").GetComponent<RawImage>();
        edgeRectTransform = transform.Find("Edge").GetComponent<RectTransform>();
        barMaskWidth = barMaskRectTransform.sizeDelta.x;
        //para tomar la vida del enemigo
        scriptEnemigo = enemy.GetComponent<EnemigoSigue>();
    }

    void Update()
    {
        float healthNormalized = scriptEnemigo.GetCurrentHealth();
        healthNormalized = healthNormalized / 100;
        //print("HealthNormalized " + healthNormalized);

        /*
        //da el efecto que se mueve la barra
        Rect uvRect = barRawImage.uvRect;
        uvRect.x += .2f * Time.deltaTime;
        barRawImage.uvRect = uvRect;
        //barImage.fillAmount = expNormalized / 100;
        */

        //Funcionamiento de la barra
        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.x = healthNormalized * barMaskWidth;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;
        edgeRectTransform.anchoredPosition = new Vector2(healthNormalized * barMaskWidth + 1, 0);
        edgeRectTransform.gameObject.SetActive(healthNormalized > 0);
    }
}
