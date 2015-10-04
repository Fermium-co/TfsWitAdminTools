using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Service;

namespace TfsWitAdminTools.Tests
{
    [TestClass]
    public class WitAdminServiceTests
    {
        [TestMethod]
        public void ExportWorkItemDefenitionShouldCallInvokeCommandsWithValidArgument()
        {
            Mock<IConfigProvider> configProvider = new Mock<IConfigProvider>();

            configProvider.Setup(c => c.GetConfig(It.IsAny<string>())).Returns(string.Empty);

            Mock<WitAdminService> witAdminService = new Mock<WitAdminService>(configProvider.Object);

            witAdminService.Setup(w => w.InvokeCommand(It.IsAny<string>(), false)).Verifiable();

            Mock<ITFManager> tfManager = new Mock<ITFManager>();

            Mock<IWitAdminProcessService> process = new Mock<IWitAdminProcessService>();

            DiManager.Current.Init();

            DiManager.Current.Register(process.Object);

            witAdminService.Object.ExportWorkItemDefenition(tfManager.Object, "Test", "Test", "Test");

            witAdminService.Verify(w => w.InvokeCommand("exportwitd /collection:/Test /p:Test /n:\"Test\"", false), Times.Once());
        }
    }
}