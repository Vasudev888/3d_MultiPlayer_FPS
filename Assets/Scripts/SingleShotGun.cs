using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleShotGun : Gun
{
    public override void Use()
    {
        Debug.Log("Using Gun " + itemInfo.itemName);
    }
}
