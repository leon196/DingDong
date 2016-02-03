using UnityEngine;
using System.Collections;

public class InterfaceManager : MonoBehaviour 
{
	WebcamManager webcam;
	TextMesh textMesh;
	Renderer textMeshRender;

	void Start () 
	{
		webcam = GameObject.FindObjectOfType<WebcamManager>();

		textMesh = GetComponentInChildren<TextMesh>();
		textMesh.text = "";
		UpdateText();
		textMeshRender = textMesh.GetComponent<Renderer>();
		textMeshRender.enabled = false;
		Shader.SetGlobalFloat("_LightRatio", textMeshRender.enabled ? 0.5f : 1f);
	}

	void Update ()
	{
		// Toggle labels
		if (Input.GetKeyDown(KeyCode.Space)) {
			textMeshRender.enabled = !textMeshRender.enabled;
			Shader.SetGlobalFloat("_LightRatio", textMeshRender.enabled ? 0.5f : 1f);
		}

		if (Input.anyKey) {
			UpdateText();
		}
	}

	void UpdateText ()
	{
		textMesh.text = "Ding Dong Debug" + '\n'
			+ "Camera name : " + webcam.webcamName
			+ "luminance treshold (R/T) : " + webcam.treshold + '\n'
			+ "fade out ratio (F/G) : " + webcam.fadeOutRatio + '\n';
	}
}