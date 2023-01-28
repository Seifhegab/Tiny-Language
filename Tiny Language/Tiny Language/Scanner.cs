using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;


public enum Token_Class
{
   T_int, T_float, T_string , T_read , T_write , T_repeat , T_until , T_if , T_elseif , T_else , T_then , T_return , T_endl, T_Main, T_end,
  
   T_Dot, T_Semicolon, T_Comma, T_Left_Paranthesis, T_Right_Paranthesis, T_Equal_Assignment, T_Equal_Operator, T_LessThan,
   T_GreaterThan, T_NotEqual, T_Plus, T_Minus, T_Multiply, T_Divide, T_GreaterThanOrEqual, T_LessThanOrEqual, T_Left_CurlyBrackets,
   T_Right_CurlyBrackets, T_Left_SquareBrackets, T_Right_SquareBrackets, T_And, T_OR,


   T_Idenifier, T_Number, T_String_Sentence
}
namespace Tiny_Language
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.T_int);
            ReservedWords.Add("float", Token_Class.T_float);
            ReservedWords.Add("string", Token_Class.T_string);
            ReservedWords.Add("read", Token_Class.T_read);
            ReservedWords.Add("write", Token_Class.T_write);
            ReservedWords.Add("repeat", Token_Class.T_repeat);
            ReservedWords.Add("until", Token_Class.T_until);
            ReservedWords.Add("if", Token_Class.T_if);
            ReservedWords.Add("elseif", Token_Class.T_elseif);
            ReservedWords.Add("else", Token_Class.T_else);
            ReservedWords.Add("then", Token_Class.T_then);
            ReservedWords.Add("return", Token_Class.T_return);
            ReservedWords.Add("endl", Token_Class.T_endl);
            ReservedWords.Add("main", Token_Class.T_Main);
            ReservedWords.Add("end", Token_Class.T_end);

            Operators.Add(".", Token_Class.T_Dot);
            Operators.Add(";", Token_Class.T_Semicolon);
            Operators.Add(",", Token_Class.T_Comma);
            Operators.Add("(", Token_Class.T_Left_Paranthesis);
            Operators.Add(")", Token_Class.T_Right_Paranthesis);
            Operators.Add("{", Token_Class.T_Left_CurlyBrackets);
            Operators.Add("}", Token_Class.T_Right_CurlyBrackets);
            Operators.Add("[", Token_Class.T_Left_SquareBrackets);
            Operators.Add("]", Token_Class.T_Right_SquareBrackets);
            Operators.Add("=", Token_Class.T_Equal_Operator);
            Operators.Add("<", Token_Class.T_LessThan);
            Operators.Add(">", Token_Class.T_GreaterThan);
            Operators.Add("+", Token_Class.T_Plus);
            Operators.Add("-", Token_Class.T_Minus);
            Operators.Add("*", Token_Class.T_Multiply);
            Operators.Add("/", Token_Class.T_Divide);

            Operators.Add(":=", Token_Class.T_Equal_Assignment);
            Operators.Add("<>", Token_Class.T_NotEqual);
           // Operators.Add(">=", Token_Class.T_GreaterThanOrEqual);
           // Operators.Add("<=", Token_Class.T_LessThanOrEqual);
            Operators.Add("&&", Token_Class.T_And);
            Operators.Add("||", Token_Class.T_OR);
        }

        public void StartScanning(string SourceCode)
        {
            int num = 0;
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();
               
                if (CurrentChar == '\n')
                {
                    num++;
                }

                if (CurrentChar == ' ' || CurrentChar =='\n' || CurrentChar == '\r' || CurrentChar == '\t')
                {
                    continue;
                }
                // Reading Identifier
                else if (CurrentChar >= 'A' && CurrentChar <= 'Z' || CurrentChar >= 'a' && CurrentChar <= 'z') 
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if (j < SourceCode.Length)
                    {
                        while (CurrentChar >= 'A' && CurrentChar <= 'Z' || CurrentChar >= 'a' && CurrentChar <= 'z' || (CurrentChar >= '0' && CurrentChar <= '9'))
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                            CurrentChar = SourceCode[j];

                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                //Reading Number
                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if (j < SourceCode.Length)
                    {

                        while ((CurrentChar >= '0' && CurrentChar <= '9') || (CurrentChar == '.'))
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                            CurrentChar = SourceCode[j];
                        } 
                    }
                    while (j < SourceCode.Length)
                    {
                        if (CurrentChar == ' ' || CurrentChar == '\n' || CurrentChar == '\r' || CurrentChar == '\t' || CurrentChar == ';')
                        {
                            FindTokenClass(CurrentLexeme);
                            i = j - 1;
                            break;
                        }
                        else if(CurrentChar >= 'A' && CurrentChar <= 'Z' || CurrentChar >= 'a' && CurrentChar <= 'z')
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                            CurrentChar = SourceCode[j];
                        }
                        else
                        {
                            FindTokenClass(CurrentLexeme);
                            i = j - 1;
                            break;
                        }
                    } 
                }


                // Reading Comment
                else if (CurrentChar == '/')
                {
                    string s = CurrentChar.ToString();
                    int check = 1;
                    j++;
                    CurrentChar = SourceCode[j];
                    if (CurrentChar == '*')
                    {
                        s = s + CurrentChar.ToString();
                        j++;
                        CurrentChar = SourceCode[j];
                    LOOP:
                        if (j < SourceCode.Length)
                        {
                            while (CurrentChar != '*')
                            {
                                s = s + CurrentChar.ToString();
                                j++;
                                CurrentChar = SourceCode[j];
                            }
                            j++;
                            CurrentChar = SourceCode[j];
                            if (CurrentChar != '/')
                            {
                                goto LOOP;
                            }
                            else
                            {
                                j++;
                                CurrentChar = SourceCode[j];
                            }
                        }
                        else
                        {
                            Errors.Error_List.Add("Unidentified Token " + s);
                        }
                    }
                    else
                    {
                        FindTokenClass(CurrentLexeme);
                    }
                    i = j - 1;
                }

                // Reading Equal Assignment(:=)
                else if (CurrentChar == ':')
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if (j < SourceCode.Length)
                    {
                        if (CurrentChar == '=')
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                // Reading Notequal (<>)
                else if (CurrentChar == '<')
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if (j < SourceCode.Length)
                    {
                        if (CurrentChar == '>')
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                        }
                        //Reading Smaller than or eaual(<=)
                        else if (CurrentChar == '=')
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                // Reading Greater than or eaual (>=)
                else if (CurrentChar == '>')
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if (j < SourceCode.Length)
                    {
                        if (CurrentChar == '=')
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                // Reading And (&&)
                else if (CurrentChar == '&')
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if (j < SourceCode.Length)
                    {
                        if (CurrentChar == '&')
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                // Reading or (||)
                else if (CurrentChar == '|')
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if (j < SourceCode.Length)
                    {
                        if (CurrentChar == '|')
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                //Reading String
                else if (CurrentChar == '"')
                {
             
                    j = i + 1;
                    int check = 1;
                    CurrentChar = SourceCode[j];
                    while (CurrentChar != '"')
                    {
                        CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                        j++;
                        CurrentChar = SourceCode[j];
                        
                    }
                    if(CurrentChar == '"')
                    {
                        while (j < SourceCode.Length)
                        {
                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                            j++;
                            CurrentChar = SourceCode[j];
                            if (CurrentChar == '\n' || CurrentChar == '\r' || CurrentChar == '\t' || CurrentChar == ';')
                            {
                                if (check == 1)
                                { 
                                    Token Tok = new Token();
                                    Tok.lex = CurrentLexeme;
                                    Tok.token_type = Token_Class.T_String_Sentence;
                                    Tokens.Add(Tok);
                                    break;
                                }
                                else
                                {
                                    String s = CurrentLexeme;
                                    Errors.Error_List.Add("Unidentified Token " + s);
                                    break;
                                }
                            }
                            else
                            {
                                check = 0;
                                CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                                j++;
                                CurrentChar = SourceCode[j];
                            }
                        }
                    }
                    i = j - 1;
                }

                //Reading (.125) unrecognized token
                else if (CurrentChar == '.')
                {
                    j = i + 1;
                    CurrentChar = SourceCode[j];
                    if((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z'))
                    {
                        FindTokenClass(CurrentLexeme);
                        i = j - 1;
                    }
                    else
                    {
                        while (j < SourceCode.Length)
                        {
                            if (CurrentChar == ' ' || CurrentChar == '\n' || CurrentChar == '\r' || CurrentChar == '\t' || CurrentChar == ';')
                            {
                                FindTokenClass(CurrentLexeme);
                                i = j - 1;
                                break;
                            }
                            else 
                            {
                                CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                                j++;
                                CurrentChar = SourceCode[j];
                            }
                        }
                    }
                }    

                // No token
                else
                {
                    FindTokenClass(CurrentLexeme);
                }
            }
            Tiny_Language_Compiler.TokenStream = Tokens;
        }

        // Define Identifier
        bool isIdentifier(string lex)
        {
            Regex identifierRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$");
            if (identifierRegex.IsMatch(lex))
            {
                return true;
            }
            return false;
        }

        // Define Number
        bool isNumber(string lex)
        {
            Regex constantRegex = new Regex(@"^[0-9]+(\.[0-9]*)?$");
            if (constantRegex.IsMatch(lex))
                return true;
            return false;
        }


        //bool isString(string lex)
        //{
        //    Regex constantRegex = new Regex(@"^ \ / \*( . |  \s |") * \ * \ /? $");
        //    if (constantRegex.IsMatch(lex))
        //        return true;
        //    return false;
        //}
   

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }

            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }

            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.T_Idenifier;
                Tokens.Add(Tok);
            }

            //Is it a Constant?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.T_Number;
                Tokens.Add(Tok);
            }
            // No Token
            else
            {
                Errors.Error_List.Add("Unidentified Token " + Lex);
            }

        }
    }
}
