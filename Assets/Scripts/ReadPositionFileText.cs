using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using GPUInstancer;

public class ReadPositionFileText : MonoBehaviour
{
    //public TextAsset textAsset;
    public static ReadPositionFileText instance;
    public List<GameObject> mapAsset = new List<GameObject>();
    //public List<PositionObject> listPosition = new List<PositionObject>();
    public GameObject prefabPixel;
    public GameObject parentObject;
    public float bound;
    GameObject hole;
    Vector3 holePos;
    public static int maxLevel;
    //public float convert;

    //[System.Serializable]
    //public class PositionObject
    //{
    //    public Color color;
    //    public Vector3 position;
    //}

    //private string[] levelTextArray;

    private void OnEnable()
    {
        maxLevel = mapAsset.Count - 1;
        hole = GameObject.FindGameObjectWithTag("Hole");
        holePos = new Vector3(hole.transform.position.x, 0, hole.transform.position.z);
        //try
        //{
        //    ReadLevelText(textAsset);
        //}
        //catch { }
        //convert = 100;
        var currentLevel = PlayerPrefs.GetInt("currentLevel");
        //if(currentLevel > maxLevel)
        //{
        //    currentLevel = 0;
        //    PlayerPrefs.SetInt("currentLevel", currentLevel);
        //}
        GameObject currentMap = mapAsset[currentLevel];
        Controller.colorCode = int.Parse(currentMap.transform.GetChild(0).GetComponent<Renderer>().sharedMaterials[0].name.ToString());
        var totalPixel = currentMap.transform.childCount;
        foreach (Transform child in currentMap.transform)
        {
            var color = child.GetComponent<Renderer>().sharedMaterial.color;
            if (totalPixel > 800)
            {
                var spawnPixel = Instantiate(prefabPixel, child.position, Quaternion.identity);
                spawnPixel.transform.parent = parentObject.transform;
                var width = child.GetComponent<MeshRenderer>().bounds.extents.x * 2;
                spawnPixel.transform.localScale = Vector3.one * width * 0.9f; 
                spawnPixel.GetComponent<Renderer>().material.color = color;
                spawnPixel.GetComponent<Ball>().ballColor = color;
            }
            else
            {

                //float side = Random.Range(0, 10);
                //float posZ = 0;
                //if (Mathf.Abs(child.transform.position.x - holePos.x) < 1)
                //{
                //    if (side > 5)
                //    {
                //        posZ = Random.Range(3f, holePos.z + 1.5f);
                //    }
                //    else
                //    {
                //        posZ = Random.Range(-3f, holePos.z - 1.5f);
                //    }
                //    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, posZ);
                //}
                //else
                //{
                //    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, Random.Range(-3f, 3f));
                //}
                int density = 1;
                var width = child.GetComponent<MeshRenderer>().bounds.extents.x * 2;
                //Debug.Log(width);
                var originValue = /*currentMap.transform.localScale.x **/ width;
                //Debug.Log(originValue);
                var scale = originValue / 4;
                for (float x = -scale; x <= scale; x += scale * 2)
                {
                    for (float y = -scale; y <= scale; y += scale * 2)
                    {
                        var spawnPixelMini = Instantiate(prefabPixel, new Vector3(child.position.x + x, child.position.y + y, child.position.z), Quaternion.identity);
                        var size = scale + (width / 5);
                        spawnPixelMini.transform.localScale = Vector3.one * size;
                        spawnPixelMini.transform.parent = parentObject.transform;
                        spawnPixelMini.GetComponent<Renderer>().material.color = color;
                        spawnPixelMini.GetComponent<Ball>().ballColor = color;
                    }
                }
            }
        }
    }

    //public void ReadLevelText(TextAsset textAsset)
    //{
    //    listPosition.Clear();
    //    levelTextArray = textAsset.text.Split('\n');

    //    for (int i = 0; i < levelTextArray.Length; i++)
    //    {
    //        levelTextArray[i] = Regex.Replace(levelTextArray[i], "cm", "");
    //        levelTextArray[i] = Regex.Replace(levelTextArray[i], @"\s", ",");
    //        string[] textFixed = levelTextArray[i].Split(',');

    //        if (i >= 1)
    //            InitPosition(textFixed);
    //    }
    //}

    //private void InitPosition(string[] textFixed)
    //{
    //    PositionObject positionObject = new PositionObject();

    //    positionObject.ID = int.Parse(textFixed[0]);
    //    positionObject.position.x = int.Parse(textFixed[1]);
    //    positionObject.position.y = int.Parse(textFixed[3]);
    //    positionObject.position.z = int.Parse(textFixed[5]);
    //    positionObject.position /= 100;
    //    var spawnPixel = Instantiate(prefabPixel, positionObject.position, Quaternion.identity);
    //    spawnPixel.transform.parent = parentObject.transform;

    //    listPosition.Add(positionObject);
    //}

}