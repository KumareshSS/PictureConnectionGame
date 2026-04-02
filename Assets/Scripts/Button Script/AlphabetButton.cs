using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetButton : MonoBehaviour
{
    public Button alphabetBtn;
    public TMP_Text label;

    public void SetCharacterButton(LetterInfo infoBtn, Action<char> onClick)
    {
        label.text = infoBtn.letter.ToString();
        label.color = infoBtn.color;
        alphabetBtn.onClick.RemoveAllListeners();
        alphabetBtn.onClick.AddListener(() => onClick(infoBtn.letter));
        gameObject.SetActive(true);
    }
}

[Serializable]
public struct LetterInfo
{
    public char letter;
    public Color color;
}
