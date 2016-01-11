using UnityEngine;
using System.Collections;

public class GameContrller : MonoBehaviour {
	public GameObject charactorObject;

	AudioSource sound;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.gameObject.tag == "Char01") {
					sound.Play();
				}
			}
		}
	}
}
