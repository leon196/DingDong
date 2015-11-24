﻿using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour 
{
    public Texture2D texture;
    public float padding = 0.05f;
    public float depthness = 50f;
    ParticleSystem system;
    ParticleSystem.Particle[] particleArray;
    int textureWidth;
    int textureHeight;
    int particleCount;

    void Start () 
    {
        textureWidth = 320;
        textureHeight = 240;
        particleCount = textureWidth * textureHeight;
        particleArray = new ParticleSystem.Particle[particleCount];

        system = GetComponent<ParticleSystem>();
        system.maxParticles = particleCount;
        system.Emit(particleCount);
    }

    void LateUpdate () 
    {
        system.GetParticles(particleArray);
        Color[] colors = texture.GetPixels();
        int i = 0;
        while (i < particleCount) 
        {
        	int gridX = i % textureWidth;
        	int gridY = textureHeight - (int)Mathf.Floor(i / textureWidth);
        	float height = colors[i].r;
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
            ++i;
        }
        system.SetParticles(particleArray, particleCount);
	}
}
