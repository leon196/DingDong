using UnityEngine;
using System.Collections;

public class WebcamIP : MonoBehaviour 
{
    public string uri = "http://192.168.1.57:8080/videofeed";
    public string username = "ponk";
    public string password = "shader";
    public int width = 1440;
    public int height = 1080;
    public Texture2D texture;

    void Start ()
    {
        texture = new Texture2D(4, 4, TextureFormat.DXT1, true);
        Material material = GetComponent<Renderer>().material;
        // if (material != null) {
        //     material.mainTexture = texture;
        // }
        StartCoroutine(Fetch());
    }

    public IEnumerator Fetch ()
    {
        while (true) {
            // Debug.Log("fetching... "+Time.realtimeSinceStartup);

            // WWWForm form = new WWWForm();
            // // required by WWWForm
            // form.AddField("dummy", "field");
            // WWW www = new WWW(uri, form.data, new System.Collections.Generic.Dictionary<string,string>() {
            // // using www.headers is depreciated by some odd reason
            //     {"Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(username+":"+password))}
            // });
            // yield return www;

            // if(!string.IsNullOrEmpty(www.error))
            //     throw new UnityException(www.error);

            // www.LoadImageIntoTexture(texture);

            // Start a download of the given URL
            var www = new WWW(uri);

            // wait until the download is done
            yield return www;

            // assign the downloaded image to the main texture of the object
            www.LoadImageIntoTexture(texture);
            GetComponent<Renderer>().material.mainTexture = texture;
        }
    }
}