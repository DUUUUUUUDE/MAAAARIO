using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public GameObject GameOverGO;

    public Text Coins;
    public Text Lifes;
    public Image Health;

    public Animation anim;

    public void GameOver ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameOverGO.SetActive(true);
    }

    public void HideGameOver ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameOverGO.SetActive(false);
        PlayerManager.instace._UI.RefreshUI();
    }


    public void RefreshUI ()
    {
        Lifes.text = (PlayerManager.instace.Lifes + 1).ToString();
        Coins.text = PlayerManager.instace.Coins.ToString();
    }

    public void RefreshHealth ()
    {
        anim.CrossFade("HPAnimation",1f);
        Health.fillAmount = (PlayerManager.instace.MaxHP - PlayerManager.instace.HP) / PlayerManager.instace.MaxHP;
    }
    
}
