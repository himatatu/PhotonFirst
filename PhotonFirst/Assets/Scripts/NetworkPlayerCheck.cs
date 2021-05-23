using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using UnityChan;
using UniRx.Triggers;
using UniRx;
using Invector.vCharacterController;
using System.Collections.Generic;

public class NetworkPlayerCheck : Photon.PunBehaviour
{

    private Vector3 position;
    private Quaternion rotation;
    [SerializeField]
    private float smooth = 10f;
    [SerializeField]
    private GameObject myCamera;

    private Animator animator;

    public GameObject head;
    public List<GameObject> hats;
    private float speed;
    private bool jump;
    private bool rest;

    [SerializeField]
    private GameObject androidController;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (photonView.isMine)
        {
            tag = "Player";
            NetworkManager nw = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

            nw.changingUi.SetActive(true);
            nw.androidController.SetActive(true);

            myCamera.SetActive(true);

            Vector3 vec3 = transform.position;

            this.UpdateAsObservable()
                .Where(_ => transform.position.y < -1)
                .Subscribe(_ => transform.position = vec3)
                .AddTo(gameObject);

            return;
        }

        GetComponent<UnityChanControlScriptWithRgidBody>().enabled = false;

        position = transform.position;
        rotation = transform.rotation;
        speed = animator.GetFloat("Speed");
        jump = animator.GetBool("Jump");
        rest = animator.GetBool("Rest");

        StartCoroutine("UpdateMove");
    }
    //　自分のキャラクター以外のデータの同期
    IEnumerator UpdateMove()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smooth);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);

            animator.SetFloat("Speed", speed);
            animator.SetBool("Jump", jump);
            animator.SetBool("Rest", rest);
            yield return null;
        }
    }

    //　Observed Componentsに設定したスクリプトで呼ばれるメソッド
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //　データの読み込み
        if (stream.isReading)
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            foreach(GameObject hat in hats)
            {
                hat.SetActive((bool)stream.ReceiveNext());
            }
            speed = (float)stream.ReceiveNext();
            jump = (bool)stream.ReceiveNext();
            rest = (bool)stream.ReceiveNext();
        }
        //　データの書き込み
        else
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            foreach(GameObject hat in hats)
            {
                stream.SendNext(hat.activeSelf);
            }
           stream.SendNext(animator.GetFloat("Speed"));
            stream.SendNext(animator.GetBool("Jump"));
            stream.SendNext(animator.GetBool("Rest"));
        }
    }
}