using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class play : MonoBehaviour {

	Button playButton;

	// Use this for initialization
	void Start () {
		Button button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick()
	{
		SceneManager.LoadScene("Game");
	}
}
