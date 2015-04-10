namespace CP.TfsAssistant.Libraires
{
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TfsHelper
    {
        public static WorkItemStore GetWorkItemStore(string projectCollectionUri)
        {
            var uri = new Uri(projectCollectionUri);
            var connection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
            var store = connection.GetService<WorkItemStore>();
            return store;
        }

        public static List<Project> GetProjects(string projectCollectionUri)
        {
            var store = GetWorkItemStore(projectCollectionUri);
            var projects = store.Projects.Cast<Project>().ToList();
            return projects;
        }

        public static Project GetProject(string projectCollectionUri, string projectName)
        {
            var project = GetProjects(projectCollectionUri).FirstOrDefault(p => p.Name == projectName);
            return project;
        }

        public static WorkItem GetWorkItem(string projectCollectionUri, int id)
        {
            var store = GetWorkItemStore(projectCollectionUri);
            return store.GetWorkItem(id);
        }

        public static void GetPathsRecursively(NodeCollection nodes, ref List<string> paths)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (Node node in nodes)
            {
                paths.Add(node.Path);
                if (node.HasChildNodes)
                {
                    GetPathsRecursively(node.ChildNodes, ref paths);
                }
            }
        }

        public static List<string> GetIterationPaths(Project project)
        {
            var iterations = new List<string>();
            GetPathsRecursively(project.IterationRootNodes, ref iterations);
            return iterations;
        }

        public static List<string> GetAreaPaths(Project project)
        {
            var areas = new List<string>();
            GetPathsRecursively(project.AreaRootNodes, ref areas);
            return areas;
        }

        public static List<string> GetWorkItemTypes(Project project)
        {
            var types = new List<string>();
            types.AddRange(project.WorkItemTypes.Cast<WorkItemType>().Select(x => x.Name));

            Category category = project.Categories["Hidden Types Category"];
            if (category != null)
            {
                types = types.Except(category.WorkItemTypes.Select(x => x.Name)).ToList();
            }

            return types;
        }
    }
}
