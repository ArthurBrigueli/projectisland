using UnityEngine;
using UnityEngine.Tilemaps;

public class ThreeSelectSpawner : MonoBehaviour
{
    public Tilemap groundTilemap;
    public GameObject treePrefab;

    private GameObject ghostTree;
    private float minY, maxY;

    void Start()
    {
        ghostTree = Instantiate(treePrefab);

        SpriteRenderer sr = ghostTree.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 5000;


        SpriteRenderer ghostSR = ghostTree.GetComponent<SpriteRenderer>();
        if (ghostSR != null)
            ghostSR.color = new Color(1, 1, 1, 0.5f);

        BoundsInt bounds = groundTilemap.cellBounds;
        minY = bounds.yMin;
        maxY = bounds.yMax;
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = groundTilemap.WorldToCell(mouseWorldPos);

        Vector3 cellCenterPos = groundTilemap.GetCellCenterWorld(cellPos);
        ghostTree.transform.position = new Vector3(cellCenterPos.x, cellCenterPos.y, 0);

        if (groundTilemap.HasTile(cellPos))
        {
            ghostTree.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                GameObject tree = Instantiate(treePrefab, new Vector3(cellCenterPos.x, cellCenterPos.y, 0), Quaternion.identity);

                SpriteRenderer sr = tree.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    // Quanto mais baixo o Y, maior o sortingOrder
                    float yRange = maxY - minY;
                    float yRelative = maxY - cellCenterPos.y; // Invertido!
                    int sortingOrder = (int)((yRelative / yRange) * 1000) + 2; // Começa em 2
                    sr.sortingOrder = sortingOrder;
                }
            }
        }
        else
        {
            ghostTree.SetActive(false);
        }
    }
}
