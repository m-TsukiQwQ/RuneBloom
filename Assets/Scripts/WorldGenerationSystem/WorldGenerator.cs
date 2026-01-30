using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class WorldGenerator : MonoBehaviour, ISaveable
{
    [Header("Map Dimensions")]
    public int width = 100;
    public int height = 100;

    [Header("Generation Settings")]
    [Tooltip("Controls the 'zoom' of the Perlin Noise. Higher values make the terrain features smaller/more chaotic. Lower values make them larger/smoother.")]
    public float noiseScale = 20f;
    public int seed = 0;

    [Tooltip("Controls the falloff gradient. 0.5 = Small Island surrounded by lots of water. 1.5 = Large Island filling most of the map.")]
    [Range(0.1f, 1.5f)] public float islandSize = 0.7f;

    [Header("References")]
    public Tilemap groundTilemap;
    public Tilemap decorationTilemap;
    public Tilemap cliffTilemap;
    public Tilemap waterTilemap;

    public TileBase grassTile;
    public TileBase cliffTile;
    public TileBase waterTile;


    [Header("Decoration Settings")]
    [Tooltip("Controls the 'zoom' of the Decoration patches. Higher = Smaller patches.")]
    public float decorationNoiseScale = 10f;
    [Tooltip("Density of decorations. 0.2 means roughly 20% of the land will have decorations.")]
    [Range(0f, 1f)] public float decorationChance = 0.2f;

    public TileBase grassDecorationTile;


    [Header("Nature Objects")]
    public GameObject treePrefab;
    public GameObject smallRockPrefab;
    public GameObject stickPrefab;
    public GameObject stumpPrefab;
    public GameObject trunkPrefab;

    public GameObject rockPrefab;
    public GameObject[] flowerPrefabs;

    public Transform objectsParent;

    [Header("Grass Variations")]
    public GameObject tallGrassPrefab;
    public GameObject mediumGrassPrefab;
    public GameObject shortGrassPrefab;

    [Tooltip("Controls how big the patches of same-height grass are. Higher = Larger fields.")]
    public float grassPatchScale = 15f;

    [Header("Spawn Chances (0.0 to 1.0)")]
    // Density Settings: Adjust these to fill your world
    [Range(0, 1)] public float rockChance = 0.02f;  // 2%
    [Range(0, 1)] public float treeChance = 0.05f;  // 5%
    [Range(0, 1)] public float stickChance = 0.05f;  // 5%
    [Range(0, 1)] public float smallRockChance = 0.2f;
    [Range(0, 1)] public float stumpChance = 0.2f;
    [Range(0, 1)] public float trunkChance = 0.2f;
    [Range(0, 1)] public float flowerChance = 0.2f;

    [Header("Visual Randomness")]
    [Tooltip("How much objects can shift from the tile center. 0 = Center, 0.5 = Full Tile Range.")]
    [Range(0f, 0.5f)] public float positionJitter = 0.3f;

    [Header("Grass Settings")]
    [Tooltip("Any noise value below this will be empty ground. 0.6 means 60% of the island is empty dirt, 40% is grass.")]
    [Range(0f, 1f)] public float grassThreshold = 0.6f;

    private HashSet<Vector2Int> _removedNatureCoordinates = new HashSet<Vector2Int>();

    // --- FIX: Store offsets generated from seed, not the seed itself ---
    private Vector2 _natureOffset;
    private Vector2 _grassOffset;
    private Vector2 _decorTileOffset;

    private void Start()
    {
        // If there is no SaveManager, generate a default world immediately.
        // If SaveManager exists, it will call LoadData -> InitializeWorld instead.
        if (FindFirstObjectByType<SaveManager>() == null)
        {
            GenerateWorld();
        }
    }

    public void GenerateWorld()
    {
        groundTilemap.ClearAllTiles();
        cliffTilemap.ClearAllTiles();
        decorationTilemap.ClearAllTiles();

        // Cleanup old objects
        foreach (Transform child in objectsParent) Destroy(child.gameObject);

        // Initialize the Random State so the same seed produces the exact same world
        Random.InitState(seed);

        // --- FIX: Generate safe offsets used for Perlin Noise ---
        // Random.Range returns floats within safe precision limits (-10k to 10k)
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        _natureOffset = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        _grassOffset = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        _decorTileOffset = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));

        // Center point used for the circular island calculations
        Vector2 center = new Vector2(width / 2f, height / 2f);
        // We use the smaller dimension to keep the island circular even if width != height
        float minDimension = Mathf.Min(width, height);


        int[,] mapData = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // A. PERLIN NOISE GENERATION
                float xCoord = (float)x / width * noiseScale + xOffset;
                float yCoord = (float)y / height * noiseScale + yOffset;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                // Calculate distance from center of the map
                float distFromCenter = Vector2.Distance(new Vector2(x, y), center);
                // Normalize distance (0.0 at center, 1.0 at edge of the defined circle)
                float t = distFromCenter / (minDimension / 2f);
                // Curve the gradient using Power function.
                float gradient = Mathf.Pow(t, 3f);

                // Combine Noise with Gradient
                float finalValue = noiseValue - (gradient / islandSize);

                // Land (1) or Water (0)
                if (finalValue > 0.2f)
                {
                    mapData[x, y] = 1;
                }
                else
                {
                    mapData[x, y] = 0;
                }
            }
        }

        int smoothingIterations = 5;

        for (int i = 0; i < smoothingIterations; i++)
        {
            int[,] cleanMap = (int[,])mapData.Clone();

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    bool waterLeft = mapData[x - 1, y] == 0;
                    bool waterRight = mapData[x + 1, y] == 0;
                    bool waterUp = mapData[x, y + 1] == 0;
                    bool waterDown = mapData[x, y - 1] == 0;

                    if (mapData[x, y] == 1)
                    {
                        if ((waterLeft && waterRight) || (waterUp && waterDown))
                        {
                            cleanMap[x, y] = 0;
                        }
                    }
                    else if (mapData[x, y] == 0)
                    {
                        int landNeighbors = 0;
                        if (!waterLeft) landNeighbors++;
                        if (!waterRight) landNeighbors++;
                        if (!waterUp) landNeighbors++;
                        if (!waterDown) landNeighbors++;

                        if (landNeighbors >= 3)
                        {
                            cleanMap[x, y] = 1;
                        }
                    }
                }
            }
            mapData = cleanMap;
        }

        // Spawn tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x - width / 2, y - height / 2, 0);

                if (mapData[x, y] == 1)
                {
                    groundTilemap.SetTile(pos, grassTile);
                    cliffTilemap.SetTile(pos, cliffTile);
                    waterTilemap.SetTile(pos, waterTile);

                    if (decorationTilemap != null && grassDecorationTile != null)
                    {
                        // Use the safe offset generated from the seed
                        float decorX = (float)x / width * decorationNoiseScale + _decorTileOffset.x;
                        float decorY = (float)y / height * decorationNoiseScale + _decorTileOffset.y;

                        float decorValue = Mathf.PerlinNoise(decorX, decorY);

                        if (decorValue > (1f - decorationChance))
                        {
                            decorationTilemap.SetTile(pos, grassDecorationTile);
                        }

                        // --- NEIGHBOR CHECK: PREVENT COAST SPAWNING ---
                        bool nextToWater = false;

                        if (x > 0 && mapData[x - 1, y] == 0) nextToWater = true;
                        else if (x < width - 1 && mapData[x + 1, y] == 0) nextToWater = true;
                        else if (y > 0 && mapData[x, y - 1] == 0) nextToWater = true;
                        else if (y < height - 1 && mapData[x, y + 1] == 0) nextToWater = true;

                        // Only spawn nature if we are NOT next to water
                        if (!nextToWater)
                        {
                            SpawnNature(pos, x, y);
                        }
                    }
                }
                else
                {
                    waterTilemap.SetTile(pos, waterTile);
                }
            }
        }
    }

    private void SpawnNature(Vector3Int gridPos, int x, int y)
    {
        Vector3 basePos = groundTilemap.CellToWorld(gridPos) + new Vector3(.5f, .5f, 0);
        float offsetX = Random.Range(-positionJitter, positionJitter);
        float offsetY = Random.Range(-positionJitter, positionJitter);
        Vector3 spawnPos = basePos + new Vector3(offsetX, offsetY, 0);
        bool isRemoved = _removedNatureCoordinates.Contains((Vector2Int)gridPos);

        // --- FIX: Use safe offsets, not raw seed addition ---
        float roll = Mathf.PerlinNoise(x * 0.8f + _natureOffset.x, y * 0.8f + _natureOffset.y);

        float currentTreshold = 0f;

        currentTreshold += rockChance;
        if (roll < currentTreshold)
        {
            if (!isRemoved)
                SpawnObject(rockPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += treeChance;
        if (roll < currentTreshold)
        {
            if (!isRemoved)
                SpawnObject(treePrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += stumpChance;
        if (roll < currentTreshold)
        {
            if (!isRemoved)
                SpawnObject(stumpPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += trunkChance;
        if (roll < currentTreshold)
        {
            if (!isRemoved)
                SpawnObject(trunkPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += stickChance;
        if (roll < currentTreshold)
        {
            if (!isRemoved)
                SpawnObject(stickPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += smallRockChance;
        if (roll < currentTreshold)
        {
            if (!isRemoved)
                SpawnObject(smallRockPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += flowerChance;
        if (roll < currentTreshold)
        {
            if (!isRemoved)
                SpawnObject(flowerPrefabs[Random.Range(0, 3)], spawnPos, objectsParent);
            return;
        }

        // --- FIX: Use safe offsets for grass patches too ---
        float xPatch = (x / grassPatchScale) + _grassOffset.x;
        float yPatch = (y / grassPatchScale) + _grassOffset.y;

        float patchNoise = Mathf.PerlinNoise(xPatch, yPatch);

        // If below threshold, this is empty ground (Dirt/Clearing)
        if (patchNoise < grassThreshold) return;

        float range = 1.0f - grassThreshold;
        float step = range / 3f;

        float shortLimit = grassThreshold + step;
        float mediumLimit = grassThreshold + (step * 2);

        GameObject grassToSpawn = null;

        if (patchNoise < shortLimit)
        {
            grassToSpawn = shortGrassPrefab;
        }
        else if (patchNoise < mediumLimit)
        {
            grassToSpawn = mediumGrassPrefab;
        }
        else
        {
            grassToSpawn = tallGrassPrefab;
        }

        if (grassToSpawn != null && !isRemoved)
            Instantiate(grassToSpawn, spawnPos, Quaternion.identity, transform);
    }

    private void Flip(GameObject gameObject)
    {
        float roll = Random.value;
        if (roll > 0.5)
            gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void SpawnObject(GameObject objectPrefab, Vector3 spawnPos, Transform parent)
    {
        if (objectPrefab)
        {
            GameObject spawnedObject = Instantiate(objectPrefab, spawnPos, Quaternion.identity, parent);
            Flip(spawnedObject);
        }
    }

    public void LoadData(GameData data)
    {
        this.seed = data.worldSeed;

        _removedNatureCoordinates.Clear();
        foreach (Vector2Int coord in data.removedWorldObjects)
        {
            _removedNatureCoordinates.Add(coord);
        }

        GenerateWorld();
    }

    public void SaveData(ref GameData data)
    {
        data.worldSeed = this.seed;
        data.removedWorldObjects = new List<Vector2Int>(_removedNatureCoordinates);
    }

    public void NotifyObjectRemoved(Vector3 worldPosition)
    {
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPosition);
        _removedNatureCoordinates.Add((Vector2Int)cellPos);
    }

    [ContextMenu("Profiling Test")]
    public void RunPerformanceBenchmark()
    {
        Stopwatch sw = new Stopwatch();

        // Test 1: Ma?a mapa (50x50)
        width = 50; height = 50;
        sw.Restart();
        GenerateWorld();
        sw.Stop();
        UnityEngine.Debug.Log($"Profiling Test [50x50]: {sw.ElapsedMilliseconds} ms");

        // Test 2: ?rednia mapa (100x100 - domy?lna)
        width = 100; height = 100;
        sw.Restart();
        GenerateWorld();
        sw.Stop();
        UnityEngine.Debug.Log($"Profiling Test [100x100]: {sw.ElapsedMilliseconds} ms");

        // Test 3: Du?a mapa (200x200) - 4x wi?cej kafelków
        width = 200; height = 200;
        sw.Restart();
        GenerateWorld();
        sw.Stop();
        UnityEngine.Debug.Log($"Profiling Test [200x200]: {sw.ElapsedMilliseconds} ms");

        // Opcjonalnie: Przywró? domy?lne 100x100 po te?cie
        width = 100; height = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            RunPerformanceBenchmark();
    }
}

