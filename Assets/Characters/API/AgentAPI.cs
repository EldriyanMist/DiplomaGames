using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AgentAPI : MonoBehaviour
{
    private string baseURL = "http://127.0.0.1:8000";

    // Method to create a new agent
    public void CreateAgent(Character character)
    {
        StartCoroutine(CreateAgentCoroutine(character));
    }

    // Method to update an existing agent
    public void UpdateAgent(int agentId, Character character)
    {
        StartCoroutine(UpdateAgentCoroutine(agentId, character));
    }

    private IEnumerator CreateAgentCoroutine(Character character)
    {
        string url = $"{baseURL}/agents/";
        character.Id = character.GetInstanceID(); // Assign a unique ID
        character.Position = character.transform.position;
        string jsonData = character.ToJson();

        Debug.Log($"Creating agent with JSON: {jsonData}");

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                Debug.Log("Agent created successfully");
            }
        }
    }

    private IEnumerator UpdateAgentCoroutine(int agentId, Character character)
    {
        string url = $"{baseURL}/agents/{agentId}";
        character.Position = character.transform.position;
        string jsonData = character.ToJson();

        Debug.Log($"Updating agent with JSON: {jsonData}");

        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, jsonData))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                Debug.Log("Agent updated successfully");
            }
        }
    }
}
