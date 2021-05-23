using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class NetworkManager : Photon.PunBehaviour
{
	[SerializeField]
	private TextMeshProUGUI informationText;
	[SerializeField]
	private GameObject loginUI;
	[SerializeField]
	private Button loginButton;
	[SerializeField]
	private Button logoutButton;
	[SerializeField]
	private TMP_Dropdown roomLists;
	[SerializeField]
	private TMP_InputField roomName;
	[SerializeField]
	private TMP_InputField playerName;
	public GameObject changingUi;
	public GameObject androidController;
	private GameObject player;

	[SerializeField]
	private GameObject uiCamera;
	void Start()
	{
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.autoJoinLobby = true;
		PhotonNetwork.ConnectUsingSettings("0.1");
		loginButton.onClick.AsObservable()
			.Subscribe(_ =>
			{
				LoginGame();
			}).AddTo(loginButton.gameObject);
		logoutButton.onClick.AsObservable()
			.Subscribe(_ =>
			{
				LogoutGame();
				changingUi.SetActive(false);
				androidController.SetActive(false);
			});
	}

	void Update()
	{
		informationText.text = PhotonNetwork.connectionStateDetailed.ToString();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("ロビーに入る");
		loginUI.SetActive(true);
	}

	public void LoginGame()
	{
		//　ルームオプションを設定
		RoomOptions ro = new RoomOptions()
		{
			//　ルームを見えるようにする
			IsVisible = true,
			//　部屋の入室最大人数
			MaxPlayers = 10
		};

		if (roomName.text != "")
		{
			//　部屋がない場合は作って入室
			PhotonNetwork.JoinOrCreateRoom(roomName.text, ro, TypedLobby.Default);
		}
		else
		{
			//　部屋が存在すれば
			if (roomLists.options.Count != 0)
			{
				Debug.Log(roomLists.options[roomLists.value].text);
				PhotonNetwork.JoinRoom(roomLists.options[roomLists.value].text);
				//　部屋が存在しなければDefaultRoomという名前で部屋を作成
			}
			else
			{
				PhotonNetwork.JoinOrCreateRoom("DefaultRoom", ro, TypedLobby.Default);
			}
		}
	}

	public override void OnReceivedRoomListUpdate()
	{
		Debug.Log("部屋更新");
		//　部屋情報を取得する
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		//　ドロップダウンリストに追加する文字列用のリストを作成
		List<string> list = new List<string>();

		//　部屋情報を部屋リストに表示
		foreach (RoomInfo room in rooms)
		{
			//　部屋が満員でなければ追加
			if (room.PlayerCount < room.MaxPlayers)
			{
				list.Add(room.Name);
			}
		}

		//　ドロップダウンリストをリセット
		roomLists.ClearOptions();

		//　部屋が１つでもあればドロップダウンリストに追加
		if (list.Count != 0)
		{
			roomLists.AddOptions(list);
		}
	}

	public override void OnJoinedRoom()
	{
		loginUI.SetActive(false);
		logoutButton.gameObject.SetActive(true);
		Debug.Log("入室");

		//　InputFieldに入力した名前を設定
		PhotonNetwork.player.NickName = playerName.text;
		player = PhotonNetwork.Instantiate("ThirdPersonController_LITE 1", Vector3.up, Quaternion.identity, 0);
		uiCamera.SetActive(false);
	}

	//　部屋の入室に失敗した
	void OnPhotonJoinRoomFailed()
	{
		Debug.Log("入室に失敗");

		//　ルームオプションを設定
		RoomOptions ro = new RoomOptions()
		{
			//　ルームを見えるようにする
			IsVisible = false,
			//　部屋の入室最大人数
			MaxPlayers = 10
		};
		//　入室に失敗したらDefaultRoomを作成し入室
		PhotonNetwork.JoinOrCreateRoom("DefaultRoom", ro, TypedLobby.Default);
	}

	public void LogoutGame()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		Debug.Log("退室");
		logoutButton.gameObject.SetActive(false);
	}
}