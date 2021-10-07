/*****************************************************************************
 *
 *
 * @File: FailoverExaustiveTests.cs
 *
 * @Owner: subhanka (subhanka@microsoft.com)
 *
 * @Created:  30/12/2009 9:45:04 PM
 *
 * Purpose:
 *
 * Exaustive test cases for failover testing
 *
 * Notes:
 *
 * @EndHeader@
 *****************************************************************************/
using System;
using System.Text;
using Microsoft.SqlServer.Test.TestShell.Core;
using Microsoft.SqlServer.Test.TestShell.Core.InputSpaceModeling;
using Microsoft.SqlServer.Test.Utilities;
using Microsoft.SqlServer.Test.DW;
using Microsoft.SqlServer.Test.DW.TestUtilities;


namespace Microsoft.SqlServer.Test.TestShellTests.DW.Failover
{
    /// <summary>
    /// Failover Exaustive test suites
    /// Reasons: all possible reasons
    /// Node: All nodes in the appliance
    /// This test suite can be used to verify setup of all services and networks on all nodes in the appliance
    /// </summary>
    [AlwaysLog]
    [TestSuite(LabRunCategory.Full, WttTestSuiteType.DWFunctional, FeatureCoverage.FailoverClustering)]
    public class FailoverExaustiveTests : InputSpaceModelingTestSuite
    {

        private ClusterHelper failoverHelper;
        private DWAppliance dwAppliance;

        /// <summary>
        /// Create the appliane object and verify that everything is fine before running any failover tests
        /// Verify that no failover has already happened yet
        /// </summary>
        public override void Setup()
        {
            base.Setup(); //Do base class-specific setup

            dwAppliance = new DWAppliance(this);
            failoverHelper.EnableAccessToClusters();
            failoverHelper.ChangeGlobalConfigToRunTests(dwAppliance);
            failoverHelper.BringApplianceToInitialState(dwAppliance);
        }

        #region Dimension declarations
        private static readonly Dimension<FailoverReason> FailReason = new Dimension<FailoverReason>("FailReasonDimension");
        private static readonly Dimension<string> ActiveNode = new Dimension<string>("ActiveNodeDimension");
        #endregion

        #region Exploration strategies
        ExplorationStrategy<FailoverReason> FailReasonStrategy;
        ExplorationStrategy<string> ActiveNodeStrategy;
        private CombinatorialStrategy _fullStrategy;

        /// <summary>
        /// Exploration strategy to use in lab runs.
        /// </summary>
        /// <value>The full strategy.</value>
        [ExplorationStrategy(LabRunCategory.Full)]
        public CombinatorialStrategy FullStrategy
        {
            get
            {
                if (_fullStrategy == null && TestEnvironment != null)
                {
                    InitializeStrategy();
                }
                return _fullStrategy;
            }
        }
        #endregion

        private void InitializeStrategy()
        {
            int nodeNameIndex = 0;

            string[] nodeNames = new string[dwAppliance.ControlCluster.ActiveNodes.Count + dwAppliance.ComputeClusters.Count * dwAppliance.ComputeClusters[0].ActiveNodes.Count];

            foreach (DWCluster clus in dwAppliance.AllClusters)
            {
                foreach (Node n in clus.ActiveNodes)
                {
                    nodeNames[nodeNameIndex++] = n.Name;
                }
            }


            Constraints.Add(new TwoDimensionalConstraint<FailoverReason, string>(FailReason, ActiveNode, delegate(FailoverReason reason, string nodeName)
            {
                return IsValid(reason, nodeName);
            }));


            _fullStrategy = new ExhaustiveCombinatorialStrategy(TestMatrix, Constraints);
            FailReasonStrategy = new ExhaustiveIEnumerableStrategy<FailoverReason>(new FailoverReason[]
                {

                    FailoverReason.DMSProcessKilled,
                    FailoverReason.DMSServiceStopped,
                    FailoverReason.EngineProcessKilled,
                    FailoverReason.EngineServiceStopped,
                    FailoverReason.SQLProcessKilled,
                    FailoverReason.SQLServiceStopped,
                    FailoverReason.Windows,
                    FailoverReason.MSDTCProcessKilled,
                    //FailoverReason.InfinityBandNetworkInterfaceDown, disabled because of defect 512035
                    FailoverReason.DMSHang,
                    FailoverReason.EngineHang,
                    FailoverReason.MSDTCHang,
                    FailoverReason.SQLHang,


                    ///FailoverReason.MSDTCServiceStopped,
                    ////FailoverReason.SQLAgentHang,
                    ////FailoverReason.SQLAgentProcessKilled,
                    ////FailoverReason.SQLAgentServiceStopped,
                    ////FailoverReason.StorageConnectionNetworkInterfaceDown,
                    FailoverReason.EnterpriseNetworkInterfaceDown,
                    ////FailoverReason.PrivateNetworkInterfaceDown
                }
            );
            ActiveNodeStrategy = new ExhaustiveIEnumerableStrategy<string>(nodeNames);
            _fullStrategy.SetDimensionStrategy(FailReason, FailReasonStrategy);
            _fullStrategy.SetDimensionStrategy(ActiveNode, ActiveNodeStrategy);
        }


