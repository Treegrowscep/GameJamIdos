using System.Collections.Generic;
using UnityEngine;

public class SkeletonTeam : MonoBehaviour
{
    public int teamID = 0;

    // Registry for efficient lookups (avoids FindGameObjectsWithTag)
    private static readonly List<SkeletonTeam> registry = new List<SkeletonTeam>();
    public static IReadOnlyList<SkeletonTeam> All => registry;

    private void OnEnable()
    {
        if (!registry.Contains(this)) registry.Add(this);
    }

    private void OnDisable()
    {
        registry.Remove(this);
    }
}