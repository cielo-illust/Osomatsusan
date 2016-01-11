using UnityEngine;
using System.Collections;

public class GameContrller : MonoBehaviour {
	public GameObject Osomatsu;
	public GameObject Karamatsu;
	public GameObject Choromatsu;
	public GameObject Ichimatsu;
	public GameObject Jushimatsu;
	public GameObject Todomatsu;

	// Use this for initialization
	void Start () {
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
