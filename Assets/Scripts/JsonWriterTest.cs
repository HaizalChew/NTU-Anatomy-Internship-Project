using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Questions
{
    public string question { get; set; }
    public string answer { get; set; }

}

public class JsonWriterTest : MonoBehaviour
{
    private void Start()
    {
        Questions question = new Questions();

        question.question = "Test Question 1";
        question.answer = "Test Answer 1";

        string output = JsonConvert.SerializeObject(question);

        Debug.Log(output);
    }
}
