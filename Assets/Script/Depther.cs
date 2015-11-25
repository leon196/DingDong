using UnityEngine;
using System;
using System.Collections;

public class Depther : MonoBehaviour 
{
	KinectSensor kinectSensor;
	short[] depthData;
	Color32[] colorArray;
	public Texture2D texture;
	public float depthMax;
	public float depthMin;

	void Awake () 
	{
		kinectSensor = GetComponent<KinectSensor>();
		kinectSensor.enabled = true;
		depthData = new short[320 * 240];
		colorArray = new Color32[320 * 240];
		for(int i = 0; i < 320 * 240; i++)
		{
			colorArray[i] = new Color32(0, 0, 0, 255);
		}
		texture = new Texture2D(320, 240, TextureFormat.ARGB32, false);
	}
	
	void Update () 
	{
	
	}
	
	void LateUpdate()
	{
		if (KinectSensor.Instance != null && KinectSensor.Instance.pollDepth())
		{
			depthMax = 0f;
			depthMin = 1f;
			for(int i = 0; i < 320 * 240; i++)
			{
				depthData[i] = (short)(KinectSensor.Instance.getDepth()[i] >> 3);
				colorArray[i].r = (byte)(depthData[i] / 32);
				depthMin = Mathf.Min(depthMin, depthData[i] / 32f / 255f);
				depthMax = Mathf.Max(depthMax, depthData[i] / 32f / 255f);
			}

			texture.SetPixels32(colorArray);
			texture.Apply(false);
		}
	}
}
