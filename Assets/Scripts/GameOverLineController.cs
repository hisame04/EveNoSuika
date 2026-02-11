using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLineController : MonoBehaviour
{
    public bool gameOver;
    bool is_currentItem;
    public int gameOverCountLimit = 2;
    float gameOverTimer;
    // bool touchItem;
    private List<Collider2D> touchingItems = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        // touchItem = false; 
        gameOverTimer = gameOverCountLimit;
    }

    // Update is called once per frame
    void Update()
    { 
        // リストにアイテムが存在する場合のみタイマーを減らす
        if (touchingItems.Count > 0)
        {
            gameOverTimer -= Time.deltaTime;
        }
        else
        {
            // 💡 アイテムが一つもなくなったらタイマーをリセット
            gameOverTimer = gameOverCountLimit; 
        }

        if(gameOverTimer<0)
        {
            gameOver = true;
        }

    }

    void OnTriggerStay2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag == "Items")
        {
            ItemController colliderItemController = collider2D.gameObject.GetComponent<ItemController>();
            if (colliderItemController == null) return;
            is_currentItem = colliderItemController.is_currentItem;
            if (!is_currentItem)
            {
                if (!touchingItems.Contains(collider2D))
                {
                    touchingItems.Add(collider2D);
            
                    // 💡 最初の一つが触れた時にタイマーをスタートさせる
                    if (touchingItems.Count == 1)
                    {
                        gameOverTimer = gameOverCountLimit;
                    }
                }
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag == "Items")
        {
            ItemController colliderItemController = collider2D.gameObject.GetComponent<ItemController>();
            if (colliderItemController == null) return;
            is_currentItem = colliderItemController.is_currentItem;
            if (!is_currentItem)
            {
                if (touchingItems.Contains(collider2D))
                {
                    touchingItems.Remove(collider2D);
                }
            }
        }
    }
}
