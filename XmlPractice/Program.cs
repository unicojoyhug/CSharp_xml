using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlPractice {
    class Program {
        static void Main(string[] args) {

            //Get list of person to parse
            List<Person> personList = new List<Person>() {
                new Person("Dave", 13), new Person("Ava", 13) , new Person("Jayson", 2)
            };

            ////Write xml and save it (uncomment this to write xml)
            //WriteXml(personList);

            //option 1) Read xml and print out on console without returning any
            //ReadXml(); 

            // option 2) return the result list for other actions
            List<Person> resultList = ReadXml(); 

            //Check who is the oldest (same age - list)
            CheckOldest(resultList);

            //Check who is the youngest (

        }

        private static void CheckOldest(List<Person> resultList) {
            List<Person> oldestList = new List<Person>();
            List<Person> temp = resultList.OrderByDescending(p => p.Age).ToList(); //need another list to sort.

            Person oldest = temp[0];
            oldestList.Add(oldest);
            for(int i = 1; i < temp.Count; i++) {
                if (oldest.Age == temp[i].Age) {
                    oldestList.Add(temp[i]);
                }
            }

            oldestList.ForEach(x => Console.WriteLine("oldest " + x));
        }

        //to run as option 1
        //private static void ReadXml() {
        private static List<Person> ReadXml() {
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;
            readerSettings.IgnoreWhitespace = true; // better to have it just in case and should be before create
            List<Person> result = new List<Person>();

            using (XmlReader reader = XmlReader.Create("../../myXml.xml", readerSettings)) {

                while (reader.ReadToFollowing("Person")) { // case sensitive, each chunk of group tag
                    Person person = new Person();
                    
                    // name as attibute or as content
                    person.Name = reader.GetAttribute("name"); //person.Name = reader.ReadElementContentAsString();
                    reader.Read(); // move to undernode
                    
                    person.Age = reader.ReadElementContentAsInt();

                    result.Add(person);
                }
                result.ForEach(x => Console.WriteLine(x));

            }

            return result;
        }

        private static void WriteXml(List<Person> personList) {
            //StringWriter stream = new StringWriter();
            StringWriterWithEncoding stream = new StringWriterWithEncoding(Encoding.UTF8); // otherwise exception encoding + unicode (my setting UTF-16)

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  "; // indentation - 2 spaces
            settings.Encoding = Encoding.UTF8;

            using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("Persons");// <Persons>
                foreach (Person person in personList) {
                    writer.WriteStartElement("Person"); // <person>
                    writer.WriteAttributeString("name", person.Name); // write the attribute with values with start element person

                    //writer.WriteElementString("name", person.Name); // write as element and content
                    writer.WriteElementString("age", person.Age.ToString());
                    writer.WriteEndElement(); // </person>
                }
                writer.WriteEndElement(); // </Persons>
                writer.WriteEndDocument();
            }

            File.WriteAllText(@"../../myXml.xml", stream.ToString());

        }
    }

    public sealed class StringWriterWithEncoding : StringWriter {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding) {
            this.encoding = encoding;
        }

        public override Encoding Encoding {
            get { return encoding; }
        }
    }
}
