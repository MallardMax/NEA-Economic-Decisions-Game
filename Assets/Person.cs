using System.Collections;
using System.Collections.Generic;

public class Person
{
    //attributes of person with getters and setters
    public int salary { get; private set; }
    public float opinion { get; private set; }

    //methods

    //constructor here
    public Person(int salary, float opinion)
    {
        this.salary = salary;
        ValidateSalary(salary);

        this.opinion = opinion;
        ValidateOpinion(opinion);
    }

    //methods to change salary and opinion indirectly 
    public void ChangeSalary(int amount)
    {
         salary = salary + amount;
        ValidateSalary(salary);
    }

    public void ChangeOpinion(float amount)
    {
        opinion = opinion + amount;
        ValidateOpinion(opinion);
    }

    //methods to make sure that person attributes cant take certain values
    private void ValidateSalary(int salaryToBeValidated)
    { 
      if (salaryToBeValidated < 0) 
      {
            salary = 0;
      }
    }

    private void ValidateOpinion(float opinionToBeValidated)
    {
        if (opinionToBeValidated < 0)
        {
            opinion = 0;
        }

        if (opinionToBeValidated > 10)
        {
            opinion = 10;
        }
    }
}
