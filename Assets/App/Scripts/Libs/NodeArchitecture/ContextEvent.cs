using System;
using JetBrains.Annotations;

namespace App.Scripts.Libs.NodeArchitecture
{
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ContextEvent : Attribute
    {
    }
}