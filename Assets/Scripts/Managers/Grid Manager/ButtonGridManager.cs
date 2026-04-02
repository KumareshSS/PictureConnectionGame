using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGridManager : MonoBehaviour
{
    [SerializeField] private GameObject alphaBtnPrefab;
    [SerializeField] private Transform alphaBtnParent;

    private AlphabetButton[] buttonPool = new AlphabetButton[26];
    private char[] alphaBetletters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();


    public void CreateAlphaBetPooling()
    {
        for (int i = 0; i < 26; i++)
        {
            GameObject obj = Instantiate(alphaBtnPrefab, alphaBtnParent);
            AlphabetButton btn = obj.GetComponent<AlphabetButton>();
            obj.SetActive(false);
            buttonPool[i] = btn;
        }
    }

    public void ShuffleAndAssignLetters()
    {
        //ShuffleArray(alphaBetletters);

        for (int i = 0; i < 26; i++)
        {
            var info = new LetterInfo
            {
                letter = alphaBetletters[i],
                color = Color.black
            };

            buttonPool[i].SetCharacterButton(info, OnLetterClicked);
        }
    }

    void ShuffleArray(char[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    void OnLetterClicked(char letter)
    {    
       GameManager.Instance.InputLetter(letter);
    }
}
