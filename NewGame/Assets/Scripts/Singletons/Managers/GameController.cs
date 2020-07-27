using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    private GameObject hearts = null;
    [SerializeField]
    private GameObject bg = null;
    [SerializeField]
    private GameObject heart_prefab = null;
    [SerializeField]
    private Camera cam = null;
    private int playerCurHP;
    private void Awake() {
        var n = PlayerProfile.Instance.PlayerMaxHP;
        playerCurHP = n;
        while(n-- > 0) {
            GameObject go = Instantiate(heart_prefab, hearts.transform);
        }
    }
    public void PlayerGetHurt() {
        if (playerCurHP == 0) return;
        StartCoroutine(FullScreenHurtEffect());
        playerCurHP--;
        hearts.transform.GetChild(playerCurHP).GetChild(0).gameObject.SetActive(true);
    }
    private IEnumerator FullScreenHurtEffect() {
        cam.GetComponent<Animator>().SetTrigger("Shake");
        bg.GetComponent<SpriteRenderer>().color = new Color(1f, .7f, .7f, 1f);
        yield return new WaitForSeconds(.35f);
        bg.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    public void PlayerAddHealth() {
        if (playerCurHP == PlayerProfile.Instance.PlayerMaxHP) return;
        hearts.transform.GetChild(playerCurHP).GetChild(0).gameObject.SetActive(false);
        playerCurHP++;
    }
}
