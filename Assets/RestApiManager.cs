using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestApiManager : MonoBehaviour
{
    [SerializeField]
    private RawImage YourRawImage;

    [SerializeField]
    private int UserId = 1;

    [SerializeField]
    private string ServerApiURL = "https://my-json-server.typicode.com/manolovillarreal/JSONServer";

    [SerializeField]
    private string RickYMortyApiURL = "https://rickandmortyapi.com/api";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetCharactersClick()
    {
        StartCoroutine(GetCharacters());
    }
    IEnumerator GetPlayerInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerApiURL+"/users/"+UserId);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
        }
        else
        {
            //Debug.Log(www.GetResponseHeader("content-type"));

            // Show results as text
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                UserJsonData user = JsonUtility.FromJson<UserJsonData>(www.downloadHandler.text);
                Debug.Log(user.name);

                foreach (int cardId in user.deck)
                {
                    Debug.Log(cardId);
                }
                
            }
            else
            {
                string mensaje = "Status :" + www.responseCode;
                mensaje += "\ncontent-type:" + www.GetResponseHeader("content-type");
                mensaje += "\nError :" + www.error;
                Debug.Log(mensaje);
            }

        }
    }
    IEnumerator GetCharacters()
    {
        UnityWebRequest www = UnityWebRequest.Get(RickYMortyApiURL + "/character/");

        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
        }
        else
        {
            //Debug.Log(www.GetResponseHeader("content-type"));
            
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                CharactersList characters = JsonUtility.FromJson<CharactersList>(www.downloadHandler.text);
                Debug.Log(characters.info.count);

                foreach (Character character in characters.results)
                {
                    Debug.Log("Name:" + character.name);
                    Debug.Log("Image:" + character.image);
                    StartCoroutine(DownloadImage(character.image));
                }
            }
            else
            {
                string mensaje = "Status :" + www.responseCode;
                mensaje += "\ncontent-type:" + www.GetResponseHeader("content-type");
                mensaje += "\nError :" + www.error;
                Debug.Log(mensaje);
            }
            
        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            YourRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

}


public class UserJsonData
{
    public int id;
    public string name;
    public int[] deck;
}

[System.Serializable]
public class CharactersList
{
    public CharactersListInfo info;
    public List<Character> results;
}
[System.Serializable]
public class CharactersListInfo
{
    public int count;
    public int pages;
    public string next;
    public string prev;
}
[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}
