using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTunnel : MonoBehaviour
{
    Inventory inven;
    Craft craft;

    public MailPost mailPost;

    private void Start()
    {
        if(GetComponentInParent<Inventory>() != null)
            inven = GetComponentInParent<Inventory>();

        if (GetComponentInParent<Craft>() != null)
            craft = GetComponentInParent<Craft>();
    }

    public void OnTunnel_ChangeWeapon()
    {
        Debug.Log("온터널 : " + this.gameObject.name);
        inven.UI_ChangeWeapon(this.gameObject.name);
    }

    public void OnTunnel_SelectMain()
    {
        inven.SelectMain();
    }

    public void OnTunnel_SelectSub()
    {
        inven.SelectSub();
    }

    public void OnTunnel_SelectHelmet()
    {
        inven.SelectHelmet();
    }

    public void OnTunnel_SelectArmor()
    {
        inven.SelectArmor();
    }

    public void OnTunnel_SelectShoulder()
    {
        inven.SelectShoulder();
    }

    public void OnTunnel_SelectGlove()
    {
        inven.SelectGlove();
    }

    public void OnTunnel_SelectPants()
    {
        inven.SelectPants();
    }

    public void OnTunnel_SelectShoes()
    {
        inven.SelectShoes();
    }

    public void OnTunnel_SelectAcc()
    {
        inven.SelectAcc();
    }

    public void OnTunnel_ClearEquip()
    {
        inven.ClearEquip(inven.selectedEquip);
    }

    public void OnTunnel_NextCraft()
    {
        craft.LoadNextCraft();
    }

    public void OnTunnel_PrevCraft()
    {
        craft.LoadPrevCraft();
    }

    public void OnTunnel_Craft()
    {
        craft.CraftItem();
    }
}
