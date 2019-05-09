using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    //private Image barImage;
    public GameObject enemy;
    private RawImage barRawImage;
    private RectTransform barMaskRectTransform;
    private RectTransform edgeRectTransform;
    private float barMaskWidth;
    //private Transform transform;


    

    private void Awake()
    {
        //enemy = GameObject.Find("EnemigoPatrulla");
        barMaskRectTransform = transform.Find("BarMask").GetComponent<RectTransform>();
        barRawImage = transform.Find("BarMask").Find("Bar").GetComponent<RawImage>();
        edgeRectTransform = transform.Find("Edge").GetComponent<RectTransform>();

        barMaskWidth = barMaskRectTransform.sizeDelta.x;
        //barImage.fillAmount = 100;
        //transform = transform.GetComponent<Transform>();
    }

    void Update()
    {
        

        float healthNormalized = (enemy.GetComponent<EnemigoPatrulla>().GetCurrentHealth());
        print("HealthNormalized " + healthNormalized);
        healthNormalized = healthNormalized / 100;
        

        /*
        Rect uvRect = barRawImage.uvRect;   //da el efecto que se mueve la barra
        uvRect.x += .2f * Time.deltaTime;
        barRawImage.uvRect = uvRect;
        //barImage.fillAmount = expNormalized / 100;
        */


        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.x = healthNormalized * barMaskWidth;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;

        edgeRectTransform.anchoredPosition = new Vector2(healthNormalized * barMaskWidth + 1, 0);

        edgeRectTransform.gameObject.SetActive(healthNormalized > 0);
    }
}
