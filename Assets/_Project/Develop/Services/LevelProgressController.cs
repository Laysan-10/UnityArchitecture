using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressController : MonoBehaviour, ISceneBootstrapper
{
    [Header("UI")]
    [SerializeField] private ScoreUI scoreUI;
    [SerializeField] private GameObject victoryUi;
    [SerializeField] private float victoryUiDuration = 5f;

    [Header("Events")]
    [SerializeField] private BossSpawner bossSpawner;
    [SerializeField] private int killsToSpawnBoss = 3;
    [SerializeField] private int killsToPlayVictoryMusic = 5;
    [SerializeField] private int scorePerKill = 100;
    [SerializeField] private string victoryMusicKey = "Victory";

    private readonly Dictionary<EnemyBase, Action> _enemyDeathHandlers = new Dictionary<EnemyBase, Action>();
    private bool _isInitialized;
    private int _killedMobCount;
    private int _score;
    private int _totalMobCount;
    private int _deadMobCount;
    private bool _bossSpawnTriggered;
    private bool _bossDefeated;
    private bool _victoryMusicTriggered;
    private IAudioService _audioService;

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;
        _audioService = AppServices.Resolve<IAudioService>();

        ResetProgress();
        SubscribeToEnemyDeaths(FindObjectsByType<EnemyBase>(FindObjectsSortMode.None));
    }

    private void OnDestroy()
    {
        UnsubscribeEnemyDeaths();
    }

    private void ResetProgress()
    {
        _killedMobCount = 0;
        _score = 0;
        _totalMobCount = 0;
        _deadMobCount = 0;
        _bossSpawnTriggered = false;
        _bossDefeated = bossSpawner == null;
        _victoryMusicTriggered = false;
        scoreUI?.SetScore(_score);

        if (victoryUi != null)
        {
            victoryUi.SetActive(false);
        }
    }

    private void SubscribeToEnemyDeaths(IEnumerable<EnemyBase> enemies)
    {
        UnsubscribeEnemyDeaths();

        if (enemies == null)
        {
            return;
        }

        foreach (EnemyBase enemy in enemies)
        {
            if (enemy == null || enemy.isBoss || enemy.health == null || enemy.health.Core == null)
            {
                continue;
            }

            _totalMobCount++;

            EnemyBase trackedEnemy = enemy;
            Action deathHandler = null;
            deathHandler = () => HandleMobKilled(trackedEnemy);
            trackedEnemy.health.Core.OnDeath += deathHandler;
            _enemyDeathHandlers[trackedEnemy] = deathHandler;
        }
    }

    private void HandleMobKilled(EnemyBase enemy)
    {
        if (enemy == null)
        {
            return;
        }

        if (_enemyDeathHandlers.TryGetValue(enemy, out Action deathHandler))
        {
            if (enemy.health != null && enemy.health.Core != null)
            {
                enemy.health.Core.OnDeath -= deathHandler;
            }

            _enemyDeathHandlers.Remove(enemy);
        }

        _killedMobCount++;
        _deadMobCount++;
        _score += Mathf.Max(0, scorePerKill);
        scoreUI?.SetScore(_score);

        if (!_bossSpawnTriggered && bossSpawner != null && _killedMobCount >= killsToSpawnBoss)
        {
            _bossSpawnTriggered = true;
            EnemyBase boss = bossSpawner.SpawnBoss();
            SubscribeToBossDeath(boss);
        }

        TryCompleteVictory();
    }

    private void SubscribeToBossDeath(EnemyBase boss)
    {
        if (boss == null || boss.health == null || boss.health.Core == null)
        {
            return;
        }

        Action deathHandler = null;
        deathHandler = () => HandleBossKilled(boss);
        boss.health.Core.OnDeath += deathHandler;
        _enemyDeathHandlers[boss] = deathHandler;
    }

    private void HandleBossKilled(EnemyBase boss)
    {
        if (boss == null)
        {
            return;
        }

        if (_enemyDeathHandlers.TryGetValue(boss, out Action deathHandler))
        {
            if (boss.health != null && boss.health.Core != null)
            {
                boss.health.Core.OnDeath -= deathHandler;
            }

            _enemyDeathHandlers.Remove(boss);
        }

        _bossDefeated = true;
        TryCompleteVictory();
    }

    private void TryCompleteVictory()
    {
        bool allMobsDefeated = _deadMobCount >= _totalMobCount;
        bool bossRequirementMet = bossSpawner == null || _bossDefeated;

        if (!_victoryMusicTriggered &&
            _killedMobCount >= killsToPlayVictoryMusic &&
            allMobsDefeated &&
            bossRequirementMet)
        {
            _victoryMusicTriggered = true;
            _audioService?.PlayMusic(victoryMusicKey);
            ShowVictoryUi();
        }
    }

    private void ShowVictoryUi()
    {
        if (victoryUi == null)
        {
            return;
        }

        victoryUi.SetActive(true);
        CancelInvoke(nameof(HideVictoryUi));
        Invoke(nameof(HideVictoryUi), Mathf.Max(0f, victoryUiDuration));
    }

    private void HideVictoryUi()
    {
        if (victoryUi != null)
        {
            victoryUi.SetActive(false);
        }
    }

    private void UnsubscribeEnemyDeaths()
    {
        foreach (KeyValuePair<EnemyBase, Action> pair in _enemyDeathHandlers)
        {
            EnemyBase enemy = pair.Key;
            if (enemy != null && enemy.health != null && enemy.health.Core != null)
            {
                enemy.health.Core.OnDeath -= pair.Value;
            }
        }

        _enemyDeathHandlers.Clear();
    }
}
