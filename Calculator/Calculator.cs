using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Calculator;

public class Calculator
{
    public string Equation { get; set; } = "";
    public List<object> List { get; set; } = new List<object>();

    /// <summary>
    /// Get the mathematical equation
    /// </summary>
    /// <returns></returns>
    public string Get_Expression()
    {
        Console.WriteLine("Please write the math equation");
        Console.WriteLine("You can use numbers [1-9], mathematical symbols [+,-,*,/], and parenthesis [ (,) ]");
        Console.WriteLine("For example: 6+7/(5+4)*6.5");
        string equation = Console.ReadLine();

        if (equation.Contains(" "))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong Input");
            Console.ForegroundColor = ConsoleColor.White;
            return Get_Expression();
        }
        else
        {
            return equation;
        }
    }
    /// <summary>
    /// Convert string equation into the list 
    /// </summary>
    /// <returns></returns>
    public List<object> Convert_Equation_To_List()
    {
        List<object> ConvertedList = new List<object>();

        var numbersAndOperators = Regex.Matches(Equation, @"(\d+|\+|\-|\*|\/|\(|\)|\.)");

        foreach (var item in numbersAndOperators)
        {
            ConvertedList.Add(item);
        }
        for (int i = 0; i < ConvertedList.Count - 1; i++)
        {
            if (Is_Digit_Or_Dot(ConvertedList[i]) && Is_Digit_Or_Dot(ConvertedList[i + 1]))
            {
                ConvertedList[i] = ConvertedList[i].ToString() + ConvertedList[i + 1].ToString();
                ConvertedList.RemoveAt(i + 1);
                i = -1;
            }
        }
        return ConvertedList;
    }
    /// <summary>
    /// checking whether the string is a number or dot(.)
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    static bool Is_Digit_Or_Dot(object number)
    {
        bool isdigit = Regex.IsMatch(number.ToString(), @"^\d+$");
        bool isdot = number.ToString().Contains(".");
        if (isdigit || isdot)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// check if the equation contains Parenthesis
    /// </summary>
    /// <returns></returns>
    public bool Contains_Parenthesis()
    {
        foreach (var item in List)
        {
            if (item.ToString().Contains("(") || item.ToString().Contains(")"))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Find 2 indexes of Parenthesis
    /// </summary>
    public void Find_Parenthesis_Indexes()
    {
        int index1 = 0;
        int index2 = 0;
        while (Contains_Parenthesis())
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].ToString() == ")")
                {
                    index1 = i;
                    for (int j = index1; j >= 0; j--)
                    {
                        if (List[j].ToString() == "(")
                        {
                            index2 = j;
                            break;
                        }
                    }
                    break;
                }
            }
            Solve_Inside_Parenthes_Equation(index2, index1);
        }


    }
    /// <summary>
    /// Solve the equation inside Parenthesis and insert the value into the list
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    public void Solve_Inside_Parenthes_Equation(int index1, int index2)
    {
        List<object> exp = new List<object>();
        for (int i = index1 + 1; i <= index2 - 1; i++)
        {
            exp.Add(List[i].ToString());
        }
        for (int i = 0; i <= index2 - index1; i++)
        {
            List.RemoveAt(index1);
        }
        double res = Calculate(exp);
        List.Insert(index1, res);
    }

    /// <summary>
    /// check which mathematical symbols are used and according them Calculate  the result
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public double Calculate(List<object> list)
    {
        if (list.Count == 1)
        {
            return Convert.ToDouble(list[0].ToString());
        }
        double n1, n2, res = 0;

        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].ToString() == "*" || list[i].ToString() == "/")
            {
                n1 = Convert.ToDouble(list[i - 1].ToString());
                n2 = Convert.ToDouble(list[i + 1].ToString());
                switch (list[i].ToString())
                {
                    case "*":
                        res = n1 * n2;
                        break;
                    case "/":
                        res = n1 / n2;
                        break;
                    default:
                        res = 0;
                        break;
                }
                list.RemoveAt(i + 1);
                list.RemoveAt(i - 1);
                list[i - 1] = res;
                i = 0;
            }
        }
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].ToString() == "-" || list[i].ToString() == "+")
            {
                n1 = Convert.ToDouble(list[i - 1].ToString());
                n2 = Convert.ToDouble(list[i + 1].ToString());
                switch (list[i].ToString())
                {
                    case "+":
                        res = n1 + n2;
                        break;
                    case "-":
                        res = n1 - n2;
                        break;
                    default:
                        res = 0;
                        break;
                }
                list.RemoveAt(i + 1);
                list.RemoveAt(i - 1);
                list[i - 1] = res;
                i = 0;

            }
        }
        return res;
    }
    /// <summary>
    /// Doing validation on the equation inputeed by the user
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool Is_Valid(List<object> list)
    {

        //check if the equation only consists of numbers, mathematical symbols [+,-,*,/], or parenthesis [ (,) ]
        foreach (object item in list)
        {
            foreach (char c in item.ToString())
            {
                if (!((c >= 46 && c <= 57) || (c >= 40 && c <= 45 && c != 44)))
                {
                    return false;
                }
            }
        }

        //check the number of counts and symbols
        int numbercount = 0;
        int symbolcount = 0;
        foreach (object item in list)
        {
            if (item.ToString() == "+" || item.ToString() == "-" || item.ToString() == "*" || item.ToString() == "/")
            {
                symbolcount++;
            }
            else if (!(item.ToString() == "(" || item.ToString() == ")"))
            {

                numbercount++;
            }

        }
        if (numbercount - symbolcount != 1)
        {
            return false;
        }

        // () check if there are equal amount of OpenParenthes and CloseParenthes
        int OpenParenthes = 0;
        int CloseParenthes = 0;
        foreach (object item in list)
        {
            if (item.ToString() == "(")
            {
                OpenParenthes++;
            }
            else if (item.ToString() == ")")
            {
                CloseParenthes++;
            }
        }

        if (OpenParenthes != CloseParenthes)
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Print the equation and the answer
    /// </summary>
    /// <param name="result"></param>
    public void Print_Answer(double result)
    {
        Console.Write("The solution   ->   ");
        foreach (var item in Equation)
        {
            Console.Write(item);
        }
        Console.Write("=");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" " + result);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }
}
