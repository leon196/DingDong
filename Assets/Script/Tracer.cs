using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {

    public float treshold = 0.0f;
	Texture2D texture;
	Color[] colors;
    int textureWidth;
    int textureHeight;
	int pixelCount;
    Depther depther;
    bool shouldUpdate = false;

	void Start () 
	{
        textureWidth = 320;
        textureHeight = 240;

		texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32,false);
		GetComponent<Renderer>().material.mainTexture = texture;

		pixelCount = textureWidth * textureHeight;
		colors = new Color[pixelCount];
        int i = 0;
        while (i < pixelCount) 
        {
        	colors[i] = new Color(0f, 0f, 0f, 1f);
            i++;
        }
		texture.SetPixels(colors);
		texture.Apply(false);
    
        depther = FindObjectOfType<Depther>();

        shouldUpdate = depther != null && depther.texture != null;
	}

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            treshold = Mathf.Clamp(treshold - 0.001f, 0f, 1f);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            treshold = Mathf.Clamp(treshold + 0.001f, 0f, 1f);
        }
    }
	
	void LateUpdate () 
	{
		if (shouldUpdate)
		{
	        Color[] heights = depther.texture.GetPixels();
	        colors = texture.GetPixels();
	        int i = 0;
	        while (i < pixelCount) 
	        {
	        	float height = 1f - heights[i].r;
	        	bool isDepth = height != 1f;
	        	if (isDepth && height > treshold) {
	        		colors[i].r = 1f;
	        	} else {
	        		colors[i].r = colors[i].r * 0.95f;
	        	}
	            i++;
	        }

			texture.SetPixels(colors);
			texture.Apply(false);
		}
	}
}
