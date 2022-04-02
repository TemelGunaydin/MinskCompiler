using System;

namespace mc
{
  class Program
  {
    static void Main(string[] args)
    {

      //1 Test amaçlı yapıldı . Sadece altta belirtilen ifadeyi girince cevap olarak 7 dönüyor.
      while (true)
      {
        Console.Write("> ");
        var line = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(line))
          return;

        //13 Alttaki tüm süreçten sonra deneme olarak alt yapıldı.
        var lexer = new Lexer(line);
        while (true)
        {
          var token = lexer.NextToken();
          if (token.Kind == SyntaxKind.EndOfFileToken)
            break;

          Console.Write($"{token.Kind}: '{token.Text}'");
          if (token.Value is not null)
            Console.Write($" {token.Value}");

          Console.WriteLine();

        }

        // if (line == "1 + 2 * 3")
        //   Console.WriteLine("7");
        // else
        //   Console.WriteLine("ERROR : Invalid Expression");

      }

    }
  }
  //10 7.yorumda yapılan if kısmında return olarak ilk defa SyntaxKind kullanıldı. Number,operator gibi şeyleri temsil etmesi için enum olarak yaratıldı.
  enum SyntaxKind
  {
    NumberToken,
    WhitepaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParanthesisToken,
    CloseParanthesisToken,
    BadToken,
    EndOfFileToken
  }

  //5 4 te ifade edilen SyntaxToken class'ı yaratılıyor
  class SyntaxToken
  {
    public int Position;
    public string Text;
    public SyntaxKind Kind;
    public object Value { get; }

    //6 contructor içindeki parametreler eğitmen tarafından lazım olacak denilerek eklendi. ilk parametre ise operator,number yada whitespace gibi şeyleri temsil etmesi için kullanılacak. Üsteki property'ler ise otomatik olarak yaratıldı.
    public SyntaxToken(SyntaxKind kind, int position, string text, object value)
    {
      Kind = kind;
      Position = position;
      Text = text;
      Value = value;
    }

  }



  //2 bir text içinde kelimeleri oluşturmak için yaratıldı.
  class Lexer
  {

    //3 contructor ile içine parametre olarak sentence alacak text eklendi. 
    private readonly string _text;
    private int _position;

    public Lexer(string text)
    {
      _text = text;
    }

    //8 7 de bahsedilen Current property'si yaratılıyor. Eğer pozisyon değer olarak sahip olduğumuz text in uzunluğundan fazla ise '\0' döneriz değilse text içindeki pozisyonu karakter olarak döneriz.
    private char Current
    {
      get
      {
        if (_position >= _text.Length)
        {
          return '\0';
        }
        return _text[_position];
      }
    }

    //9 pozisyon değerimizi arttıran method olarak yaratıldı.
    private void Next()
    {
      _position++;
    }

    //4 Token->word olarak ifade edilir. Kısaca bize sıradaki kelimeyi üretecek.
    public SyntaxToken NextToken()
    {

      //14 Cümle sonunu ifade etmesi için yaratıldı.
      if (_position >= _text.Length)
      {
        return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
      }

      //7 Mevcut karakter number mı değil mi kontrolü yapıyoruz. Bunun için  char.isDigit() kullanılacak. içine parametre olarak ise mevcut karakteri temsil etmesi için bir property olan Current yaratılıp kullanılacak. Mevcut karakter sayı olduğu sürece pozisyon değeri arttırılacak en son ise sayıyı word olarak geri dönüyoruz.
      if (char.IsDigit(Current))
      {
        var start = _position;
        while (char.IsDigit(Current))
        {
          Next();
        }
        var length = _position - start;
        var text = _text.Substring(start, length);
        int.TryParse(text, out var value);
        return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
      }

      //11 şimdi ise whitespace için yapıyoruz, üsttekinin aynısı diyebiliriz.
      if (char.IsWhiteSpace(Current))
      {
        var start = _position;
        while (char.IsWhiteSpace(Current))
        {
          Next();
        }
        var length = _position - start;
        var text = _text.Substring(start, length);
        return new SyntaxToken(SyntaxKind.WhitepaceToken, start, text, null);
      }

      //12 şimdi ise operator sembolleri için yapılıyor.
      if (Current == '+')
      {
        return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
      }
      else if (Current == '-')
      {
        return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
      }
      else if (Current == '*')
      {
        return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
      }
      else if (Current == '/')
      {
        return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
      }
      else if (Current == '(')
      {
        return new SyntaxToken(SyntaxKind.OpenParanthesisToken, _position++, "(", null);
      }
      else if (Current == ')')
      {
        return new SyntaxToken(SyntaxKind.CloseParanthesisToken, _position++, ")", null);
      }

      return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);

    }

  }


}

