using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

public class WorldGeneratorTests
{
    // --- WORLD GENERATOR TESTS ---

    [Test]
    public void WorldGenerator_NotifyObjectRemoved_AddsCoordinateToRemovedSet()
    {
        // Purpose: Verify that the WorldGenerator correctly registers removed object coordinates.
        // This simulates a scenario where an object (like a tree) tells the generator it has been destroyed.

        // 1. Setup Environment (Grid & Tilemap are required for WorldToCell calculations)
        GameObject gridGO = new GameObject("Test_Grid");
        gridGO.AddComponent<Grid>();

        GameObject tmGO = new GameObject("Test_Tilemap");
        tmGO.transform.SetParent(gridGO.transform);
        Tilemap tilemap = tmGO.AddComponent<Tilemap>();

        // 2. Setup WorldGenerator
        GameObject genGO = new GameObject("Test_Generator");
        WorldGenerator worldGen = genGO.AddComponent<WorldGenerator>();
        // Manually assign the tilemap reference (normally done in Inspector)
        worldGen.groundTilemap = tilemap;

        // 3. Define Test Data
        // Position (10.5, 5.5) corresponds to cell (10, 5) on a standard grid
        Vector3 testWorldPosition = new Vector3(10.5f, 5.5f, 0);
        Vector3Int expectedCell = tilemap.WorldToCell(testWorldPosition);

        // 4. Act: Directly call the notification method
        // Since we don't have a WorldObject script, we invoke the generator's method directly
        worldGen.NotifyObjectRemoved(testWorldPosition);

        // 5. Assert: Check internal state via Reflection
        // We verify that the private HashSet _removedNatureCoordinates now contains the cell.
        var removedField = typeof(WorldGenerator).GetField("_removedNatureCoordinates", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(removedField, "Reflection Error: Field '_removedNatureCoordinates' not found in WorldGenerator.");

        var removedSet = removedField.GetValue(worldGen) as HashSet<Vector2Int>;
        Assert.IsNotNull(removedSet, "Logic Error: _removedNatureCoordinates HashSet is null.");

        bool wasRemoved = removedSet.Contains((Vector2Int)expectedCell);
        Assert.IsTrue(wasRemoved, $"WorldGenerator should mark cell {expectedCell} as removed after notification.");

        // Cleanup
        Object.DestroyImmediate(genGO);
        Object.DestroyImmediate(tmGO);
        Object.DestroyImmediate(gridGO);
    }
}