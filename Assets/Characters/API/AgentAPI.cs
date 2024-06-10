using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class AgentAPI : MonoBehaviour
{
    private string baseURL = "http://127.0.0.1:8000";

    // Method to create a new agent
    public void CreateAgent(Character character)
    {
        if (character == null)
        {
            Debug.LogError("Character is null in CreateAgent.");
            return;
        }
        StartCoroutine(CreateAgentCoroutine(character));
    }

    // Method to update an existing agent
    public void UpdateAgent(int agentId, Character character)
    {
        if (character == null)
        {
            Debug.LogError("Character is null in UpdateAgent.");
            return;
        }
        StartCoroutine(UpdateAgentCoroutine(agentId, character));
    }

    private IEnumerator CreateAgentCoroutine(Character character)
    {
        string url = $"{baseURL}/agents/";
        character.Status = "active"; // Set the status field
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
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
            else if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Agent created successfully");
                Debug.Log($"Response: {webRequest.downloadHandler.text}");

                // Parse response to get the ID and assign it to the character
                var responseData = JsonUtility.FromJson<Dictionary<string, string>>(webRequest.downloadHandler.text);
                if (responseData.TryGetValue("id", out string id))
                {
                    character.Id = int.Parse(id);
                }
            }
            else
            {
                Debug.LogError($"Unexpected response status: {webRequest.responseCode}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
        }
    }

    private IEnumerator UpdateAgentCoroutine(int agentId, Character character)
    {
        string url = $"{baseURL}/agents/{agentId}";
        string jsonData = character.ToJson();

        Debug.Log($"Updating agent with JSON: {jsonData}");

        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, jsonData))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
            else if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Agent updated successfully");
                Debug.Log($"Response: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Unexpected response status: {webRequest.responseCode}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
        }
    }
}
