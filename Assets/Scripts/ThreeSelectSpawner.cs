using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class ThreeSelectSpawner : MonoBehaviour
{
    public Tilemap groundTilemap;
    public GameObject treePrefab;
    private bool IsTreeSelect = true;
    public int countTreeSpawn;

    public TextMeshProUGUI pointTreeText;

    private GameObject ghostTree;

    public LayerMask wallLayer; // Layer da parede (Tilemap)

    public TreeManager treeManager;

    public GameObject ghostTreePrefab;

    void Start()
    {
        countTreeSpawn = 1;

        if (IsTreeSelect)
        {
            ghostTree = Instantiate(ghostTreePrefab);
            SpriteRenderer ghostSR = ghostTree.GetComponent<SpriteRenderer>();
            if (ghostSR != null)
                ghostSR.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    void Update()
    {
        pointTreeText.text = countTreeSpawn.ToString();

        if (OpenShop.isShopOpen)
        {
            if (ghostTree != null)
                ghostTree.SetActive(false);

            return;
        }

        if (countTreeSpawn > 0)
        {
            SpawnTree();
        }
        else if (ghostTree != null)
        {
            ghostTree.SetActive(false);
        }
    }


    void SpawnTree()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = groundTilemap.WorldToCell(mouseWorldPos);
        Vector3 cellCenterPos = groundTilemap.GetCellCenterWorld(cellPos);

        if (groundTilemap.HasTile(cellPos))
        {
            ghostTree.transform.position = cellCenterPos;
            ghostTree.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                BoxCollider2D box = treePrefab.GetComponent<BoxCollider2D>();
                if (box == null)
                {
                    Debug.LogError("O prefab precisa de BoxCollider2D!");
                    return;
                }

                Vector2 boxSize = box.size;
                Vector2 boxOffset = box.offset;
                Vector2 checkPos = (Vector2)cellCenterPos + boxOffset;

                Collider2D hit = Physics2D.OverlapBox(checkPos, boxSize, 0f, wallLayer);

                if (hit != null)
                {
                    Debug.Log("Tem parede no local! Não pode plantar.");
                }
                else
                {
                    Debug.Log("Local livre! Plantando árvore.");
                    // Instancia a árvore
                    GameObject tree = Instantiate(treePrefab, cellCenterPos, Quaternion.identity);
                    // Registra no TreeManager para começar a gerar moedas
                    treeManager.RegisterTree(tree);

                    countTreeSpawn--;
                }
            }
        }
        else
        {
            ghostTree.SetActive(false);
        }
    }
}
