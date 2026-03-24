using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //attributes

    public List<Person> people { get; private set; }
    //above is a collection of person objects, that is yet to be populated
    public float popularity { get; private set; }
    public float totalProfit { get; private set; }
    public float highScore { get; private set; }
    public float taxRate { get; private set; }
    public int day { get; private set; }
    public int decisionsToday { get; private set; }
    public bool isGameOver { get; private set; }
    private List<string> eventDescriptions { get; set; }
    //above is a collection of strings that i will write in the future, that is yet to be written
    private int eventID_A { get; set; }
    private int eventID_B { get; set; }
    //queues for complex events
    public Queue<int> complexEventsActive { get; private set; }
    public Queue<int> complexEventsDaysRemaining { get; private set; }

    //attributes added during development
    public Button eventButtonA;
    public Button eventButtonB;
    public GameObject loseScreen;
    public GameObject winScreen;
    public GameObject totalProfitDisplayText;

    //visible info
    public GameObject person1Text;
    public GameObject person2Text;
    public GameObject person3Text;
    public GameObject person4Text;
    public GameObject person5Text;
    public GameObject person6Text;
    public GameObject person7Text;
    public GameObject person8Text;
    public GameObject person9Text;
    public GameObject person10Text;
    public GameObject numberOfDecisionsMadeText;
    public GameObject dayNumberText;
    public GameObject totalProfitText;
    public GameObject popularityText;

    //methods

    //FUNCTIONALITY

    //changes the popularity indirectly (specifying set/adjust), and calls validation
    public void ChangePopularity(float newPopularity, bool callType)
    {
        if (callType == false)
        {
            popularity = newPopularity;
            ValidatePopularity(popularity);
        }

        else if (callType == true)
        {
            popularity += newPopularity;
            ValidatePopularity(popularity);
        }

        else
        {
            Debug.Log("Specify alteration type");
        }
    }

    //validates the popularity is within bounds
    private void ValidatePopularity(float popularityToBeValidated)
    {
        if (popularityToBeValidated < 0)
        {
            popularity = 0;
        }
        if (popularityToBeValidated > 10)
        {
            popularity = 10;
        }
    }

    //changes the total pofi indirectly (specifying set/adjust), and calls validation
    public void ChangeTotalProfit(float newTotalProfit, bool callType)
    {
        if (callType == false)
        {
            totalProfit = newTotalProfit;
            ValidateTotalProfit(totalProfit);
        }

        else if (callType == true)
        {
            totalProfit += newTotalProfit;
            ValidateTotalProfit(totalProfit);
        }

        else
        {
            Debug.Log("Specify alteration type");
        }
    }

    //validates the TotalProfit is within bounds
    private void ValidateTotalProfit(float totalProfitToBeValidated)
    {
        if (totalProfitToBeValidated < 0)
        {
            totalProfit = 0;
        }
    }

    private void CreatePopulation(int numberOfPersons)
    {
        //initiaise a counter outside of loop so that everytime 
        //this method is called the number of created obj = input
        //and NOT number of created obj = input plus last time
        int counter = 0;
        //uses the while loop to create person objects
        while (counter < numberOfPersons)
        {
            people.Add(new Person(10000, 5f));
            counter += 1;
        }
    }

    //sums all opinons, and divides them by the number of person obj in people, returns this average as float
    public void CalculatePopularity()
    {
        float sumOfOpinions = 0f;
        int numberOfPersonObjects = 0;

        foreach (Person p in people)
        {
            numberOfPersonObjects += 1;
            sumOfOpinions += p.opinion;
        }

        float averagePopularity = sumOfOpinions / numberOfPersonObjects;
        ChangePopularity(averagePopularity, false);
    }

    //iterates over the population, multiplying current tax rate by the 
    //individual salaries, and adding that to the total profit
    public void CollectTaxes()
    {
        foreach (Person p in people)
        {
            ChangeTotalProfit(taxRate * p.salary, true);
        }
    }

    public void GenerateEventIDs()
    {

        //making the eventID
        //assumed 8 events, change as necassary
        eventID_A = Random.Range(1, 9);
        eventID_B = Random.Range(1, 9);

        //make sure the player doesnt get duplicate events
        while (eventID_A == eventID_B)
        {
            eventID_B = Random.Range(1, 9);
        }

        //updating the button text to match the event
        GameObject.Find("eventButtonA").GetComponentInChildren<Text>().text = eventDescriptions[eventID_A - 1];
        GameObject.Find("eventButtonB").GetComponentInChildren<Text>().text = eventDescriptions[eventID_B - 1];
    }

    private void ApplyEvent(int eventID)
    {
        if (eventID == 1)
        {
            InflationEvent();
        }

        else if (eventID == 2)
        {
            TechAdvanceEvent();
        }

        else if (eventID == 3)
        {
            RaiseTaxes();
        }

        else if (eventID == 4)
        {
            Event4();
        }

        else if (eventID == 5)
        {
            Event5();
        }

        else if (eventID == 6)
        {
            Event6();
        }

        else if (eventID == 7)
        {
            Event7();
        }

        else if (eventID == 8)
        {
            Event8();
        }

        else
        {
            Debug.Log("no event matches that ID");
        }
    }

    private void TriggerTimedEvents()
    {
        Debug.Log("triger timed events called");

        int iterations = complexEventsActive.Count;
        //while loop that evaluates each event once
        while (iterations != 0)
        {
            //get and store the required info for the next event to be evaluated
            int currentEventID = complexEventsActive.Dequeue();
            int currentDaysRemaining = complexEventsDaysRemaining.Dequeue();

            //selection and application of the day specific effect
            if (currentEventID == 6 & currentDaysRemaining == 2)
            {
                ComplexEvent6_day2();
            }

            else if (currentEventID == 6 & currentDaysRemaining == 1)
            {
                ComplexEvent6_day1();
            }

            else if (currentEventID == 7 & currentDaysRemaining == 3)
            {
                ComplexEvent7_day3();
            }

            else if (currentEventID == 7 & currentDaysRemaining == 2)
            {
                ComplexEvent7_day2();
            }

            else if (currentEventID == 7 & currentDaysRemaining == 1)
            {
                ComplexEvent7_day1();
            }

            currentDaysRemaining -= 1;

            //moving events back into the queue if required
            if (currentDaysRemaining != 0)
            {
                complexEventsActive.Enqueue(currentEventID);
                complexEventsDaysRemaining.Enqueue(currentDaysRemaining);
                iterations -= 1;
                Debug.Log("days remaining for this event was NOT 0, enqueued items, iteration count decremented");
            }

            //ending without moving events back into queue
            else
            {
                iterations -= 1;
                Debug.Log("days remaining for this event was 0, iteration count decremented");
            }
        }

        Debug.Log("iterations is 0, loop ended");

    }

    public void NextDay()
    {
        day += 1;
        decisionsToday = 0;
        TriggerTimedEvents();

        /*output all the required info (view code here) -> */
        {
            GameObject.Find("person 1").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[0].salary.ToString("#.##") + " and opinion : " + people[0].opinion.ToString("#.##");
            GameObject.Find("person 2").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[1].salary.ToString("#.##") + " and opinion : " + people[1].opinion.ToString("#.##");
            GameObject.Find("person 3").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[2].salary.ToString("#.##") + " and opinion : " + people[2].opinion.ToString("#.##");
            GameObject.Find("person 4").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[3].salary.ToString("#.##") + " and opinion : " + people[3].opinion.ToString("#.##");
            GameObject.Find("person 5").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[4].salary.ToString("#.##") + " and opinion : " + people[4].opinion.ToString("#.##");
            GameObject.Find("person 6").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[5].salary.ToString("#.##") + " and opinion : " + people[5].opinion.ToString("#.##");
            GameObject.Find("person 7").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[6].salary.ToString("#.##") + " and opinion : " + people[6].opinion.ToString("#.##");
            GameObject.Find("person 8").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[7].salary.ToString("#.##") + " and opinion : " + people[7].opinion.ToString("#.##");
            GameObject.Find("person 9").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[8].salary.ToString("#.##") + " and opinion : " + people[8].opinion.ToString("#.##");
            GameObject.Find("person 10").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[9].salary.ToString("#.##") + " and opinion : " + people[9].opinion.ToString("#.##");
            GameObject.Find("decisions counter").GetComponentInChildren<Text>().text = "The number of decisions made today is : " + decisionsToday.ToString();
            GameObject.Find("day counter").GetComponentInChildren<Text>().text = "Today is day number : " + day.ToString();
            GameObject.Find("total profit tracker").GetComponentInChildren<Text>().text = "The total profit accumulated is : " + totalProfit.ToString("#.##");
            GameObject.Find("popularity tracker").GetComponentInChildren<Text>().text = "Your popularity is : " + popularity.ToString("#.##");
        }

        CheckWinCondition();
        CheckLoseCondition();
    }

    private void CheckLoseCondition()
    {
        if (popularity < 2)
        {
            isGameOver = true;
            //disable buttons
            eventButtonA.enabled = false;
            eventButtonB.enabled = false;
            eventButtonA.gameObject.SetActive(false);
            eventButtonB.gameObject.SetActive(false);
            //show the loss screen
            loseScreen.gameObject.SetActive(true);


        }
    }

    private void CheckWinCondition()
    {
        if (day == 31)
        {
            isGameOver = true;
            highScore = totalProfit;
            //disable buttons
            eventButtonA.enabled = false;
            eventButtonB.enabled = false;
            eventButtonA.gameObject.SetActive(false);
            eventButtonB.gameObject.SetActive(false);
            //show the win screen
            winScreen.gameObject.SetActive(true);
            totalProfitDisplayText.gameObject.SetActive(true);
            GameObject.Find("totalProfitDisplay").GetComponentInChildren<Text>().text = "High score : " + highScore;
        }
    }

    private void ProcessDecision(int eventID)
    {
        Debug.Log("process decision called with event ID " + eventID);
        Debug.Log(totalProfit);
        if (isGameOver == true)
        {
            Debug.Log("game is over, no longer progressing gameplay");
            return;
        }

        else
        {
            //update variables
            ApplyEvent(eventID); //do the event
            CollectTaxes(); // update total profit
            CalculatePopularity(); //update popularity
            decisionsToday += 1;

            /*output all the required info (view code here) -> */
            {
                GameObject.Find("person 1").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[0].salary.ToString("#.##") + " and opinion : " + people[0].opinion.ToString("#.##");
                GameObject.Find("person 2").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[1].salary.ToString("#.##") + " and opinion : " + people[1].opinion.ToString("#.##");
                GameObject.Find("person 3").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[2].salary.ToString("#.##") + " and opinion : " + people[2].opinion.ToString("#.##");
                GameObject.Find("person 4").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[3].salary.ToString("#.##") + " and opinion : " + people[3].opinion.ToString("#.##");
                GameObject.Find("person 5").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[4].salary.ToString("#.##") + " and opinion : " + people[4].opinion.ToString("#.##");
                GameObject.Find("person 6").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[5].salary.ToString("#.##") + " and opinion : " + people[5].opinion.ToString("#.##");
                GameObject.Find("person 7").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[6].salary.ToString("#.##") + " and opinion : " + people[6].opinion.ToString("#.##");
                GameObject.Find("person 8").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[7].salary.ToString("#.##") + " and opinion : " + people[7].opinion.ToString("#.##");
                GameObject.Find("person 9").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[8].salary.ToString("#.##") + " and opinion : " + people[8].opinion.ToString("#.##");
                GameObject.Find("person 10").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[9].salary.ToString("#.##") + " and opinion : " + people[9].opinion.ToString("#.##");
                GameObject.Find("decisions counter").GetComponentInChildren<Text>().text = "The number of decisions made today is : " + decisionsToday.ToString();
                GameObject.Find("day counter").GetComponentInChildren<Text>().text = "Today is day number : " + day.ToString();
                GameObject.Find("total profit tracker").GetComponentInChildren<Text>().text = "The total profit accumulated is : " + totalProfit.ToString("#.##");
                GameObject.Find("popularity tracker").GetComponentInChildren<Text>().text = "Your popularity is : " + popularity.ToString("#.##");
            }

            //check if the day ticks over or not.
            //Either way, make the next events and then check  for win/loss
            if (decisionsToday < 3) 
            {
                GenerateEventIDs();
                CheckLoseCondition();
                CheckWinCondition();
            }

            else
            {
                NextDay();
                Debug.Log("day updated. it is now day number " + day);
                GenerateEventIDs();
                
            }

        }
    }

    //buttons

    public void eventButtonAClicked()
    {
        ProcessDecision(eventID_A);
    }

    public void eventButtonBClicked()
    {
        ProcessDecision(eventID_B);
    }

    public void QuitGame()
    { //see method name
        Debug.Log("the quit game method was called");
        Application.Quit();
    }

    //EVENTS

    private void InflationEvent()
    {
        Debug.Log("inflation called");
        foreach (Person p in people)
        {
            p.ChangeSalary(0.2f * p.salary);
            p.ChangeOpinion(-0.5f);
        }
    }

    private void TechAdvanceEvent()
    {
        Debug.Log("TechAdvance was called");
        foreach (Person p in people)
        {
            p.ChangeSalary(-0.25f * p.salary);
            p.ChangeOpinion(0.5f);
        }
    }

    private void RaiseTaxes()
    {
        Debug.Log("RaiseTaxes was called");
        taxRate += 0.05f;
        foreach (Person p in people)
        {
            p.ChangeOpinion(-0.4f);
        }
    }

    private void Event4()
    {
        Debug.Log("test event 4 was called");
    }

    private void Event5()
    {
        Debug.Log("test event 5 was called");
    }

    private void Event6()
    {
        Debug.Log("test event 6 was called");
    }

    private void ComplexEvent6_day2()
    {
        Debug.Log("complex behaiviour for day 2 of event 6 was called");
    }

    private void ComplexEvent6_day1()
    {
        Debug.Log("complex behaiviour for day 1 of event 6 was called");
    }

    private void Event7()
    {
        Debug.Log("test event 7 was called, adding behaviour to queue");
        complexEventsActive.Enqueue(7); // the event id is 7 because these effects are caused by event 7
        complexEventsDaysRemaining.Enqueue(3);//the number of days remaining is 3, because event 7 lasts 3 days
    }

    private void ComplexEvent7_day3()
    {
        Debug.Log("complex behaiviour for day 3 of event 7 was called");
    }

    private void ComplexEvent7_day2()
    {
        Debug.Log("complex behaiviour for day 2 of event 7 was called");
    }

    private void ComplexEvent7_day1()
    {
        Debug.Log("complex behaiviour for day 1 of event 7 was called");
    }

    private void Event8()
    {
        Debug.Log("test event 8 was called");
    } 

    void Start() 
	{
        Debug.Log(highScore);

        //initialise values
        people = new List<Person>();
        popularity = 0f;
        totalProfit = 0f;
        taxRate = 0.15f;
        day = 1;
        decisionsToday = 0;
        isGameOver = false;
        eventDescriptions = new List<string>();
        complexEventsActive = new Queue<int>();
        complexEventsDaysRemaining = new Queue<int>();
        //enable buttons
        eventButtonA.enabled = true;
        eventButtonB.enabled = true;
        eventButtonA.gameObject.SetActive(true);
        eventButtonA.gameObject.SetActive(true);
        //hide any win/loss screens
        loseScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
        totalProfitDisplayText.gameObject.SetActive(false);

        //populate descriptions
        eventDescriptions.Add("Inflation : Cause a surge of increase in the amount of money in circulation, and potentially worry the public");
        eventDescriptions.Add("Technological advancement : A new era of automation improves the quality of goods, services and quality of living, but saturates the job market");
        eventDescriptions.Add("Raise taxes : Increase the reigonal tax rate");
        eventDescriptions.Add("event4 : does event 4 stuff");
        eventDescriptions.Add("event5 : does event 5 stuff");
        eventDescriptions.Add("event6 : does event 6 stuff");
        eventDescriptions.Add("event7 : does event 7 stuff");
        eventDescriptions.Add("event8 : does event 8 stuff");
        //initialise population
        CreatePopulation(10);

        //make a new set of event IDs
        GenerateEventIDs();

        //testing
        day = 31;
        CheckWinCondition();

        /*output all the required info (view code here) -> */ {
            GameObject.Find("person 1").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[0].salary.ToString("#.##") + " and opinion : " + people[0].opinion.ToString("#.##");
            GameObject.Find("person 2").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[1].salary.ToString("#.##") + " and opinion : " + people[1].opinion.ToString("#.##");
            GameObject.Find("person 3").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[2].salary.ToString("#.##") + " and opinion : " + people[2].opinion.ToString("#.##");
            GameObject.Find("person 4").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[3].salary.ToString("#.##") + " and opinion : " + people[3].opinion.ToString("#.##");
            GameObject.Find("person 5").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[4].salary.ToString("#.##") + " and opinion : " + people[4].opinion.ToString("#.##");
            GameObject.Find("person 6").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[5].salary.ToString("#.##") + " and opinion : " + people[5].opinion.ToString("#.##");
            GameObject.Find("person 7").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[6].salary.ToString("#.##") + " and opinion : " + people[6].opinion.ToString("#.##");
            GameObject.Find("person 8").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[7].salary.ToString("#.##") + " and opinion : " + people[7].opinion.ToString("#.##");
            GameObject.Find("person 9").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[8].salary.ToString("#.##") + " and opinion : " + people[8].opinion.ToString("#.##");
            GameObject.Find("person 10").GetComponentInChildren<Text>().text = "This person has a salary of : " + people[9].salary.ToString("#.##") + " and opinion : " + people[9].opinion.ToString("#.##");
            GameObject.Find("decisions counter").GetComponentInChildren<Text>().text = "The number of decisions made today is : " + decisionsToday.ToString();
            GameObject.Find("day counter").GetComponentInChildren<Text>().text = "Today is day number : " + day.ToString();
            GameObject.Find("total profit tracker").GetComponentInChildren<Text>().text = "The total profit accumulated is : " + totalProfit.ToString("#.##");
            GameObject.Find("popularity tracker").GetComponentInChildren<Text>().text = "Your popularity is : " + popularity.ToString("#.##");
        }

    }
	
}
