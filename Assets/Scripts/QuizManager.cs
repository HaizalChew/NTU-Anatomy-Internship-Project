using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.Networking;

public class QuizManager : MonoBehaviour
{
    //public TextAsset textFile;
    List<Course> topic = new List<Course>();
    public BasicInteractions basicInteractions;
    private bool answerCheck;

    [Header("Questions")]
    [SerializeField] public int topicIndex;
    [SerializeField] List<QuesDatum> questions = new List<QuesDatum>();
    [SerializeField] private TextMeshProUGUI questionTextDisplay, questionNumberTextDisplay;
    [SerializeField] private GameObject submitButton, notificationPanel, notificationRight, notificationWrong, notificationCompleted;
    [SerializeField] private string currentQuestion, currentAnswer;
    [SerializeField] private int questionNumberCount;

    private float originSliderValue;

    [Serializable]
    public class QuesDatum
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    
    [Serializable]
    public class Course
    {
        public string CourseName { get; set; }
        public List<QuesDatum> Ques_Data { get; set; }
    }

    void Start()
    {
        if (questionTextDisplay == null)
        {
            questionTextDisplay = GameObject.FindGameObjectWithTag("QuestionText").GetComponent<TextMeshProUGUI>();
        }

        //using (StreamReader r = new StreamReader("Assets/test3.json"))
        //{
        //    string json = r.ReadToEnd(); 
        //    topic = JsonConvert.DeserializeObject<List<Course>>(json);
        //}

        StartCoroutine(GetStreamingAssetFile("test3.json"));

    }

    IEnumerator GetStreamingAssetFile(string url)
    {
        string path = Path.Combine(Application.streamingAssetsPath, url);

        using (var request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                yield break;
            }

            string jsonFile = request.downloadHandler.text;
            topic = JsonConvert.DeserializeObject<List<Course>>(jsonFile);
        }
    }

    private void Update()
    {

        questionTextDisplay.text = currentQuestion;

        if (basicInteractions.selectedObj)
        {
            submitButton.SetActive(true);
        }
        else
        {
            submitButton.SetActive(false);
        }

        questionNumberTextDisplay.text = "Question " + (questionNumberCount).ToString() + " out of " + questions.Count;
    }

    public void OnQuizStart()
    {
        questions = GetQuizQuestions(topicIndex);
        questionNumberCount = 0;
        basicInteractions.coronaryModel.gameObject.SetActive(false);
        originSliderValue = basicInteractions.sliderValue;
        if(basicInteractions.selectedObj != null)
        {
            basicInteractions.selectedObj.gameObject.SetActive(true);
            basicInteractions.selectedObj = null;
            Destroy(basicInteractions.selectedInstantiatedObj);
        }
        basicInteractions.model = basicInteractions.coronarySideModel;
        if (basicInteractions.veinCheck)
        {
            basicInteractions.sliderValue = 0;
        }
        basicInteractions.ToggleVeinTransparent(basicInteractions.coronarySideModel, basicInteractions.veinCheck);
    }

    public void OnQuizExit()
    {
        basicInteractions.coronaryModel.gameObject.SetActive(true);
        basicInteractions.sliderValue = originSliderValue;
        basicInteractions.ToggleVeinTransparent(basicInteractions.coronaryModel, basicInteractions.veinCheck);
    }

    public void OnMoveOnNextQuestion()
    {
        if (questionNumberCount < questions.Count)
        {
            // Get next quiz
            currentQuestion = questions[questionNumberCount].Question;
            currentAnswer = questions[questionNumberCount].Answer;
            questionNumberCount++;
        }
        else
        {
            // Quiz Completed
            notificationPanel.SetActive(true);
            notificationCompleted.SetActive(true);
            SaveStateManager.instance.topicChecklistCompleted[topicIndex] = true;
        }
        
    }

    public void OnCheckForRightAnswer()
    {
        if (basicInteractions.selectedObj.name == "Phr R" || basicInteractions.selectedObj.name == "Phr L")
        {
            if (CheckAnswer("Phr R or Phr L"))
            {
                notificationPanel.SetActive(true);
                notificationRight.SetActive(true);

            }
            else
            {
                notificationPanel.SetActive(true);
                notificationWrong.SetActive(true);
            }

        }
        else
        {
            if (CheckAnswer(basicInteractions.selectedObj.name))
            {
                notificationPanel.SetActive(true);
                notificationRight.SetActive(true);

            }
            else
            {
                notificationPanel.SetActive(true);
                notificationWrong.SetActive(true);
            }
        }
    }

    private bool CheckAnswer(string userAnswer)
    {
        if (currentAnswer.ToLower() == userAnswer.ToLower())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<QuesDatum> GetQuizQuestions(int topicIndex)
    {
        
        List<QuesDatum> listOfQuestions = new List<QuesDatum>();

        for (int i = topic[topicIndex].Ques_Data.Count - 1; i >= 0; i--)
        {
            listOfQuestions.Add(topic[topicIndex].Ques_Data[i]);
        }

        listOfQuestions = shuffleList(listOfQuestions);

        return listOfQuestions;

    }

    //private IEnumerator StartQuiz()
    //{
    //    if (basicInteractions.selectedObj != null)
    //    {

    //    }

    //}

    
    private List<QuesDatum> shuffleList(List<QuesDatum> inputList)
    {    //take any list of GameObjects and return it with Fischer-Yates shuffle
        int i = 0;
        int t = inputList.Count;
        int r = 0;
        QuesDatum p = null;
        List<QuesDatum> tempList = new List<QuesDatum>();
        tempList.AddRange(inputList);

        while (i < t)
        {
            r = UnityEngine.Random.Range(i, tempList.Count);
            p = tempList[i];
            tempList[i] = tempList[r];
            tempList[r] = p;
            i++;
        }

        return tempList;
    }



}
