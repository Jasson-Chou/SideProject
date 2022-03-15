using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;


namespace Win32TaskScheduler_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b_isExit = false;
            do
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1. Add Logon Trigger Task");
                Console.WriteLine("2. Remove Task");
                Console.WriteLine("3. Check Added Trigger Task");
                Console.WriteLine("99. Exit");

                var sz_input = Console.ReadLine();
                Console.Clear();
                if (int.TryParse(sz_input, out int i_input))
                {
                    switch (i_input)
                    {
                        case 1:
                            {
                                Console.WriteLine("Add Logon Trigger Task");
                                Console.WriteLine("----------------------");
                                Console.WriteLine("AppName:");
                                string sz_AppName = Console.ReadLine();
                                Console.WriteLine("AppFileName:");
                                string sz_AppFileName = Console.ReadLine();
                                Console.WriteLine("Author:");
                                string sz_Author = Console.ReadLine();
                                DateTime dt_StartTime = DateTime.Now;
                                DateTime dt_EndTime = DateTime.Now.AddYears(5);

                                AddLogonTask(sz_AppName, sz_AppFileName, sz_Author, dt_StartTime, dt_EndTime);

                                Console.WriteLine("Add Task Done!");
                                Console.WriteLine($"Information:");
                                Console.WriteLine($"AppName:{sz_AppName}");
                                Console.WriteLine($"AppFileName:{sz_AppFileName}");
                                Console.WriteLine($"Author:{sz_Author}");
                                Console.WriteLine($"----------------------------------");
                                

                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("Remove Task");
                                Console.WriteLine("----------------------");
                                Console.WriteLine("AppName:");
                                string sz_AppName = Console.ReadLine();
                                RemoveTask(sz_AppName);
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("Check Added Trigger Task");
                                Console.WriteLine("----------------------");
                                Console.WriteLine("AppName:");
                                string sz_AppName = Console.ReadLine();
                                bool b_isAdded = IsAddedTask(sz_AppName);
                                Console.WriteLine($"Is Added {sz_AppName} Task = [{b_isAdded.ToString()}]");
                                break;
                            }
                        case 99:
                            b_isExit = true;
                            break;
                        default:
                            Console.WriteLine($"Not Definition Menu Number [{i_input}]");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Input Must be Number");
                }
                Console.WriteLine();
                Console.WriteLine();
            } while (!b_isExit);
        }

        /// <summary>
        /// Add Logon Trigger Task
        /// </summary>
        /// <param name="AppName">App Name Without Extension</param>
        /// <param name="AppFileName">App Full File Name</param>
        /// <param name="Author">Excute Task Author</param>
        /// <param name="StartBoundary">Set Start Boundary</param>
        /// <param name="EndBoundary">Set End Boundary</param>
        public static void AddLogonTask(string AppName, string AppFileName, string Author, DateTime StartBoundary, DateTime EndBoundary)
        {
            using (TaskService ts = new TaskService())
            {
                TaskFolder root = ts.RootFolder;
                //Create Task
                var tsDef = ts.NewTask();
                var RegInfo = tsDef.RegistrationInfo;
                RegInfo.Description = $"Task Will execute {AppName} and Spedcified user logs on.";
                RegInfo.Author = Author;

                var settings = tsDef.Settings;
                settings.StartWhenAvailable = true;
                /* 
                 * UserId
                 *Summary:
                 *    Gets or sets The identifier of the user. For example, "MyDomain\MyName" or for
                 *    a local account, "Administrator".
                 *    This property can be in one of the following formats:
                 *    • User name or SID: The task is started when the user logs on to the computer.
                 *    • NULL: The task is started when any user logs on to the computer.
                 *
                 *Exception:
                 *  T:Microsoft.Win32.TaskScheduler.NotV1SupportedException:
                 *    Not supported under Task Scheduler 1.0.
                 *
                 *Note:
                 *    If you want a task to be triggered when any member of a group logs on to the
                 *    computer rather than when a specific user logs on, then do not assign a value
                 *    to the LogonTrigger.UserId property. Instead, create a logon trigger with an
                 *    empty LogonTrigger.UserId property and assign a value to the principal for the
                 *    task using the Principal.GroupId property.
                */
                string UserId = null;
                //Create a logon trigger
                var triggers = tsDef.Triggers;
                triggers.Add(new LogonTrigger()
                {
                    UserId = UserId,
                    StartBoundary = StartBoundary,
                    EndBoundary = EndBoundary,
                    Enabled = true,
                });

                tsDef.Principal.RunLevel = TaskRunLevel.Highest;

                //Create the action for the task to execute.
                tsDef.Actions.Add(new ExecAction(AppFileName));

                root.RegisterTaskDefinition(AppName, tsDef);
            }
        }

        public static void RemoveTask(string AppName)
        {
            using (TaskService ts = new TaskService())
            {
                ts.RootFolder.DeleteTask(AppName, false);
            }
        }

        public static bool IsAddedTask(string AppName)
        {
            bool result = false;
            using (TaskService ts = new TaskService())
            {
                var task = ts.RootFolder.AllTasks.SingleOrDefault(item => item.Name == AppName);
                result = task != null;
            }
            return result;
        }
    }
}
