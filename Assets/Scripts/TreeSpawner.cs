using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TreeSpawner : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public GameObject treePrefab;
    public int numTreesToSpawn;
    private Vector2Int spawnAreaMin;
    private Vector2Int spawnAreaMax;

    private List<Vector3> spawnedTreePositions = new List<Vector3>();
    public float minDistanceBetweenTrees;

    // 👇 NOVOS CAMPOS
    private GameObject previewTree; // árvore "fantasma" para seguir o mouse
    private bool plantingMode = false;

    void Start()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        spawnAreaMin = new Vector2Int(bounds.xMin, bounds.yMin);
        spawnAreaMax = new Vector2Int(bounds.xMax, bounds.yMax);

        SpawnTrees();

        // 👇 Ativa o modo de plantio manual ao iniciar o jogo
        StartManualPlantingMode();
    }

    void Update()
    {
        if (plantingMode)
        {
            UpdatePreviewTree();

            // Quando clicar com o botão esquerdo
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTree();
            }
        }
    }

    void StartManualPlantingMode()
    {
        plantingMode = true;

        // Cria a árvore fantasma
        previewTree = Instantiate(treePrefab);
        SpriteRenderer sr = previewTree.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f); // semitransparente para indicar pré-visualização
        }
    }

    void UpdatePreviewTree()
    {
        // Pega a posição do mouse na world
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = groundTilemap.WorldToCell(mouseWorldPos);
        Vector3 cellCenterWorldPos = groundTilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);

        previewTree.transform.position = cellCenterWorldPos;
    }

    void PlaceTree()
    {
        Vector3 position = previewTree.transform.position;
        Vector3Int cellPos = groundTilemap.WorldToCell(position);

        // Só planta se tiver tile no chão e não tiver parede e respeitar distância
        if (groundTilemap.HasTile(cellPos) && !IsNearWall(cellPos))
        {
            bool tooClose = false;
            foreach (Vector3 pos in spawnedTreePositions)
            {
                if (Vector3.Distance(pos, position) < minDistanceBetweenTrees)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                // Planta a árvore definitiva
                GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);
                SpriteRenderer sr = tree.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = 5000;
                }

                spawnedTreePositions.Add(position);

                Debug.Log("Plantou uma árvore!");
            }
            else
            {
                Debug.Log("Muito perto de outra árvore!");
            }
        }
        else
        {
            Debug.Log("Não pode plantar aqui!");
        }
    }

    void SpawnTrees()
    {
        int spawned = 0;
        int maxAttempts = numTreesToSpawn * 5;
        int attempts = 0;

        while (spawned < numTreesToSpawn && attempts < maxAttempts)
        {
            attempts++;

            int x = Random.Range(spawnAreaMin.x, spawnAreaMax.x + 1);
            int y = Random.Range(spawnAreaMin.y, spawnAreaMax.y + 1);
            Vector3Int cellPos = new Vector3Int(x, y, 0);

            if (groundTilemap.HasTile(cellPos) && !IsNearWall(cellPos))
            {
                Vector3 worldPos = groundTilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);

                bool tooClose = false;
                foreach (Vector3 pos in spawnedTreePositions)
                {
                    if (Vector3.Distance(pos, worldPos) < minDistanceBetweenTrees)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (tooClose)
                {
                    continue;
                }

                GameObject tree = Instantiate(treePrefab, worldPos, Quaternion.identity);
                SpriteRenderer sr = tree.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = 5000;
                }

                spawnedTreePositions.Add(worldPos);
                spawned++;
            }
        }

        Debug.Log($"Spawned {spawned} árvores automaticamente.");
    }

    bool IsNearWall(Vector3Int cellPos)
    {
        int margin = 2; // Ajusta a margem que quiser
        for (int dx = -margin; dx <= margin; dx++)
        {
            for (int dy = -margin; dy <= margin; dy++)
            {
                Vector3Int neighborPos = new Vector3Int(cellPos.x + dx, cellPos.y + dy, 0);
                if (wallTilemap.HasTile(neighborPos))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
