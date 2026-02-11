using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemController : MonoBehaviour
{
    // 左右移動速度 (Inspectorから設定)
    public float moveSpeed = 8f;
    //左右移動制限　(Inspectorから設定)
    public float limitHorizontal = 5f;

    //Input Systemからの入力値を格納
    private Vector2 currentMovementInput;
    //ItemControls クラスのインスタンス
    private ItemControls controls;
    public bool onGround;
    public int itemKind;
    public bool is_currentItem;
    public bool is_Merging;
    public GameObject[] items;
    int[] point;
    public Rigidbody2D rb;
    public bool canMove;
    public bool isDrop;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        controls = new ItemControls();
        controls.Item.Move.performed += OnMove;
        controls.Item.Move.canceled += OnMove;
        controls.Item.Drop.performed += OnDrop;

        controls.Enable();

        canMove = true;
        isDrop = false;
        is_Merging = false;

        point = new int[12] { 1, 3, 6, 10, 13, 15, 21, 28, 36, 45, 55, 66 };

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        //合体した新しいオブジェクト
        if (!is_currentItem || isDrop)
        {
            canMove = false;
            rb.constraints = RigidbodyConstraints2D.None;
        }
        //上から生成されたもの
        else
        {
            if (canMove)
            {
                Move();
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Items" || collision2D.gameObject.tag == "Ground")
        {
            rb.constraints = RigidbodyConstraints2D.None;
            onGround = true;
            is_currentItem = false;
        }

        if (collision2D.gameObject.tag == "Items")
        {
            //衝突相手のItemControllerスクリプトを取得
            ItemController collisionScript = collision2D.gameObject.GetComponent<ItemController>();
            if (collisionScript == null) return;
            //衝突相手のアイテムの種類の変数を取得
            int collisionKind = collisionScript.itemKind;

            if (collisionKind == this.itemKind)//衝突したアイテムと同じ種類だったら
            {
                if (!collisionScript.is_Merging)//相手のオブジェクトが合体中ではなかったら
                {
                    this.is_Merging = true;
                    collisionScript.is_Merging = true;

                    //自身のオブジェクトを削除
                    Destroy(this.gameObject);
                    //相手のオブジェクトを削除
                    Destroy(collision2D.gameObject);

                    //IDの小さい場合のみ次のレベルのアイテムを生成
                    if (this.gameObject.GetInstanceID() < collision2D.gameObject.GetInstanceID())
                    {
                        //ポイントの追加を実行
                        if (GameManager.Instance != null)
                        {
                            // スコアの加算（推奨されるメソッド経由）
                            GameManager.Instance.AddPoint(point[itemKind]);

                            // または直接変数を参照
                            // GameManager.Instance.point += 100;
                        }

                        if (this.itemKind != (items.Length - 1))//最後のアイテムではなかったら
                        {
                            //次のレベルのオブジェクトを設定
                            int nextKind = itemKind + 1;//次の種類
                            GameObject nextItem = items[nextKind];
                            Vector3 instantiatePosition = this.transform.position;

                            //次のレベルのアイテムを生成
                            GameObject nextItemObject = Instantiate(nextItem, instantiatePosition, Quaternion.identity);
                            ItemController nextItemController = nextItemObject.GetComponent<ItemController>();
                            nextItemController.is_currentItem = false;
                        }
                    }


                }
            }
        }

    }
    //Input Actionから呼び出される
    //A/Dorボタンで左右移動
    private void OnMove(InputAction.CallbackContext context)
    {
        // 2D Vector (X成分が左右入力: -1 to 1) の値を取得
        currentMovementInput = context.ReadValue<Vector2>();
    }
    //Spaceキーorボタンで落とす
    private void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canMove)
            {
                isDrop = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                canMove = false;
                rb.WakeUp();
            }
        }
    }

    void OnDisable()
    {
        // 終了時に入力システムを無効化（推奨）
        controls.Disable();
    }

    void Move()
    {
        float minX = -limitHorizontal;
        float maxX = limitHorizontal;
        float currentX = transform.position.x;

        float inputX = currentMovementInput.x;

        //位置が制限を超えた場合
        if ((currentX <= minX && inputX < 0) || (currentX >= maxX && inputX > 0))
        {
            inputX = 0f;
        }

        //移動
        float horizontalMove = inputX * moveSpeed;
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);
    }

}
