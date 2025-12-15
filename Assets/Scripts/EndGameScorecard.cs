using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameScorecard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform tableRoot;
    [SerializeField] private GameObject cellPrefab;

    [Header("Build settings assumptions")]
    [Tooltip("Usually 0 = MainMenu, so first level starts at 1")]
    [SerializeField] private int firstLevelBuildIndex = 1;

    private void Start()
    {
        BuildTable();
    }

    private void BuildTable()
    {
        // Clear existing cells
        for (int i = tableRoot.childCount - 1; i >= 0; i--)
            Destroy(tableRoot.GetChild(i).gameObject);

        // Header
        AddCell("Level", true);
        AddCell("Par", true);
        AddCell("Best", true);
        AddCell("+/-", true);

        int lastPlayableIndex = SceneManager.sceneCountInBuildSettings - 2;

        for (int buildIndex = firstLevelBuildIndex; buildIndex <= lastPlayableIndex; buildIndex++)
        {
            string name = RunScoreStore.Instance.GetLevelName(buildIndex);
            int par = RunScoreStore.Instance.GetPar(buildIndex);

            int best = RunScoreStore.Instance.HasBest(buildIndex)
                ? RunScoreStore.Instance.GetBest(buildIndex)
                : int.MaxValue;

            AddCell(name);
            AddCell(par.ToString());

            if (best == int.MaxValue)
            {
                AddCell("-");
                AddCell("-");
            }
            else
            {
                AddCell(best.ToString());
                int diff = best - par;
                AddCell(diff == 0 ? "E" : (diff > 0 ? $"+{diff}" : diff.ToString()));
            }
        }
    }

    private void AddCell(string text, bool bold = false)
    {
        GameObject cell = Instantiate(cellPrefab, tableRoot);
        var tmp = cell.GetComponentInChildren<TMPro.TMP_Text>();
        tmp.text = text;
        if (bold) tmp.fontStyle = TMPro.FontStyles.Bold;
    }
}
