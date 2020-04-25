using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;

using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;

namespace ProjectBusinessCustomer.Models
{
    public class ExecuteQuery
    {
        public MyDbContext myDbContext = new MyDbContext();
        //params
         string _uri;
         string _token;
         string _project;
        ///<summary>
        /// Constructor to instanciate necessary params 
        /// </summary>
        public ExecuteQuery()
        {
            _uri = "https://inlsprl.visualstudio.com";
            _token = "stngl4sijouz5mgo3tteeatmgzusxetxl75lhyp2sbsseaz22qma";
            _project = "Armona";
        }
        ///<summary>
        /// Constructor with params  to instanciate the class
        /// </summary>
        public ExecuteQuery(string uri, string token , string projectname)
        {
            this._uri = uri;
            this._token = token;
            this._project = projectname;
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

            VssBasicCredential credentials = new VssBasicCredential("", _token);

            //create a wiql object and build our query
            Wiql wiql = new Wiql()
            {
                Query = "Select  * " +
                        "From WorkItems " +
                        "Where " +
                        "[System.TeamProject] = '" + project + "' " +
                        // "[System.Id] = '2600'" +
                        //// " And [System.Id] < '2884'" +

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
                    var workItems = workItemTrackingHttpClient.GetWorkItemsAsync(arr, fields, workItemQueryResult.AsOf).Result;
                    return workItems;
                }

                return null;
            }
        }

        public void synchronisation()
        {

            var workItems = WorkItemsByIds();
            if(workItems != null)
            {
                foreach (var workItem in workItems)
                {

                    if (!Exists(Convert.ToInt32(workItem.Id)))
                    {
                        Task task = new Task();

                        task.TaskIdDev = Convert.ToInt32(workItem.Id);
                        task.Title = Convert.ToString(workItem.Fields["System.Title"]);
                        task.State = Convert.ToString(workItem.Fields["System.State"]);
                        task.AssignedTo = Convert.ToString(workItem.Fields.ContainsKey("System.AssignedTo") ? ((Microsoft.VisualStudio.Services.WebApi.IdentityRef)workItem.Fields["System.AssignedTo"]).DisplayName : "Unassigned");
                        task.AreaPath = Convert.ToString(workItem.Fields["System.AreaPath"]);
                        task.WorkItemType = Convert.ToString(workItem.Fields["System.WorkItemType"]);
                        task.ChangedDate = Convert.ToString(workItem.Fields["System.ChangedDate"]);
                        task.Description = Convert.ToString(workItem.Fields.ContainsKey("System.Description") ? workItem.Fields["System.Description"] : " ");
                        task.IterationPath = Convert.ToString(workItem.Fields["System.IterationPath"]);
                        task.TeamProject = Convert.ToString(workItem.Fields["System.TeamProject"]);
                        task.ChangedBy = Convert.ToString(((Microsoft.VisualStudio.Services.WebApi.IdentityRef)workItem.Fields["System.ChangedBy"]).DisplayName);
                        task.CreatedDate = Convert.ToString(workItem.Fields["System.CreatedDate"]);
                        task.CommentCount = Convert.ToInt32(workItem.Fields["System.CommentCount"]);
                        task.CreatedBy = Convert.ToString(((Microsoft.VisualStudio.Services.WebApi.IdentityRef)workItem.Fields["System.CreatedBy"]).DisplayName);
                        task.Reason = Convert.ToString(workItem.Fields["System.Reason"]);
                        task.StateChangeDate = Convert.ToString(workItem.Fields["Microsoft.VSTS.Common.StateChangeDate"]);
                        task.Priority = Convert.ToInt32(workItem.Fields["Microsoft.VSTS.Common.Priority"]);
                        task.History = Convert.ToString(workItem.Fields.ContainsKey("System.History") ? workItem.Fields["System.History"] : " ");
                        var project = myDbContext.Projects.Where(p => p.Name == task.TeamProject).FirstOrDefault();
                        task.ProjectID = project.ProjectID;
                        task.userEmail = Convert.ToString(workItem.Fields.ContainsKey("System.AssignedTo") ? ((Microsoft.VisualStudio.Services.WebApi.IdentityRef)workItem.Fields["System.AssignedTo"]).UniqueName : "Unassigned");
                        myDbContext.Tasks.Add(task);
                    }


                }

                myDbContext.SaveChanges();
            }
        
        }

        //public void updateUserEmail()
        //{
        //    foreach (var workItem in WorkItemsByIds())
        //    {
        //        var task =myDbContext.Tasks.Where()
        //    }
        //}
        public void DeleteTask()
        {
            List<Task> oldtasks = myDbContext.Tasks.ToList();
            oldtasks.ForEach(task =>
            {
                myDbContext.Tasks.Remove(task);
            }
            );
            myDbContext.SaveChanges();

        }
        public bool Exists(int TaskIdDev)
        {
            var task = myDbContext.Tasks.Where(t => t.TaskIdDev == TaskIdDev).FirstOrDefault();
            if (task != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// method that interacts with the web service
        /// </summary>
        /// <param name="url">the url to fire</param>
        /// <returns>json response</returns>
         public  string ReturnJsonFromService(string url)
        {
                HttpClient client = new HttpClient(); //fournit une classe de base pour envoyer des requetes HTTP et recevoir des réponses HTTP d'une ressource identifie par URL
                string response = string.Empty;  //Représente la chaîne vide. Ce champ est en lecture seule.
                try
                {
                    client.BaseAddress = new Uri(url);//obtient ou définit l'adresse de base de l'URI de la ressource Internet utilisée pour envoyer des demandes
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.
                                                                    ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _token)))); //obtient les en-Tetes 
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

                return response; //get a String representation of  the json 
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
                    //convertit la chaîne json en une série d'objets
                    Projects root = JsonConvert.DeserializeObject<Projects>(response);
                    //compter tous les projets
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
        public void synchronisationProject(List<Project> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!ExistsProject(list[i].Id))
                {
                    Project project = new Project();

                    project.Id = list[i].Id;
                    project.Name = list[i].Name;
                    project.Description = list[i].Description;
                    project.Url = list[i].Url;
                    project.State = list[i].State;
                    project.Revision = list[i].Revision;
                    project.Visibilty = list[i].Visibilty;
                    project.LastUpdateTime = list[i].LastUpdateTime;
                    myDbContext.Projects.Add(project);
                }

            }
            myDbContext.SaveChanges();
        }
        public bool ExistsProject(string Id)
        {
            var project = myDbContext.Projects.Where(p => p.Id == Id).FirstOrDefault();
            if (project != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void DeleteProject()
        {
            List<Project> oldprojects = myDbContext.Projects.ToList();
            oldprojects.ForEach(project =>
            {
                myDbContext.Projects.Remove(project);
            }
            );
            myDbContext.SaveChanges();

        }
    }
}