        /// <summary>
        /// initialze the test matrix in the constructor
        /// </summary>
        public FailoverExaustiveTests()
        {
            failoverHelper = new ClusterHelper(this);
            TestMatrix = new Matrix(FailReason, ActiveNode);
        }

        /// <summary>
        /// Runs the test specified by the given vector.
        /// </summary>
        /// <param name="input">The test vector from the defined test matrix.</param>
        public override void RunTest(Vector input)
        {
            System.Threading.Thread.Sleep(300000);
            bool success = false;
            //Verify the current state everything should be exactly same as the initial configuration spare nodes should be passive
            //Otherwise try to bring the appliance to it's initial state.
            failoverHelper.BringApplianceToInitialState(dwAppliance);
            ShellTracer.DefaultTraceSource.TraceInformation("Brought Appliance to the initial state");

            // input combinations
            string failingNodeName = input.GetValue(ActiveNode);
            FailoverReason reason = input.GetValue(FailReason);
            ShellTracer.DefaultTraceSource.TraceInformation("Current combination is: Failover reason: {0}, Failing node name:{1}", reason, failingNodeName);

            //Pick node based on input type
            Node node = failoverHelper.GetNodeByName(input.GetValue(ActiveNode), dwAppliance);
            ShellTracer.DefaultTraceSource.TraceInformation("target node name: " + node.Name);

            //configure the service not to be restarted in the node ( so that the node directly fails over )
            failoverHelper.ChangeLocalConfig(node, reason);
            ShellTracer.DefaultTraceSource.TraceInformation("Changed local config");

            //cause failover on that node for the particular reason
            bool problemCaused = failoverHelper.CauseFailover(node, reason);
            ShellTracer.DefaultTraceSource.TraceInformation("Caused failover");

            //verify that failover has happened
            success = failoverHelper.VerifyFailover(node, dwAppliance, problemCaused);
            ShellTracer.DefaultTraceSource.TraceInformation("Verified failover");

            //Bring back the failed resource in the failed node
            failoverHelper.BringBackFailedResources(node, reason);
            ShellTracer.DefaultTraceSource.TraceInformation("Brought back the failed resource");

            // move the resource to the previously active node
            failoverHelper.MoveGroupToPreviousActiveNode(node.OwningGroup.Name, node.ParentCluster.Name, node.Name, dwAppliance);
            ShellTracer.DefaultTraceSource.TraceInformation("Moved the resource group to the previously active node");

            //revert back the configuration changes in the node
            failoverHelper.RevertBackLocalConfigChanges(node, reason);
            ShellTracer.DefaultTraceSource.TraceInformation("Reverted back local config changes");

            if (!success)
                Verify.Fail("Failover test failed. Reason: " + reason + ", node: " + node.Name);

        }

        //validate the input combination
        private bool IsValid(FailoverReason reason, string nodeName)
        {
            foreach (DWCluster clus in dwAppliance.AllClusters)
            {
                foreach (Node n in clus.ActiveNodes)
                {
                    if (n.Name == nodeName && clus.Type == ClusterType.ComputeCluster)
                    {
                        if ((reason == FailoverReason.EngineHang || reason == FailoverReason.EngineProcessKilled || reason == FailoverReason.EngineServiceStopped || reason == FailoverReason.EnterpriseNetworkInterfaceDown))
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
            failoverHelper.RevertBackGlobalConfigChanges(dwAppliance);
            failoverHelper.BringApplianceToInitialState(dwAppliance);
        }

    }
}
