using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class LineVec3
{
    public Vector3 a, b;

    public LineVec3 ()
    {
        a = new Vector3();
        b = new Vector3();
    }
}

class RectVec3 
{
    public Vector3 a, b, c, d;
    public string name;

    public RectVec3 (string defaultName = "")
    {
        a = new Vector3();
        b = new Vector3();
        c = new Vector3();
        d = new Vector3();
        name = defaultName;
    }

    public void SetRect (Rect rect)
    {
        a.x = d.x = rect.x;
        a.y = b.y = rect.y;
        b.x = c.x = rect.x + rect.width;
        c.y = d.y = rect.y + rect.height;
    }
}

class GridVec3
{
    public LineVec3[] columnArray;
    public LineVec3[] rowArray;
    public string name;

    public GridVec3 (string defaultName = "")
    {
        columnArray = new LineVec3[0];
        rowArray = new LineVec3[0];
        name = defaultName;
    }

    public void SetGrid (int width, int height)
    {
        columnArray = new LineVec3[width - 1];
        for (int x = 0; x < width - 1; ++x) {
            LineVec3 lineVec3 = new LineVec3();
            lineVec3.a.x = lineVec3.b.x = x / width;
            lineVec3.a.y = 0f;
            lineVec3.b.y = 1f;
            columnArray[x] = lineVec3;
        }
        rowArray = new LineVec3[height - 1];
        for (int y = 0; y < height - 1; ++y) {
            LineVec3 lineVec3 = new LineVec3();
            lineVec3.a.y = lineVec3.b.y = y / height;
            lineVec3.a.y = 0f;
            lineVec3.b.y = 1f;
            columnArray[y] = lineVec3;
        }
    }
}

public class DrawLine : MonoBehaviour 
{
    Material material;

    List<RectVec3> rectangleList;
    List<GridVec3> gridList;
    Dictionary<string, int> rectangleKeyValueMap;
    Dictionary<string, int> gridKeyValueMap;

    void Start ()
    {
        material = new Material(Shader.Find("Hidden/Line"));
        rectangleList = new List<RectVec3>();
        gridList = new List<GridVec3>();
        rectangleKeyValueMap = new Dictionary<string, int>();
        gridKeyValueMap = new Dictionary<string, int>();
    }

    public void AddRectangle (string name, Rect rect)
    {
        rectangleKeyValueMap.Add(name, rectangleList.Count);
        RectVec3 rectVec3 = new RectVec3(name);
        rectangleList.Add(rectVec3);
        rectVec3.SetRect(rect);
    }

    public void UpdateRectangle (string name, Rect rect) 
    {
        RectVec3 rectVec3 = rectangleList[rectangleKeyValueMap[name]];
        rectVec3.SetRect(rect);
    }

    public void AddGrid (string name, int width, int height)
    {
        gridKeyValueMap.Add(name, gridList.Count);
        GridVec3 gridVec3 = new GridVec3(name);
        gridList.Add(gridVec3);
        gridVec3.SetGrid(width, height);
    }

    public void UpdateGrid (string name, int width, int height) 
    {
        GridVec3 gridVec3 = gridList[gridKeyValueMap[name]];
        gridVec3.SetGrid(width, height);
    }

    void OnPostRender () 
    {
        GL.PushMatrix();
        material.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Color(Color.white);
        foreach (RectVec3 rectVec3 in rectangleList) 
        {
            GL.Vertex(rectVec3.a);
            GL.Vertex(rectVec3.b);
            GL.Vertex(rectVec3.b);
            GL.Vertex(rectVec3.c);
            GL.Vertex(rectVec3.c);
            GL.Vertex(rectVec3.d);
            GL.Vertex(rectVec3.d);
            GL.Vertex(rectVec3.a);
        }
        GL.End();
        GL.PopMatrix();
    }
}