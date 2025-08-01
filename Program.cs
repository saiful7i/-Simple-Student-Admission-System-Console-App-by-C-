using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public string Name { get; set; }
    public int Roll { get; set; }
    public double GPA { get; set; }
    public List<string> Choices { get; set; }
    public string AdmittedDepartment { get; set; } = "Not Admitted";
}

class Program
{
    static List<string> Departments = new List<string> { "CSE", "EEE", "BBA", "ENG", "LLB" };
    static int MaxSeatPerDept = 5;

    static void Main()
    {
        bool programSuccess = false;

        while (!programSuccess) 
        {
            try
            {
                Console.Write("Enter number of students: ");
                int total = int.Parse(Console.ReadLine());

                List<Student> students = new List<Student>();

                for (int i = 0; i < total; i++)
                {
                    bool studentSuccess = false;
                    while (!studentSuccess)
                    {
                        try
                        {

                            Console.WriteLine($"\nStudent #{i + 1}");
                            Console.Write("Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Roll: ");
                            int roll = int.Parse(Console.ReadLine());

                            if (students.Any(s => s.Roll == roll))
                                throw new Exception("Duplicate roll number detected. Please enter a unique roll number.");

                            double gpa;

                            while (true)
                            {
                                try
                                {
                                    Console.Write("GPA: ");
                                    gpa = double.Parse(Console.ReadLine());

                                    if (gpa >= 1.0 && gpa <= 5.0)
                                    {
                                        if (gpa < 2.0)
                                            throw new Exception("GPA must be at least 2.0 to be eligible for admission.");
                                        break; 
                                    }
                                    else
                                    {
                                        Console.WriteLine("GPA must be between 1.0 and 5.0. Try again.");
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid GPA format. Please enter a number between 1.0 and 5.0.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }                            

                            Console.WriteLine("Enter 3 department choices (CSE, EEE, BBA, ENG, LLB):");
                            var choices = new List<string>();
                            while (choices.Count < 3)
                            {
                                Console.Write($"Choice #{choices.Count + 1}: ");
                                string choice = Console.ReadLine().ToUpper();
                                if (Departments.Contains(choice) && !choices.Contains(choice))
                                    choices.Add(choice);
                                else
                                    Console.WriteLine("Invalid or duplicate. Try again.");
                            }

                            students.Add(new Student { Name = name, Roll = roll, GPA = gpa, Choices = choices });
                            studentSuccess = true; // Exit inner loop if successful
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid input. Please enter the correct format.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    programSuccess = true;
                }
                // Sort by GPA (Merit list)
                var meritList = students.OrderByDescending(s => s.GPA).ThenBy(s => s.Roll).ToList();

                // Seat count
                var departmentSeats = Departments.ToDictionary(d => d, d => 0);

                // Admission
                foreach (var student in meritList)
                {
                    var available = student.Choices.FirstOrDefault(d => departmentSeats[d] < MaxSeatPerDept);
                    if (available != null)
                    {
                        student.AdmittedDepartment = available;
                        departmentSeats[available]++;
                    }
                }

                // Group by Admitted Department
                var grouped = meritList
                    .GroupBy(s => s.AdmittedDepartment)
                    .OrderBy(g => g.Key); // Alphabetical by department

                Console.WriteLine("\n--- Department-wise Admission List ---");
                foreach (var group in grouped)
                {
                    Console.WriteLine($"\nDepartment: {group.Key}");
                    foreach (var student in group)
                    {
                        Console.WriteLine($"  Roll: {student.Roll}, Name: {student.Name}, GPA: {student.GPA}");
                    }
                }

                // Optional: Display seat summary
                Console.WriteLine("\n--- Department-wise Seat Summary ---");
                departmentSeats.ToList().ForEach(d =>
                {
                    Console.WriteLine($"{d.Key}: {d.Value} students admitted");
                });  
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter the correct format.");
            }

        }

    }
}