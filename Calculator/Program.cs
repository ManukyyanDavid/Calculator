using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Calculator;

public class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Calculator calculator = new Calculator();
            calculator.Equation = calculator.Get_Expression();

            calculator.List = calculator.Convert_Equation_To_List();
            bool IsValid = calculator.Is_Valid(calculator.List);
            if (IsValid)
            {
                //check if the expression contains Parenthesis
                bool contains_parenthesis = calculator.Contains_Parenthesis();
                if (contains_parenthesis)
                {
                    calculator.Find_Parenthesis_Indexes();
                }
                double result = calculator.Calculate(calculator.List);
                calculator.Print_Answer(result);
                Console.WriteLine("Press any key to start again");
                Console.ReadKey();
            }
            else
            {
                calculator.Equation = "";
                calculator.List.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something is wrong");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


    }
}
