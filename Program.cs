using System;
using Microsoft.EntityFrameworkCore;
using StudentApp.Data;
using StudentApp.Models;
using StudentApp.Helpers;

namespace StudentApp
{
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
                Console.WriteLine("6. Search Students");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                int choice = InputHelper.GetInt("", 0, 6);

                switch (choice)
                {
                    case 1: AddStudent(db); break;
                    case 2: ViewStudents(db); break;
                    case 3: UpdateStudent(db); break;
                    case 4: DeleteStudent(db); break;
                    case 5: ManageCourses(db); break;
                    case 6: SearchStudent(db); break;
                    case 0: return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddStudent(AppDbContext db)
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
            foreach (var c in courses)
                Console.WriteLine($"{c.Id}: {c.Title}");

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
                var student = new Student { Name = name, Age = age, CourseId = courseId };
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

       
        static void UpdateStudent(AppDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== Update Student ===");

            var students = db.Students.ToList();
            if (students.Count == 0)
            {
                Console.WriteLine("No students found.");
                Console.ReadKey();
                return;
            }

            foreach (var s in students)
                Console.WriteLine($"{s.Id}: {s.Name}");

            int id = InputHelper.GetInt("Enter Student Id to update:");
            var student = db.Students.Find(id);
            if (student == null)
            {
                Console.WriteLine("Student not found!");
                Console.ReadKey();
                return;
            }

            var newName = InputHelper.GetString("Enter new name:");
            var newAge = InputHelper.GetInt("Enter new age (10–100): ", 10, 100);

            student.Name = newName;
            student.Age = newAge;

            db.SaveChanges();
            Console.WriteLine("✅ Student updated successfully!");
            Console.ReadKey();
        }

        static void DeleteStudent(AppDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== Delete Student ===");

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
            Console.ReadKey();
        }

        static void ManageCourses(AppDbContext db)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Manage Courses ===");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. View Courses");
                Console.WriteLine("3. Delete Course");
                Console.WriteLine("0. Back");
                Console.Write("Enter choice: ");

                int ch = InputHelper.GetInt("", 0, 3);

                try
                {
                    switch (ch)
                    {
                        case 1:
                            string title = InputHelper.GetString("Enter Course Title:");
                            db.Courses.Add(new Course { Title = title });
                            db.SaveChanges();
                            Console.WriteLine("✅ Course added!");
                            Console.ReadKey();
                            break;

                        case 2:
                            var courses = db.Courses.Include(c => c.Students).ToList();
                            foreach (var c in courses)
                                Console.WriteLine($"{c.Id}: {c.Title} ({c.Students.Count} students)");
                            Console.ReadKey();
                            break;

                        case 3:
                            var allCourses = db.Courses.Include(c => c.Students).ToList();
                            if (allCourses.Count == 0)
                            {
                                Console.WriteLine("⚠️ No courses available.");
                                Console.ReadKey();
                                break;
                            }

                            foreach (var c in allCourses)
                                Console.WriteLine($"{c.Id}: {c.Title} ({c.Students.Count} students)");

                            int courseIdToDelete = InputHelper.GetInt("Enter Course Id to delete:");
                            var courseToDelete = db.Courses.Include(c => c.Students)
                                                           .FirstOrDefault(c => c.Id == courseIdToDelete);

                            if (courseToDelete == null)
                                Console.WriteLine("❌ Course not found.");
                            else if (courseToDelete.Students.Count > 0)
                                Console.WriteLine("⚠️ Cannot delete course with enrolled students.");
                            else
                            {
                                db.Courses.Remove(courseToDelete);
                                db.SaveChanges();
                                Console.WriteLine("✅ Course deleted successfully!");
                            }

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

        static void SearchStudent(AppDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== Search Student ===");

            string name = InputHelper.GetString("Enter student name to search:");
            try
            {
                var students = db.Students.Include(s => s.Course)
                                          .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                                          .ToList();

                if (students.Count == 0)
                    Console.WriteLine("❌ No matching students found.");
                else
                {
                    foreach (var s in students)
                        Console.WriteLine($"{s.Id}: {s.Name} ({s.Age}) - {s.Course?.Title}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error searching students: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
