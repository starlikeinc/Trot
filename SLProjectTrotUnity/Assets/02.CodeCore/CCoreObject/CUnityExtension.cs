using System;
using UnityEngine;

public static class Extension
{
    public static ENUM StringToEnum<ENUM>(this string value) where ENUM : Enum
    {
        if (!Enum.IsDefined(typeof(ENUM), value))
            return default;
        return (ENUM)Enum.Parse(typeof(ENUM), value, true);
    }
}

public struct CTransform
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
}

[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute
{
    public readonly bool runtimeOnly;

    public ReadOnlyAttribute(bool runtimeOnly = false)
    {
        this.runtimeOnly = runtimeOnly;
    }
}
