using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] private Player _player;
    [Header("Spawn Settings")]
    [SerializeField] private float _safeDistance; //same at min spawn distance
    [SerializeField] private float _maxSpawnDistance;
    [SerializeField] private float _despawnDistance;
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private int _spawnAttempts;
    [SerializeField] private LayerMask _obstacleMask;


    [Header("Animals setup")]
    [SerializeField] private List<GameObject> _animals;
    [SerializeField] private List<GameObject> _activeAnimals = new List<GameObject>();
    [SerializeField] private int _maxAnimals;
    [SerializeField] private float _animalSpawnInterval;
    [SerializeField] private GameObject _animalContainer;

    [Header("Enemies setup")]
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private List<GameObject> _activeEnemies = new List<GameObject>();
    [SerializeField] private int _maxEnemies;
    [SerializeField] private float _enemySpawnInterval;
    [SerializeField] private GameObject _enemyContainer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        
    }

    private void Start()
    {
        _player = FindFirstObjectByType<Player>();

        StartCoroutine(AnimalSpawnCoroutine());
        StartCoroutine(EnemySpawnCoroutine());

    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPoint = Random.insideUnitCircle;
        Vector3 randomDirection = randomPoint.normalized;

        float randomDistance = Random.Range(_safeDistance, _maxSpawnDistance);

        var offset = randomDirection * randomDistance;

        return (Vector3)_player.transform.position + offset;
    }

    private Vector3 FindPosition()
    {
        Vector3 spawnPosition = new Vector3();
        for (int i = 0; i < _spawnAttempts; i++)
        {
            spawnPosition = GetRandomSpawnPosition();
            Vector3Int cellPosition = _groundTilemap.WorldToCell(spawnPosition);
            if (!_groundTilemap.HasTile(cellPosition))
            {
               continue;
            }

            bool obstacle = Physics2D.OverlapCircle(spawnPosition, .5f, _obstacleMask);

            if (obstacle)
            {
                continue;
            }

            return spawnPosition;

        }


        return Vector3.zero;
    }

    private IEnumerator AnimalSpawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_animalSpawnInterval);
            if (_activeAnimals.Count < _maxAnimals)
            {
                Vector3 position = FindPosition();
                if (position != Vector3.zero)
                {
                    GameObject newAnimal = Instantiate(_animals[Random.Range(0, _animals.Count)], 
                                                            position, 
                                                            Quaternion.identity, 
                                                            _animalContainer.transform);
                    _activeAnimals.Add(newAnimal);
                }
            }
        }    
    }

    private IEnumerator EnemySpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_enemySpawnInterval);
            if (_activeEnemies.Count < _maxEnemies)
            {
                Vector3 position = FindPosition();
                if (position != Vector3.zero)
                {
                    GameObject newEnemy = Instantiate(_enemies[Random.Range(0, _enemies.Count)],
                                                            position,
                                                            Quaternion.identity,
                                                            _enemyContainer.transform);
                    _activeEnemies.Add(newEnemy);
                }
            }
        }
    }

    public void NotifyDeath(GameObject entity, bool isEnemy)
    {
        if (isEnemy)
        {
            _activeEnemies.Remove(entity);
        }
        else
        {
            _activeAnimals.Remove(entity);
        }
    }

    private void OnDrawGizmos()
    {
        if (_player == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_player.transform.position, _safeDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_player.transform.position, _maxSpawnDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_player.transform.position, _despawnDistance);

    }


}
