using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    public Button btnlevel1;
    public Button btnlevel2;
    public Button btnlevel3;

    public Transform panelStart;
    public Transform panelCard;
    public Transform panelEnd;

    // Start is called before the first frame update
    void Start()
    {
        
        btnlevel1.onClick.AddListener(()=> {
            Debug.Log("receive click");
            panelStart.gameObject.SetActive(false);
            panelCard.gameObject.SetActive(true);
            loadLevelCard(3, 2);
        });
        btnlevel2.onClick.AddListener(() => {
            Debug.Log("receive click");
            panelStart.gameObject.SetActive(false);
            panelCard.gameObject.SetActive(true);
            loadLevelCard(4, 2);
        });
        btnlevel3.onClick.AddListener(() => {
            Debug.Log("receive click");
            panelStart.gameObject.SetActive(false);
            panelCard.gameObject.SetActive(true);
            loadLevelCard(5, 2);
        });
        Button btnToStart = panelEnd.Find("Button_btn_start").GetComponent<Button>();
        btnToStart.onClick.RemoveAllListeners();
        btnToStart.onClick.AddListener(ToGameStartPage);
    }

    private void ToGameStartPage()
    {
        panelStart.gameObject.SetActive(true);
        panelCard.gameObject.SetActive(false);
        panelEnd.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void loadLevelCard(int width, int height)
    {
        Debug.Log("load card");
        Sprite[] sps = Resources.LoadAll<Sprite>("Cards");

        int totalCount = width * height/2;
        List<Sprite> spsList = new List<Sprite>();
        for (int i = 0; i < sps.Length; i++)
        {
            spsList.Add(sps[i]);
        }
        List<Sprite> needShowCardList = new List<Sprite>();
        while (totalCount > 0)
        {
            int randomIndex = Random.Range(0, spsList.Count);
            needShowCardList.Add(spsList[randomIndex]);
            needShowCardList.Add(spsList[randomIndex]);
            spsList.Remove(spsList[randomIndex]);
            totalCount--;
        }
        //for(int i = 0; i < needShowCardList.Count; i++)
        //{
        //    Debug.LogError(needShowCardList[i].name);
        //}
        Transform contentRoot = panelCard.Find("Panel");
        int maxCount = Mathf.Max(contentRoot.childCount, needShowCardList.Count);
        GameObject itemPrefab = contentRoot.GetChild(0).gameObject;
        for(int i = 0; i < maxCount; i++)
        {
            GameObject itemObject = null;
            if (i < contentRoot.childCount)
            {
                itemObject = contentRoot.GetChild(i).gameObject;
            }
            else
            {
                itemObject = GameObject.Instantiate<GameObject>(itemPrefab);
                itemObject.transform.SetParent(contentRoot, false);
            }
            itemObject.transform.Find("Image_front").GetComponent<Image>().sprite = needShowCardList[i];
            CardFlipAnimationCtrl cardAnictrl = itemObject.GetComponent<CardFlipAnimationCtrl>();
            cardAnictrl.setDefaultState();
        }
        GridLayoutGroup glg = contentRoot.GetComponent<GridLayoutGroup>();

        float panelWidth = width * glg.cellSize.x + glg.padding.left + glg.padding.right+(width-1)*glg.spacing.x;
        float panelHeight = height * glg.cellSize.y + glg.padding.top + glg.padding.bottom + (height - 1) * glg.spacing.y;
        contentRoot.GetComponent<RectTransform>().sizeDelta =
            new Vector2(panelWidth, panelHeight);


    }
    public void CheckIsGameOver()
    {
        CardFlipAnimationCtrl[] allCards = GameObject.FindObjectsOfType<CardFlipAnimationCtrl>();
        if (allCards != null && allCards.Length > 0)
        {
            List<CardFlipAnimationCtrl> cardInFront = new List<CardFlipAnimationCtrl>();
            for(int i = 0; i < allCards.Length; i++)
            {
                CardFlipAnimationCtrl cardTem = allCards[i];
                if(cardTem.isInfront&& !cardTem.isOver)
                {
                    cardInFront.Add(cardTem);
                }
                if (cardInFront.Count >= 2)
                {
                    string cardImagename1 = cardInFront[0].GetCardImageName();
                    string cardImagename2 = cardInFront[1].GetCardImageName();
                    if(cardImagename1== cardImagename2)
                    {
                        cardInFront[0].MatchSuccess();
                        cardInFront[1].MatchSuccess();
                    }
                    else
                    {
                        cardInFront[0].MatchFail();
                        cardInFront[1].MatchFail();
                    }
                    allCards = GameObject.FindObjectsOfType<CardFlipAnimationCtrl>();
                    bool isallover = true;
                    for(int o = 0; o < allCards.Length; o++)
                    {
                        isallover &= allCards[o].isOver;
                    }
                    if (isallover)
                    {
                        ToGameOverPage();
                    }
                    break;
                }
            }
        }
    }

    public void ToGameOverPage()
    {
        panelStart.gameObject.SetActive(false);
        panelCard.gameObject.SetActive(false);
        panelEnd.gameObject.SetActive(true);
    }
}
