using UnityEngine;
using System.Collections;

public class ClickOpenURL : MonoBehaviour {

	public string url;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenURL()
	{
		Application.OpenURL(url);
    }
}
