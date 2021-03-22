using System;
using System.Collections;


namespace calculator_v1
{
    public class Calculate
    {
        readonly private string _chars = "0123456789+-/*()"; //mask "vector" contain with allowed signs.
        readonly private string _numbers = "0123456789"; //string with only numbers
        readonly private string[] _array_operators = { "(", "+-)", "*/" }; // string array witch growing order priority operations

        private char[] _ch_expression; //_ch_expression spread over sign boards.
        private string _expression; //field witch expresions, is fills up by property Expresion 

        Queue _num_queue = new Queue();
        Stack _operator_stack = new Stack();

        int _op_or1 = 0;
        int _op_or2 = 0;
        string bin;

        public Calculate()
        {
            _expression = null;
        }
        /*property below set value class _expression field, after earlier check and adjustment value 
          to requirements (checking signs if belong to the collection contained in __chars string, 
          and it removes all spaces from expression)*/
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
                        if (!(SignBelongs(_chars, _ch_expression[num]))) //if return !(false), will be call ArgumentException
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
        //method accepts string with allowed signs, and specific checked actually sign, if sign contain 
        //out inside of the string then method will return false
        private bool SignBelongs(string vector, char sign)
        {
            if (vector.Contains(sign))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*method check operator priority in array operators array, where operators priority be at the same 
         *time index array. operators are arranged in an array in growing order.
         *Method CheckPriorityOperator is used in*/
        /* 
         private byte CheckPriorityOperator(string[] signArray, char sign) 
         {
             for (byte _i = 0; _i<= signArray.Length; _i++ )
             {
                 string stringToSignArray = signArray[_i];
                 if (SignBelongs(stringToSignArray, sign)) 
                 {
                     return _i;
                 }
                 if (SignBelongs(stringToSignArray, sign))  
                 {
                     return _i;
                 }
                 if (SignBelongs(stringToSignArray, sign)) 
                 {
                     return _i;
                 }
             }
             return 100;
         }
        */
        /*Method for checking prioritet between two operator, method uses to decidet that operator 
         *downloaded from expression has high/lower prioroty than operator on stack*/
        private bool CheckPrioryty(string[] array_operator, char char_expression)
        {
            if (_operator_stack.Count == 0)
            {
                return false;
            }
            else
            {
                _op_or1 = 0;
                _op_or2 = 0;
                for (int _i = 0; _i <= array_operator.Length - 1; _i++)
                {
                    if (array_operator[_i].Contains(char_expression))
                    {
                        _op_or1 = _i;
                    }
                    if (array_operator[_i].Contains(Convert.ToChar(_operator_stack.Peek())))
                    {
                        _op_or2 = _i;
                    }
                }
            }
            if (_op_or1 <= _op_or2)
            {
                return false;
            }
            return true;
        }
        /*Method decompositing expressing to single components and lays on the stack and queue*/
        public void DecompositionExpression()
        {
            _ch_expression = _expression.ToCharArray();
            int _i = 0;

            //Loop to count index sign in expression for decomposition them.
            for (int _n = 0; _n <= _ch_expression.Length - 1; _n++)
            {
                string _str_temp = _ch_expression[_n].ToString();
                //Condition for check if sign it is NUMBER
                if (SignBelongs(_numbers, _ch_expression[_n]))
                {
                    //CheckNextSign from block diagram
                    _i = _n;

                    //While loop for check if next sign, it's the number, if yes add sign to _str_temp for later convert _str_temp to int
                    while ((_i + 1 <= _ch_expression.Length - 1) && SignBelongs(_numbers, _ch_expression[_i + 1]))
                    {
                        _i = _i + 1;
                        _str_temp = _str_temp + _ch_expression[_i].ToString();
                    }
                    _n = _i;
                    //End CheckNextSign from block diagram
                    _num_queue.Enqueue(Int32.Parse(_str_temp));
                }
                //if not a number so it is OPERATOR
                else
                {
                    //first check if sign is ")", for notation without brackets 
                    //tutaj porobiłem zmiany zasadnicze
                    if (_ch_expression[_n] == Convert.ToChar(")"))
                    {
                        if (!(_operator_stack.Peek().ToString() == "("))
                        {
                            _num_queue.Enqueue(_operator_stack.Pop());
                        }

                    }
                    else
                    {
                       // while (CheckPrioryty(_array_operators, _ch_expression[_n]))
                        //{
                            if (!(_operator_stack.Count == 0))
                            {
                                if (_operator_stack.Peek().ToString() == "(")
                                {
                                    _operator_stack.Pop();
                                }
                                else
                                {
                                do
                                {
                                  if (CheckPrioryty(_array_operators, _ch_expression[_n]))
                                    {
     
                                        _operator_stack.Push(_ch_expression[_n]);
                                    }
                                    else
                                    {
                                        _num_queue.Enqueue(_operator_stack.Pop());
                                    }
                                } while (CheckPrioryty(_array_operators, _ch_expression[_n]) && !(_operator_stack.Count == 0));
                                }
                            }
                       // }
                        _operator_stack.Push(_ch_expression[_n]);
                    }
                    //CheckPrioryty from block diagram

                    _i = 0;
                }
            }
            //loop translatesall expresion parts after decomposition from stack to queue
            while (!(_operator_stack.Count == 0))
            {
                _num_queue.Enqueue(_operator_stack.Pop());
            }

            foreach (var ele in _num_queue)
            {
                Console.Write(ele);
                Console.Write(" ");
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
            exp1.DecompositionExpression();

            //zle działa należy poprzeprowadzać pruby z wyrażeniem (2+3)*5
        }
    }
}