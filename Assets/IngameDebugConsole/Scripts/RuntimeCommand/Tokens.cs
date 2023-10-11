namespace IngameDebugConsole
{
    public enum TokenType
    {
        Eof, //end-of-file
        Keyword,

        // 标识符（变量）
        Identifier,
        Operator,
        Number,
        String,


        SepSemi, //;
        SepComma, //,
        SepDot, //.
        SepColon, //:
        SepLparen, //(
        SepRparen, //)
        SepLbrack, //[
        SepRbrack, //]
        SepLcurly, //{
        SepRcurly, //}


        OpAsssign, //=
        OpAdd, //+
        OpMinus, //-
        OpMul, //*
        OpDiv, // /
        OpAddOne, // ++
        OpMinusOne, // --
        OpPow, //^
        OpMod, //%
        OpBAnd, //&
        OpBOr, //|
        OpShR, //>>
        OpShL, //<<
        OpLt, //<
        OpLe, //<=
        OpGt, //>
        OpGe, //>=
        OpEq, //==
        OpNe, //!=
        OpLen, //#
        OpAnd, // &&
        OpOr, // ||
        OpNot, // !
        OpNumNegative = OpMinus, // 负号


        KwBreak, //break
        KwContinue,
        KwDo, //do
        KwIf, //if
        KwElse, //else
        KwElseif, //elseif
        KwFalse, //false
        KwTrue, //true
        KwFor, //for
        KwVoid,
        KwPrivate,
        KwPublic,
        KwProtected,
        KwNull, //null
        KwReturn, //return
        KwWhile, //while
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
    }
}