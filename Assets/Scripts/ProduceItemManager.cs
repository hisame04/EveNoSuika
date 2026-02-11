using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceItemManager : MonoBehaviour
{
    int produceItemLimit = 3;
    Vector3 producePos = new Vector3(0,4.5f,0);
    public GameObject[] items;
    GameObject currentItem;
    public GameObject nextItem; 
    ItemController currentItemController;
    bool itemOnGround ;
    // Start is called before the first frame update
    void Start()
    {
        SetNextItem();
        ProduceItem();
        SetNextItem();
        currentItemController = currentItem.GetComponent<ItemController>();
        itemOnGround = currentItemController.onGround;
    }

    // Update is called once per frame
    public void ProduceItemUpdate()
    {
        if (currentItem != null)
        {
            if (currentItemController == null) // Start/ProduceItem で取得済みのはず
            {
                // currentItem は破壊済みとみなし、次の生成を促す
                currentItem = null; 
                itemOnGround = true; 
            } 
            else 
            {
                currentItemController = currentItem.GetComponent<ItemController>();
                itemOnGround = currentItemController.onGround;
            }
        }

        if(itemOnGround || currentItem==null)
        {
            ProduceItem();
            SetNextItem();
        }
    }

    void SetNextItem()
    {
        int nextKind = Random.Range(0,produceItemLimit);
        nextItem = items[nextKind];
    }

    void ProduceItem()
    {
        currentItem = Instantiate(nextItem,producePos,Quaternion.identity);
        currentItemController = currentItem.GetComponent<ItemController>();
        
        currentItemController.is_currentItem = true;

        itemOnGround = currentItemController.onGround;
        nextItem = null;
    }
}
