using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text parText;
    [SerializeField] private TMP_Text bestText;
    [SerializeField] private TMP_Text diffText;

    public void Set(string levelName, int par, int best)
    {
        levelText.text = levelName;
        parText.text = par.ToString();

        if (best == int.MaxValue)
        {
            bestText.text = "-";
            diffText.text = "-";
            return;
        }

        bestText.text = best.ToString();
        int diff = best - par;
        diffText.text = diff == 0 ? "E" : (diff > 0 ? $"+{diff}" : diff.ToString());
    }
}
