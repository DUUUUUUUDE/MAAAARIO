using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCoin : PickUpOBJ
{

    public override void PickUp()
    {
        PlayerManager.instace.Coins++;
        PlayerManager.instace._UI.RefreshUI();
    }

}
