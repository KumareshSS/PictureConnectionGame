using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : WordManager
{
    public static GameManager Instance;

    [Header("Life Properties")]
    [SerializeField] private int Life;
    public System.Action<int> OnLifeUpdated;

    [SerializeField] private int Score;
    public System.Action<int> OnScoreUpdated;

    [Header("Instance Classes")]
    [SerializeField] private ButtonGridManager gridManagerBtns;

    [Header("Question Properties")]
    private int currentQuestionIndex = 0;
    private int currentQuestionScore;

    public List<QuestionData> allQuestions;
    [SerializeField] private QuestionDatabase questionDatabase;


    public GameObject Gameover, GameCompleted,GamePlayPanel;
    public Button Restart,Exitbtn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        InitalizeGame();
        VerifyBtn.onClick.AddListener(VerifyAnswer);
        ClearBtn.onClick.AddListener(ResetAnswer);
        Restart.onClick.AddListener(RestartGame);
        Exitbtn.onClick.AddListener(QuitGame);

    }

    #region Game Logic
    private void InitalizeGame()
    {
        GamePlayPanel.SetActive(true);

        gridManagerBtns.CreateAlphaBetPooling();
        gridManagerBtns.ShuffleAndAssignLetters();
        allQuestions = questionDatabase.questions;
        Life = 5;
        ScoreTxt.text = "0";
        LifeTxt.text = Life.ToString();

        for (int i = 0; i < clueDisplayImage.Length; i++)
        {
            clueDisplayImage[i].gameObject.SetActive(false);
        }
        UpdateCurrentClueScoreText();
        LoadNextQuestion();
        StartTimer();
    }
    private void ResetGame()
    {
        ResetAnswer();
        Life = 0;
    }
    public void InputLetter(char letter)
    {
        if (currentLetterIndex < letterTexts.Length)
        {
            FillLetter(currentLetterIndex, letter);
            enteredLetters[currentLetterIndex] = letter;
            currentLetterIndex++;
        }
    }

    #endregion

    #region Adding / Subtraction Subscribition
    private void AddScore()
    {
        OnScoreUpdated?.Invoke(Score);
        ScoreTxt.text = Score.ToString();
    }
    private void SubtractLife()
    {
        Life--;
        OnLifeUpdated?.Invoke(Life);
        LifeTxt.text = Life.ToString();
    }

    #endregion

    #region Verifying Process
    private bool IsCorrectAnswer()
    {
        string entered = new string(enteredLetters).ToUpper();
        return entered == currentAnswer;
    }
    private void VerifyAnswer()
    {
        ValidationErrorCheck();
    }

    private void ValidationErrorCheck()
    {
        StartCoroutine(ValidateErrorCheck());
    }

    private IEnumerator ValidateErrorCheck()
    {
        ValidationTxt.text = "";

        if (IsCorrectAnswer())
        {
            ValidationTxt.color = Color.green;
            ValidationTxt.text = $"Correct! +{currentQuestionScore} points";
            Score += currentQuestionScore;
            AddScore();
            HighlightCorrectAnswer();
            yield return new WaitForSeconds(1f);
            ValidationTxt.text = "";

            LoadNextQuestion();
            ResetLetterHighlight();
        }
        else
        {
            ValidationTxt.color = Color.red;
            ValidationTxt.text = "Wrong answer. Try again.";
            SubtractLife();
            yield return new WaitForSeconds(1.5f);
            ValidationTxt.text = "";

            ResetAnswer();
        }
        if(Life <= 0)
        {
            Gameover.SetActive(true);
        }
    }
    public void RestartGame()
    {
        GamePlayPanel.SetActive(false);
        SceneLoader.Instance.LoadSceneWithLoading("Gameplay", 2f);
    }

    public void QuitGame()
    {
        Debug.Log("Game Quitted !!");
        Application.Quit();
    }

    private void HighlightCorrectAnswer()
    {
        for (int i = 0; i < letterTexts.Length; i++)
        {
 
            letterTexts[i].DOColor(Color.green, 0.3f).SetEase(Ease.InOutQuad);
            letterTexts[i].transform.DOScale(1.2f, 0.8f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutBack);
        }
    }
    private void ResetLetterHighlight()
    {
        foreach (var txt in letterTexts)
        {
            txt.color = Color.black;
            txt.transform.localScale = Vector3.one;
        }
    }


    #endregion

    #region  Question DataBase Operations


    public void LoadNextQuestion()
    {
        if (currentQuestionIndex < allQuestions.Count)
        {
            var currentQuestion = allQuestions[currentQuestionIndex];

            SetNewAnswer(currentQuestion.answers);
            UpdateQuestionText(currentQuestion.questionText);

            currentClues = allQuestions[currentQuestionIndex].clueImages;
            clueIndex = 0;

            ResetClues(currentQuestion.clueImages);


            currentQuestionScore = 6;
            UpdateCurrentClueScoreText();
            ShowNextClue();

            if (clueRevealCoroutine != null)
                StopCoroutine(clueRevealCoroutine);
            clueRevealCoroutine = StartCoroutine(AutoRevealClues());

            clueButton.onClick.RemoveAllListeners();
            clueButton.onClick.AddListener(ShowNextClue);


            currentQuestionIndex++;
        }
        else
        {
            Debug.LogError("No more questions Here");
            GameCompleted.SetActive(true);
        }
    }

    private void ShowNextClue()
    {
        if (clueIndex < currentClues.Length)
        {
            clueDisplayImage[clueIndex].sprite = currentClues[clueIndex];
            clueDisplayImage[clueIndex].gameObject.SetActive(true);
            clueIndex++;

            if (currentQuestionScore > 1)
                currentQuestionScore--;

            UpdateCurrentClueScoreText();
        }
        else
        {
            Debug.LogError("All clues  are  shown");

        }
    }
    private Coroutine clueRevealCoroutine;

    private IEnumerator AutoRevealClues(int delaySeconds = 12)
    {
        while (clueIndex < currentClues.Length)
        {
            for (int secondsLeft = delaySeconds; secondsLeft > 0; secondsLeft--)
            {
                countdownText.text = $"Next clue reveal in {secondsLeft} seconds";
                yield return new WaitForSeconds(1f);
            }

            ShowNextClue();
        }

        countdownText.text = "All clues revealed!";
        yield return new WaitForSeconds(1.5f);
        countdownText.text = string.Empty;
    }


    void UpdateQuestionText(string _newQuestion)
    {
        Debug.Log($"New Question Here {_newQuestion}");
        QuestionTxt.text = _newQuestion;
    }
    void ResetClues(Sprite[] _clues)
    {
        currentClues = _clues;

        for (int i = 0; i < clueDisplayImage.Length; i++)
        {
            clueDisplayImage[i].gameObject.SetActive(false);

            if (i < currentClues.Length && currentClues[i] != null)
            {
                clueDisplayImage[i].sprite = currentClues[i];
            }
        }

        clueIndex = 0;
    }
    private void UpdateCurrentClueScoreText()
    {
        currentRevivalScoreTxt.text = $"Your current score for this question: {currentQuestionScore}";
    }


    #endregion

}
