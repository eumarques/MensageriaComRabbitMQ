namespace EasyNetQ.Console
{
    public class Person
    {
        public Person(string fullName, string document, DateTime birthDate)
        {
            fullName = fullName;
            Document = document;
            birthDate = birthDate;
        }

        public string fullName { get; set; }
        public string Document { get; set; }
        public DateTime birthDate { get; set; }
    }
}
