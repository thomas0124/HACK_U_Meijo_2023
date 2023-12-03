using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class OnlineMenuManager : MonoBehaviourPunCallbacks
{
    private bool isRoom;
    private bool isMatching;

    //インスタンスを生成する親オブジェクト
    public GameObject parentUI;

    public void OnMatchingButton() {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinRandomRoom();
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        isRoom = true;
        /*
        // 自身のアバター（ネットワークオブジェクト）を生成する
        var relativePosition = new Vector3(-200, -350); // 親オブジェクトからの相対的な位置
        var newCharacter = PhotonNetwork.Instantiate("AllyCharacter", parentUI.transform.position + relativePosition, Quaternion.identity);
        newCharacter.transform.parent = parentUI.transform;
        */
    }

    public override void OnJoinRandomFailed(short returnCode, string massage)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2}, TypedLobby.Default); 
    }

    void Update() 
    {
        if(isMatching)
        {
            return;
        }
        if(isRoom)
        {
            if(PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                isMatching = true;
                SceneManager.LoadScene("Scene7");
            }
        }
    }
}