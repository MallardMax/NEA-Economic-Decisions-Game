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
            totalProfit = totalProfit + newTotalProfit;
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
            PublicServices();
        }

        else if (eventID == 5)
        {
            PublicSpeech();
        }

        else if (eventID == 6)
        {
            EconomicShift();
        }

        else if (eventID == 7)
        {
            TaxHaven();
        }

        else if (eventID == 8)
        {
            TaxReform();
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
            p.ChangeOpinion(-1f);
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

    private void PublicServices()
    {
        Debug.Log("Invest in public services was called");
        foreach (Person p in people)
        {
            p.ChangeOpinion(0.5f + (0.2f*p.opinion));
        }
        ChangeTotalProfit(-500000, true);
    }

    private void PublicSpeech()
    {
        Debug.Log("public speech was called");
        int random = Random.Range(1, people.Count + 1);
        int counter = 1;
        foreach (Person p in people)
        {
            if (counter <= random)
            {
                p.ChangeOpinion(0.3f*p.opinion);
                counter += 1;
            }

            else
            {
                p.ChangeOpinion(-0.3f * p.opinion);
            }
        }
    }

    private void EconomicShift()
    {
        Debug.Log("test event 6 was called");
        complexEventsActive.Enqueue(6); // the event id is 6 because these effects are caused by event 6
        complexEventsDaysRemaining.Enqueue(2);//the number of days remaining is 2, because event 6 lasts 2 days
    }

    private void ComplexEvent6_day2()
    {
        Debug.Log("complex behaiviour for day 2 of economic shift was called");
        int random = Random.Range(40000, 200001);
        ChangeTotalProfit(random, true);
    }

    private void ComplexEvent6_day1()
    {
        Debug.Log("complex behaiviour for day 1 of economic shift was called");
        foreach (Person p in people)
        {
            if (totalProfit > 500000)
            {
                p.ChangeSalary(0.2f*p.salary);
            }

            else
            {
                p.ChangeSalary(-0.2f * p.salary);
            }
        }
    }

    private void TaxHaven()
    {
        Debug.Log("tax haven was called, adding behaviour to queue");
        complexEventsActive.Enqueue(7); // the event id is 7 because these effects are caused by event 7
        complexEventsDaysRemaining.Enqueue(3);//the number of days remaining is 3, because event 7 lasts 3 days
    }

    private void ComplexEvent7_day3()
    {
        Debug.Log("complex behaiviour for day 3 of tax haven was called");
        ChangeTotalProfit(100000 + (0.05f*totalProfit), true); //increase is 100k plus 5 percent
    }

    private void ComplexEvent7_day2()
    {
        Debug.Log("complex behaiviour for day 2 of tax haven was called");
        ChangeTotalProfit(150000 + (0.1f * totalProfit), true); //increase is 150k plus 10 percent
    }

    private void ComplexEvent7_day1()
    {
        Debug.Log("complex behaiviour for day 1 of tax haven was called");
        foreach (Person p in people)
        {
            float theConsequencesOfYourActions = 0.5f + ((0.1f * totalProfit)/100000);
            p.ChangeOpinion(-theConsequencesOfYourActions);
            //decrease is 0.2 plus some preportion of the money you made (approximately)
        }
    }

    private void TaxReform()
    {
        Debug.Log("Tax Reform was called");
        float upperBracket = 15000;
        float lowerBracket = 6000;

        foreach (Person p in people)
        {
            if (p.salary >= upperBracket)
            {
                p.ChangeOpinion(-2f);
            }

            else if (p.salary >= lowerBracket & p.salary < upperBracket)
            {
                p.ChangeOpinion(-0.2f);
            }

            else
            {
                p.ChangeOpinion(1f);
            }
        }
    } 

    void Start() 
	{
        Debug.Log(highScore);

        //initialise values
        people = new List<Person>();
        popularity = 5f;
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
        eventDescriptions.Add("Invest in public services : Spend more money to improve the public services in the city greatly");
        eventDescriptions.Add("Perform a speech : Speak your mind to the public; but remember, different things for different people");
        eventDescriptions.Add("Economic shift : an influx of buisness hits the town, for good or for bad? Up to you to find out");
        eventDescriptions.Add("Offshore tax haven : Attempt to circumvent the system for IMMENSE riches, but the truth will come out eventually");
        eventDescriptions.Add("Tax reform : tax the rich more heavily than the poor, and alter their thoughts on you respectively");
        //initialise population
        CreatePopulation(10);

        //make a new set of event IDs
        GenerateEventIDs();

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

    }
	
}
