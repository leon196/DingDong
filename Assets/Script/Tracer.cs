using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {

	Texture2D texture;
	Color[] colors;
    int textureWidth;
    int textureHeight;
	int particleCount;
    Texture2D depthTexture;
    float treshold = 0.84f;
    bool pressedKey = false;

	public void Setup () 
	{
        textureWidth = 320;
        textureHeight = 240;

		texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32,false);
		GetComponent<Renderer>().material.mainTexture = texture;

		particleCount = textureWidth * textureHeight;
		colors = new Color[particleCount];
        int i = 0;
        while (i < particleCount) 
        {
        	colors[i] = new Color(0f, 0f, 0f, 1f);
            i++;
        }
		texture.SetPixels(colors);
		texture.Apply(false);
    
        depthTexture = FindObjectOfType<DisplayDepth>().tex;
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
        Color[] heights = depthTexture.GetPixels();
        colors = texture.GetPixels();
        int i = 0;
        while (i < particleCount) 
        {
        	int gridX = i % textureWidth;
        	int gridY = textureHeight - (int)Mathf.Floor(i / textureWidth);
        	float height = 1f - heights[i].r;
        	// bool isDepth = height != 0f;
        	if (height > treshold) {
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
