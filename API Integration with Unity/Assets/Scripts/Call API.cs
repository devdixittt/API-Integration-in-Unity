using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    private const string API_URL = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    public delegate void OnDataFetched(List<Client> clients);
    public static event OnDataFetched DataFetched;

    public void FetchClientData()
    {
        StartCoroutine(GetClientData());
    }

    private IEnumerator GetClientData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(API_URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                // Deserialize JSON to List<Client>.
                List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(json);

                DataFetched?.Invoke(clients);
            }
            else
            {
                Debug.LogError($"Failed to fetch data: {request.error}");
            }
        }
    }
}

[System.Serializable]
public class Client
{
    public string label;
    public int points;
    public string role; // "manager" or "non-manager"
    public string address;
}
