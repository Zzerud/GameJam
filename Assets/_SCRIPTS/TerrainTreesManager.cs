using UnityEngine;

public class TerrainTreesManager : MonoBehaviour
{
    [SerializeField] private GameObject colliderPrefab, collidersParents;
    private void Start()
    {
        Terrain[] terrains = FindObjectsOfType<Terrain>();

        for (int i = 0; i < terrains.Length; i++)
        {
            TreeInstance[] trees = terrains[i].terrainData.treeInstances;
            foreach (var tree in trees)
            {
                Vector3 pos = Vector3.Scale(tree.position, terrains[i].terrainData.size) + terrains[i].transform.position;
                GameObject col = Instantiate(colliderPrefab, pos, Quaternion.identity);
                col.transform.SetParent(collidersParents.transform);
            }
        }
    }
}
