using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
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

    private void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        groundTilemap.ClearAllTiles();
        cliffTilemap.ClearAllTiles();
        decorationTilemap.ClearAllTiles();

        // Initialize the Random State so the same seed produces the exact same world
        Random.InitState(seed);

        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

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
                // Calculate the coordinates to sample from the noise map
                float xCoord = (float)x / width * noiseScale + xOffset;
                float yCoord = (float)y / height * noiseScale + yOffset;
                // Get value between 0.0 and 1.0 (creates natural, cloud-like patterns)
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                // Calculate distance from center of the map
                float distFromCenter = Vector2.Distance(new Vector2(x, y), center);
                // Normalize distance (0.0 at center, 1.0 at edge of the defined circle)
                float t = distFromCenter / (minDimension / 2f);
                // Curve the gradient using Power function.
                // t^3 makes the "safe zone" in the center wider, then drops off sharply at edges.
                float gradient = Mathf.Pow(t, 3f);

                // Combine Noise with Gradient:
                // - Center: Noise - 0 = Original Noise (Likely Land)
                // - Edges: Noise - Large Number = Negative (Forced Water)
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
                    // Check the 4 immediate neighbors (Up, Down, Left, Right) in the OLD data
                    bool waterLeft = mapData[x - 1, y] == 0;
                    bool waterRight = mapData[x + 1, y] == 0;
                    bool waterUp = mapData[x, y + 1] == 0;
                    bool waterDown = mapData[x, y - 1] == 0;


                    // If a land tile is squeezed between water on opposite sides, it's too thin.
                    if (mapData[x, y] == 1)
                    {

                        if ((waterLeft && waterRight) || (waterUp && waterDown))
                        {
                            cleanMap[x, y] = 0;
                        }
                    }

                    // If a water tile is surrounded by mostly land, fill it in
                    else if (mapData[x, y] == 0)
                    {
                        int landNeighbors = 0;
                        if (!waterLeft) landNeighbors++;
                        if (!waterRight) landNeighbors++;
                        if (!waterUp) landNeighbors++;
                        if (!waterDown) landNeighbors++;

                        // If 3 or more neighbors are land, turn this water into land
                        if (landNeighbors >= 3)
                        {
                            cleanMap[x, y] = 1;
                        }
                    }
                }
            }

            mapData = cleanMap;
        }

        //spawn tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calculate position relative to center so (0,0) is the middle of the island
                Vector3Int pos = new Vector3Int(x - width / 2, y - height / 2, 0);

                if (mapData[x, y] == 1)
                {
                    groundTilemap.SetTile(pos, grassTile);
                    cliffTilemap.SetTile(pos, cliffTile);
                    if (decorationTilemap != null && grassDecorationTile != null)
                    {
                        // Calculate specific noise for decorations (using a different scale if desired)
                        // Adding 'seed' shifts the noise so decoration patches move when seed changes
                        float decorX = (float)x / width * decorationNoiseScale + seed;
                        float decorY = (float)y / height * decorationNoiseScale + seed;

                        float decorValue = Mathf.PerlinNoise(decorX, decorY);

                        // If noise is high enough (clump logic), spawn decoration
                        // (1 - chance) ensures that if chance is 0.2, we only spawn above 0.8
                        if (decorValue > (1f - decorationChance))
                        {
                            decorationTilemap.SetTile(pos, grassDecorationTile);
                        }

                        // --- NEIGHBOR CHECK: PREVENT COAST SPAWNING ---
                        bool nextToWater = false;

                        // Check neighbors boundaries to avoid IndexOutOfRange
                        // (x > 0 && mapData[x-1,y] == 0) means neighbor is water
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

                waterTilemap.SetTile(pos, waterTile);

            }
        }
    }

    private void SpawnNature(Vector3Int gridPos, int x, int y)
    {
        Vector3 basePos = groundTilemap.CellToWorld(gridPos) + new Vector3(.5f, .5f, 0);
        float offsetX = Random.Range(-positionJitter, positionJitter);
        float offsetY = Random.Range(-positionJitter, positionJitter);
        Vector3 spawnPos = basePos + new Vector3(offsetX, offsetY, 0);



        float roll = Random.value;
        float currentTreshold = 0f;

        currentTreshold += rockChance;
        if (roll < currentTreshold)
        {
            SpawnObject(rockPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += treeChance;
        if (roll < currentTreshold)
        {
            SpawnObject(treePrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += stumpChance;
        if (roll < currentTreshold)
        {
            SpawnObject(stumpPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += trunkChance;
        if (roll < currentTreshold)
        {
            SpawnObject(trunkPrefab, spawnPos, objectsParent);
            return;
        }



        currentTreshold += stickChance;
        if (roll < currentTreshold)
        {
            SpawnObject(stickPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += smallRockChance;
        if (roll < currentTreshold)
        {
            SpawnObject(smallRockPrefab, spawnPos, objectsParent);
            return;
        }

        currentTreshold += flowerChance;
        if (roll < currentTreshold)
        {
            SpawnObject(flowerPrefabs[Random.Range(0,3)], spawnPos, objectsParent);
            return;
        }



        float grassNoiseOffset = seed * 55.5f;
        float xPatch = (x / grassPatchScale) + grassNoiseOffset;
        float yPatch = (y / grassPatchScale) + grassNoiseOffset;

        float patchNoise = Mathf.PerlinNoise(xPatch, yPatch);

        // If below threshold, this is empty ground (Dirt/Clearing)
        if (patchNoise < grassThreshold) return;

        // Calculate ranges for the 3 types based on what's left (e.g., 0.6 to 1.0)
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

        if (grassToSpawn != null)
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
}


