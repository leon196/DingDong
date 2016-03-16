using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI : MonoBehaviour 
{
	Webcam webcam;
	Grid grid;

	public TextMesh scoreMesh;
	public TextMesh textMesh;
	public TextMesh titleMesh;
	public TextMesh startMesh;
	public TextMesh authorMesh;

	Color textColor;
	Color textColorNext;
	Color colorAlpha;
	float textColorRatio = 0f;
	float textSize;
	float textColorHue = 0f;
	float textSizeOver = 0.05f;

	Material materialGrid;
	bool shouldDrawGrid;

	void Start () 
	{
		webcam = GameObject.FindObjectOfType<Webcam>();
		shouldDrawGrid = textMesh.GetComponent<Renderer>().enabled;
		
		Shader.SetGlobalFloat("_LightRatio", textMesh.GetComponent<Renderer>().enabled ? 0.5f : 1f);
		SetColor(Color.white);

		materialGrid = new Material(Shader.Find("Hidden/Line"));

		textSize = scoreMesh.characterSize;
		textColor = Color.white;
		textColorNext = ColorHSV.GetRandomColor();
		colorAlpha = new Color(1f,1f,1f,0f);
		UpdateText();
	}

	public void UpdateCooldownSize ()
	{
		// scoreMesh.characterSize = Mathf.Lerp(scoreMesh.characterSize, textSize * time.cooldownRatio, Time.deltaTime * 4f);
	}

	public void UpdateScore ()
	{
		// float ratio = Mathf.Sin((1f - time.cooldownRatio) * Mathf.PI);
		// scoreMesh.characterSize = Mathf.Lerp(0f, textSizeOver, ratio);

		// textColorRatio = textColorRatio + Time.deltaTime * 2f;
		// SetColor(Color.Lerp(textColor, textColorNext, Mathf.Clamp(textColorRatio, 0f, 1f)));

		// if (textColorRatio > 1f) {
		// 	textColorRatio = 0f;
		// 	textColor = textColorNext;
		// 	textColorHue = (textColorHue + 30f) % 360f;
		// 	textColorNext = ColorHSV.GetRandomColor(textColorHue, 1, 1);
		// }
	}

	void Update ()
	{
		// Toggle labels
		if (Input.GetKeyDown(KeyCode.Space)) {
			textMesh.GetComponent<Renderer>().enabled = !textMesh.GetComponent<Renderer>().enabled;
			shouldDrawGrid = textMesh.GetComponent<Renderer>().enabled;
			Shader.SetGlobalFloat("_LightRatio", textMesh.GetComponent<Renderer>().enabled ? 0.5f : 1f);
		}

		if (Input.anyKey) {
			UpdateText();
		}

		// textColorRatio = textColorRatio + Time.deltaTime;
		// SetColor(Color.Lerp(textColor, textColorNext, Mathf.Clamp(textColorRatio, 0f, 1f)));

		// if (textColorRatio > 1f) {
		// 	textColorRatio = 0f;
		// 	textColor = textColorNext;
		// 	textColorHue = (textColorHue + 10f) % 360f;
		// 	textColorNext = ColorHSV.GetColor(textColorHue, 1, 1);
		// }
	}

	void UpdateText ()
	{
		textMesh.text = "Ding Dong" + '\n'
		+ "luminance treshold (Left / Right) : " + webcam.treshold + '\n'
		+ "fade out ratio (Up / Down) : " + webcam.fadeOutRatio + '\n'
		+ "mirror webcam (X / Y)" + '\n';
			// + "cooldown delay (P / M) : " + time.cooldownDelay + '\n';
			// + "bonus size (+ / -) : " + bonus.bonusSize;
	}

	public void GotoTitle ()
	{
		titleMesh.GetComponent<Renderer>().enabled = true;
		startMesh.GetComponent<Renderer>().enabled = true;
		scoreMesh.GetComponent<Renderer>().enabled = false;
		authorMesh.GetComponent<Renderer>().enabled = true;
	}

	public void GotoGame ()
	{
		titleMesh.GetComponent<Renderer>().enabled = false;
		startMesh.GetComponent<Renderer>().enabled = false;
		scoreMesh.GetComponent<Renderer>().enabled = true;
		authorMesh.GetComponent<Renderer>().enabled = false;
	}

	public void SetColor (Color color)
	{
		Shader.SetGlobalColor("_GUIColor", color);
		textColor = color;
	}

	public void UpdateAlpha (float ratio)
	{
		Shader.SetGlobalColor("_GUIColor", Color.Lerp(colorAlpha, textColor, ratio));
	}

	public void SetScore (float score)
	{
		scoreMesh.text = "x" + score;
	}

	public void CenterScore ()
	{
		Vector3 position = scoreMesh.transform.position;
		position.y = 0f;
		scoreMesh.transform.position = position;
		scoreMesh.anchor = TextAnchor.MiddleCenter;
	}

	public void SnapScore ()
	{
		Vector3 position = scoreMesh.transform.position;
		position.y = -1f;
		scoreMesh.transform.position = position;
		scoreMesh.anchor = TextAnchor.LowerCenter;
	}

	void OnPostRender () 
	{
		if (shouldDrawGrid) 
		{
			GL.PushMatrix();
			materialGrid.SetPass(0);
			GL.LoadOrtho();
			GL.Begin(GL.LINES);
			GL.Color(Color.white);

			for (int w = 0; w < Grid.width; ++w) {
				Vector3 r = Vector3.right * w / Grid.width;
				GL.Vertex(Vector3.up + r);
				GL.Vertex(Vector3.zero + r);            
			}

			for (int h = 0; h < Grid.height; ++h) {
				Vector3 u = Vector3.up * h / Grid.height;
				GL.Vertex(Vector3.right + u);
				GL.Vertex(Vector3.zero + u);            
			}

			GL.End();
			GL.PopMatrix();
		}
	}
}