using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public int cristal = 0;
    public int iron = 0;
    public int mineral = 0;
    public int core = 0;
    public int soulGem = 0;
    public int redStone = 0;

    public void GetItme(int gem)
    {
        Gem item = (Gem)gem;
        switch (item)
        {
            case Gem.Crystal:
                cristal++;
                break;
            case Gem.Iron:
                iron++;
                break;
            case Gem.Mineral:
                mineral++;
                break;
            case Gem.Core:
                core++;
                break;
            case Gem.SoulGem:
                soulGem++;
                break;
            case Gem.RedStone:
                redStone++;
                break;
        }
    }

}
