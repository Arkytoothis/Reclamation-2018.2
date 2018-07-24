using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeCalculationOpperator
{
    Add, Subtract, Multiply, Multiply_Neg, Average, 
    None, Number
}

[System.Serializable]
public class AttributeCalculation
{
    public CharacteristicModifier Attribute1;
    public CharacteristicModifier Attribute2;
    public CharacteristicModifier Attribute3;
    public AttributeCalculationOpperator Operator1;
    public AttributeCalculationOpperator Operator2;

    public AttributeCalculation()
    {
        Attribute1 = new CharacteristicModifier();
        Attribute2 = new CharacteristicModifier();
        Attribute3 = new CharacteristicModifier();
        Operator1 = AttributeCalculationOpperator.None;
        Operator2 = AttributeCalculationOpperator.None;
    }

    public AttributeCalculation(CharacteristicModifier att1, CharacteristicModifier att2, CharacteristicModifier att3, AttributeCalculationOpperator op1, AttributeCalculationOpperator op2)
    {
        if (att1 != null)
            Attribute1 = new CharacteristicModifier(att1);
        if (att2 != null)
            Attribute2 = new CharacteristicModifier(att2);
        if (att3 != null)
            Attribute3 = new CharacteristicModifier(att3);
        Operator1 = op1;
        Operator2 = op2;
    }

    public AttributeCalculation(AttributeCalculation calc)
    {
        if (calc != null)
        {
            if (calc.Attribute1 != null)
                Attribute1 = new CharacteristicModifier(calc.Attribute1);
            if (calc.Attribute2 != null)
                Attribute2 = new CharacteristicModifier(calc.Attribute2);
            if (calc.Attribute3 != null)
                Attribute3 = new CharacteristicModifier(calc.Attribute3);
            Operator1 = calc.Operator1;
            Operator2 = calc.Operator2;
        }
    }
}