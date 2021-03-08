using System;

namespace calculator_v1
{
    public class Calculate
    {
        readonly private string _chars = "0123456789+-/*()"; //mask "vector" contain with allowed signs.
        private char[] _ch_expression; //_ch_expression spread over sign boards.
        private string _expression;
        
        public Calculate()
        {
            _expression = null;
        }
        
        public string Expression
        {
            get => _expression;
            set
            {
                if (!(string.IsNullOrEmpty(value)))
                {
                    value = value.Replace(" ", "");
                    _ch_expression = value.ToCharArray();
                    for (int num = 0; num < _ch_expression.Length; num++)
                    {
                        if (CheckSign(_chars, _ch_expression[num])) //if return true, will be call ArgumentException
                        {
                            throw new ArgumentException("Expression contain unallowed sign(s).");
                        }
                    }
                    _expression = value;
                }
                else
                {
                    throw new ArgumentException("Expression not exist");
                }
            }
        }

        private bool CheckSign(string vector, char sign) //method accepts string with allowed signs, and specific checked actually sign, if sign contain out inside of the string then method will return false
        {
            if (vector.Contains(sign))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Calculate exp1 = new Calculate();

            Console.WriteLine("Enter expression with only numbers and * / + - ( )");
            Console.Write("Expression: ");
            exp1.Expression = Console.ReadLine();
            Console.WriteLine(exp1.Expression);
        }
    }
}
