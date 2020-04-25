using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWorkItems
{
    class Program
    {
        // REST API end point given by microsoft,Get all team projects in the project collection that the authenticated user has access to
        private static string endPoint = "https://inlsprl.visualstudio.com/DefaultCollection/_apis/projects?stateFilter=All";
        static void Main(string[] args)
        {
            ExecuteQuery executeQuery = new ExecuteQuery();
            //#region Get Projetcs
            //// Get projects
            ////  Console.WriteLine("Retrieving List of projects...");
            //List<Project> list = executeQuery.GetProjects(endPoint);
            //executeQuery.WriteToConsole(list);
            //#endregion

            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------");

            #region Get items
            // Get items

            Console.WriteLine("Retrieving List of work items...");
            Console.WriteLine("-------------------------------------------------------");
            var items = executeQuery.WorkItemsByIds();
            Console.WriteLine(items);
            #endregion

            Console.ReadKey();
            Console.ReadLine();
        }
    }
}
