using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunScoreStore : MonoBehaviour
{
    public static RunScoreStore Instance { get; private set; }

    private readonly Dictionary<int, int> bestStrokesByBuildIndex = new();
    private readonly Dictionary<int, int> parByBuildIndex = new();
    private readonly Dictionary<int, string> nameByBuildIndex = new();

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterLevelMeta(int buildIndex, string levelName, int par)
    {
        nameByBuildIndex[buildIndex] = levelName;
        parByBuildIndex[buildIndex] = par;
    }

    public string GetLevelName(int buildIndex)
        => nameByBuildIndex.TryGetValue(buildIndex, out var v) ? v : $"Level {buildIndex}";

    public int GetPar(int buildIndex)
        => parByBuildIndex.TryGetValue(buildIndex, out var v) ? v : 0;

    public bool HasBest(int buildIndex) => bestStrokesByBuildIndex.ContainsKey(buildIndex);

    public int GetBest(int buildIndex)
        => bestStrokesByBuildIndex.TryGetValue(buildIndex, out var v) ? v : int.MaxValue;

    // Save only if improved (lower) within THIS run
    public bool TrySetBest(int buildIndex, int strokes)
    {
        int current = GetBest(buildIndex);
        if (strokes < current)
        {
            bestStrokesByBuildIndex[buildIndex] = strokes;
            return true;
        }
        return false;
    }

    public void ResetRun()
    {
        bestStrokesByBuildIndex.Clear();
        parByBuildIndex.Clear();
        nameByBuildIndex.Clear();
    }
}
