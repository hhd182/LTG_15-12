using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(CompositeShadowCaster2D))]
public class ShadowCaster2DFromComposite : MonoBehaviour
{
    public bool interior;
    public bool castsShadows = true;
    public bool selfShadows = false;

    static readonly FieldInfo _meshField;
    static readonly FieldInfo _shapePathField;
    static readonly FieldInfo _shapePathHash;
    static readonly MethodInfo _generateShadowMeshMethod;

    ShadowCaster2D[] _shadowCasters;

    int deleted;
    Tilemap[] _tilemaps;
    CompositeCollider2D _compositeCollider;
    List<Vector2> _compositeVerts = new List<Vector2>();

    /// <summary>
    /// Using reflection to expose required properties in ShadowCaster2D
    /// </summary>
    static ShadowCaster2DFromComposite()
    {
        _meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
        _shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
        _shapePathHash = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);

        _generateShadowMeshMethod = typeof(ShadowCaster2D)
                                    .Assembly
                                    .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
                                    .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);
    }

    /// <summary>
    /// Rebuilds ShadowCaster2Ds for all ShadowCaster2DFromComposite in scene
    /// </summary>
    [MenuItem("2DLights/Rebuild Tilemap")]
    public static void RebuildAll()
    {
        foreach (var item in FindObjectsOfType<ShadowCaster2DFromComposite>())
        {
            item.Rebuild();
        }
    }

    private void OnEnable()
    {
        _tilemaps = GetComponentsInChildren<Tilemap>();
        Tilemap.tilemapTileChanged += this.RebuildOnTilePlacement;
    }

    private void OnDisable()
    {
        Tilemap.tilemapTileChanged -= this.RebuildOnTilePlacement;
    }

    private void RebuildOnTilePlacement(Tilemap arg1, Tilemap.SyncTile[] arg2)
    {
        if (_tilemaps.Contains(arg1))
            Rebuild();
    }

    private void Start()
    {
        Rebuild();
    }

    /// <summary>
    /// Rebuilds this specific ShadowCaster2DFromComposite
    /// </summary>
    private void Rebuild()
    {
        deleted = 0;
        _compositeCollider = GetComponent<CompositeCollider2D>();
        CreateShadowGameObjects();
        _shadowCasters = GetComponentsInChildren<ShadowCaster2D>();
        for (int i = 0; i < _compositeCollider.pathCount; i++)
        {
            GetCompositeVerts(i);
        }
    }

    /// <summary>
    /// Automatically creates holder gameobjects for each needed ShadowCaster2D, depending on complexity of tilemap
    /// </summary>
    private void CreateShadowGameObjects()
    {
        //Delete all old objects
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).name.Contains("ShadowCaster"))
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
        //Generate new ones
        for (int i = 0; i < _compositeCollider.pathCount; i++)
        {
            GameObject newShadowCaster = new GameObject("ShadowCaster");
            newShadowCaster.transform.parent = transform;
            newShadowCaster.AddComponent<ShadowCaster2D>();
        }
    }

    /// <summary>
    /// Gathers all the verts of a given path shape in a CompositeCollider2D
    /// </summary>
    /// <param name="path">The path index to fetch verts from</param>
    private void GetCompositeVerts(int path)
    {
        _compositeVerts = new List<Vector2>();
        Vector2[] pathVerts = new Vector2[_compositeCollider.GetPathPointCount(path)];
        Vector2[] oldPathVerts;

        _compositeCollider.GetPath(path, pathVerts);
        _compositeVerts.AddRange(pathVerts);

        //THIS IS EXTREMELY BUSTED AND I CANT BE ASSED TO FIX IT
        //JUST DONT CREATE HOLLOW SPACES IN YOUR TILEMAP
        //IF YOU MUST CREATE HOLLOW SPACES, 'CHEAT' BY CREATING 2 TILEMAPS
        /*
        if (path > 0)
        {
            GameObject[] casterChildren = GetComponentsInChildren<Transform>()
                .Where(child => child.name.Contains("ShadowCaster"))
                .Select(g => g.gameObject).ToArray();

            var cc = casterChildren[path - 1 - deleted].GetComponent<ShadowCaster2D>();
            oldPathVerts = ConvertArray((Vector3[])_shapePathField.GetValue(cc));
            _compositeCollider.GetPath(path - 1, oldPathVerts);

            if (InsideOtherShape(oldPathVerts, pathVerts))
            {
                DestroyImmediate(casterChildren[path - 1 - deleted]);
                deleted++;

                int placeToAdd = System.Array.IndexOf(pathVerts, pathVerts.OrderBy(v => Vector2.Distance(v, oldPathVerts[0])).FirstOrDefault());
                _compositeVerts.InsertRange(placeToAdd, oldPathVerts);
                _compositeVerts.Insert(placeToAdd + oldPathVerts.Count(), _compositeVerts[1]);
                _compositeVerts.Insert(1, _compositeVerts[placeToAdd +oldPathVerts.Count() + 1]);
            }
        }
        */

        UpdateCompositeShadow(_shadowCasters[path]);
    }

    /// <summary>
    /// Sets the verts of each ShadowCaster2D to match their corresponding
    /// verts in CompositeCollider2D and then generates the mesh
    /// </summary>
    /// <param name="caster"></param>
    private void UpdateCompositeShadow(ShadowCaster2D caster)
    {
        caster.castsShadows = castsShadows;
        caster.selfShadows = selfShadows;

        Vector2[] points = _compositeVerts.ToArray();
        var threes = ConvertArray(points);

        _shapePathField.SetValue(caster, threes);
        _meshField.SetValue(caster, new Mesh());

        int hash = GetShapePathHash(caster.shapePath);
        _shapePathHash.SetValue(caster, hash);

        _generateShadowMeshMethod.Invoke(caster,
            new object[] { _meshField.GetValue(caster),
                _shapePathField.GetValue(caster) });
    }

    private bool InsideOtherShape(Vector2[] outside, Vector2[] inside)
    {
        Vector2[] outsideLeftToRight = outside.OrderBy(l => l.x).ToArray();
        Vector2[] outsideTopToBottom = outside.OrderBy(l => l.y).ToArray();
        Vector2[] insideLeftToRight = inside.OrderBy(l => l.x).ToArray();
        Vector2[] insideTopToBottom = inside.OrderBy(l => l.x).ToArray();

        Debug.Log($"{outsideLeftToRight[0].x}, {outsideTopToBottom[0].y}");
        Debug.Log($"{outsideLeftToRight[outside.Length - 1].x}, {outsideTopToBottom[outside.Length - 1].y}");

        Debug.Log($"{insideLeftToRight[0].x}, {insideTopToBottom[0].y}");
        Debug.Log($"{insideLeftToRight[inside.Length - 1].x}, {insideTopToBottom[inside.Length - 1].y}");

        if (outsideLeftToRight[0].x < insideLeftToRight[0].x
            && outsideLeftToRight[outside.Length - 1].x > insideLeftToRight[inside.Length - 1].x
            && outsideTopToBottom[0].y < insideTopToBottom[0].y
            && outsideTopToBottom[outside.Length - 1].y > insideTopToBottom[inside.Length - 1].y)
        {
            return true;
        }

        return false;
    }

    //Quick method for converting a Vector2 array into a Vector3 array
    Vector3[] ConvertArray(Vector2[] v2)
    {
        Vector3[] v3 = new Vector3[v2.Length];
        for (int i = 0; i < v3.Length; i++)
        {
            Vector2 tempV2 = v2[i];
            v3[i] = new Vector3(tempV2.x, tempV2.y);
        }
        return v3;
    }

    //Quick method for converting a Vector3 array into a Vector2 array
    Vector2[] ConvertArray(Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v2.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }

    public static int GetShapePathHash(Vector3[] path)
    {
        unchecked
        {
            int hashCode = (int)2166136261;

            if (path != null)
            {
                foreach (var point in path)
                    hashCode = hashCode * 16777619 ^ point.GetHashCode();
            }
            else
            {
                hashCode = 0;
            }

            return hashCode;
        }
    }

}