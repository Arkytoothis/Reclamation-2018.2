using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction
{
    public ActionType Type;
    public Ability Ability;
    public Item Item;
    public List<DamageData> DamageList;
    public int Value1;

    public CharacterAction()
    {
        Type = ActionType.None;
        Ability = null;
        Item = null;
        DamageList = new List<DamageData>();
    }

    public CharacterAction(ActionType type, int value1, Ability ability = null, Item item = null)
    {
        Type = type;
        Value1 = value1;

        if (ability != null)
            Ability = new Ability(ability);
        else
            Ability = null;

        if (item != null)
            Item = new Item(item);
        else
            Item = null;

        DamageList = new List<DamageData>();
    }

    public CharacterAction(CharacterAction action)
    {
        Type = action.Type;
        Value1 = action.Value1;

        if (action.Ability != null)
            Ability = new Ability(action.Ability);
        else
            Ability = null;

        if (action.Item != null)
            Item = new Item(action.Item);
        else
            Item = null;

        DamageList = new List<DamageData>();
        for (int i = 0; i < action.DamageList.Count; i++)
        {
            DamageList.Add(new DamageData(action.DamageList[i]));
        }
    }
}