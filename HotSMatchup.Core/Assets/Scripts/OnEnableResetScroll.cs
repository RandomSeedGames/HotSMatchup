using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnEnableResetScroll : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {

		gameObject.GetComponent<Scrollbar>().value = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
