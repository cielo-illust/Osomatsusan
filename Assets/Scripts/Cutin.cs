using UnityEngine;
using System.Collections;

public class Cutin : MonoBehaviour {
	public float maxAlpha = 1.0f;
	public float fadeTime = 0.6f;
	public float fadeTime2 = 1.0f;
	public float fadeTime3 = 0.6f;
	public float srcPos1 = -7.0f;
	public float dstPos1 = 1.0f;
	public float dstPos2 = 1.5f;
	public float dstPos3 = 8.5f;

	private float currentRemainTime = 0.0f;
	private SpriteRenderer spRenderer;

	int state = 0;

	// Use this for initialization
	void Start () {
		spRenderer = GetComponent<SpriteRenderer>();
		spRenderer.color = new Color(spRenderer.color.r, spRenderer.color.g, spRenderer.color.b, 0);
		transform.position = new Vector3(srcPos1, transform.position.y, transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		currentRemainTime += Time.deltaTime;
		float alpha = currentRemainTime / fadeTime;
		float srcPos = 0.0f;
		float dstPos = 0.0f;

		// フェードイン
		if (state == 0) {
			srcPos = srcPos1;
			dstPos = dstPos1;
			if ( currentRemainTime >= fadeTime ) {
				state = 1;
				currentRemainTime = 0.0f;
				return;
			}
			spRenderer.color = new Color(spRenderer.color.r, spRenderer.color.g, spRenderer.color.b, alpha * maxAlpha);
			transform.position = new Vector3((dstPos * alpha + srcPos * (1.0f - alpha)), transform.position.y, transform.position.z);
		}
		// ステイ
		if (state == 1) {
			srcPos = dstPos1;
			dstPos = dstPos2;
			fadeTime = fadeTime2;
			if ( currentRemainTime >= fadeTime) {
				state = 2;
				currentRemainTime = 0.0f;
				return;
			}
			transform.position = new Vector3((dstPos * alpha + srcPos * (1.0f - alpha)), transform.position.y, transform.position.z);
		}
		// フェードアウト
		if (state == 2) {
			srcPos = dstPos2;
			dstPos = dstPos3;
			fadeTime = fadeTime3;
			if (currentRemainTime >= 1.0f) {
				GameObject.Destroy (gameObject); // 残り時間が無くなったら自分自身を消滅
				return;
			}
			spRenderer.color = new Color(spRenderer.color.r, spRenderer.color.g, spRenderer.color.b, (fadeTime - alpha) * maxAlpha);
			transform.position = new Vector3((dstPos * alpha + srcPos * (1.0f - alpha)), transform.position.y, transform.position.z);
		}
	}
}