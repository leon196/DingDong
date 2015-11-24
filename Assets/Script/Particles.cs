using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour 
{
    ParticleSystem particleSystem;
    DisplayDepth depther;
    DisplayColor colorer;
    Texture2D depthTexture;
    Texture2D colorTexture;
    ParticleSystem.Particle[] particleArray;
    int textureWidth;
    int textureHeight;
    int particleCount;
    float padding = 0.05f;
    float depthness = 50f;

    public void Setup () 
    {
        depther = FindObjectOfType<DisplayDepth>();
        depthTexture = depther.tex;
        colorer = FindObjectOfType<DisplayColor>();
        colorTexture = colorer.tex;

        textureWidth = 320;
        textureHeight = 240;
        particleCount = textureWidth * textureHeight;
        particleArray = new ParticleSystem.Particle[particleCount];

        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.maxParticles = particleCount;
        particleSystem.Emit(particleCount);
    }

    void LateUpdate () 
    {
        int particleCountAlive = particleSystem.GetParticles(particleArray);
        Color[] heights = depthTexture.GetPixels();
        Color[] colors = colorTexture.GetPixels();
        int i = 0;
        while (i < particleCount) 
        {
        	int gridX = i % textureWidth;
        	int gridY = textureHeight - (int)Mathf.Floor(i / textureWidth);
        	float height = heights[i].r;
        	bool isDepth = height != 0f;

        	float x = gridX * padding - textureWidth * padding / 2;
        	float y = gridY * padding - textureHeight * padding / 2;
        	float z = height * depthness;

            // Color color = colors[i];
            float lum = Mathf.SmoothStep(0.5f, 0.75f, 1f - height);// * (color.r + color.b + color.g) / 3.0f; 

            particleArray[i].position = new Vector3(x, y, z);
            // particleArray[i].color = colors[i];
            particleArray[i].color = new Color(lum, lum, lum, 1f);
            particleArray[i].size = isDepth ? 0.1f : 0f;
            particleArray[i].startLifetime = isDepth ? 5f : 0f;
            particleArray[i].lifetime = isDepth ? 5f : 0f;
            // particleArray[i].velocity = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 10f + Random.Range(1f, 2f));
            // particleArray[i].
            i++;
        }
        particleSystem.SetParticles(particleArray, particleCount);
	}
}
