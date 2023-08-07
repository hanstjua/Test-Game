using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Map : MonoBehaviour
{
    private Vector3 _offset;
    private Vector3 _scale;
    private List<GameObject> _highlights = new();

    public Vector3 Offset { get => _offset; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Position ToDomainPosition(Vector3 position)
    {
        var coords = position - _offset;
        
        return new Position((int) (coords.x / _scale.x), (int) (coords.z / _scale.z), (int) (coords.y / _scale.y));
    }

    public Vector3 ToUIPosition(Position position)
    {
        var coords = new Vector3(position.X * _scale.x, position.Z * _scale.y, position.Y * _scale.z);

        return coords + _offset;
    }

    public void CalculateOffset(GameObject mapObject)
    {
        var cellSize = mapObject.GetComponentInChildren<Block>().GetComponent<BoxCollider>().size;
        _offset = new Vector3(cellSize.x / 2, 0, cellSize.z / 2);  // Tilemap uses XZY convention
    }

    public void CalculateScale(GameObject mapObject)
    {
        var cellSize = mapObject.GetComponentInChildren<Block>().GetComponent<BoxCollider>().size;
        _scale = new Vector3(cellSize.x, cellSize.y, cellSize.z);
    }

    public void Highlight(Vector3 position)
    {
        var highlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
        highlight.transform.position = position + new Vector3(0, 0.2f, 0);
        highlight.transform.rotation *= Quaternion.Euler(90, 0, 0);

        var material = highlight.GetComponent<MeshRenderer>().material;
        material.color = new Color(material.color.r + 1, material.color.g, material.color.b - 1, 100);

        _highlights.Add(highlight);
    }

    public void Unhighlight(Vector3 position)
    {
        var toRemove = _highlights.Find(h => h.transform.position.x == position.x && h.transform.position.z == position.z);
        if (toRemove != null)
        {
            _highlights.Remove(toRemove);
            GameObject.Destroy(toRemove);
        }
    }

    public void ClearHighlights()
    {
        foreach (var h in _highlights)
        {
            GameObject.Destroy(h);
        }

        _highlights.Clear();
    }
}
