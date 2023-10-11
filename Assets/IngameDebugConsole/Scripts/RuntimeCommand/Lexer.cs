using System.Collections.Generic;

namespace IngameDebugConsole
{
    public class Lexer
    {

        private string code;
        private int curIndex;

        public Lexer(string code)
        {
            this.code = code;
            Line = 1;
            curIndex = 0;
        }

        public int Line
        {
            get;
            private set;
        }

        private void NextToken(out int line, out string token, out TokenType type)
        {
            SkipWhiteSpace();
            line = Line;
            token = default;
            type = default;

            if (curIndex >= code.Length)
            {
                type = TokenType.Eof;
                token = type.ToString();
                return;
            }

            char c = code[curIndex];
            
            //分隔符,运算符和字符串
            switch (c)
            {
                case ';':
                    Next(1);
                    type = TokenType.SepSemi;
                    token = ";";
                    return;

                case ',':
                    Next(1);
                    type = TokenType.SepComma;
                    token = ",";
                    return;

                case '(':
                    Next(1);
                    type = TokenType.SepLparen;
                    token = "(";
                    return;

                case ')':
                    Next(1);
                    type = TokenType.SepRparen;
                    token = ")";
                    return;

                case ']':
                    Next(1);
                    type = TokenType.SepRbrack;
                    token = "]";
                    return;

                case '{':
                    Next(1);
                    type = TokenType.SepLcurly;
                    token = "{";
                    return;

                case '}':
                    Next(1);
                    type = TokenType.SepRcurly;
                    token = "}";
                    return;

                case '+':
                    if (Test("++"))
                    {
                        Next(2);
                        type = TokenType.OpAddOne;
                        token = "++";
                        return;
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpAdd;
                        token = "+";
                        return;
                    }

                case '-':
                    if (Test("--"))
                    {
                        Next(2);
                        type = TokenType.OpMinusOne;
                        token = "--";
                        return;
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpMinus;
                        token = "-";
                        return;
                    }
                case '*':
                    Next(1);
                    type = TokenType.OpMul;
                    token = "*";
                    return;
                case '^':
                    Next(1);
                    type = TokenType.OpPow;
                    token = "^";
                    return;
                case '%':
                    Next(1);
                    type = TokenType.OpMod;
                    token = "&";
                    return;
                case '&':
                    if (Test("&&"))
                    {
                        Next(2);
                        type = TokenType.OpAnd;
                        token = "&&";
                        return;
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpBAnd;
                        token = "&";
                        return;
                    }
                case '|':
                    if (Test("||"))
                    {
                        Next(2);
                        type = TokenType.OpOr;
                        token = "||";
                        return;
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpBOr;
                        token = "|";
                        return;
                    }
                case '#':
                    Next(1);
                    type = TokenType.OpLen;
                    token = "#";
                    return;

                case ':':
                    Next(1);
                    type = TokenType.SepColon;
                    token = ":";
                    return;

                case '/':

                    Next(1);
                    type = TokenType.OpDiv;
                    token = "/";
                    return;

                case '!':
                    if (Test("!="))
                    {
                        Next(2);
                        type = TokenType.OpNe;
                        token = "!=";
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpNot;
                        token = "!";
                    }

                    return;

                case '=':
                    if (Test("=="))
                    {
                        Next(2);
                        type = TokenType.OpEq;
                        token = "==";
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpAsssign;
                        token = "=";
                    }

                    return;

                case '<':
                    if (Test("<<"))
                    {
                        Next(2);
                        type = TokenType.OpShL;
                        token = "<<";
                    }
                    else if (Test("<="))
                    {
                        Next(2);
                        type = TokenType.OpLe;
                        token = "<=";
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpLt;
                        token = "<";
                    }

                    return;

                case '>':
                    if (Test(">>"))
                    {
                        Next(2);
                        type = TokenType.OpShR;
                        token = ">>";
                    }
                    else if (Test(">="))
                    {
                        Next(2);
                        type = TokenType.OpGe;
                        token = ">=";
                    }
                    else
                    {
                        Next(1);
                        type = TokenType.OpGt;
                        token = ">";
                    }
                    return;
                case '.':
                    Next(1);
                    type = TokenType.SepDot;
                    token = ".";
                    return;
                case '[':
                    Next(1);
                    type = TokenType.SepLbrack;
                    token = "[";
                    return;
                case '"':
                    Next(1);
                    type = TokenType.String;
                    token = ScanString();
                    return;
                case '$':
                    if (Test("$\""))
                    {
                        Next(2);
                        type = TokenType.String;
                        token = ScanString();
                        return; 
                    }
                    break;
            }

        }
        
        /// <summary>
        /// 跳过n个字符
        /// </summary>
        private void Next(int n)
        {
            curIndex += n;
        }

        private string ScanString()
        {
            return "";
        }
        
        /// <summary>
        /// 剩余的源代码是否以s开头
        /// </summary>
        private bool Test(string s)
        {
            return code.Substring(curIndex).StartsWith(s);
        } 

        /// <summary>
        /// 跳过空白字符，回车，换行与注释
        /// </summary> 
        private void SkipWhiteSpace()
        {
            while (curIndex < code.Length)
            {
                if (Test("//"))
                {
                    //跳过注释
                    SkipComment();
                    continue;
                }

                if (Test("\r\n") || Test("\n\r"))
                {
                    //跳过回车与换行
                    Next(2);
                    Line++;
                    continue;
                }

                if (IsNewLine(code[curIndex]))
                {
                    //跳过回车或换行
                    Next(1);
                    Line++;
                    continue;
                }

                if (IsWhiteSpace(code[curIndex]))
                {
                    //跳过空白字符
                    Next(1);
                    continue;
                }

                return;
            } 
        }

        /// <summary>
        /// 跳过注释
        /// </summary>
        private void SkipComment()
        {
            while (curIndex < code.Length)
            {
                char c = code[curIndex];
                Next(1);
                if (IsNewLine(c))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 是否是回车或换行符
        /// </summary>
        private bool IsNewLine(char c)
        {
            return c == '\r' || c == '\n';
        } 
        
        private bool IsWhiteSpace(char c)
        {
            switch (c)
            {
                case '\t':
                case '\n':
                case '\v':
                case '\f':
                case '\r':
                case ' ':
                    return true;
            }

            return false;
        }
    }
    
    
}