using UnityEngine;
using System.Collections;

public class Particler : MonoBehaviour 
{
    public Texture2D texture;
    public float size = 0.1f;
    public float padding = 0.05f;
    public float depthness = 50f;
    ParticleSystem system;
    ParticleSystem.Particle[] particleArray;
    int textureWidth;
    int textureHeight;
    int particleCount;

    Depther depther;
    bool shouldUpdate = false;

    void Start () 
    {
        textureWidth = 320;
        textureHeight = 240;
        particleCount = textureWidth * textureHeight;
        particleArray = new ParticleSystem.Particle[particleCount];

        system = GetComponent<ParticleSystem>();
        system.maxParticles = particleCount;
        system.Emit(particleCount);

        depther = FindObjectOfType<Depther>();
        if (texture == null) {
            texture = depther.texture;
        }
        shouldUpdate = depther != null && texture != null;
    }

    void LateUpdate () 
    {
        if (shouldUpdate)
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

                float lum = Mathf.SmoothStep(depther.depthMin, depther.depthMax, 1f - height);

                particleArray[i].position = new Vector3(x, y, z);
                particleArray[i].color = new Color(lum, lum, lum, 1f);
                particleArray[i].size = isDepth ? size : 0f;
                particleArray[i].startLifetime = isDepth ? 5f : 0f;
                particleArray[i].lifetime = isDepth ? 5f : 0f;
                // particleArray[i].velocity = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 10f + Random.Range(1f, 2f));
                ++i;
            }
            system.SetParticles(particleArray, particleCount);
        }
	}
}
