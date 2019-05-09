using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{

    //private Image barImage;
    private GameObject player;
    private RawImage barRawImage;
    private RectTransform barMaskRectTransform;
    private RectTransform edgeRectTransform;
    private float barMaskWidth;


    

    private void Awake()
    {
        player = GameObject.Find("Player");
        barMaskRectTransform = transform.Find("BarMask").GetComponent<RectTransform>();
        barRawImage = transform.Find("BarMask").Find("Bar").GetComponent<RawImage>();
        edgeRectTransform = transform.Find("Edge").GetComponent<RectTransform>();

        barMaskWidth = barMaskRectTransform.sizeDelta.x;
        //barImage.fillAmount = 0;

    }

    void Update()
    {
        float expNormalized = (player.GetComponent<Player>().GetExperience());
        expNormalized = expNormalized / 100;
        


        Rect uvRect = barRawImage.uvRect;   //da el efecto que se mueve la barra
        uvRect.x += .2f * Time.deltaTime;
        barRawImage.uvRect = uvRect;
        //barImage.fillAmount = expNormalized / 100;

        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.x = expNormalized * barMaskWidth;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;

        edgeRectTransform.anchoredPosition = new Vector2(expNormalized * barMaskWidth + 1, 0);

        edgeRectTransform.gameObject.SetActive(expNormalized > 0);
    }






}
