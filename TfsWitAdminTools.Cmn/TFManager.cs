using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public class TFManager : ITFManager
    {
        public string TfsAddress { get; private set; }

        public Dictionary<string, TfsTeamProjectCollection> ProjectCollections { get; private set; }
        public Dictionary<string, ProjectInfo[]> TeamProjects { get; private set; }

        public WorkItemStore WorkItemStore { get; set; }
        public ICommonStructureService4 CommonStructureService4 { get; set; }

        public Dictionary<short, TeamFoundationIdentity> UserIdentities;
        public Dictionary<short, string> ProjectWorkSpaces;

        public VersionControlServer VersionControlServer { get; set; }

        public Workspace Workspace { get; set; }

        public TFManager(string serverAddress)
        {
            TfsAddress = serverAddress;
            Init();
        }

        private void Init()
        {
            if (TfsAddress == null || TfsAddress == string.Empty)
                return;

            var serverName = Regex.Replace(TfsAddress.ToLower(), @"\A[h][t][t][p][:]\/\/|[:][8][0][8][0]\S+\z", string.Empty);
            //var collectionName = "eitprojects";
            //var projectCollectionName = serverAddress + collectionName;
            Uri tfsUri = new Uri(TfsAddress);
            TfsConfigurationServer configurationServer =
                TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);

            ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(
                new[] { CatalogResourceTypes.ProjectCollection },
                false, CatalogQueryOptions.None);

            ProjectCollections = new Dictionary<string, TfsTeamProjectCollection>();
            TeamProjects = new Dictionary<string, ProjectInfo[]>();

            foreach (CatalogNode collectionNode in collectionNodes)
            {
                #region Init TeamProject

                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                TfsTeamProjectCollection projectCollection = configurationServer.GetTeamProjectCollection(collectionId);
                var nodeCollectionName = projectCollection.Uri.ToString().Replace(tfsUri + "/", string.Empty);

                // Acting behalf of PM user
                //var identityService = teamProjectCollection.GetService<IIdentityManagementService>();
                //var identity = identityService.ReadIdentity(
                //        IdentitySearchFactor.AccountName,
                //        "ProjectManager",
                //        MembershipQuery.None,
                //        ReadIdentityOptions.None);
                //var impersonatedTeamProjectCollection = new TfsTeamProjectCollection(teamProjectCollection.Uri, identity.Descriptor);
                //teamProjectCollection = impersonatedTeamProjectCollection;

                ProjectCollections.Add(nodeCollectionName, projectCollection);

                //ReadOnlyCollection<CatalogNode> teamProjectNodes = teamProjectCollection.CatalogNode.QueryChildren(
                //    new Guid[] { CatalogResourceTypes.TeamProject },
                //    false, CatalogQueryOptions.None
                //    );


                // Retrieve team projects
                var css4 = projectCollection.GetService<ICommonStructureService4>();
                var projectInfos = css4.ListAllProjects();
                TeamProjects.Add(nodeCollectionName, projectInfos);


                //if (nodeCollectionName == collectionName)
                //{
                WorkItemStore = projectCollection.GetService<WorkItemStore>();
                if (WorkItemStore == null)
                    throw new Exception("WorkItemStore not found!");
                CommonStructureService4 = projectCollection.GetService<ICommonStructureService4>();

                var identityManagementSrv = WorkItemStore.TeamProjectCollection.GetService<IIdentityManagementService>();
                var _usersGroup = identityManagementSrv
                    .ReadIdentity(IdentitySearchFactor.AccountName, "[" + nodeCollectionName + "]\\Project Collection Valid Users", MembershipQuery.Expanded, ReadIdentityOptions.None);
                TeamFoundationIdentity[] groupMember = identityManagementSrv.ReadIdentities(_usersGroup.Members, MembershipQuery.None, ReadIdentityOptions.None)
                    .Where(member => member.IsContainer == false && member.UniqueName != "EIT\\Administrator" && Regex.IsMatch(member.UniqueName, @"\G(\A[e][i][t]\\\D+\z)", RegexOptions.IgnoreCase)).ToArray();


                #region Users

                //var users = new EntityList<UserBase>(true)
                //    .Where(user => user.Corp == 20 && Regex.IsMatch(user.UserName, @"\G(\A\D+[@][e][i][t]+\z)", RegexOptions.IgnoreCase))
                //    .ToList();

                //var userIdentities = new Dictionary<short, TeamFoundationIdentity>();
                //groupMember.ForEach(member =>
                //{
                //    UserBase correspondingPMUser = null;
                //    if (
                //        users.TryFindFirst(
                //            user => user.UserName.Substring(0, user.UserName.IndexOf('@'))
                //                .SameText(member.UniqueName.Substring(4)),
                //            out correspondingPMUser
                //            )
                //        )
                //        userIdentities.Add(correspondingPMUser.UserId.Value, member);
                //});
                //UserIdentities = userIdentities;

                #endregion

                #region projects

                //var projectWorkSpaces = new Dictionary<short, string>();
                //var projects = new EntityList<Tools.Cmn.Project>(true);
                //projects.ForEach(project =>
                //{
                //    var developerWorkSpace = project.DeveloperWorkSpace ?? (project.AppInfoEntity == null ? null : project.AppInfoEntity.DeveloperWorkSpace);
                //    if (developerWorkSpace != null)
                //        projectWorkSpaces.Add(project.Id.Value, developerWorkSpace);
                //});
                //ProjectWorkSpaces = projectWorkSpaces;

                #endregion
                //}

                if (ProjectCollections.Count() == 0)
                {
                    var exceptionMessage = string.Format("Tfs team project collection with \"{0}\" name not found", nodeCollectionName);
                    var projectCollectionsText = "projectCollections : \n" +
                        ProjectCollections.Keys.Aggregate((str, next) => str = str + Environment.NewLine + next);
                    var logText = string.Format("{0}\n\n{1}", exceptionMessage, projectCollectionsText);
                    throw new Exception(exceptionMessage);
                }

                #endregion

                #region Init VersionControl

                VersionControlServer = projectCollection.GetService<VersionControlServer>();

                #endregion

                #region Init Workspace

                var workspaceName = string.Format("{0}", Environment.MachineName);
                Workspace = VersionControlServer.GetWorkspace(workspaceName, VersionControlServer.AuthorizedUser);

                #endregion
            }
        }
    }
}
