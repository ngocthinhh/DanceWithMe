using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField]
    private GameObject slider;
    [SerializeField]
    private float speedTime;
    private float value = 0f;

    [Header("Arrow Bar")]
    [SerializeField]
    private GameObject arrowUpPrefab;
    [SerializeField]
    private GameObject arrowDownPrefab;
    [SerializeField]
    private GameObject arrowLeftPrefab;
    [SerializeField]
    private GameObject arrowRightPrefab;
    [SerializeField]
    private GameObject containArrow;
    [SerializeField]
    private int maxNumOfArrow;
    private List<int> listArrowCurrent = new List<int>();

    [Header("Arrow Result Bar")]
    [SerializeField]
    private GameObject arrowUpResultPrefab;
    [SerializeField]
    private GameObject arrowDownResultPrefab;
    [SerializeField]
    private GameObject arrowLeftResultPrefab;
    [SerializeField]
    private GameObject arrowRightResultPrefab;
    [SerializeField]
    private GameObject containArrowResult;

    [Header("Result")]
    [SerializeField]
    private int indexArrowResult = 0;
    [SerializeField]
    private bool isFinish = false;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int scoreInt = 0;
    [SerializeField]
    private bool canCheck = true;
    [SerializeField]
    private GameObject character;

    [Header("Sound")]
    [SerializeField]
    private List<AudioSource> audioSources;
    [SerializeField]
    private bool isSetSound = true;
    [SerializeField]
    private GameObject soundObject;

    [Header("Tutorial")]
    [SerializeField]
    private GameObject tutorialObject;

    private void Awake()
    {
        GetListSoundFromSoundObject();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isSetSound)
        {
            HandleSound();
        }

        HandleTimeWithSlider();
        if (canCheck)
        {
            HandleResult();
            HandleArrow();
        }
    }

    // Called to take list sound in Sound Object
    private void GetListSoundFromSoundObject()
    {
        audioSources = new List<AudioSource>();

        for (int i = 0; i < soundObject.transform.childCount; i++)
        {
            audioSources.Add(soundObject.transform.GetChild(i).gameObject.GetComponent<AudioSource>());
        }
    }

    // Called when play (Press spacebar in yellow line or red line)
    private void HideTutorial()
    {
        tutorialObject.GetComponent<Animator>().Play("Hide");
    }

    // Called when first or lose
    private void ShowTutorial()
    {
        tutorialObject.GetComponent<Animator>().Play("Show");
    }

    // Called when first or lose
    private void ResetScore()
    {
        scoreInt = 0;
        scoreText.text = scoreInt.ToString();
    }

    // Called when won every round
    private void InscreaseScore(int scorePlus)
    {
        scoreInt += scorePlus;
        scoreText.text = scoreInt.ToString();
    }

    // Called to play random sound in game
    private void HandleSound()
    {
        isSetSound = false;

        int randomNumber = Random.Range(0, audioSources.Count);

        audioSources[randomNumber].Play();

        StartCoroutine(RandomSoundAfterSecond(audioSources[randomNumber].clip.length));
    }

    IEnumerator RandomSoundAfterSecond(float timeSound)
    {
        yield return new WaitForSeconds(timeSound);

        isSetSound = true;
    }

    // Called to controll character dancing
    private void HandleDancingOfCharacter(bool isDance)
    {
        if (isDance)
        {
            character.GetComponent<Animator>().SetTrigger("NextDance");
        }
        else if (!isDance)
        {
            if (!maxNumOfArrow.Equals(0))
            {
                character.GetComponent<Animator>().SetTrigger("Fall");
                character.GetComponent<Animator>().ResetTrigger("NextDance");
            }
        }
    }

    // Called to run slider (it's like time in game)
    private void HandleTimeWithSlider()
    {
        if (value >= 10f)
        {
            canCheck = true;
            if (!isFinish)
            {
                HandleDancingOfCharacter(false);
                maxNumOfArrow = 0;
                ShowTutorial();
            }
            else if (isFinish && maxNumOfArrow < 9f)
            {
                maxNumOfArrow += 1;
            }
            else if (isFinish)
            {

            }

            DestroyArrow();
            ProduceArrow();

            value = 0f;
            isFinish = false;
        }
        value += Time.deltaTime * speedTime;
        slider.GetComponent<Slider>().value = value;
    }

    // Called when you press spacebar
    private void HandleResult()
    {
        if (value >= 8.4f && value <= 8.6f
            && Input.GetKeyDown(KeyCode.Space)
            && indexArrowResult == maxNumOfArrow)
        {
            SendText("Fantastic");
            isFinish = true;
            canCheck = false;
            HandleDancingOfCharacter(true);
            if (maxNumOfArrow.Equals(0))
            {
                ResetScore();
                HideTutorial();
            }
            else
            {
                InscreaseScore(3);
            }
        }
        else if (value >= 8f && value <= 9f
            && Input.GetKeyDown(KeyCode.Space)
            && indexArrowResult == maxNumOfArrow)
        {
            SendText("Great");
            isFinish = true;
            canCheck = false;
            HandleDancingOfCharacter(true);
            if (maxNumOfArrow.Equals(0))
            {
                ResetScore();
                HideTutorial();
            }
            else
            {
                InscreaseScore(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SendText("Fall");
            BackUpArrow();
            indexArrowResult = 0;
        }
    }

    // Print congratulation or bad result for you
    private void SendText(string text)
    {
        resultText.text = text;

        switch (text)
        {
            case "Fall":
                resultText.color = Color.yellow;
                break;
            case "Great":
                resultText.color = Color.green;
                break;
            case "Fantastic":
                resultText.color = Color.cyan;
                break;
        }

        resultText.GetComponent<Animator>().Play("Show");   
    }

    // Called when you press any arrow key
    private void HandleArrow()
    {
        if (indexArrowResult < maxNumOfArrow)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && listArrowCurrent[indexArrowResult].Equals(1) ||
            Input.GetKeyDown(KeyCode.DownArrow) && listArrowCurrent[indexArrowResult].Equals(2) ||
            Input.GetKeyDown(KeyCode.LeftArrow) && listArrowCurrent[indexArrowResult].Equals(3) ||
            Input.GetKeyDown(KeyCode.RightArrow) && listArrowCurrent[indexArrowResult].Equals(4))
            {
                containArrowResult.transform.GetChild(indexArrowResult).gameObject.GetComponent<Image>().enabled = true;
                indexArrowResult += 1;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && !listArrowCurrent[indexArrowResult].Equals(1) ||
                Input.GetKeyDown(KeyCode.DownArrow) && !listArrowCurrent[indexArrowResult].Equals(2) ||
                Input.GetKeyDown(KeyCode.LeftArrow) && !listArrowCurrent[indexArrowResult].Equals(3) ||
                Input.GetKeyDown(KeyCode.RightArrow) && !listArrowCurrent[indexArrowResult].Equals(4))
            {
                for (int i = 0; i < indexArrowResult; i++)
                {
                    containArrowResult.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
                }
                indexArrowResult = 0;
            }
        }
    }

    // Called to create arrows by maxNumOfArrow (Count Of Arrow)
    public void ProduceArrow()
    {
        listArrowCurrent.Clear();
        for (int i = 0; i < maxNumOfArrow; i++)
        {
            int randomNumber = Random.Range(1, 5);
            listArrowCurrent.Add(randomNumber);
            TransformNumberToArrow(randomNumber);
            TransformNumberToArrowResult(randomNumber);
        }
    }

    // Called to transform number to arrow
    private void TransformNumberToArrow(int number)
    {
        switch (number)
        {
            case 1:
                Instantiate(arrowUpPrefab, containArrow.transform);
                break;
            case 2:
                Instantiate(arrowDownPrefab, containArrow.transform);
                break;
            case 3:
                Instantiate(arrowLeftPrefab, containArrow.transform);
                break;
            case 4:
                Instantiate(arrowRightPrefab, containArrow.transform);
                break;
        }
    }

    // Called to transform number to arrow but invisible (for result arrow - blue arrow)
    private void TransformNumberToArrowResult(int number)
    {
        GameObject arrow;
        switch (number)
        {
            case 1:
                arrow = Instantiate(arrowUpResultPrefab, containArrowResult.transform);
                arrow.GetComponent<Image>().enabled = false;
                break;
            case 2:
                arrow = Instantiate(arrowDownResultPrefab, containArrowResult.transform);
                arrow.GetComponent<Image>().enabled = false;
                break;
            case 3:
                arrow = Instantiate(arrowLeftResultPrefab, containArrowResult.transform);
                arrow.GetComponent<Image>().enabled = false;
                break;
            case 4:
                arrow = Instantiate(arrowRightResultPrefab, containArrowResult.transform);
                arrow.GetComponent<Image>().enabled = false;
                break;
        }
    }

    // Called to remove all arrow in bar
    public void DestroyArrow()
    {
        GameObject arrowDestroy;
        for (int i = 0; i < listArrowCurrent.Count; i++)
        {
            arrowDestroy = containArrow.transform.GetChild(i).gameObject;
            Destroy(arrowDestroy);
        }
        for (int i = 0; i < listArrowCurrent.Count; i++)
        {
            arrowDestroy = containArrowResult.transform.GetChild(i).gameObject;
            Destroy(arrowDestroy);
        }
        listArrowCurrent.Clear();
        indexArrowResult = 0;
    }

    // Called when you finish arrow but miss pressing spacebar 
    private void BackUpArrow()
    {
        for (int i = 0; i < listArrowCurrent.Count; i++)
        {
            containArrowResult.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
        }
    }
}
