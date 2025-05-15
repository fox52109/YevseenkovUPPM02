public class Date
{
    public int year, month, day;

    public Date()
    {
        year = 2006;
        month = 3;
        day = 11;
    }
    public Date(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
    }
    public Date(string date)
    {
        string[] splitDate = date.Split(' ', ',', '/', '.', ':', '\t');
        day = Convert.ToInt32(splitDate[0]);
        month = Convert.ToInt32(splitDate[1]);
        year = Convert.ToInt32(splitDate[2]);
    }

    private static readonly int[] monthDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    private static bool IsLeapYear(int year)
    {
        return (year % 400 == 0) || (year % 4 == 0 && year % 100 != 0);
    }

    private static int DaysInMonth(int year, int month)
    {
        if (month == 2 && IsLeapYear(year))
            return 29;
        return monthDays[month - 1];
    }

    public void AddDays(int ndays)
    {
        int n = ndays;

        while (ndays != 0)
        {
            if (day < DaysInMonth(year, month)) day++;
            else if (month < 12)
            {
                day = 1;
                month++;
            }
            else
            {
                day = 1;
                month = 1;
                year++;
            }
            --ndays;
        }
        Console.WriteLine($"\nДобавленно {n} дней, текущая дата: {day}.{month}.{year}");
    }

    public void AddDays(Date date, int ndays)
    {
        day = date.day;
        month = date.month;
        year = date.year;
        int n = ndays;

        while (ndays != 0)
        {
            if (day < DaysInMonth(year, month)) day++;
            else if (month < 12)
            {
                day = 1;
                month++;
            }
            else
            {
                day = 1;
                month = 1;
                year++;
            }
            --ndays;
        }
        Console.WriteLine($"\nДобавленно {n} дней, текущая дата: {day}.{month}.{year}");
    }

    public static (int years, int months, int days) Difference(Date date1, Date date2)
    {
        int d1 = date1.day, m1 = date1.month, y1 = date1.year;
        int d2 = date2.day, m2 = date2.month, y2 = date2.year;

        int years = y2 - y1;
        int months = m2 - m1;
        int days = d2 - d1;

        if (days < 0)
        {
            months--;
            days += DaysInMonth(y2, m2 == 1 ? 12 : m2 - 1);
        }

        if (months < 0)
        {
            years--;
            months += 12;
        }

        return (years, months, days);
    }

    public void Subtraction(Date date2)
    {
        Date start, end;
        if (IsEarlier(this, date2))
        {
            start = this;
            end = date2;
        }
        else
        {
            start = date2;
            end = this;
        }

        var (days, months, years) = Difference(start, end);

        Console.WriteLine($"\nРазница между {start.day}.{start.month}.{start.year} и {end.day}.{end.month}.{end.year} составляет:" +
            $"\n{years} г.; {months} мес.; {days} дн.");
    }

    private static bool IsEarlier(Date d1, Date d2)
    {
        if (d1.year != d2.year)
            return d1.year < d2.year;
        if (d1.month != d2.month)
            return d1.month < d2.month;
        return d1.day < d2.day;
    }

    public void Compare(Date date2)
    {
        int d2 = date2.day, m2 = date2.month, y2 = date2.year;

        if (year > y2)
            if (month > m2)
                if (day > d2)
                    Console.WriteLine($"\nДата {day}.{month}.{year} больше чем {d2}.{m2}.{y2}");
                else goto elsePrint;
            else goto elsePrint;
        else goto elsePrint;

        elsePrint:
            Console.WriteLine($"\nДата {d2}.{m2}.{y2} больше чем {day}.{month}.{year}");
    }
    
	public string ConvertToString()
    {
        Console.WriteLine($"\nУспешная конвертация даты {day}.{month}.{year} в строку");
        return $"{day}.{month}.{year}";
    }

    public static (int, int, int) GetFromString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return (0, 0, 0);

        for (int i = 0; i <= input.Length - 10; i++)
        {
            if (input[i + 2] == '.' && input[i + 5] == '.')
            {
                string dayStr = input.Substring(i, 2);
                string monthStr = input.Substring(i + 3, 2);
                string yearStr = input.Substring(i + 6, 4);

                if (int.TryParse(dayStr, out int d) &&
                    int.TryParse(monthStr, out int m) &&
                    int.TryParse(yearStr, out int y))
                {
                    if (IsValidDate(y, m, d))
                    {
                        Console.WriteLine($"{d}.{m}.{y}");
                        return (d, m, y);
                    }
                }
            }
        }
        
        return (0, 0, 0);
    }
    private static bool IsValidDate(int year, int month, int day)
    {
        try
        {
            var dt = new DateTime(year, month, day);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class Program
{
    public static void Main()
    {
        //Создаем даты
        Date myBirthday = new("13/06/2006");
        Date googleCreationDate = new("04/09/1998");
        Date anotherBirthday = new();
        
        //Ищем разницу между датой моего дня рождения и другогой даты
        myBirthday.Subtraction(anotherBirthday);
        //Преобразовываем дату моего др
        myBirthday.ConvertToString();

        //Добавляем 150 дней к дате создания Google
        googleCreationDate.AddDays(150);
        //Сравниваем какая дата больше, другой день рождения или дата создания Google
        anotherBirthday.Compare(googleCreationDate);

        //Получаем дату из строки текста
        Date.GetFromString("Проверка метода получения 30.11.2001 даты из строки");
    }
}
