using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class QuizManager : MonoBehaviour
{
    public TextAsset textFile;

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

    List<Course> topic = new List<Course>();
    public List<int> questionList = new List<int>() { 1, 2, 3, 4, 5 };
    System.Random random = new System.Random();


    // jsonString = File.ReadAllText("Assets/QuizJSON.txt");
    void Start()
    {
        //Debug.Log(jsonString);
        //Heart topic = JsonConvert.DeserializeObject<Heart>(jsonString);

        using (StreamReader r = new StreamReader("Assets/test3.json"))
        {
            string json = r.ReadToEnd(); 
            //Debug.Log(json);
            topic = JsonConvert.DeserializeObject<List<Course>>(json);
            //Debug.Log(topic[1].CourseName);
            //Debug.Log(topic[1].Ques_Data[4].Question);
            //Debug.Log(topic[1].Ques_Data[0].Answer);
            LarynxQuiz();
        }


    }

    public void LarynxQuiz()
    {
        Debug.Log(topic[1].Ques_Data.Count);
        
        List<QuesDatum> listOfQuestions = new List<QuesDatum>();

        for (int i = questionList.Count - 1; i >= 0; i--)
        {
            listOfQuestions.Add(topic[1].Ques_Data[i]);
        }

        listOfQuestions = shuffleList(listOfQuestions);

        for (int i = 0; i < listOfQuestions.Count; i++)
        {
            Debug.Log(listOfQuestions[i].Question);
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
