using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour 
{
	public TextMesh scoreMesh;
	public TextMesh textMesh;
	Renderer textMeshRender;
	
	WebcamManager webcam;
	TimeManager time;

	Color textColor;
	Color textColorNext;
	float textColorRatio = 0f;
	float textSize;
	float textColorHue = 0f;
	float textSizeOver = 0.05f;

	void Start () 
	{
		webcam = GameObject.FindObjectOfType<WebcamManager>();
		time = GameObject.FindObjectOfType<TimeManager>();
		textMeshRender = textMesh.GetComponent<Renderer>();
		Shader.SetGlobalFloat("_LightRatio", textMeshRender.enabled ? 0.5f : 1f);
		SetColor(Color.white);

		textSize = scoreMesh.characterSize;
		textColor = scoreMesh.color;
		textColorNext = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		UpdateText();
	}

	public void UpdateCooldownSize ()
	{
		scoreMesh.characterSize = Mathf.Lerp(scoreMesh.characterSize, textSize * time.cooldownRatio, Time.deltaTime * 4f);
	}

	public void UpdateScore ()
	{
		float ratio = Mathf.Sin((1f - time.cooldownRatio) * Mathf.PI);
		scoreMesh.characterSize = Mathf.Lerp(0f, textSizeOver, ratio);

		textColorRatio = textColorRatio + Time.deltaTime * 2f;
		SetColor(Color.Lerp(textColor, textColorNext, Mathf.Clamp(textColorRatio, 0f, 1f)));

		if (textColorRatio > 1f) {
			textColorRatio = 0f;
			textColor = textColorNext;
			textColorHue = (textColorHue + 30f) % 360f;
			textColorNext = ColorHSV.GetRandomColor(textColorHue, 1, 1);
		}
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
		textMesh.text = "Ding Dong" + '\n'
			+ "luminance treshold (Left / Right) : " + webcam.treshold + '\n'
			+ "fade out ratio (Up / Down) : " + webcam.fadeOutRatio + '\n'
			+ "mirror webcam (X / Y)" + '\n'
			+ "cooldown delay (P / M) : " + time.cooldownDelay;
	}

	public void SetColor (Color color)
	{
		Shader.SetGlobalColor("_GUIColor", color);
		textColor = color;
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
}