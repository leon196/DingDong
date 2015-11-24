using UnityEngine;
using System;
using System.Collections;

public class Depther : MonoBehaviour 
{
	KinectSensor kinectSensor;
	short[] depthData;
	Color32[] colorArray;
	public Texture2D depthTexture;

	void Awake () 
	{
		kinectSensor = GetComponent<KinectSensor>();
		kinectSensor.enabled = true;
		depthData = new short[320 * 240];
		colorArray = new Color32[320 * 240];
		for(int i = 0; i < 320 * 240; i++)
		{
			colorArray[i] = new Color32(255, 255, 255, 255);
		}
		depthTexture = new Texture2D(320, 240, TextureFormat.ARGB32, false);
	}
	
	void Update () 
	{
	
	}
	
	void LateUpdate()
	{
		if (KinectSensor.Instance != null && KinectSensor.Instance.pollDepth())
		{
			for(int i = 0; i < 320 * 240; i++)
			{
				depthData[i] = (short)(KinectSensor.Instance.getDepth()[i] >> 3);
				colorArray[i].r = (byte)(depthData[i] / 32);
				colorArray[i].g = (byte)(depthData[i] / 32);
				colorArray[i].b = (byte)(depthData[i] / 32);
			}

			depthTexture.SetPixels32(colorArray);
			depthTexture.Apply(false);
		}
	}
}
