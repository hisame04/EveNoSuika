using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public ProduceItemManager produceItemManager;
    public GameObject stateTextObject;
    public Text stateText;
    public Text pointText;
    public Image nextItemImage;
    Sprite nextItemSprite;
    // Start is called before the first frame update
    void Start()
    {
        stateTextObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        stateText.text = "GAME OVER...";
        stateTextObject.SetActive(true);
    }

    // public void GameClear()
    // {
    //     stateText.text = "GAME CLEAR!";
    //     stateTextObject.SetActive(true);
    // }

    public void SetNextItem()
    {
        nextItemSprite = produceItemManager.nextItem.GetComponent<SpriteRenderer>().sprite;
        nextItemImage.sprite = nextItemSprite;
    }

    public void UpdatePoint(int point)
    {
        pointText.text = "Score:" +point;
    }
}
