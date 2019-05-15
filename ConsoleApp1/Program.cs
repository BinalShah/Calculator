using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   
    public class Calculator
    {
        Regex rx = new Regex("\\d+");
        private Hashtable context_dict = new Hashtable();
       
        private double evaluate(object expList)
        {
            if (expList is string)
            {
                MatchCollection matches = rx.Matches(expList.ToString());
                if (matches.Count > 0)
                {
                    return int.Parse(expList.ToString());
                }
                else
                {
                    return (double)(context_dict[expList.ToString()]);
                }
            }
            else
            {
                ArrayList exp_list = (ArrayList)expList;
               
                string op = exp_list[0].ToString();
                object exp = exp_list[1];
                if (op.Equals("root", StringComparison.CurrentCultureIgnoreCase))
                {
                    return evaluate(exp);
                }
                else if (op.Equals("add", StringComparison.CurrentCultureIgnoreCase))
                {
                    return evaluate(exp) + evaluate(exp_list[2]);
                }
                else if (op.Equals("mult", StringComparison.CurrentCultureIgnoreCase))
                {
                    return evaluate(exp) * evaluate(exp_list[2]);
                }
                else if (op.Equals("div", StringComparison.CurrentCultureIgnoreCase))
                {
                    return evaluate(exp) / evaluate(exp_list[2]);
                }
                else if (op.Equals("let", StringComparison.CurrentCultureIgnoreCase))
                {
                    if(context_dict.ContainsKey(exp))
                    {
                        context_dict[exp] = evaluate(exp_list[2]);
                    }
                    else
                    {
                        context_dict.Add(exp, evaluate(exp_list[2]));
                    }
                    
                    return evaluate(exp_list[3]);
                }
                return 0;
            }
        }

        private object[] parse_exp_str(string exp_str, string func_name, int index)
        {
            ArrayList exp_list = new ArrayList();

	    exp_list.Add(func_name);

	    string tmp_str = "";
	    try{
                while (true)
                {
                    char currentChar = exp_str[index];
                    if (currentChar == '(')
                    {
                        object[] returned = parse_exp_str(exp_str, tmp_str, index + 1);
                        index = (int)returned[1];
                        exp_list.Add(returned[0]);
                        tmp_str = "";
                        if (func_name.Equals("root", StringComparison.CurrentCultureIgnoreCase)) break;
                    }
                    else if (currentChar == ')')
                    {
                        if (tmp_str.Length > 0) exp_list.Add(tmp_str);
                        break;
                    }
                    else if (currentChar == ',')
                    {
                        if (tmp_str.Length > 0) exp_list.Add(tmp_str);
                        tmp_str = "";
                    }
                    else
                    {
                        tmp_str += exp_str[index];
                    }
                    index++;
                }
            }
            catch (Exception e) {
				    	
		}

	    return new object[]{exp_list, index};  
	}

        public double evaluateExpression(string expression)
        {
		return evaluate(parse_exp_str(expression.Replace(" ", ""), "root", 0)[0]);
	}

    static void Main(string[] args)
        {
            Console.WriteLine("Enter expression: ");
            string temp = Console.ReadLine();
            Calculator impl = new Calculator();
            try
            {
                double result = impl.evaluateExpression(temp);
                Console.WriteLine(temp + " ==> " + result);
                System.Threading.Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadLine();
            }

         }
    }
}
