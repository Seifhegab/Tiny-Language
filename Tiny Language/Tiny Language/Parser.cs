using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tiny_Language
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        public List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        //Function Call
        Node Id()
        {
            Node id = new Node("id");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Comma && InputPointer < TokenStream.Count)
                {
                    id.Children.Add(match(Token_Class.T_Comma));
                    if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count)
                    {
                        id.Children.Add(match(Token_Class.T_Idenifier));
                        id.Children.Add(Id());
                    }
                    else if (TokenStream[InputPointer].token_type == Token_Class.T_Number && InputPointer < TokenStream.Count)
                    {
                        id.Children.Add(match(Token_Class.T_Number));
                        id.Children.Add(Id());
                    }
                }
                else
                {
                    return null;
                }
            }
            return id;
        }

        Node Function_call()
        {
            Node funcall = new Node("funcall");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count)
                {
                    funcall.Children.Add(match(Token_Class.T_Idenifier));
                    funcall.Children.Add(match(Token_Class.T_Left_Paranthesis));
                    if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count)
                    {
                        funcall.Children.Add(match(Token_Class.T_Idenifier));
                        funcall.Children.Add(Id());
                        funcall.Children.Add(match(Token_Class.T_Right_Paranthesis));
                    }
                    else if (TokenStream[InputPointer].token_type == Token_Class.T_Number && InputPointer < TokenStream.Count)
                    {
                        funcall.Children.Add(match(Token_Class.T_Number));
                        funcall.Children.Add(Id());
                        funcall.Children.Add(match(Token_Class.T_Right_Paranthesis));
                    }
                }
            }
            return funcall;
        }

        // Term
        Node Term()
        {
            Node term = new Node("term");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Number && InputPointer < TokenStream.Count)
                {
                    term.Children.Add(match(Token_Class.T_Number));
                }
                else if ((TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count) &&
                    (TokenStream[InputPointer + 1].token_type == Token_Class.T_Left_Paranthesis && InputPointer + 1 < TokenStream.Count))
                {
                    term.Children.Add(Function_call());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier)
                {
                    term.Children.Add(match(Token_Class.T_Idenifier));
                }
            }
            return term;
        }

        // Equation

        Node Equ()
        {
            Node equ = new Node("equ");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Plus && InputPointer < TokenStream.Count)
                {
                    equ.Children.Add(match(Token_Class.T_Plus));
                    if (TokenStream[InputPointer].token_type == Token_Class.T_Left_Paranthesis && InputPointer < TokenStream.Count)
                    {
                        equ.Children.Add(match(Token_Class.T_Left_Paranthesis));
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                        equ.Children.Add(match(Token_Class.T_Right_Paranthesis));
                        equ.Children.Add(Equ());
                    }
                    else
                    {
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                    }
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Minus && InputPointer < TokenStream.Count)
                {
                    equ.Children.Add(match(Token_Class.T_Minus));
                    if (TokenStream[InputPointer].token_type == Token_Class.T_Left_Paranthesis && InputPointer < TokenStream.Count)
                    {
                        equ.Children.Add(match(Token_Class.T_Left_Paranthesis));
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                        equ.Children.Add(match(Token_Class.T_Right_Paranthesis));
                        equ.Children.Add(Equ());
                    }
                    else
                    {
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                    }
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Multiply && InputPointer < TokenStream.Count)
                {
                    equ.Children.Add(match(Token_Class.T_Multiply));
                    if (TokenStream[InputPointer].token_type == Token_Class.T_Left_Paranthesis && InputPointer < TokenStream.Count)
                    {
                        equ.Children.Add(match(Token_Class.T_Left_Paranthesis));
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                        equ.Children.Add(match(Token_Class.T_Right_Paranthesis));
                        equ.Children.Add(Equ());
                    }
                    else
                    {
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                    }
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Divide && InputPointer < TokenStream.Count)
                {
                    equ.Children.Add(match(Token_Class.T_Divide));
                    if (TokenStream[InputPointer].token_type == Token_Class.T_Left_Paranthesis && InputPointer < TokenStream.Count)
                    {
                        equ.Children.Add(match(Token_Class.T_Left_Paranthesis));
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                        equ.Children.Add(match(Token_Class.T_Right_Paranthesis));
                        equ.Children.Add(Equ());
                    }
                    else
                    {
                        equ.Children.Add(Term());
                        equ.Children.Add(Equ());
                    }
                }
                else
                {
                    return null;
                }
            }
            return equ;
        }

        Node Equation()
        {
            Node equation = new Node("equation");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Left_Paranthesis && InputPointer < TokenStream.Count)
                {
                    equation.Children.Add(match(Token_Class.T_Left_Paranthesis));
                    equation.Children.Add(Term());
                    equation.Children.Add(Equ());
                    equation.Children.Add(match(Token_Class.T_Right_Paranthesis));
                    equation.Children.Add(Equ());
                }
                else
                {
                    equation.Children.Add(Term());
                    equation.Children.Add(Equ());
                }
            }
            return equation;
        }

        //Expression
        Node Expression()
        {
            Node expression = new Node("expression");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_String_Sentence && InputPointer < TokenStream.Count)
                {
                    expression.Children.Add(match(Token_Class.T_String_Sentence));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Number && InputPointer < TokenStream.Count)
                {
                    expression.Children.Add(Equation());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Left_Paranthesis && InputPointer < TokenStream.Count)
                {
                    expression.Children.Add(Equation());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count)
                {
                    expression.Children.Add(Equation());
                }
            }
            return expression;
        }

        //Assignment-statement 
        Node Assignment_statement()
        {
            Node assigstat = new Node("assigstat");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count)
                {
                    assigstat.Children.Add(match(Token_Class.T_Idenifier));
                    assigstat.Children.Add(match(Token_Class.T_Equal_Assignment));
                    assigstat.Children.Add(Expression());
                    //important
                    assigstat.Children.Add(match(Token_Class.T_Semicolon));
                }
            }
            return assigstat;
        }

        // DataType
        Node Datatype()
        {
            Node datatype = new Node("datatype");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_int && InputPointer < TokenStream.Count)
                {
                    datatype.Children.Add(match(Token_Class.T_int));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_float && InputPointer < TokenStream.Count)
                {
                    datatype.Children.Add(match(Token_Class.T_float));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_string && InputPointer < TokenStream.Count)
                {
                    datatype.Children.Add(match(Token_Class.T_string));
                }
            }
            return datatype;
        }

        //Declaration-statement
        Node Assign()
        {
            Node assign = new Node("assign");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Comma && InputPointer < TokenStream.Count)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.T_Idenifier && InputPointer + 1 < TokenStream.Count)
                    {
                        if (TokenStream[InputPointer + 2].token_type == Token_Class.T_Equal_Assignment && InputPointer + 2 < TokenStream.Count)
                        {
                            assign.Children.Add(match(Token_Class.T_Comma));
                            assign.Children.Add(Assignment_statement());
                            assign.Children.Add(Assign());
                        }
                        else
                        {
                            assign.Children.Add(match(Token_Class.T_Comma));
                            assign.Children.Add(match(Token_Class.T_Idenifier));
                            assign.Children.Add(Assign());
                        }
                    }
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Equal_Assignment && InputPointer < TokenStream.Count)
                {
                    assign.Children.Add(match(Token_Class.T_Equal_Assignment));
                    assign.Children.Add(Expression());
                    assign.Children.Add(Assign());
                }
                else
                {
                    return null;
                }
            }
            return assign;
        }

        Node Declaration_statement()
        {
            Node declstat = new Node("declstat");
            if (InputPointer < TokenStream.Count)
            {
                declstat.Children.Add(Datatype());
                declstat.Children.Add(match(Token_Class.T_Idenifier));
                declstat.Children.Add(Assign());
                declstat.Children.Add(match(Token_Class.T_Semicolon));
            }
            return declstat;
        }

        //Write-Statement 

        Node WriteS()
        {
            Node writes = new Node("writes");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_endl && InputPointer < TokenStream.Count)
                {
                    writes.Children.Add(match(Token_Class.T_endl));
                }
                else
                {
                    writes.Children.Add(Expression());
                }
            }
            return writes;
        }

        Node Write_Statement()
        {
            Node wristat = new Node("wristat");
            if (InputPointer < TokenStream.Count)
            {
                wristat.Children.Add(match(Token_Class.T_write));
                wristat.Children.Add(WriteS());
                wristat.Children.Add(match(Token_Class.T_Semicolon));
            }
            return wristat;
        }

        //Read-Statement 
        Node Read_Statement()
        {
            Node readstat = new Node("readstat");
            if (InputPointer < TokenStream.Count)
            {
                readstat.Children.Add(match(Token_Class.T_read));
                readstat.Children.Add(match(Token_Class.T_Idenifier));
                readstat.Children.Add(match(Token_Class.T_Semicolon));
            }
            return readstat;
        }

        //Return-Statement 
        Node Return_Statement()
        {
            Node retstat = new Node("retstat");
            if (InputPointer < TokenStream.Count)
            {
                retstat.Children.Add(match(Token_Class.T_return));
                retstat.Children.Add(Expression());
                retstat.Children.Add(match(Token_Class.T_Semicolon));
            }
            return retstat;
        }

        //Condition
        Node Condition()
        {
            Node condition = new Node("condition");
            if (InputPointer < TokenStream.Count)
            {
                condition.Children.Add(match(Token_Class.T_Idenifier));

                if (TokenStream[InputPointer].token_type == Token_Class.T_LessThan && InputPointer < TokenStream.Count)
                {
                    condition.Children.Add(match(Token_Class.T_LessThan));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_GreaterThan && InputPointer < TokenStream.Count)
                {
                    condition.Children.Add(match(Token_Class.T_GreaterThan));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Equal_Operator && InputPointer < TokenStream.Count)
                {
                    condition.Children.Add(match(Token_Class.T_Equal_Operator));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_NotEqual && InputPointer < TokenStream.Count)
                {
                    condition.Children.Add(match(Token_Class.T_NotEqual));
                }

                condition.Children.Add(Term());
            }
            return condition;
        }

        //Condition_statement

        Node Cond()
        {
            Node cond = new Node("cond");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_And && InputPointer < TokenStream.Count)
                {
                    cond.Children.Add(match(Token_Class.T_And));
                    cond.Children.Add(Condition());
                    cond.Children.Add(Cond());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_OR && InputPointer < TokenStream.Count)
                {
                    cond.Children.Add(match(Token_Class.T_OR));
                    cond.Children.Add(Condition());
                    cond.Children.Add(Cond());
                }
                else
                {
                    return null;
                }
            }
            return cond;
        }

        Node Condition_statement()
        {
            Node condstat = new Node("condstat");
            if (InputPointer < TokenStream.Count)
            {
                condstat.Children.Add(Condition());
                condstat.Children.Add(Cond());
            } 
            return condstat;
        }

        //If_statement

        Node Statementss()
        {
            Node statementss = new Node("statementss");
            if (InputPointer < TokenStream.Count)
            {
                //Write_Statement
                if (TokenStream[InputPointer].token_type == Token_Class.T_write && InputPointer < TokenStream.Count)
                {
                    statementss.Children.Add(Write_Statement());
                    statementss.Children.Add(Statementss());
                }
                //Read_Statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_read && InputPointer < TokenStream.Count)
                {
                    statementss.Children.Add(Read_Statement());
                    statementss.Children.Add(Statementss());
                }
                //Return_Statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_return && InputPointer < TokenStream.Count)
                {
                    statementss.Children.Add(Return_Statement());
                    statementss.Children.Add(Statementss());
                }
                //Repeat_statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_repeat && InputPointer < TokenStream.Count)
                {
                    statementss.Children.Add(Repeat_statement());
                    statementss.Children.Add(Statementss());
                }
                //If_statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_if && InputPointer < TokenStream.Count)
                {
                    statementss.Children.Add(If_statement());
                    statementss.Children.Add(Statementss());
                }
                //Assignment_statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count)
                {
                    statementss.Children.Add(Assignment_statement());
                    statementss.Children.Add(Statementss());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_int && InputPointer < TokenStream.Count ||
                         TokenStream[InputPointer].token_type == Token_Class.T_float && InputPointer < TokenStream.Count ||
                         TokenStream[InputPointer].token_type == Token_Class.T_string && InputPointer < TokenStream.Count)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.T_Idenifier && InputPointer + 1 < TokenStream.Count)
                    {
                        //Function_Statement();
                        if (TokenStream[InputPointer + 2].token_type == Token_Class.T_Left_Paranthesis && InputPointer + 2 < TokenStream.Count)
                        {
                            statementss.Children.Add(Function_Statement());
                            statementss.Children.Add(Statementss());
                        }
                        //Declaration_statement();
                        else
                        {
                            statementss.Children.Add(Declaration_statement());
                            statementss.Children.Add(Statementss());
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            return statementss;
        }

        Node Elstat()
        {
            Node elstat = new Node("elstat");
            if (InputPointer < TokenStream.Count)
            {
                //Elseif_statement
                if (TokenStream[InputPointer].token_type == Token_Class.T_elseif && InputPointer < TokenStream.Count)
                {
                    elstat.Children.Add(Elseif_statement());
                }
                //Else_statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_else && InputPointer < TokenStream.Count)
                {
                    elstat.Children.Add(Else_statement());
                }
                // End Token
                else if (TokenStream[InputPointer].token_type == Token_Class.T_end && InputPointer < TokenStream.Count)
                {
                    elstat.Children.Add(match(Token_Class.T_end));
                }
                else
                {
                    return null;
                }
            }
            return elstat;
        }

        Node If_statement()
        {

            Node ifstat = new Node("ifstat");
            if (InputPointer < TokenStream.Count)
            {
                ifstat.Children.Add(match(Token_Class.T_if));
                ifstat.Children.Add(Condition_statement());
                ifstat.Children.Add(match(Token_Class.T_then));
                ifstat.Children.Add(Statementss());
                ifstat.Children.Add(Elstat());
            }
            return ifstat;
        }

        //Elseif_statement
        Node Elseif_statement()
        {
            Node elsifstat = new Node("elsifstat");
            if (InputPointer < TokenStream.Count)
            {
                elsifstat.Children.Add(match(Token_Class.T_elseif));
                elsifstat.Children.Add(Condition_statement());
                elsifstat.Children.Add(match(Token_Class.T_then));
                elsifstat.Children.Add(Statementss());
                elsifstat.Children.Add(Elstat());
            }
            return elsifstat;
        }

        //Else_statement
        Node Else_statement()
        {
            Node elsstat = new Node("elsstat");
            if (InputPointer < TokenStream.Count)
            {
                elsstat.Children.Add(match(Token_Class.T_else));
                elsstat.Children.Add(Statementss());
                elsstat.Children.Add(match(Token_Class.T_end));
            }
            return elsstat;
        }

        //Repeat_statement
        Node Repeat_statement()
        {
            Node repstat = new Node("repstat");
            if (InputPointer < TokenStream.Count)
            {
                repstat.Children.Add(match(Token_Class.T_repeat));
                repstat.Children.Add(Statementss());
                repstat.Children.Add(match(Token_Class.T_until));
                repstat.Children.Add(Condition_statement());
            }
            return repstat;
        }

        // Function-Name
        Node Function_Name()
        {
            Node funcname = new Node("funcname");
            if (InputPointer < TokenStream.Count)
            {
                funcname.Children.Add(match(Token_Class.T_Idenifier));
            }
            return funcname;
        }

        // Parameters
        Node Parameters()
        {
            Node param = new Node("param");
            if (InputPointer < TokenStream.Count)
            {
                param.Children.Add(Datatype());
                param.Children.Add(match(Token_Class.T_Idenifier));
            }
            return param;
        }

        //Function_Declaration
        Node Fuctions()
        {
            Node functions = new Node("functions");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_Comma && InputPointer < TokenStream.Count)
                {
                    functions.Children.Add(match(Token_Class.T_Comma));
                    functions.Children.Add(Parameters());
                    functions.Children.Add(Fuctions());
                }
                else
                {
                    return null;
                }
            }
            return functions;
        }

        Node Function_Declaration()
        {
            Node funcdec = new Node("funcdec");
            if (InputPointer < TokenStream.Count)
            {
                funcdec.Children.Add(Datatype());
                funcdec.Children.Add(Function_Name());
                funcdec.Children.Add(match(Token_Class.T_Left_Paranthesis));
                if (TokenStream[InputPointer].token_type == Token_Class.T_int && InputPointer < TokenStream.Count ||
                         TokenStream[InputPointer].token_type == Token_Class.T_float && InputPointer < TokenStream.Count ||
                         TokenStream[InputPointer].token_type == Token_Class.T_string && InputPointer < TokenStream.Count)
                {
                    funcdec.Children.Add(Parameters());
                    funcdec.Children.Add(Fuctions());
                }
                funcdec.Children.Add(match(Token_Class.T_Right_Paranthesis));
            }
            return funcdec;
        }

        // Function_Body
        Node Func_Statementss()
        {
            Node funcstatementss = new Node("funcstatementss");
            if (InputPointer < TokenStream.Count)
            {
                //Write_Statement
                if (TokenStream[InputPointer].token_type == Token_Class.T_write && InputPointer < TokenStream.Count)
                {
                    funcstatementss.Children.Add(Write_Statement());
                    funcstatementss.Children.Add(Func_Statementss());
                }
                //Read_Statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_read && InputPointer < TokenStream.Count)
                {
                    funcstatementss.Children.Add(Read_Statement());
                    funcstatementss.Children.Add(Func_Statementss());
                }
                //Repeat_statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_repeat && InputPointer < TokenStream.Count)
                {
                    funcstatementss.Children.Add(Repeat_statement());
                    funcstatementss.Children.Add(Func_Statementss());
                }
                //If_statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_if && InputPointer < TokenStream.Count)
                {
                    funcstatementss.Children.Add(If_statement());
                    funcstatementss.Children.Add(Func_Statementss());
                }
                //Assignment_statement
                else if (TokenStream[InputPointer].token_type == Token_Class.T_Idenifier && InputPointer < TokenStream.Count)
                {
                    funcstatementss.Children.Add(Assignment_statement());
                    funcstatementss.Children.Add(Func_Statementss());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.T_int && InputPointer < TokenStream.Count ||
                         TokenStream[InputPointer].token_type == Token_Class.T_float && InputPointer < TokenStream.Count ||
                         TokenStream[InputPointer].token_type == Token_Class.T_string && InputPointer < TokenStream.Count)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.T_Idenifier && InputPointer + 1 < TokenStream.Count)
                    {
                        //Function_Statement();
                        if (TokenStream[InputPointer + 2].token_type == Token_Class.T_Left_Paranthesis && InputPointer + 2 < TokenStream.Count)
                        {
                            funcstatementss.Children.Add(Function_Statement());
                            funcstatementss.Children.Add(Func_Statementss());
                        }
                        //Declaration_statement();
                        else
                        {
                            funcstatementss.Children.Add(Declaration_statement());
                            funcstatementss.Children.Add(Func_Statementss());
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            return funcstatementss;
        }

        Node Function_Body()
        {
            Node funcbody = new Node("funcbody");
            if (InputPointer < TokenStream.Count)
            {
                funcbody.Children.Add(match(Token_Class.T_Left_CurlyBrackets));
                funcbody.Children.Add(Func_Statementss());
                funcbody.Children.Add(Return_Statement());
                funcbody.Children.Add(match(Token_Class.T_Right_CurlyBrackets));
            }
            return funcbody;
        }

        //Function_Statement
        Node Function_Statement()
        {
            Node funcstat = new Node("funcstat");
            if (InputPointer < TokenStream.Count)
            {
                funcstat.Children.Add(Function_Declaration());
                funcstat.Children.Add(Function_Body());
            }
            return funcstat;
        }

        //Main_Function
        Node Main_Function()
        {
            Node mainf = new Node("mainf");
            if (InputPointer < TokenStream.Count)
            {
                mainf.Children.Add(Datatype());
                mainf.Children.Add(match(Token_Class.T_Main));
                mainf.Children.Add(match(Token_Class.T_Left_Paranthesis));
                mainf.Children.Add(match(Token_Class.T_Right_Paranthesis));
                mainf.Children.Add(Function_Body());
            }
            return mainf;
        }
        bool noMain = false;
        // Program
        Node Program()
        {
            Node program = new Node("Program");
            if (InputPointer < TokenStream.Count)
            {
                program.Children.Add(Prog());

                if (noMain == false) {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + Token_Class.T_Main.ToString() +"\r\n");
                }

                //program.Children.Add(Main_Function());
                MessageBox.Show("Success");
            }
            return program;
        }

        Node Prog()
        {
            Node prog = new Node("prog");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.T_int && InputPointer < TokenStream.Count ||
                     TokenStream[InputPointer].token_type == Token_Class.T_float && InputPointer < TokenStream.Count ||
                     TokenStream[InputPointer].token_type == Token_Class.T_string && InputPointer < TokenStream.Count)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.T_Main && InputPointer + 1 < TokenStream.Count)
                    {
                        noMain = true;
                        prog.Children.Add(Main_Function());
                    }
                    else
                    {
                        prog.Children.Add(Function_Statement());
                        prog.Children.Add(Prog());
                    }

                }
                else
                {
                    return null;
                }
            }
            return prog;
        }



//////////////////////////////////////////////////////////////////////////////////////////////////////////


        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }


        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
