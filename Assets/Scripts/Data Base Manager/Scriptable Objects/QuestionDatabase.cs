
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "QuestionDatabase", menuName = "Game/Question Database")]
public class QuestionDatabase : ScriptableObject
{
    public List<QuestionData> questions;
}
