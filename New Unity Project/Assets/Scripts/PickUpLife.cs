using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLife : PickUpOBJ
{

    public override void PickUp()
    {

        if (PlayerManager.instace.HP < PlayerManager.instace.MaxHP)
        {
            PlayerManager.instace.HP += 1;
            if (PlayerManager.instace.HP > PlayerManager.instace.MaxHP)
            {
                PlayerManager.instace.HP = PlayerManager.instace.MaxHP;
            }
        }
        else
        {
            PlayerManager.instace.Coins += 5;
        }

        PlayerManager.instace._UI.RefreshUI();
    }

}
