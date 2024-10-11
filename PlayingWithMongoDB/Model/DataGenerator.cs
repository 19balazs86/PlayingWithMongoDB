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

    public static class ForTemperature
    {
        // Records: 1,758
        public static List<TemperatureReport> Generate()
        {
            string[] devices = ["Device01", "Device02", "Device03"];

            List<TemperatureReport> readings = [];

            DateTime now   = DateTime.Today;
            DateTime start = now.AddMonths(-4);

            TimeSpan timeInterval = TimeSpan.FromMinutes(60 * 5); // Every (X ± jitter) minutes create a TemperatureRecord

            var (minC, maxC) = (-10.0, 30.0);

            foreach (string deviceId in devices)
            {
                DateTime currentTime = start;

                while (currentTime <= now)
                {
                    TimeSpan jitter = TimeSpan.FromSeconds(Random.Shared.Next(-15, 16)); // Time jitter of ±15 seconds

                    DateTime timestampWithJitter = currentTime.Add(jitter);

                    double temperature = generateTemperature(minC, maxC);

                    readings.Add(new TemperatureReport
                    {
                        Id          = Guid.NewGuid(),
                        DeviceId    = deviceId,
                        Timestamp   = timestampWithJitter,
                        Temperature = temperature
                    });

                    currentTime = currentTime.Add(timeInterval);
                }
            }

            return readings;
        }

        private static double generateTemperature(double min, double max)
        {
            return Math.Round(min + (Random.Shared.NextDouble() * (max - min)), 2);
        }
    }
}
