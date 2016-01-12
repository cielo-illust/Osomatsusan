using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactorController : MonoBehaviour {
	public GameObject sprite;
	public GameContrller gameContrller;
	public GameObject effect;
	public GameObject cutin;
	public GameObject cutin2;

	const float Oso = 1.0f;
	const float Kara = 2.0f;
	const float Choro = 3.0f;
	const float Ichi = 4.0f;
	const float Jushi = 5.0f;
	const float Todo = 6.0f;

	Animator animator;
	AudioSource sound;
	AudioSource soundSub;
	bool playNow = false;
	bool allPlayNow = false;
	bool hitChara = false;
	float startTime = 0.0f;
	float endTime = 0.01f;
	bool playAll = false;
	List<float[]> times = new List<float[]>();

	// Use this for initialization
	void Start () {
		AudioSource[] audioSources = GetComponents<AudioSource>();
		sound = audioSources[0];
		soundSub = audioSources[1];
		animator = sprite.GetComponent<Animator> ();

		initTimes ();
	}

	// Update is called once per frame
	void Update () {
	}

	public void Play() {
		if ((!playNow) && (gameContrller.getTime () != -1.0f)) {
			animator.SetBool ("Play", true);
			startTime = 0.0f;
			endTime = 0.0f;
			float thisTime = Time.time - gameContrller.getTime ();
			for (int i = 0; i < times.Count; i++) {
				float t0 = (i == 0) ? -0.5f : times[i-1][0];
				float t1 = times[i][0];
				float t2 = times[i][1];
				float flag = times[i][2];
				float chara = times[i][3];
				if (((t0 + 0.5f) <= thisTime) && ((t1 + 0.5f) > thisTime)) {
					startTime = t1;
					endTime = t2;
					if (flag == 1.0f) {	// 全員で
						gameContrller.playAll ();
						playAll = true;
					}
					if (flag == 2.0f) {	// 全員で(トド松以外)
						gameContrller.playAll2 ();
						playAll = true;
					}
					if (flag == 3.0f) {	// エフェクト
						Instantiate(cutin, new Vector3(0,-0.5f,-2), Quaternion.identity);
						Instantiate(cutin2, new Vector3(0,-2.5f,-1.9f), Quaternion.identity);
						Instantiate(effect, new Vector3(0,-4,-5), Quaternion.Euler(new Vector3(-90,0,0)));
					}
					if ((hitChar (chara)) && ((flag != 3.0f))) {
						Instantiate(effect, new Vector3(0,-4,-5), Quaternion.Euler(new Vector3(-90,0,0)));
						sound.time = startTime;
						sound.Play ();
					} else {
						soundSub.time = startTime;
						soundSub.Play ();
					}
					break;
				}
			}
			playNow = true;
			StartCoroutine(PlayStop(endTime - startTime));
		}
	}

	IEnumerator PlayStop(float time) {
		yield return new WaitForSeconds (time);
		sound.Stop();
		soundSub.Stop();
		playNow = false;

		if (!allPlayNow) {
			animator.SetBool ("Play", false);
		}

		// 全員で停止
		if (playAll) {
			gameContrller.playAllOff ();
			playAll = false;
		}
	}

	public void PlayAll() {
		allPlayNow = true;
		animator.SetBool ("Play", true);
	}

	public void PlayAllOff() {
		allPlayNow = false;
		animator.SetBool ("Play", false);
	}

	float tt(string t) {
		return float.Parse(t.Substring(0,1)) * 60.0f + float.Parse(t.Substring(2));
	}

	float[] addTime(string t1, string t2, float flag, float chara) {
		float[] t = new float[4];
		t[0] = tt(t1);
		t[1] = tt(t2);
		t[2] = flag;
		t[3] = chara;
		return t;
	}

	bool hitChar(float chara) {
		bool res = false;
		if (chara == 0.0f) res = true;
		if ((chara == Oso) && (sprite.tag == "OsomatsuSprite")) res = true;
		if ((chara == Kara) && (sprite.tag == "KaramatsuSprite")) res = true;
		if ((chara == Choro) && (sprite.tag == "ChoromatsuSprite")) res = true;
		if ((chara == Ichi) && (sprite.tag == "IchimatsuSprite")) res = true;
		if ((chara == Jushi) && (sprite.tag == "JushimatsuSprite")) res = true;
		if ((chara == Todo) && (sprite.tag == "TodomatsuSprite")) res = true;
		return res;
	}

	void initTimes () {
		times.Add (addTime ("0:01.9", "0:02.9", 0.0f, Oso));	// 松野おそ松！
		times.Add (addTime ("0:02.9", "0:03.9", 0.0f, Kara));	// 松野カラ松。
		times.Add (addTime ("0:04.0", "0:04.9", 0.0f, Choro));	// 松野チョロ松
		times.Add (addTime ("0:04.9", "0:06.0", 0.0f, Ichi));	// 松野一松…
		times.Add (addTime ("0:06.0", "0:07.1", 0.0f, Jushi));	// 松野十四松！
		times.Add (addTime ("0:07.1", "0:08.2", 0.0f, Todo));	// 松野トド松
		times.Add (addTime ("0:08.2", "0:12.1", 1.0f, 0.0f));	// おれたち六つ子！おんなじ顔が六つあったって…いいよな？
		// SHAKE!
		times.Add (addTime ("0:17.5", "0:18.6", 0.0f, Oso));	// さっき会ったよね？
		times.Add (addTime ("0:18.6", "0:20.4", 0.0f, Kara));	// 覚えてたぜ、お前の顔
		times.Add (addTime ("0:20.4", "0:22.0", 0.0f, Choro));	// 一回、二回じゃないんですよね
		times.Add (addTime ("0:22.0", "0:23.8", 0.0f, Ichi));	// 多分…これで六度目だよ。
		times.Add (addTime ("0:23.8", "0:26.1", 0.0f, Jushi));	// 今夜はサイコー！ねぇ、踊ろーよ～！
		times.Add (addTime ("0:26.1", "0:27.5", 0.0f, Todo));	// キミの笑顔、かわいいよね
		times.Add (addTime ("0:27.5", "0:28.7", 0.0f, Choro));	// あ、心配しないで！
		times.Add (addTime ("0:28.7", "0:29.8", 0.0f, Jushi));	// 僕らも６人っ！！
		times.Add (addTime ("0:29.8", "0:30.8", 0.0f, Kara));	// オレがアイツで、
		times.Add (addTime ("0:30.8", "0:31.7", 0.0f, Ichi));	// 僕たちは僕
		times.Add (addTime ("0:31.7", "0:33.5", 0.0f, Todo));	// うん、ボクたち六つ子なんだ
		times.Add (addTime ("0:33.5", "0:34.8", 0.0f, Oso));	// キミたちもそうだろ？
		// SHAKE!…ダレがダレでもおんなじざんす！
		// おフランスでミーは知ったざんす
		times.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		// 愛はチミたちのココロの中にあって
		times.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		// ほんのチョッピリわけてちょ、ミーにも…ウヒョ～！
		// Oh! So much,Sons!
		times.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));	// 「ひたすらあそんで暮らして～」
		// “SIX SAME FACES”
		// ああ、なんてややこしいキョーダイざんしょ
		// マゼちゃって
		times.Add (addTime ("1:00.0", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// Don't be a nuisance!
		// コンガラガってコンランざんす
		// おふざけやめてちょ！
		times.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		// “SIX SAME FACES”
		// どれがだれだか教えてちょ！
		// もう、ミーを主役にしてほしいざんす！
		// キャラが一番立ってるざんしょ？
		times.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!
		// まぁ、そろいもそろって…スットンキョ～な連中ざんす！
		times.Add (addTime ("1:22.1", "1:26.4", 1.0f, 0.0f));	// She,Yeah!!!!!!
		times.Add (addTime ("1:27.9", "1:29.0", 0.0f, Oso));	// 長男おそ松！
		times.Add (addTime ("1:29.0", "1:30.0", 0.0f, Kara));	// 次男カラ松。
		times.Add (addTime ("1:30.0", "1:31.0", 0.0f, Choro));	// 三男チョロ松！
		times.Add (addTime ("1:31.0", "1:32.1", 0.0f, Ichi));	// 四男一松。
		times.Add (addTime ("1:32.1", "1:33.2", 0.0f, Jushi));	// 五男十四松っ！
		times.Add (addTime ("1:33.2", "1:34.4", 0.0f, Todo));	// 末弟トド松
		times.Add (addTime ("1:34.4", "1:38.2", 1.0f, 0.0f));	// おれたち六つ子！まだまだ働かなくたって…いいよな？
		// SHAKE!
		times.Add (addTime ("1:34.4", "1:45.0", 0.0f, Choro));	// なんか…楽しいことしません？
		times.Add (addTime ("1:45.0", "1:46.9", 0.0f, Jushi));	// キミたちとあえてラッキーだよ！
		times.Add (addTime ("1:46.9", "1:48.6", 0.0f, Todo));	// これってホント…奇跡だよね
		times.Add (addTime ("1:48.6", "1:50.6", 0.0f, Ichi));	// 六つ子と六つ子の出会いなんて
		times.Add (addTime ("1:50.6", "1:51.9", 0.0f, Choro));	// ババ抜きにならないよね
		times.Add (addTime ("1:51.9", "1:54.3", 0.0f, Kara));	// フッ、確かに同じカードだからな
		times.Add (addTime ("1:54.3", "1:55.8", 0.0f, Ichi));	// 数字だけ違ってね
		times.Add (addTime ("1:55.8", "1:57.9", 0.0f, Jushi));	// ダレがアニキで弟かって？
		times.Add (addTime ("1:57.9", "1:59.7", 0.0f, Oso));	// ま、おれが一番かっこいいだろ？
		times.Add (addTime ("1:59.7", "2:01.8", 0.0f, Todo));	// ねぇ、…ボクを選んでよ
		// SHAKE! …ダレがダレでもおんなじざんす！
		// おフランスはミーの実家ざんす 
		times.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		// 今度チミたちもつれてってほしいざんしょ？ 
		times.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		// コート・ダ・ジョール、よいとこざんすよ…ウヒョ～！
		// Oh! So much,Suns!
		times.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		// “SIX SAME FACES”
		// ああ、なんてすばらしい人生ざんしょ！
		// イヤになって
		times.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// Don't be a nuisance!
		// コンガラガってコンランざんす
		// おふざけやめてちょ！
		times.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// “SIX SAME FACES”
		// もっとガバチョと愛してちょ
		// チミ、ミーのおよめにきてほしいざんす
		// 誰でもいいのならミーにしてちょ！
		times.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// ほら、りっぱざんしょ…おフランスの洋服ざんす!
		times.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		times.Add (addTime ("2:53.2", "3:53.8", 0.0f, 0.0f));	// ついに世界初男女の六つ子による合コンに扱ぎつけたぞ～
		times.Add (addTime ("3:53.8", "3:57.3", 2.0f, 0.0f));	// 早退すんじゃねぇー!
		// “SIX SAME FACES”
		// ああ、なんてめずらしいキョーダイざんしょ！
		// ほうりだして
		times.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// Don't be a nuisance!
		// お見合いパーティー中ざんす
		// おふざけやめてちょ！
		times.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// “SIX SAME FACES”
		// だれが好みか教えてちょ
		// チミ、ミーをえらべばおしあわせざんす
		times.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// 六つ子ちゃんたち、帰ってちょ～～っ！
		// まぁ、そろいもそろって…スットンキョ～な連中ざんす！
		times.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!
		times.Add (addTime ("4:31.0", "4:31.7", 1.0f, 0.0f));	// おそまつ！
	}
}
