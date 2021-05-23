using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> items;
    [SerializeField]
    private List<Sprite> itemImage;
    public Dictionary<GameObject, Sprite> itemDic;
    public Dictionary<GameObject, bool> isItemBuy;

    [SerializeField]
    private RectTransform shopUi;
    [SerializeField]
    private GameObject shopItem;
    [SerializeField]
    private Transform contents;
    // Start is called before the first frame update
    void Start()
    {
        itemDic = new Dictionary<GameObject, Sprite>();
        isItemBuy = new Dictionary<GameObject, bool>();
        int i = 0;
        foreach(GameObject item in items)
        {
            Debug.Log(item.name);
            Debug.Log(itemImage[i]);
            itemDic.Add(item, itemImage[i]);
            isItemBuy.Add(item, false);
            i++;
        }
        foreach(GameObject item in itemDic.Keys)
        {
            ShopItemUi shopItemUi = shopItem.GetComponent<ShopItemUi>();
            shopItemUi.item = item;
            shopItemUi.image.GetComponent<Image>().sprite = itemDic[item];
            Instantiate(shopItem,Vector3.one, Quaternion.identity).transform.parent = contents;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.tag == "Player")) return;

        shopUi.DOScale(Vector3.zero, 0);
        shopUi.gameObject.SetActive(true);
        shopUi.DOScale(Vector3.one,0.2f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!(other.gameObject.tag == "Player")) return;

        shopUi.DOScale(Vector3.zero, 0.2f);
        DOVirtual.DelayedCall(0.2f, () => shopUi.gameObject.SetActive(false));
    }
}
