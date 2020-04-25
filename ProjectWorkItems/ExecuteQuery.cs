using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi;


namespace ProjectWorkItems
{
    class ExecuteQuery
    {
        //params
        readonly string _uri;
        readonly string _token;
        readonly string _project;
        readonly string endPoint;

        ///<summary>
        /// Constructor to instanciate necessary params 
        /// </summary>
         public ExecuteQuery()
        {
            _uri = "https://inlsprl.visualstudio.com";
            _token = "5zjhforb2jj2m4c3r5zsjgcmuda4ecsyv7mmxoo6y43tbtkev36a";
            _project = "PHNet";
            endPoint = "https://inlsprl.visualstudio.com/DefaultCollection/_apis/projects?stateFilter=All";
        }
        /// <summary>
        /// method that interacts with the web service
        /// </summary>
        /// <param name="url">the url to fire</param>
        /// <returns>json response</returns>
        public string ReturnJsonFromService(string url)
        {
            HttpClient client = new HttpClient(); //fournit une classe de base pour envoyer des requetes HTTP et recevoir des réponses HTTP d'une ressource identifie par URL
            string response = string.Empty;  //Représente la chaîne vide. Ce champ est en lecture seule.
            try
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                                              System.Text.ASCIIEncoding.ASCII.GetBytes(
                                              string.Format("{0}:{1}", "", _token))));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (client.GetStringAsync(url).Status != TaskStatus.Faulted)
                    response = client.GetStringAsync(url).Result;
                else response = "Bad Result";
            }

            return response;
        }

        /// <summary>
        /// return list of projects
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<Project> GetProjects(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new Exception("URL param in GetProjects Method is Null");
            List<Project> projects = new List<Project>();
            try
            {
                string response = ReturnJsonFromService(url);
                if (response == "Bad Result") return null;
                Projects root = JsonConvert.DeserializeObject<Projects>(response);

                for (int i = 0; i < root.count; i++)
                {
                    Project project = new Project();
                    project.Id = root.value[i].id;
                    project.Name = root.value[i].name;
                    project.Description = root.value[i].description;
                    project.Url = root.value[i].url;
                    project.State = root.value[i].state;
                    project.Revision = root.value[i].revision;
                    project.Visibilty = root.value[i].visibility;
                    project.LastUpdateTime = root.value[i].lastUpdateTime;
                    projects.Add(project);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);

            }
            return projects;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void WriteToConsole(List<Project> list)
        {
            Console.WriteLine("{0} Projects found", list.Count);
            for (int i = 1; i < list.Count; i++)
            {
                Console.WriteLine("Project number {0}", i);
                Console.WriteLine("Id : {0}", list[i].Id);
                Console.WriteLine("Name : {0}", list[i].Name);
                Console.WriteLine("Description : {0}", list[i].Description);
                Console.WriteLine("Url : {0}", list[i].Url);
                Console.WriteLine("State : {0}", list[i].State);
                Console.WriteLine("Revision : {0}", list[i].Revision);
                Console.WriteLine("Visibilty : {0}", list[i].Visibilty);
                Console.WriteLine("LastUpdateTime : {0}", list[i].LastUpdateTime);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Get List of work items based on query
        /// </summary>
        /// <returns></returns>
        public List<WorkItem> WorkItemsByIds()
        {
            Uri uri = new Uri(_uri);
            string personalAccessToken = _token;
            string project = _project;
            List<Project> projects = GetProjects(endPoint);

            VssBasicCredential credentials = new VssBasicCredential("", _token);

            //create a wiql object and build our query

            Wiql wiql = new Wiql()
            {
                Query = "Select  *" +
               "From WorkItems " +
                  // "Where " +
                  //"[System.TeamProject] = '" + project + "' " +
                  //"And [System.Id] > '2878'" +
                  //" And [System.Id] < '2884'" +
                  "Order By [State] Asc, [Changed Date] Desc"
            };





            //create instance of work item tracking http client
            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(uri, credentials))
            {
                //execute the query to get the list of work items in the results
                WorkItemQueryResult workItemQueryResult = workItemTrackingHttpClient.QueryByWiqlAsync(wiql).Result;


                //some error handling                
                if (workItemQueryResult.WorkItems.Count() != 0)
                {
                    //need to get the list of our work item ids and put them into an array
                    List<int> list = new List<int>();
                    foreach (var item in workItemQueryResult.WorkItems)
                    {
                        list.Add(item.Id);
                    }
                    int[] arr = list.ToArray();

                    //build a list of the fields we want to see
                    string[] fields = new string[18];
                    fields[0] = "System.Id";
                    fields[1] = "System.Title";
                    fields[2] = "System.State";
                    fields[3] = "System.AssignedTo";
                    fields[4] = "System.AreaPath";
                    fields[5] = "System.WorkItemType";
                    fields[6] = "System.ChangedDate";
                    fields[7] = "System.Description";
                    fields[8] = "System.IterationPath";
                    fields[9] = "System.TeamProject";
                    fields[10] = "System.ChangedBy";
                    fields[11] = "System.CreatedDate";
                    fields[12] = "System.CommentCount";
                    fields[13] = "System.CreatedBy";
                    fields[14] = "System.Reason";
                    fields[15] = "Microsoft.VSTS.Common.StateChangeDate";
                    fields[16] = "Microsoft.VSTS.Common.Priority";
                    fields[17] = "System.History";





                    //get work items for the ids found in query
                    var workItems = workItemTrackingHttpClient.GetWorkItemsAsync(arr, fields, workItemQueryResult.AsOf).Result.ToList();


                    Console.WriteLine("Query Results: {0} items found", workItems.Count);

                    Console.WriteLine(" ID           Title                                                         Assigned To          State       Area Path             Comments          Activity Date         ");
                    //loop though work items and write to console
                    foreach (var workItem in workItems)
                    {



                        //Console.WriteLine("{0}           {1}           {2}          {3}       {4}             {5}          {6}     {7}  {8}", workItem.Id, workItem.Fields["System.Title"], workItem.Fields.ContainsKey("System.AssignedTo") ? ((Microsoft.VisualStudio.Services.WebApi.IdentityRef)workItem.Fields["System.AssignedTo"]).DisplayName : "Unassigned",
                        //     workItem.Fields["System.State"], workItem.Fields["System.AreaPath"], workItem.Fields["System.CommentCount"], workItem.Fields["System.ChangedDate"], workItem.Fields.ContainsKey("System.Description") ? workItem.Fields["System.Description"] : " ",
                        //                                 ((Microsoft.VisualStudio.Services.WebApi.IdentityRef)workItem.Fields["System.ChangedBy"]).DisplayName);

                        Console.WriteLine("{0}           {1}          {2}      {3}   {4}   {5}      {6}", workItem.Id, workItem.Fields["System.Title"], workItem.Fields.ContainsKey("System.AssignedTo") ? ((Microsoft.VisualStudio.Services.WebApi.IdentityRef)workItem.Fields["System.AssignedTo"]).DisplayName : "Unassigned", workItem.Fields["System.State"]
                                                                                                                    , workItem.Fields["System.AreaPath"], workItem.Fields["System.CommentCount"], workItem.Fields["System.ChangedDate"]);


                    }

                    return workItems;
                }

                return null;
            }
        }

    }
}
