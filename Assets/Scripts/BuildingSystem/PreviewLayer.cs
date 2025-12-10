using UnityEngine;

public class PreviewLayer : TilemapLayer
{
    [SerializeField] private SpriteRenderer _previewRenderer;

    public void ShowPreview(BuildableItemSO item, Vector3 worldCoords, bool isValid)
    {
        var coords = _tilemap.WorldToCell(worldCoords);
        if (item.GameObject != null)
            _previewRenderer.transform.localScale = Vector3.one;

        else
            _previewRenderer.transform.localScale = new Vector3(0.5f, .5f, .5f);

        _previewRenderer.enabled = true;
        _previewRenderer.transform.position = _tilemap.CellToWorld(coords) + _tilemap.cellSize / 4 + item.tileOffset / 2;

        _previewRenderer.sprite = item.previewSprite;
        _previewRenderer.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
    }

    public void ClearPreview()
    {
        _previewRenderer.enabled = false;
    }
}
