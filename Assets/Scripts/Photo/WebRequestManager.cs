using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MiniJSON;

public class WebRequestManager : MonoBehaviour
{
    public static WebRequestManager Instance {get ; private set;}

    public struct StatusInfo
    {
        public string type;
        public int hp;
        public int attack;
        public int defense;
        public int specialAttack;
        public int specialDefense;
        public int speed;
    }
    public StatusInfo statusInfo {get; private set;}

    public bool isRequesting {get; set;} = false;
    public bool isError {get; private set;} = false;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void GetStatus(string path)
    {
        isRequesting = true;
        StartCoroutine(StatusRequest(path));
    }

    private IEnumerator StatusRequest(string path)
    {
        string url = "https://fastapi-m66l.onrender.com/status/";
        string imagePath = path;

        WWWForm form = new WWWForm();
        form.AddField("accept", "application/json");
        form.AddField("Content-Type", "multipart/form-data");

        byte[] fileData = System.IO.File.ReadAllBytes(imagePath);
        string fileName = Path.GetFileName(imagePath);
        form.AddBinaryData("files", fileData, fileName, "image/png");

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            Debug.Log("Request start");
            request.timeout = 20;
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogWarning("タイムアウト");
                isError = true;
                isRequesting = false;
                yield break;
            }
            Debug.Log("Request end");

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning(request.error);
                isError = true;
                isRequesting = false;
            }
            else
            {
                Debug.Log("Request successful");
                if(request.downloadHandler.text == null)
                {
                    isError = true;
                    isRequesting = false;
                    yield break;
                }
                
                // JSONデータをデシリアライズ
                Dictionary<string, object> statusDictionary = Json.Deserialize(request.downloadHandler.text) as Dictionary<string, object>;
                var statuses = statusDictionary["status"] as List<object>;

                Debug.Log(request.downloadHandler.text);

                // ステータスを取得
                var localStatusInfo = statusInfo;
                localStatusInfo.type = statuses[0] as string;
                localStatusInfo.hp = int.Parse(statuses[1].ToString());
                localStatusInfo.attack = int.Parse(statuses[2].ToString());
                localStatusInfo.defense = int.Parse(statuses[3].ToString());
                localStatusInfo.specialAttack = int.Parse(statuses[4].ToString());
                localStatusInfo.specialDefense = int.Parse(statuses[5].ToString());
                localStatusInfo.speed = int.Parse(statuses[6].ToString());
                statusInfo = localStatusInfo; 

                isError = false;
                isRequesting = false; 
            }
        }
    }
}
