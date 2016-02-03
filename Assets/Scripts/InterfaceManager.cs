using UnityEngine;
using System.Collections;

public class InterfaceManager : MonoBehaviour 
{
	public GameObject circle;
	Transform target;
	Vector3 position;
	Material material;

	void Start () 
	{
		target = circle.transform;
		target.localPosition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).direction;
		target.localScale = Vector3.one * 0.1f;

		material = circle.GetComponent<Renderer>().material;

	}

	public void EnableCircle ()
	{
		material.color = Color.red;
	}

	public void DisableCircle ()
	{
		material.color = Color.yellow;
	}
}