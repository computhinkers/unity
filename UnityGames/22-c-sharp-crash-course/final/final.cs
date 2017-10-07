//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Your code goes here
            Console.WriteLine("Hello, world!");
            
            var meaningOfLife = 42;
            var smallPi = 3.14;
            var bigPi = 3.14159265359;
            var vaporWare = "Half Life 3";
            const bool likesPizza = true;
            
            string[] writers = { "Brian", "Anthony", "Sean", "Eric" };
            string[] editors = new string[5];
            
            Console.WriteLine(writers[1]);
            writers[0] = "Ray";
            
            if (likesPizza == false) {
              Console.WriteLine("You monster!");
            }
            bool isMonster = (likesPizza == true) ? false : true;
            for (int i=0; i < 10; i++) {
              Console.WriteLine("C# Rocks!");
            }
            foreach (string writer in writers) {
              Console.WriteLine(writer);
            }
            if (meaningOfLife == 42) {
              bool inScope = true;
            }
            Point2D myPoint = new Point2D();
            myPoint.X = 10;
            myPoint.Y = 20;
            Point2D anotherPoint = new Point2D();
            anotherPoint.X = 5;
            anotherPoint.Y = 15;  
            myPoint.AddPoint(anotherPoint);
            
            Console.WriteLine(myPoint.X);
            Console.WriteLine(myPoint.Y);
            
            Point2D yetAnotherPoint = myPoint;
            yetAnotherPoint.X = 100;
            
            Console.WriteLine(myPoint.X);
            Console.WriteLine(yetAnotherPoint.X);
            
            Point2DRef pointRef = new Point2DRef();
            pointRef.X = 20;
            Point2DRef anotherRef = pointRef;
            anotherRef.X = 25;
            
            Console.WriteLine(pointRef.X);
            Console.WriteLine(anotherRef.X);

            pointRef = null;
            anotherRef.X = 125;
            Console.WriteLine(anotherRef.X);
            anotherRef = null;
            
            RenFairePerson person = new RenFairePerson();
            person.Name = "Igor the Ratcatcher";
            person.SayHello();
            
        }

    }
    struct Point2D {
      public int X;
      public int Y;
      public void AddPoint(Point2D anotherPoint) {
        this.X = this.X + anotherPoint.X;
        this.Y = this.Y + anotherPoint.Y;
      }
    }
    class Point2DRef {
      public int X;
      public int Y;
      public void AddPoint(Point2DRef anotherPoint) {
        this.X = this.X + anotherPoint.X;
        this.Y = this.Y + anotherPoint.Y;
      }
    }
    class Person {
      public string Name;
      public virtual void SayHello() {
        Console.WriteLine("Hello");
      }
    }
    class RenFairePerson : Person {
      public override void SayHello() {
        base.SayHello();
        Console.Write("Huzzah!");
      }
    }
}