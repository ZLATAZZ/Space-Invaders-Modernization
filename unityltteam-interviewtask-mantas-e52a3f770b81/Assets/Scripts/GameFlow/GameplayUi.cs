using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUi : MonoBehaviour
{

    [SerializeField] private TMP_Text _labelScore;
    [SerializeField] private RectTransform _health;
    private int _score = 0;

    public int Score { get => _score; private set => _score=value;}

    private void Awake()
    {
        _labelScore.text = "0";
        UpdateHealth(0);
    }

    public void AddScore(int s)
    {
        _score += s;
        _labelScore.text = _score.ToString();
    }

    public void SetBestScore()
    {
        if(SaveDataManager.Instance.score < _score)
        {
            SaveDataManager.Instance.score = _score;
        }
    }

    public void UpdateHealth(int h)
    {
        for (int i = 0; i < _health.childCount; i++)
        {
            _health.GetChild(i).gameObject.SetActive((i + 1) <= h);
        }
    }
    public void UpdateEnemyHealthBar(int health, Slider _enemyHealthBar)
    {
        _enemyHealthBar.value = health;
    }
}
