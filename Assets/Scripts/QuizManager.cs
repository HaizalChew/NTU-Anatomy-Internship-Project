using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

public class QuizManager : MonoBehaviour
{
    //public TextAsset textFile;
    List<Course> topic = new List<Course>();
    public List<int> questionList = new List<int>() { 1, 2, 3, 4, 5 };
    int sceneNo = 0;
    public BasicInteractions basicInteractions;
    private bool answerCheck;

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
        sceneNo = SceneManager.GetActiveScene().buildIndex;

        using (StreamReader r = new StreamReader("Assets/test3.json"))
        {
            string json = r.ReadToEnd(); 
            topic = JsonConvert.DeserializeObject<List<Course>>(json);
            //GetQuizQuestion();
            StartCoroutine(GetQuizQuestion());
        }


    }

    void Update()
    {
       
    }
    
    bool answerStatus()
    {
        if (basicInteractions.selectedObj == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

   
    private IEnumerator GetQuizQuestion()
    {
        List<QuesDatum> listOfQuestions = new List<QuesDatum>();

        for (int i = questionList.Count - 1; i >= 0; i--)
        {
            listOfQuestions.Add(topic[sceneNo - 1].Ques_Data[i]);
        }

        listOfQuestions = shuffleList(listOfQuestions);


        for (int i = 0; i < listOfQuestions.Count;)
        {
            Debug.Log(i);
            Debug.Log(listOfQuestions[i].Question);
            Debug.Log(listOfQuestions[i].Answer);
            yield return new WaitUntil(answerStatus);
            if (basicInteractions.selectedObj.name == listOfQuestions[i].Answer)
            {
                basicInteractions.selectedObj.gameObject.SetActive(true);
                Destroy(basicInteractions.selectedInstantiatedObj);
                basicInteractions.selectedObj = null;
                i++;
                Debug.Log("Correct");
            }
            else
            {
                basicInteractions.selectedObj.gameObject.SetActive(true);
                Destroy(basicInteractions.selectedInstantiatedObj);
                basicInteractions.selectedObj = null;
                Debug.Log("Try again");
            }
        }
        Debug.Log("quiz ended");

    }

    //private IEnumerator StartQuiz()
    //{
    //    if (basicInteractions.selectedObj != null)
    //    {

    //    }

    //}

    private void CheckAnswer(string answerName, string selectedObjName = null)
    {
        if (selectedObjName == answerName)
        {
            Debug.Log("WellDone");
        }
        else
        {
            Debug.Log("Well");
        }
    }
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
