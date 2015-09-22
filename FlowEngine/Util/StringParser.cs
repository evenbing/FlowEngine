using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Util
{
    class StringParser
    {
        private string s, dCh, qCh;
        private List tokens;
        int tokPos = 0;
        char[] curToken = new char[50];
        IEnumerator enumvar;

        public StringParser(String s, char delimCh, char quoteCh)
        {
            parse(s, delimCh.ToString(), quoteCh.ToString(), true);
        }

        public StringParser(String s, String delimCh, String quoteCh, bool inclQuoteCh)
        {
            parse(s, delimCh, quoteCh, inclQuoteCh);
        }

        private void parse(String s, String delimCh, String quoteCh, bool inclQuoteCh)
        {
            this.s = s;
            this.dCh = delimCh;
            this.qCh = quoteCh;
            this.tokens = new List();
            bool inLiteral = false;
            char quoteChar = '\0';
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if (ch == quoteChar)
                {
                    inLiteral = false;
                    quoteChar = '\0';
                    if (inclQuoteCh)
                    {
                        addTokChar(ch);
                    }
                }
                else if ((quoteChar == 0) && (this.qCh.IndexOf(ch) != -1))
                {
                    quoteChar = ch;
                    inLiteral = true;
                    if (inclQuoteCh)
                    {
                        addTokChar(ch);
                    }
                }
                else if (this.dCh.IndexOf(ch) != -1)
                {
                    if (i > 0)
                    {
                        if (inLiteral)
                        {
                            addTokChar(ch);
                        }
                        else
                        {
                            this.tokens.addElement(getTokString());
                            this.tokPos = 0;
                        }
                    }
                }
                else if (ch == '\\')
                {
                    if ((i < s.Length - 1) && (((!inLiteral) && (this.dCh.IndexOf(s[i + 1]) >= 0)) || (this.qCh.IndexOf(s[i + 1]) >= 0)))
                    {
                        addTokChar(s[++i]);
                    }
                    else
                    {
                        addTokChar(ch);
                    }
                }
                else
                {
                    addTokChar(ch);
                }
            }
            if (this.tokPos > 0)
            {
                this.tokens.addElement(getTokString());
            }
            this.enumvar = tokens.elements();
        }

        public void addTokChar(char ch)
        {
            if (tokPos >= curToken.Length)
            {
                char[] newTok = new char[curToken.Length * 2];
                Array.Copy(curToken, 0, newTok, 0, curToken.Length);
                curToken = newTok;
            }
            curToken[(tokPos++)] = ch;
        }

        public String getTokString()
        {
            return new string(curToken, 0, tokPos);
        }
        
        public bool moveNextToken()
        {
            return enumvar.MoveNext();
        }

        public string currentToken()
        {
            return enumvar.Current as string;
        }
    }
}
