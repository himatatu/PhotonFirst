using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

public class Changing : MonoBehaviour
{
    [SerializeField]
    private Button changingButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button removeButton;
    [SerializeField]
    private RectTransform changingUi;
    [SerializeField]
    private GameObject buyItemUi;
    [SerializeField]
    private Transform contents;
    private GameObject playerHead;
    [SerializeField]
    private ShopManager shopManager;
    // Start is called before the first frame update
    void Start()
    {
        playerHead = GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerCheck>().head;
        changingButton.onClick.AsObservable()
            .Subscribe(_ =>
            {
                UiMake();
                changingUi.DOScale(Vector3.zero, 0);
                changingUi.gameObject.SetActive(true);
                changingUi.DOScale(Vector3.one, 0.2f);
            })
            .AddTo(gameObject);

        backButton.onClick.AsObservable().
            Subscribe(_ =>
            {
                changingUi.DOScale(Vector3.zero, 0.2f);
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    changingUi.gameObject.SetActive(false);
                    changingButton.gameObject.SetActive(true);
                });
            })
            .AddTo(gameObject);

        removeButton.onClick.AsObservable().
            Subscribe(_ =>
            {
                foreach (Transform itemTra in playerHead.transform)
                {
                    if (itemTra.gameObject.name == "VBOT_:HeadTop_End_end") continue;
                    itemTra.gameObject.SetActive(false);
                }
            }).AddTo(gameObject);
    }

    private void UiMake()
    {
        foreach(Transform tra in contents)
        {
            Destroy(tra.gameObject);
        }
        foreach (GameObject item in shopManager.itemDic.Keys)
        {
            if (!shopManager.isItemBuy[item]) continue;
            BuyItemUi buyItemUiScript = buyItemUi.GetComponent<BuyItemUi>();
            buyItemUiScript.item = item;
            buyItemUiScript.image.GetComponent<Image>().sprite = shopManager.itemDic[item];

            GameObject instans = Instantiate(buyItemUi, Vector3.zero, Quaternion.identity);
            instans.transform.parent = contents;
            instans.transform.localScale = new Vector3(1, 1, 1);
        }
        changingButton.gameObject.SetActive(false);
    }
}
