using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour 
{
	public TextMesh comboMesh;
	public TextMesh textMesh;
	Renderer textMeshRender;
	
	WebcamManager webcam;
	TimeManager time;

	float textSize;
	float textSizeOver = 0.05f;

	void Start () 
	{
		webcam = GameObject.FindObjectOfType<WebcamManager>();
		time = GameObject.FindObjectOfType<TimeManager>();
		textMeshRender = textMesh.GetComponent<Renderer>();

		textSize = comboMesh.characterSize;
		Shader.SetGlobalFloat("_LightRatio", textMeshRender.enabled ? 0.5f : 1f);
		UpdateText();
	}

	void Update ()
	{
		if (time.cooldownOver) {
			float size = Mathf.Min(comboMesh.characterSize + Time.deltaTime * 4f, textSizeOver);
			comboMesh.characterSize = Mathf.Lerp(comboMesh.characterSize, size, Time.deltaTime * 4f);

		} else {
			comboMesh.characterSize = Mathf.Lerp(comboMesh.characterSize, textSize * time.cooldownRatio, Time.deltaTime * 4f);
		}

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
		comboMesh.color = color;
	}

	public void SetCombo (float combo)
	{
		comboMesh.text = "x" + combo;
	}

	public void CenterCombo ()
	{
		Vector3 position = comboMesh.transform.position;
		position.y = 0f;
		comboMesh.transform.position = position;
		comboMesh.anchor = TextAnchor.MiddleCenter;
	}

	public void SnapCombo ()
	{
		Vector3 position = comboMesh.transform.position;
		position.y = -1f;
		comboMesh.transform.position = position;
		comboMesh.anchor = TextAnchor.LowerCenter;
	}
}