using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GPUInstancer;

public class LevelGenerator : MonoBehaviour {

    public List<Texture2D> list2DMaps = new List<Texture2D>();
	public Texture2D map;
    public Tile tilePrefab;
    public GameObject parentObject;
    Transform currentParent;
    Vector3 originalPos;

    void OnEnable()
    {
        originalPos = parentObject.transform.position;
        //var bound = map.width / 2;
        //    //for (int i = -bound; i < bound; i++)
        //    //{
        //    //    var tileParentCopy = Instantiate(tileParent);
        //    //    tileParentCopy.transform.parent = tileParent.parent;
        //    //    tileParentCopy.transform.position = new Vector3(tileParentCopy.transform.position.x, tileParentCopy.transform.position.y, tileParent.position.z + (i * ((float)8 / (float)map.width)));
        //    //    currentParent = tileParentCopy;
        //    //    var posZ = tileParentCopy.transform.position.z;
        //    //    GenerateMap(map);
        //    //    tileParentCopy.transform.position = new Vector3(tileParentCopy.transform.position.x, tileParentCopy.transform.position.y, posZ);
        //    //}
        currentParent = parentObject.transform;
        GenerateMap(map);
        parentObject.transform.position = originalPos;
    }

    private void Start()
    {
        
    }

    private void GenerateMap(Texture2D texture)
    {
        float ratioX = (float)8 / (float)texture.width;
        float ratioY = ratioX;

        Vector3 positionTileParent = new Vector3(-((texture.width - 1) * ratioX / 2), -((texture.height - 1) * ratioY / 2), 0);
        currentParent.localPosition = positionTileParent;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                GenerateTile(texture, x, y, ratioX);
            }
        }
        //AddRemoveInstances.instance.RefreshColor();
    }

    private void GenerateTile(Texture2D texture, int x, int y, float ratio)
    {
        Color pixelColor = texture.GetPixel(x, y);

        if (pixelColor.a == 0) return;

        Vector2 pos = new Vector2(x - texture.width/2, y - texture.width / 2) * ratio;
        Vector3 scale = Vector3.one * ratio;

        Tile instance;
        instance = Instantiate(tilePrefab);
        instance.transform.SetParent(currentParent);

        instance.Init();
        instance.SetTransfrom(pos, scale);
        instance.SetColor(pixelColor);
    }

}
