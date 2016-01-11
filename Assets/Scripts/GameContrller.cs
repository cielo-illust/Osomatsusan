using UnityEngine;
using System.Collections;

public class GameContrller : MonoBehaviour {
	public GameObject Osomatsu;
	public GameObject Karamatsu;
	public GameObject Choromatsu;
	public GameObject Ichimatsu;
	public GameObject Jushimatsu;
	public GameObject Todomatsu;

	float time;
	public float getTime() {
		return time;
	}

	public void playAll() {
		Osomatsu.SendMessage("PlayAll");
		Karamatsu.SendMessage("PlayAll");
		Choromatsu.SendMessage("PlayAll");
		Ichimatsu.SendMessage("PlayAll");
		Jushimatsu.SendMessage("PlayAll");
		Todomatsu.SendMessage("PlayAll");
	}

	public void playAllOff() {
		Osomatsu.SendMessage("PlayAllOff");
		Karamatsu.SendMessage("PlayAllOff");
		Choromatsu.SendMessage("PlayAllOff");
		Ichimatsu.SendMessage("PlayAllOff");
		Jushimatsu.SendMessage("PlayAllOff");
		Todomatsu.SendMessage("PlayAllOff");
	}

	// Use this for initialization
	void Start () {
		time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.gameObject.tag == "Osomatsu") {
					Osomatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Karamatsu") {
					Karamatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Choromatsu") {
					Choromatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Ichimatsu") {
					Ichimatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Jushimatsu") {
					Jushimatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Todomatsu") {
					Todomatsu.SendMessage("Play");
				}
			}
		}
	}
}
