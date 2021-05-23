using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BuyItemUi : MonoBehaviour
{
    [SerializeField]
    private Button button;
    private GameObject playerHead;
    private GameObject player;
    private NetworkPlayerCheck npc;
    public GameObject item;
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        npc = player.GetComponent<NetworkPlayerCheck>();
        playerHead = npc.head;
        button.onClick.AsObservable()
            .Subscribe(_ => BuyItemSet())
            .AddTo(gameObject);
    }

    private void BuyItemSet()
    {
        foreach (Transform itemTra in playerHead.transform)
        {
            if (itemTra.gameObject.name == "VBOT_:HeadTop_End_end") continue;
            Debug.Log(item.name);
            itemTra.gameObject.SetActive(item.name == itemTra.name);
        }
    }
}
