using System;
using StudentApp.Models;
using StudentApp.Data;
using Microsoft.EntityFrameworkCore;
using StudentApp.Helpers;
class Program
{
    static void Main(string[] args)
    {
        using var db = new AppDbContext();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== STUDENT MANAGEMENT SYSTEM =====");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Manage Courses");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    AddStuent(db);
                    break;
                case 2:
                    ViewStudents(db);
                    break;
                case 3:
                    UpdateStudent(db);
                    break;
                case 4:
                    DeleteStudent(db);
                    break;
                case 5:
                    ManageCourses(db);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
            void AddStudent(AppDbContext db)
            {

                Console.Clear();
                Console.WriteLine("=== Add Student ===");

                var name = InputHelper.GetString("Enter Name:");

                var age = InputHelper.GetInt("Enter Age (10–100): ", 10, 100);

                var courses = db.Courses.ToList();
                if (courses.Count == 0)
                {
                    Console.WriteLine("⚠️ No courses found. Please add a course first!");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Available Courses:");
                foreach (var course in courses)
                {
                    onsole.WriteLine($"{course.Id}: {course.Title}");
                }


                var courseId = InputHelper.GetInt("Enter CourseId:");

                var course = db.Courses.Find(courseId);
                if (course == null)
                {
                    Console.WriteLine("⚠️ Invalid Course Id.");
                    Console.ReadKey();
                    return;
                }
                try
                {
                    var student = new Student { Name = name ?? "", Age = age, CourseId = courseId };
                    db.Students.Add(student);
                    db.SaveChanges();
                    Console.WriteLine("✅ Student added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to add student: {ex.Message}");
                }
                Console.ReadKey();

            }
            void ViewStudents(AppDbContext db)
            {
                Console.Clear();
                Console.WriteLine("=== All Students ===");
                try
                {
                    var students = db.Students.Include(s => s.Course).ToList();
                    if (students.Count == 0)
                        Console.WriteLine("No students found.");
                    else
                    {
                        foreach (var s in students)
                        {
                            Console.WriteLine($"{s.Id}: {s.Name} ({s.Age}) - {s.Course?.Title}");

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error loading students: {ex.Message}");
                }
                Console.ReadKey();
            }
            void UpdateStudent(AppDbContext db)
            {
                Console.Clear();
                Console.WriteLine("=== Update Student ===");
                try
                {
                    var students = db.Students.ToList();
                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        Console.ReadKey();
                        return;
                    }
                    foreach (var s in students)
                        Console.WriteLine($"{s.Id}: {s.Name}");

                    int id = InputHelpe.GetInt("Enter Student Id to update:");
                    var student = db.Students.Find(id);
                    if (student == null)
                    {
                        Console.WriteLine("Student not found!");
                        Console.ReadKey();
                        return;
                    }

                    var newName = InputHelper.GetString("Enter new Name (leave empty to keep current):", required: false);
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        student.Name = newName;
                    }

                    int age = InputHelper.GetInt("Enter new Age (0 to keep current):", 0, 100);
                    if (age > 0)
                    {
                        student.Age = age;
                    }
                    db.SaveChanges();
                    Console.WriteLine("✅ Student updated successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error updating student: {ex.Message}");
                }
                Console.ReadKey();
            }
            void DeleteStudent(AppDbContext db)
            {
                Console.Clear();
                Console.WriteLine("=== Delete Student ===");
                try
                {
                    var students = db.Students.ToList();
                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        Console.ReadKey();
                        return;
                    }
                    foreach (var s in students)
                        Console.WriteLine($"{s.Id}: {s.Name}");

                    int id = InputHelper.GetInt("Enter Student Id to delete:");
                    var student = db.Students.Find(id);
                    if (student == null)
                    {
                        Console.WriteLine("Student not found!");
                        Console.ReadKey();
                        return;
                    }
                    db.Students.Remove(student);
                    db.SaveChanges();
                    Console.WriteLine("✅ Student deleted successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error deleting student: {ex.Message}");
                }
                Console.ReadKey();
            }
            void ManageCourses(AppDbContext db)
            {
                while (true)
                {

                    Console.Clear();
                    Console.WriteLine("=== Manage Courses ===");
                    Console.WriteLine("1. Add Course");
                    Console.WriteLine("2. View Courses");
                    Console.WriteLine("0. Back");
                    Console.Write("Enter choice: ");
                    int ch = int.Parse(Console.ReadLine());
                    try
                    {
                        switch (ch)
                        {
                            case 1:
                                Console.Write("Enter Course Title: ");
                                string? title = Console.ReadLine();
                                db.Courses.Add(new Course { Title = title ?? "" });
                                db.SaveChanges();
                                Console.WriteLine("✅ Course added!");
                                Console.ReadKey();
                                break;
                            case 2:
                                var courses = db.Courses.Include(c => c.Student).ToList();
                                foreach (var c in courses)
                                    Console.WriteLine($"{c.Id}: {c.Title} ({c.Students.Count} students)");
                                Console.ReadKey();
                                break;
                            case 0:
                                return;
                            default:
                                Console.WriteLine("Invalid choice.");
                                Console.ReadKey();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error: {ex.Message}");
                        Console.ReadKey();
                    }

                }
            }

        }








        var course = new Course { Title = "C# Fundamentals" };
        db.Courses.Add(course);
        db.SaveChanges();
        var student = new Student { Name = "John", Age = 21, CourseId = course.Id };
        db.Students.Add(student);
        db.SaveChanges();

        var students = db.Students.Include(s => s.Course).ToList();
        foreach (var s in students)
        {
            Console.WriteLine($"{s.Id}:{s.Name}({s.Age})-{s.Course?.Title}");
        }
        var entry = db.Entry(student);
        Console.WriteLine($"Entity State: {entry.State}");
        student.Age = 25;
        Console.WriteLine($"Before: {db.Entry(student).State}");
        db.SaveChanges();
        Console.WriteLine($"After: {db.Entry(student).State}");
        db.Students.Remove(student);
        Console.WriteLine($"After Remove: {db.Entry(student).State}");
        db.SaveChanges();
        Console.WriteLine($"After SaveChanges: {db.Entry(student).State}");

    }
}
