using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
    [Header("Templates")]
    public List<TerrainTemplateController> terrainTemplates;
    public float terrainTemplateWidth;

    [Header("Generator Area")]
    public Camera gameCamera;
    public float areaStartOffset;
    public float areaEndOffset;

    private const float debugLineHeight = 10f;
    List<GameObject> spawnedTerrain;
    float lastGeneratedPosX;

    [Header("Force Early Template")]
    public List<TerrainTemplateController> earlyTerrainTemplates;

    float lastRemovePosX;

    private Dictionary<string, List<GameObject>>pool;


    private void Start() 
    {
        pool = new Dictionary<string, List<GameObject>>();

        spawnedTerrain = new List<GameObject>();
        lastGeneratedPosX = GetHorizontalPositionStart();
        lastRemovePosX = lastGeneratedPosX - terrainTemplateWidth;
    

        foreach(TerrainTemplateController terrain in earlyTerrainTemplates)
        {
            EarlyGenerateTerrain(lastGeneratedPosX, terrain);
            lastGeneratedPosX += terrainTemplateWidth;
        }

        while (lastGeneratedPosX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPosX);
            lastGeneratedPosX += terrainTemplateWidth;
        }
    }

    private GameObject GenerateFromPool(GameObject item, Transform parent)
    {
        if(pool.ContainsKey(item.name))
        {
            if(pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else
        {
            pool.Add(item.name, new List<GameObject>());
        }

        GameObject newItem = Instantiate(item,parent);
        newItem.name = item.name;
        return newItem;
    }

    private void ReturnToPool(GameObject item)
    {
        if(!pool.ContainsKey(item.name))
        {
            Debug.LogError("Invalid Pool Item");
        }

        pool[item.name].Add(item);
        item.SetActive(false);
    }


    private void Update() 
    {
        while (lastGeneratedPosX < GetHorizontalPositionEnd())
        {   
            GenerateTerrain(lastGeneratedPosX);
            lastGeneratedPosX += terrainTemplateWidth; 
        }

        while(lastRemovePosX + terrainTemplateWidth < GetHorizontalPositionStart())
        {
            lastRemovePosX += terrainTemplateWidth;
            RemoveTerrain(lastRemovePosX);
        }
    }

    void RemoveTerrain(float posX)
    {
        GameObject terrainToRemove = null;

        foreach(GameObject item in spawnedTerrain)
        {
            if(item.transform.position.x == posX)
            {
                terrainToRemove = item;
                break;
            }
        }

        if(terrainToRemove != null)
        {
            spawnedTerrain.Remove(terrainToRemove);
            Destroy(terrainToRemove);
        }
    }

    private void GenerateTerrain(float posX, TerrainTemplateController forceterrain = null)
    {
        GameObject newTerrain = Instantiate(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);
        newTerrain.transform.position = new Vector2(posX, 0f);
        spawnedTerrain.Add(newTerrain);
    }

    private void EarlyGenerateTerrain(float posX, TerrainTemplateController forceterrain = null)
    {
        GameObject newTerrain = Instantiate(terrainTemplates[0].gameObject, transform);
        newTerrain.transform.position = new Vector2(posX, 0f);
        spawnedTerrain.Add(newTerrain);
    }


    private float GetHorizontalPositionStart()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(0f,0f)).x + areaStartOffset;
    }

    private float GetHorizontalPositionEnd()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f,0f)).x + areaEndOffset;
    }

    private void OnDrawGizmos() 
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalPositionEnd();

        Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight / 2, Color.red);
        Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight / 2, Color.red);
    }


}
