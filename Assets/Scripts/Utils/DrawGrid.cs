using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawGrid : MonoBehaviour 
{
    Material material;

    [HideInInspector] public int width = 4;
    [HideInInspector] public int height = 4;
    public bool shouldDrawGrid = false;

    void Awake ()
    {
        material = new Material(Shader.Find("Hidden/Line"));

        // width *= Screen.width / (float)Screen.height;
    }

    void OnPostRender () 
    {
        if (shouldDrawGrid) 
        {
            GL.PushMatrix();
            material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.white);

            for (int w = 0; w < width; ++w) {
                Vector3 r = Vector3.right * w / width;
                GL.Vertex(Vector3.up + r);
                GL.Vertex(Vector3.zero + r);            
            }

            for (int h = 0; h < height; ++h) {
                Vector3 u = Vector3.up * h / height;
                GL.Vertex(Vector3.right + u);
                GL.Vertex(Vector3.zero + u);            
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}