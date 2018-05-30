using System.Collections;
using UnityEngine;

public class StarBlink : MonoBehaviour {

    private Animator animator;
    private Animator animNewStar;
    public Sprite[] stars;
    public GameObject starPrefab;

    [SerializeField]
    [Range(0, 2f)]
    private float distance;
    [SerializeField]
    [Range(0, 2f)]
    private float lifeTime;

    [SerializeField]
    [Range(0, 10f)]
    private float speedFade;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        StartCoroutine(spawn());
    }

    private IEnumerator spawn() {
        while (true) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(
                Random.Range(Screen.width * 0.1f, Screen.width),
                Random.Range(Screen.height * 0.3f, Screen.height),
                0));
            GameObject newStar = Instantiate(starPrefab, pos, Quaternion.identity);
            animNewStar = newStar.GetComponent<Animator>();
            SpriteRenderer sr = newStar.GetComponent<SpriteRenderer>();

            Sprite star = stars[Random.Range(0, stars.Length)];
            sr.sprite = star;
            playBlinkAnimation();
            StartCoroutine(Fall(newStar));
            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
    }

    private IEnumerator Fall(GameObject newStar) {
        bool isFade = false;

        Vector3 pos = newStar.transform.position;
        float angleX = Random.Range(0, distance);
        float angleY = Random.Range(0, distance);

        for (float t = 0; t < lifeTime; t += Time.deltaTime) {           
            Vector3 dist = new Vector3(pos.x - angleX, pos.y - angleY);
            pos = Vector3.Lerp(pos, dist, t / lifeTime);
            newStar.transform.position = pos;
            if (t >= lifeTime * 0.5 && !isFade) {
                playFadeAnimation();
                isFade = true;
            }
            yield return new WaitForFixedUpdate();
        }
        Destroy(newStar);
    }

    private void playBlinkAnimation() {
        //Debug.Log("BlinkAnim");
        animator.SetTrigger("blink");
        animNewStar.SetTrigger("blink");
    }

    private void playFadeAnimation() {
        //Debug.Log("FadeAnim");
        animator.SetTrigger("fade");
        animNewStar.SetTrigger("fade");

    }
}
