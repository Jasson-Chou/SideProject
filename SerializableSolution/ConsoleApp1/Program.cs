using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("One Person:");

            var person = new Person { Name = "Jasson Chou", ID = 108 };

            SerializeObject("Person.json", person);
            
            var result = DeserializeObject<Person>("Person.json").ToString();

            Console.WriteLine(result);

            Console.WriteLine("**************************************************");

            Console.WriteLine("Multi-Person:");

            var personList = new List<Person>
            {
                new Person { Name = "Jasson Chou", ID = 108 },
                new Person { Name = "Nico", ID = 109 },
                new Person { Name = "Eric", ID = 120 },
            };

            SerializeObject("PersonList.json", personList);

            var listresult = DeserializeObject<List<Person>>("PersonList.json").ToPrint();

            Console.WriteLine(listresult);

            Console.WriteLine("**************************************************");

            Console.ReadLine();
        }

        private static void SerializePerson(Person person)
        {
            using (var file = new StreamWriter("Person.json"))
            {
                var json = JsonSerializer.Serialize(person);
                file.Write(json);
                Console.WriteLine(json);
            }
        }

        private static void DeserializePerson()
        {
            using (var reader = new StreamReader("Person.json"))
            {
                var json = reader.ReadToEnd();
                var deserializedPerson = JsonSerializer.Deserialize<Person>(json);
                Console.WriteLine(deserializedPerson);
            }
        }

        private static void SerializePersonList(List<Person> personList)
        {
            using (var file = new StreamWriter("PersonList.json"))
            {
                var json = JsonSerializer.Serialize(personList);
                file.Write(json);
                Console.WriteLine(json);
            }
        }

        private static void DeserializePersonList()
        {
            using (var reader = new StreamReader("PersonList.json"))
            {
                var json = reader.ReadToEnd();
                var deserializedPersonList = JsonSerializer.Deserialize<List<Person>>(json);
                foreach (var person in deserializedPersonList)
                {
                    Console.WriteLine(person);
                }
            }
        }

        private static void SerializeObject<T>(string fileName, T Obj)
        {
            using (var file = new StreamWriter(fileName))
            {
                var json = JsonSerializer.Serialize(Obj);
                file.Write(json);
                Console.WriteLine(json);
            }
        }

        private static T DeserializeObject<T>(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                var json = reader.ReadToEnd();
                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }

    [Serializable]
    class Person
    {
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonIgnore]
        public string Formatter => $"{Name}: {ID}";

        public override string ToString()
        {
            return Formatter;
        }
    }

    static class PersonListExtension
    {
        public static string ToPrint(this List<Person> people)
        {
            return string.Join(", ", people.Select(item => item.ToString()));
        }
    }

}