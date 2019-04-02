using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerScript : MonoBehaviour
{

    private Vector3 direction;
    public GameObject Particle;
    public GameObject DestroyDimond;
    public GameObject DimondBonusText;

    public GameObject[] UITexts;
    private Text[] ScoreTexts;

    public GameObject Camera;
    public Material Floor;

    private bool isDead;
    private bool canRetry;
    private bool floorColorChange;

    private int newFloorColor;
    private int score;

    public float speed;
    private float targetX;
    private float targetY;
    private float ratioScale;
    private float targetScale;

    string[] htmlValue = {
        "#FF6666", "#FFB266", "#FFFF66", "#B2FF66", "#66FF66", "#66FFB2",
        "#66FFFF", "#66B2FF", "#6666FF", "#B266FF", "#FF66FF", "#FF66B2"};


    void Start()
    {

        targetX = (float)Screen.width / 1125f;
        targetY = (float)Screen.height / 2436f;
        targetScale = Math.Min(targetX, targetY);

        UITexts[0].gameObject.transform.position = new Vector3(562.5f * targetX + 400f * targetX, 1218f * targetY + 1120f * targetY, 0);
        UITexts[1].gameObject.transform.position = new Vector3((float)Screen.width * 2, 1218f * targetY + 600f * targetY, 0);
        UITexts[2].gameObject.transform.position = new Vector3((float)Screen.width * 2, 1218f * targetY + 400f * targetY, 0);
        UITexts[3].gameObject.transform.position = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);
        UITexts[4].gameObject.transform.position = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        ScoreTexts = new Text[UITexts.Length];
        int i = 0;
        foreach (GameObject UItext in UITexts)
        {
            UItext.transform.localScale = new Vector3(1f * targetScale, 1f * targetScale, 1f);
            ScoreTexts[i] = UItext.gameObject.GetComponent<Text>();
            i++;
        }

        score = 0;
        isDead = false;
        canRetry = false;
        floorColorChange = false;
        direction = Vector3.zero;

        if (ColorUtility.TryParseHtmlString(htmlValue[7], out Color newCol))
            Floor.color = newCol;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            UITexts[4].SetActive(false);
            UITexts[0].SetActive(true);

            ScoreTexts[0].text = score.ToString();

            if (score != 0 && score % 25 == 0)
            {
                floorColorChange = true;
                newFloorColor = UnityEngine.Random.Range(0, htmlValue.Length);
            }

            score++;
            if (direction == Vector3.forward)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.forward;
            }
        }
        else if (Input.GetMouseButtonDown(0) && canRetry)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        if (!isDead)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, 
                new Vector3(Camera.transform.position.x, Camera.transform.position.y, transform.position.z - 15f), 
                0.7f * Time.deltaTime);
        }
        else
        {
            UITexts[1].gameObject.transform.position = Vector3.Lerp(
                UITexts[1].gameObject.transform.position,
                new Vector3((float)Screen.width / 2, UITexts[1].gameObject.transform.position.y, 0),
                6f * Time.deltaTime);

           UITexts[2].gameObject.transform.position = Vector3.Lerp(
               UITexts[2].gameObject.transform.position,
               new Vector3((float)Screen.width / 2, UITexts[2].gameObject.transform.position.y, 0),
               3f * Time.deltaTime);

            direction = Vector3.Lerp(direction, Vector3.down * 2f, 3f * Time.deltaTime);
        }

        if (floorColorChange)
        {
            if (ColorUtility.TryParseHtmlString(htmlValue[newFloorColor], out Color newCol))
                Floor.color = Color.Lerp(Floor.color, newCol, Time.deltaTime);

            if (Floor.color == newCol)
            {
                floorColorChange = false;
            }
        }

        transform.Translate(direction * speed * Time.deltaTime);
    }


    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Dimond")
        {
            other.gameObject.SetActive(false);
            Instantiate(Particle, new Vector3(transform.position.x, 3f, transform.position.z), new Quaternion(-0.7f, 0, 0, -0.7f));
            Instantiate(DestroyDimond, transform.position, Quaternion.identity);
            Instantiate(DimondBonusText, transform.position, Quaternion.identity);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
        {   
            Ray downRay = new Ray(transform.position, -Vector3.up);
            if (!Physics.Raycast(downRay, out RaycastHit hit) && !isDead)
            {
                isDead = true;
                UITexts[1].SetActive(true);
                UITexts[2].SetActive(true);     
                UITexts[0].SetActive(false);
                if (!PlayerPrefs.HasKey("BestScore") || score > PlayerPrefs.GetInt("BestScore"))
                {
                    PlayerPrefs.SetInt("BestScore", score);
                }
                ScoreTexts[1].text = "Score: " + score.ToString();
                ScoreTexts[2].text = "Best Score: " + PlayerPrefs.GetInt("BestScore").ToString();
                Invoke("Retry", 1.5f);
            }
        }
    }
    void Retry()
    {
        UITexts[3].SetActive(true);
        canRetry = true;
    }
}
