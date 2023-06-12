using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GPUInstancer;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class Controller : MonoBehaviour
{
    public static Controller instance;
    public int speed;
    float fh;
    float fv;
    float h;
    float v;
    bool isDrag = false;
    Rigidbody rigid;
    public GameObject hole;
    public static Vector3 holePos;
    int ballCount;
    public GameObject winPanel;
    public GameObject losePanel;
    public Slider levelProgress;
    public Text currentLevelText;
    public Text nextLevelText;
    public Text scoreText;
    private GameObject ballSpawn;
    public static int limitFall = 10;
    public static int limitParticle = 10;
    int currentLevel;
    int score;
    int totalScore;
    bool isStartGame = false;
    public GameObject menu;
    public GameObject red;
    public List<Vector3> listRedPos = new List<Vector3>();
    public List<Texture2D> listBackground = new List<Texture2D>();
    public GameObject ground;
    public GameObject back;
    public GameObject plusVarPrefab;
    public Canvas canvas;
    public GameObject conffeti;
    bool isVibrate = true;
    public static int colorCode;
    Vector3 fp;
    Vector3 cp;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
        instance = this;
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        totalScore = PlayerPrefs.GetInt("totalScore");
        scoreText.text = totalScore.ToString();
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
        hole = GameObject.FindGameObjectWithTag("Hole");
        rigid = GetComponent<Rigidbody>();
        holePos = new Vector3(hole.transform.position.x, 0, hole.transform.position.z);
        limitFall = 10;
        limitParticle = 5;
        int randomBg = 0;
        if(colorCode == 1)
        {
            randomBg = Random.Range(0, listBackground.Count - 1);
        }
        else if (colorCode == 2)
        {
            randomBg = Random.Range(1, listBackground.Count - 1);
        }
        else
        {
            randomBg = listBackground.Count - 1;
        }
        ground.GetComponent<MeshRenderer>().material.mainTexture = listBackground[randomBg];
        back.GetComponent<MeshRenderer>().material.mainTexture = listBackground[randomBg];

        ballCount = transform.childCount;
        ballCount--;
        levelProgress.maxValue = ballCount;
        levelProgress.value = score;
        Debug.Log(ballCount);
        //foreach (Transform child in transform)
        //{
        //    if (child.tag != "Red")
        //    {
        //        float side = Random.Range(0, 10);
        //        float posZ = 0;
        //        if (Mathf.Abs(child.transform.position.x - holePos.x) < 1)
        //        {
        //            if (side > 5)
        //            {
        //                posZ = Random.Range(2, holePos.z + 1f);
        //            }
        //            else
        //            {
        //                posZ = Random.Range(-2, holePos.z - 1f);
        //            }
        //            child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, posZ);
        //        }
        //        else
        //        {
        //            child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, Random.Range(-2, 2));
        //        }
        //        //child.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        //    }
        //}
        //Vector3 center = transform.position;
        //Vector3 pos = RandomCircle(center, 5.5f);

        if (currentLevel != 0)
        {
            var randomPick = Random.Range(0, listRedPos.Count);
            Vector3 pos = listRedPos[randomPick];
            red.transform.localPosition = pos;
        }
        else
        {
            Destroy(red);
        }
    }


    //Vector3 RandomCircle(Vector3 center, float radius)
    //{
    //    float ang = Random.value * 360;
    //    Vector3 pos;
    //    pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
    //    pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
    //    pos.z = center.z;
    //    var posCheck = new Vector3(pos.x, 0, pos.z);
    //    var checkDis = Vector3.Distance(posCheck, holePos);
    //    if(checkDis <= 1)
    //    {
    //        pos.x += 2;
    //    }
    //    return pos;
    //}

    private void FixedUpdate()
    {
        if (isStartGame)
        {
            if (Input.GetMouseButtonDown(0))
            {
                fh = Input.GetAxis("Mouse X") * speed;
                fv = Input.GetAxis("Mouse Y") * speed;
            }

            if (Input.GetMouseButton(0))
            {
                OnMouseDrag();
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
            }
        }
        if (currentLevel != 0 && red != null)
        {
            var redPos = new Vector3(red.transform.position.x, 0, red.transform.position.z);
            var distanceCheck = Vector3.Distance(redPos, holePos);
            if (distanceCheck <= 0.5f)
            {
                red.transform.parent = null;
                red.GetComponent<Rigidbody>().useGravity = true;
                holePos = new Vector3(red.transform.position.x, hole.transform.position.y - 0.5f, red.transform.position.z);
                Lose();
            }
        }
    }

    void OnMouseDrag()
    {
#if UNITY_EDITOR
        h = Input.GetAxis("Mouse X") * speed;
        v = Input.GetAxis("Mouse Y") * speed;
        if (Mathf.Abs(h - fh) > 0.25f)
        {
            isDrag = true;
        }
        if(Mathf.Abs(v - fv) > 0.25f)
        {
            isDrag = true;
        }
#endif
#if UNITY_IOS
        if (Input.touchCount > 0)
        {
            h = Input.touches[0].deltaPosition.x/8;
            v = Input.touches[0].deltaPosition.y/8;
            isDrag = true;
        }
#endif
        if (isDrag)
        {
            var rot = new Vector3(v, -h, 0);
            rigid.AddTorque(rot, ForceMode.VelocityChange);
        }
    }

    public void PlusEffect(Vector3 pos)
    {
        if (!UnityEngine.iOS.Device.generation.ToString().Contains("5") && isVibrate)
        {
            isVibrate = false;
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            StartCoroutine(delayVibrate());
            Debug.Log("Vibrate");
        }
        var plusVar = Instantiate(plusVarPrefab);
        plusVar.transform.parent = canvas.transform;
        plusVar.transform.localScale = new Vector3(1, 1, 1);
        plusVar.transform.position = worldToUISpace(canvas, pos);
        plusVar.SetActive(true);
        plusVar.transform.DOMoveY(plusVar.transform.position.y + Random.Range(50,90), 0.5f);
        Destroy(plusVar, 0.5f);
    }

    IEnumerator delayVibrate()
    {
        yield return new WaitForSeconds(0.2f);
        isVibrate = true;
    }

    public void Scoring()
    {
        ballCount--;
        score++;
        levelProgress.value = score;
        scoreText.text = (totalScore + score).ToString();
        if (ballCount <= 0)
        {
            Win();
        }
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    public void Win()
    {
        isStartGame = false;
        MMVibrationManager.Vibrate();
        StartCoroutine(delayEnd(winPanel, 0.8f));
        conffeti.SetActive(true);
        totalScore += score;
        currentLevel++;
        if (currentLevel > ReadPositionFileText.maxLevel)
        {
            currentLevel = 0;
        }
        PlayerPrefs.SetInt("totalScore", totalScore);
        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }

    public void Lose()
    {
        isStartGame = false;
        StartCoroutine(delayEnd(losePanel, 1));
    }

    IEnumerator delayEnd(GameObject panel, float time)
    {
        yield return new WaitForSeconds(time);
        Camera.main.GetComponent<BlurOptimized>().enabled = true;
        panel.SetActive(true);
    }

    public void StartGameButton()
    {
        isStartGame = true;
        Camera.main.GetComponent<BlurOptimized>().enabled = false;
        menu.SetActive(false);
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(0);
    }

    public void LimitFallChange()
    {
        limitFall--;
        StartCoroutine(delayFall());
    }

    IEnumerator delayFall()
    {
        yield return new WaitForSeconds(0.001f);
        limitFall++;
    }

    public void LimitParticleChange()
    {
        limitParticle--;
        StartCoroutine(delayParticle());
    }

    IEnumerator delayParticle()
    {
        yield return new WaitForSeconds(0.3f);
        limitParticle++;
        Debug.Log(limitParticle);
    }

    public void NextMap()
    {
        isStartGame = false;
        Camera.main.GetComponent<BlurOptimized>().enabled = true;
        winPanel.SetActive(true);
        currentLevel++;
        if (currentLevel > ReadPositionFileText.maxLevel)
        {
            currentLevel = 0;
        }
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        StartCoroutine(delayLoadScene());
    }

    IEnumerator delayLoadScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(0);
    }
}