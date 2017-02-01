using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlPractice {
    class Person {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person() { }

        public Person(string name, int age) {
            this.Name = name;
            this.Age = age;
        }

        public override string ToString() {
            return "Person : " + Name + " is " + Age + " years old." ;
        }
    }
}
