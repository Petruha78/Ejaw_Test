using Interfaces;
using UnityEngine;

public class Item : Movable, IItem
{
    public virtual void GetFromPoolReset()
    {
    }
}