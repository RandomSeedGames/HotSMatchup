using UnityEngine;
using System.Collections;

public class HoverPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void Moving()
	{
		Vector3[] objectCorners = new Vector3[4];
		GetComponent<RectTransform>().GetWorldCorners(objectCorners);

		float hoverWidth = GetComponent<RectTransform>().sizeDelta.x;
		float maxMouseX = Mathf.Min(Input.mousePosition.x, Screen.width - hoverWidth/2);
		float minMouseX = Mathf.Max(Input.mousePosition.x, hoverWidth / 2);

		Vector2 pos = new Vector2();
		if (Input.mousePosition.x > Screen.width / 2)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, new Vector3(maxMouseX, Input.mousePosition.y, Input.mousePosition.z), Camera.main, out pos);
		}
		else
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, new Vector3(minMouseX, Input.mousePosition.y, Input.mousePosition.z), Camera.main, out pos);
		}
		transform.position = transform.TransformPoint(new Vector2(pos.x, pos.y - 15));
	}

	// Update is called once per frame
	void Update () {
		
		Moving();
	}
}
