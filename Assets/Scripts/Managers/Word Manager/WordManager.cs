using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    [SerializeField] protected GameObject letterPlaceHolderObj;
    [SerializeField] protected Transform wordPlaceHolderParent;

    private List<TMP_Text> letterTextPool = new List<TMP_Text>();
    [SerializeField] protected TMP_Text[] letterTexts;

    [SerializeField] protected string currentAnswer = "WORD";

    [SerializeField] protected Button VerifyBtn, ClearBtn, clueButton;

    [SerializeField] protected int currentLetterIndex = 0;
    [SerializeField] protected char[] enteredLetters;

    [SerializeField] protected TMP_Text ScoreTxt, LifeTxt, QuestionTxt, ValidationTxt, countdownText,currentRevivalScoreTxt;


    [SerializeField] protected Sprite[] currentClues;
    [SerializeField] protected int clueIndex = 0;

    [Header("Timer Properties ")]
    [SerializeField] protected TMP_Text timerText;
    [SerializeField] protected Image timerFillImage;
    [SerializeField] protected Image[] clueDisplayImage;

    [Header("Timer Settings")]
    [SerializeField] protected float maxTime = 120f;

    private float currentTime;
    private Coroutine timerCoroutine;

    public System.Action OnTimerEnd;
    protected void SetNewAnswer(string answer)
    {
        currentAnswer = answer.ToUpper();
        int wordLength = currentAnswer.Length;
        currentLetterIndex = 0;
        letterTexts = new TMP_Text[wordLength];
        enteredLetters = new char[wordLength];
        for (int i = 0; i < wordLength; i++)
        {
            TMP_Text answerTxt;

            if (i < letterTextPool.Count)
            {
                answerTxt = letterTextPool[i];
                answerTxt.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                GameObject ansObj = Instantiate(letterPlaceHolderObj, wordPlaceHolderParent);
                answerTxt = ansObj.GetComponentInChildren<TMP_Text>();
                letterTextPool.Add(answerTxt);
            }

            answerTxt.text = "_";
            letterTexts[i] = answerTxt;
        }

        for (int i = wordLength; i < letterTextPool.Count; i++)
        {
            letterTextPool[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void FillLetter(int index, char letter)
    {
        if (index >= 0 && index < letterTextPool.Count)
        {
            letterTextPool[index].text = letter.ToString();
        }
    }
    public void ResetAnswer()
    {
        for (int i = 0; i < letterTexts.Length; i++)
        {
            letterTexts[i].text = "_";
            enteredLetters[i] = '\0';
        }
        currentLetterIndex = 0;
    }
    public void StartTimer()
    {
        StopTimer();
        currentTime = maxTime;
        timerCoroutine = StartCoroutine(RunTimer());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator RunTimer()
    {
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            UpdateTimerUI();

            yield return null;
        }

        currentTime = 0f;
        UpdateTimerUI();
        OnTimerEnd?.Invoke();
    }

    private void UpdateTimerUI()
    {
        timerText.text = Mathf.CeilToInt(currentTime).ToString();
        timerFillImage.fillAmount = currentTime / maxTime;
    }



}


/// <summary>
/// Pause & Resume
/// </summary>
//private bool isPaused = false;

//public void PauseTimer() => isPaused = true;
//public void ResumeTimer() => isPaused = false;

//private IEnumerator RunTimer()
//{
//    while (currentTime > 0f)
//    {
//        if (!isPaused)
//        {
//            currentTime -= Time.deltaTime;
//            UpdateTimerUI();
//        }
//        yield return null;
//    }

//    currentTime = 0f;
//    UpdateTimerUI();
//    OnTimerEnd?.Invoke();
//}



