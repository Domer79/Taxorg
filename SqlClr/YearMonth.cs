using System;
using System.Data.SqlTypes;
using System.Globalization;
using Microsoft.SqlServer.Server;

namespace SqlClr
{
    [Serializable]
    [SqlUserDefinedType(Format.Native, IsByteOrdered = true, ValidationMethodName = "ValidateYearMonth")]
//    [CLSCompliant(false)]
    public struct YearMonth : INullable, IComparable
    {
        public const int Day = 1;

        public YearMonth(DateTime dateTime)
            : this()
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
        }

        public YearMonth(string yearMonth) 
            : this()
        {
            var ym = YearMonthParse(yearMonth);
            Year = ym.Year;
            Month = ym.Month;
        }

        public int Year
        {
            get { return _year; }
            set
            {
                var temp = _year;
                _year = value;

                if (_month != 0 && !ValidateYearMonth())
                {
                    _year = temp;
                    throw new ArgumentException("Неверное значение года");
                }
            }
        }

        public int Month
        {
            get { return _month; }
            set
            {
                var temp = _month;
                _month = value;

                if (_year != 0 && !ValidateYearMonth())
                {
                    _month = temp;
                    throw new ArgumentException("Неверное значение месяца");
                }
            }
        }

        [SqlMethod(OnNullCall = true)]
        public SqlInt64 GetTicks()
        {
            return new DateTime(Year, Month, Day).Ticks;
        }

        #region Operators

        public static bool operator ==(YearMonth date1, YearMonth date2)
        {
            if (date1.IsNull && date2.IsNull)
                return true;

            if (date1.IsNull)
                return false;

            if (date2.IsNull)
                return false;


            return date1.Equals(date2);
        }

        public static bool operator !=(YearMonth date1, YearMonth date2)
        {
            return !(date1 == date2);
        }

        public static bool operator ==(YearMonth yearMonth, DateTime date)
        {
            if (yearMonth.IsNull)
                return false;

            return yearMonth.Year == date.Year && yearMonth.Month == date.Month;
        }

        public static bool operator !=(YearMonth yearMonth, DateTime date)
        {
            return !(yearMonth == date);
        }

        public static bool operator >(YearMonth date1, YearMonth date2)
        {
            if (date1.IsNull || date2.IsNull)
                return false;

            if (date1.Year == date2.Year)
                return date1.Month > date2.Month;

            return date1.Year > date2.Year;
        }

        public static bool operator <(YearMonth date1, YearMonth date2)
        {
            return !(date1 > date2) && date1 != date2;
        }

        public static bool operator <=(YearMonth date1, YearMonth date2)
        {
            if (date1.IsNull || date2.IsNull)
                return false;

            return date1 < date2 || date1 == date2;
        }

        public static bool operator >=(YearMonth date1, YearMonth date2)
        {
            if (date1.IsNull || date2.IsNull)
                return false;

            return date1 > date2 || date1 == date2;
        }

        public static YearMonth operator +(YearMonth yearMonth, int i)
        {
            var year = yearMonth.Year + (yearMonth.Month + i)/12;
            var month = (yearMonth.Month + i)%12;

            year = month == 0 ? year - 1 : year;
            month = month == 0 ? 12 : month;

            var ym = new YearMonth {Year = year, Month = month};

            return ym;
        }

        public static YearMonth operator -(YearMonth yearMonth, int i)
        {
            var year = yearMonth.Year - ((12 - yearMonth.Month) + i)/12;
            var month = 12 - ((12 - yearMonth.Month) + i)%12;

            return new YearMonth {Year = year, Month = month};
        }

        public static YearMonth operator ++(YearMonth year)
        {
            return year + 1;
        }

        public static YearMonth operator --(YearMonth year)
        {
            return year - 1;
        }

        public static implicit operator YearMonth(string yearMonth)
        {
            return YearMonthParse(yearMonth);
        }

        public static implicit operator YearMonth(DateTime date)
        {
            return new YearMonth(date);
        }

        #endregion

        #region Equals operators

        private bool Equals(YearMonth other)
        {
            return Year == other.Year && Month == other.Month;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            return obj.GetType() == GetType() && Equals((YearMonth) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Year*397) ^ Month;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            var yearMonth = (YearMonth)obj;

            return Year.CompareTo(yearMonth.Year) == 0 ? Month.CompareTo(yearMonth.Month) : Year.CompareTo(yearMonth.Year);
        }

        #endregion

        [SqlMethod(DataAccess = DataAccessKind.Read)]
        public override string ToString()
        {
            if (IsNull)
                return "NULL";

            var zeros = new[] {"00", "0", ""};
            return string.Format("{0}.{1}", zeros[Month.ToString(CultureInfo.InvariantCulture).Length] + Month, Year);
        }

        public string Period
        {
            get { return ToString(); }
        }

        public bool IsNull
        {
            get { return _null; }
        }

        public static YearMonth Null
        {
            get
            {
                var h = new YearMonth {_null = true};
                return h;
            }
        }

        public static YearMonth Parse(SqlString s)
        {
            if (s.IsNull)
                return Null;

            return YearMonthParse(s.Value);
        }

        private static YearMonth YearMonthParse(string s)
        {
            var ym = new YearMonth();
            var yearMonthes = s.Split(".".ToCharArray());
            ym._month = int.Parse(yearMonthes[0]);
            ym._year = int.Parse(yearMonthes[1]);

            if (!ym.ValidateYearMonth())
                throw new ArgumentException("Не верное значение даты");

            // Введите здесь код
            return ym;
        }

        public YearMonth Minus(int period)
        {
            return this - period;
        }

        public bool ValidateYearMonth()
        {
            if (_year <= 0 || _month <= 0)
                return false;

            return _year <= 2100 && _month <= 12;
        }

        // Это метод-заполнитель
        public string Method1()
        {
            // Введите здесь код
            return string.Empty;
        }

        // Закрытый член
        private bool _null;
        private int _year;
        private int _month;

        public static YearMonth ParseDateTime(DateTime period)
        {
            return new YearMonth(period);
        }
    }
}