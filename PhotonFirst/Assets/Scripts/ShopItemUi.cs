using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShopItemUi : MonoBehaviour
{
    public GameObject item;
    [SerializeField]
    private Transform rawImage;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject soldout;
    public GameObject image;
    private ShopManager shopManager;
    // Start is called before the first frame update
    void Start()
    {
        shopManager = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
        button.onClick.AsObservable()
            .Subscribe(_ => {
                soldout.SetActive(true);
                shopManager.isItemBuy[item] = true;
            })
            .AddTo(gameObject);
    }
}
