namespace PlayingWithMongoDB.Model;
public static class DataGenerator
{
    public static class ForStudent
    {
        private static readonly DateTime _toDate   = DateTime.Now.AddYears(-10);
        private static readonly DateTime _fromDate = _toDate.AddYears(-90);
        private static readonly Random _random     = new Random();
        private static readonly string[] _subjects = ["English", "Mathematics", "Physics", "Chemistry", "Spanish"];

        public static Student Create(int? nameIndex = null)
        {
            var dateOfBirth = getRandomDate(_fromDate, _toDate);

            return new Student
            {
                Id          = Guid.NewGuid(),
                Name        = $"Name #{nameIndex ?? _random.Next(100, 1000)}",
                DateOfBirth = dateOfBirth.Date, // Has to be just .Date, otherwise error: "TimeOfDay component is not zero".
                Age         = calculateAge(dateOfBirth),
                Subjects    = shuffleSubjects(),
                Gender      = (Gender)_random.Next(0, 2)
            };
        }

        public static IEnumerable<Student> Generate(ushort count)
        {
            return Enumerable.Range(1, count).Select(i => Create(i)).ToList();
        }

        private static IEnumerable<string> shuffleSubjects()
        {
            int length = _random.Next(_subjects.Length);

            return _random.GetItems(_subjects, length);
        }

        //private static IEnumerable<string> shuffleSubjects_BeforeNet8()
        //{
        //    var list = new List<string>(_subjects);

        //    for (int n = list.Count - 1; n > 0; n--)
        //    {
        //        int swapIndex = _random.Next(n + 1);

        //        (list[n], list[swapIndex]) = (list[swapIndex], list[n]);
        //    }

        //    return list.Take(_random.Next(_subjects.Length)).ToList();
        //}

        private static DateTime getRandomDate(DateTime from, DateTime to)
        {
            TimeSpan range = to - from;

            TimeSpan randomTimeSpan = TimeSpan.FromTicks((long)(range.Ticks * Random.Shared.NextDouble()));

            return from + randomTimeSpan;
        }

        private static int calculateAge(DateTime dob)
        {
            var today = DateTime.Today;

            int age = today.Year - dob.Year;

            if (dob.DayOfYear < today.DayOfYear)
            {
                age--;
            }

            return age;
        }
    }
}
