using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Clotris.UI
{
    public class Score : MonoBehaviour
    {
        int currentScore = 0;
        TextMeshProUGUI tmp;

        // Start is called before the first frame update
        void Start()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            tmp.SetText(currentScore.ToString());
        }

        public void UpdateScore(int addScore)
        {
            currentScore += addScore;
            tmp.SetText(currentScore.ToString());
        }
    }
}

