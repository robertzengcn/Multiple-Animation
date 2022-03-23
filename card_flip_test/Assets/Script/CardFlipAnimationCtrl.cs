using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardFlipAnimationCtrl : MonoBehaviour,IPointerClickHandler
{
    Transform cardFront;
    Transform cardBack;
    float flipDuration=0.2f;
    internal bool isInfront = false;
    internal bool isOver=false;


    // Start is called before the first frame update
    void Awake()
    {
        cardFront = transform.Find("Image_front");
        cardBack = transform.Find("Image_back");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("test click");
        if (isInfront)
        {
            StartCoroutine(FlipCardToback());
        }
        else
        {
            StartCoroutine(FlipCardTofront());
        }
       

    }
    IEnumerator FlipCardTofront()
    {
        //1,翻转反面到90度
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(true);
        cardFront.rotation = Quaternion.identity;
        while (cardBack.rotation.eulerAngles.y < 90)
        {
            //Debug.Log(cardBack.rotation.eulerAngles.y);
            cardBack.rotation *= Quaternion.Euler(0, Time.deltaTime*90f*(1f/flipDuration), 0);
           if(cardBack.rotation.eulerAngles.y > 90)
            {
                cardBack.rotation= Quaternion.Euler(0, 90, 0);
                break;
            }
            yield return new WaitForFixedUpdate();

        }
        cardFront.gameObject.SetActive(true);
        cardBack.gameObject.SetActive(false);
        cardFront.rotation = Quaternion.Euler(0, 90, 0);
        while (cardFront.rotation.eulerAngles.y > 0)
        {
            //Debug.Log(cardBack.rotation.eulerAngles.y);
            cardFront.rotation *= Quaternion.Euler(0, -Time.deltaTime * 90f * (1f / flipDuration), 0);
            if (cardFront.rotation.eulerAngles.y >90)
            {
                cardFront.rotation = Quaternion.Euler(0, 0, 0);
                break;
            }
            yield return new WaitForFixedUpdate();

        }
        isInfront = true;
        Camera.main.gameObject.GetComponent<GameMain>().CheckIsGameOver();
    }

    IEnumerator FlipCardToback()
    {
        //1,翻转反面到90度
        cardFront.gameObject.SetActive(true);
        cardBack.gameObject.SetActive(false);
        cardFront.rotation = Quaternion.identity;
        while (cardFront.rotation.eulerAngles.y < 90)
        {
            //Debug.Log(cardBack.rotation.eulerAngles.y);
            cardFront.rotation *= Quaternion.Euler(0, Time.deltaTime * 90f * (1f / flipDuration), 0);
            if (cardFront.rotation.eulerAngles.y > 90)
            {
                cardFront.rotation = Quaternion.Euler(0, 90, 0);
                break;
            }
            yield return new WaitForFixedUpdate();

        }
        cardBack.gameObject.SetActive(true);
        cardFront.gameObject.SetActive(false);
        cardBack.rotation = Quaternion.Euler(0, 90, 0);
        while (cardBack.rotation.eulerAngles.y > 0)
        {
            //Debug.Log(cardBack.rotation.eulerAngles.y);
            cardBack.rotation *= Quaternion.Euler(0, -Time.deltaTime * 90f * (1f / flipDuration), 0);
            if (cardBack.rotation.eulerAngles.y > 90)
            {
                cardBack.rotation = Quaternion.Euler(0, 0, 0);
                break;
            }
            yield return new WaitForFixedUpdate();

        }
        isInfront = false;
    }

    internal void setDefaultState()
    {
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(true);
        this.isOver = false;
        this.isInfront = false;
        cardFront.rotation = Quaternion.identity;
        cardBack.rotation = Quaternion.identity;
    }

    internal string GetCardImageName()
    {
        return cardFront.GetComponent<Image>().sprite.name;
    }

    internal void MatchSuccess()
    {
        isOver = true;
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(false);
    }

    internal void MatchFail()
    {
        StartCoroutine(FlipCardToback());
    }
}
