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

            string serverName = Regex.Replace(TfsAddress.ToLower(), @"\A[h][t][t][p][:]\/\/|[:][8][0][8][0]\S+\z", string.Empty);
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
                string nodeCollectionName = projectCollection.Uri.ToString().Replace(tfsUri + "/", string.Empty);

                #region Acting behalf of PM user

                //var identityService = teamProjectCollection.GetService<IIdentityManagementService>();
                //var identity = identityService.ReadIdentity(
                //        IdentitySearchFactor.AccountName,
                //        "ProjectManager",
                //        MembershipQuery.None,
                //        ReadIdentityOptions.None);
                //var impersonatedTeamProjectCollection = new TfsTeamProjectCollection(teamProjectCollection.Uri, identity.Descriptor);
                //teamProjectCollection = impersonatedTeamProjectCollection;

                #endregion

                ProjectCollections.Add(nodeCollectionName, projectCollection);

                // Retrieve team projects
                ICommonStructureService4 css4 = projectCollection.GetService<ICommonStructureService4>();
                ProjectInfo[] projectInfos = css4.ListAllProjects();
                TeamProjects.Add(nodeCollectionName, projectInfos);

                WorkItemStore = projectCollection.GetService<WorkItemStore>();
                if (WorkItemStore == null)
                    throw new Exception("WorkItemStore not found!");
                CommonStructureService4 = projectCollection.GetService<ICommonStructureService4>();

                #region Identities

                IIdentityManagementService identityManagementSrv = WorkItemStore.TeamProjectCollection.GetService<IIdentityManagementService>();
                TeamFoundationIdentity _usersGroup = identityManagementSrv
                    .ReadIdentity(IdentitySearchFactor.AccountName, "[" + nodeCollectionName + "]\\Project Collection Valid Users", MembershipQuery.Expanded, ReadIdentityOptions.None);
                TeamFoundationIdentity[] groupMember = identityManagementSrv.ReadIdentities(_usersGroup.Members, MembershipQuery.None, ReadIdentityOptions.None)
                    .Where(member => member.IsContainer == false && member.UniqueName != "Domain\\UserName" && Regex.IsMatch(member.UniqueName, @"\G(\A[D][o][m][a][i][n]\\\D+\z)", RegexOptions.IgnoreCase)).ToArray();

                #endregion

                if (!ProjectCollections.Any())
                {
                    string exceptionMessage = string.Format("Tfs team project collection with \"{0}\" name not found", nodeCollectionName);
                    string projectCollectionsText = "projectCollections : \n" +
                        ProjectCollections.Keys.Aggregate((str, next) => str = str + Environment.NewLine + next);
                    string logText = string.Format("{0}\n\n{1}", exceptionMessage, projectCollectionsText);
                    throw new Exception(exceptionMessage);
                }

                #endregion

                #region Init VersionControl

                VersionControlServer = projectCollection.GetService<VersionControlServer>();

                #endregion

                #region Init Workspace

                string workspaceName = string.Format("{0}", Environment.MachineName);
                Workspace = VersionControlServer.GetWorkspace(workspaceName, VersionControlServer.AuthorizedUser);

                #endregion
            }
        }
    }
}
