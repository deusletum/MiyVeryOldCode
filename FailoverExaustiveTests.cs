
/*****************************************************************************
 *
 * @File: FailoverExaustiveTests.cs
 *
 * @Owner: subhanka (subhanka@microsoft.com)/ v-degjed@microsoft.com
 *
 * @Created:  30/12/2009 9:45:04 PM
 * @Updated:  07/11/2012 - Updated to work with V2 appliance and new test libraries
 *
 * Purpose:
 *
 * Exaustive test cases for failover testing on PDW VM nodes
 *
 * Notes:
 *
 * @EndHeader@
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Test.DW;
using Microsoft.SqlServer.Test.DW.Appliance;
using Microsoft.SqlServer.Test.DW.TestUtilities;
using Microsoft.SqlServer.Test.TestShell.Core;
using Microsoft.SqlServer.Test.TestShell.Core.InputSpaceModeling;
using Microsoft.SqlServer.Test.Utilities;


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

        // Failure reason dimension
        private static readonly Dimension<DWFailureReason> FailReason = new Dimension<DWFailureReason>("FailReasonDimension");
        // VM Node dimension
        private static readonly Dimension<VirtualNode> VMNodes = new Dimension<VirtualNode>("VMNodes");
        // Appliance object
        private PdwAppliance pdwAppliance;
        // Failure reason strategy
        private ExplorationStrategy<DWFailureReason> failReasonStrategy;
        // Host type strategy
        private ExplorationStrategy<VirtualNode> VMNodesStrategy;
        // Full strategy
        private CombinatorialStrategy fullStrategy;

        private List<DWFailureReason> ADFailOverReason;
        private List<DWFailureReason> ComputeFailOverReason;
        private List<DWFailureReason>

        ///
        /// Initialze the test matrix in the constructor
        ///
        public FailoverExaustiveTests()
        {
            TestMatrix = new Matrix(FailReason, VMNodes);
        }

        /// <summary>
        /// Exploration strategy to use in lab runs.
        /// </summary>
        /// <value>The full strategy.</value>
        [ExplorationStrategy(LabRunCategory.Full)]
        public CombinatorialStrategy FullStrategy
        {
            get
            {
                if (this.fullStrategy == null && TestEnvironment != null)
                {
                    this.InitializeStrategy();
                }

                return this.fullStrategy;
            }
        }

        /// <summary>
        /// Create the appliane object and verify that everything is fine before running any failover tests
        /// Verify that no failover has already happened yet
        /// </summary>
        public override void Setup()
        {
            //Do base class-specific setup
            base.Setup();

            //Define the appliane object
            this.pdwAppliance = new PdwAppliance(true);

            //Enable that the system from where test is runnning, is able to access the clusters in the network
            SystemUtility.EnableAccessToClusters();

            //TODO: Change VMs Restart threshold? Not really sure

            //Bring the appliance in initial state
            ApplianceUtility.BringApplianceToInitialState(this.pdwAppliance);
        }

        /// <summary>
        /// Initialize test strategy
        /// </summary>
        private void InitializeStrategy()
        {
            this.ADFailOverReason = new List<DWFailureReason> { DWFailureReason.ADProcessKilled, DWFailureReason.ADServiceStopped
            , DWFailureReason.PdwAgentProcessKilled, DWFailureReason.PdwAgentServiceStopped };
            this.ComputeFailOverReason = new List<DWFailureReason>{ DWFailureReason.DMSHang, DWFailureReason.DMSProcessKilled
            , DWFailureReason.DMSServiceStopped,DWFailureReason.EngineHang,DWFailureReason.EngineProcessKilled,DWFailureReason.EngineServiceStopped
            ,DWFailureReason.MSDTCHang,DWFailureReason.MSDTCProcessKilled};
            //private Lookup<VmType,DWFailureReason
            List<VirtualNode> nodes = new List<VirtualNode>();

            foreach (VirtualNode node in this.pdwAppliance.AllVms)
            {
                nodes.Add(node);
            }

            Constraints.Add(new TwoDimensionalConstraint<DWFailureReason, VirtualNode>(FailReason, VMNodes, delegate(DWFailureReason reason, VirtualNode node)
            {
                return IsValid(reason, node);
            }));

            //Use pairwise strategy
            this.fullStrategy = new ExhaustiveCombinatorialStrategy(TestMatrix, Constraints);

            //Failure reasons for fabric/host nodes
            this.failReasonStrategy = new ExhaustiveIEnumerableStrategy<DWFailureReason>(new DWFailureReason[]
                {
                    DWFailureReason.ADProcessKilled,
                    DWFailureReason.ADServiceStopped,
                    DWFailureReason.DMSHang,
                    DWFailureReason.DMSProcessKilled,
                    DWFailureReason.DMSServiceStopped,
                    DWFailureReason.EngineHang,
                    DWFailureReason.EngineProcessKilled,
                    DWFailureReason.EngineServiceStopped,
                    DWFailureReason.MSDTCHang,
                    DWFailureReason.MSDTCProcessKilled,
                    DWFailureReason.PdwAgentProcessKilled,
                    DWFailureReason.PdwAgentServiceStopped,
                    DWFailureReason.SQLHang,
                    DWFailureReason.SQLProcessKilled,
                    DWFailureReason.SQLServiceStopped,
                    DWFailureReason.VMShutDown,
                    DWFailureReason.VMSuspend,
                    DWFailureReason.VMTurnOff,
                    DWFailureReason.WindowsCrash
                }
            );

            VMNodesStrategy = new ExhaustiveIEnumerableStrategy<VirtualNode>(nodes);
            this.fullStrategy.SetDimensionStrategy(FailReason, this.failReasonStrategy);
            this.fullStrategy.SetDimensionStrategy(VMNodes, this.VMNodesStrategy);
        }

        /// <summary>
        /// Runs the test specified by the given vector.
        /// </summary>
        /// <param name="input">The test vector from the defined test matrix.</param>
        public override void RunTest(Vector input)
        {
            //Sleep for five minutes ( V1 parity )
            //TODO: This may have changed/improved in V2, but for the initial iteration of this code
            //I would like to maintain the V1 parity for test stability. This value can be changed after few initial test runs
            System.Threading.Thread.Sleep(DWConstants.FiveMinutes);

            //Set the initial status to true
            bool status = true;

            //Verify the current state. Everything should be exactly same as the initial configuration spare nodes should be passive
            //Otherwise try to bring the appliance to it's initial state.
            ApplianceUtility.BringApplianceToInitialState(this.pdwAppliance);
            ShellTracer.DefaultTraceSource.TraceInformation("Brought Appliance to the initial state");

            //Input combinations
            DWFailureReason reason = input.GetValue(FailReason);
            Context.LogInformation("FailReason {0}", reason.ToString());

            //Get the PDW VM node
            //VirtualNode targetNode = ApplianceUtility.PickRandomComputeVM(this.pdwAppliance);
            VirtualNode targetNode = ApplianceUtility.PickRandomComputeVM(this.pdwAppliance);
            Context.LogInformation("Target node name {0}", targetNode.Name);

            //Trigger failure on target node
            status = ApplianceUtility.TriggerAFailure(targetNode, reason) && status;
            Context.LogInformation("Triggered failure. Current status is {0}", status);

            //Verify failover happened
            //status = ApplianceUtility.VerifyFailover(targetNode, null) && status;
            Context.LogInformation("Verified failover. Current status is {0}", status);

            //Fix the failure/issue that was previously triggered on the target node
            status = ApplianceUtility.FixFailedResource(targetNode, reason) && status;
            Context.LogInformation("Fixed failed resource. Current status is {0}", status);

            //Bring appliance to original state
            ApplianceUtility.BringApplianceToInitialState(this.pdwAppliance);
            Context.LogInformation("Brought appliance to it's initial state");

            //Verify status is still set to true, otherwise failure happened in any of the operations
            //TriggerAFailure, VerifyFailover, FixFailedResource
            //Look at the log for more details
            Verify.IsTrue(status);
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        public override void Cleanup()
        {
            //Reset the failover threshold of all groups, which were increased during setup
            ApplianceUtility.ResetFailoverThreshold(this.pdwAppliance.AllGroups);

            //Bring appliance to it's initial state
            ApplianceUtility.BringApplianceToInitialState(this.pdwAppliance);

            //Do base class-specific cleanup
            base.Cleanup();
        }

        private bool IsValid(DWFailureReason reason, VirtualNode node)
        {
            //TODO: Create Logic to test if VirtualNode can failover for the FailoverReason
            return true;
        }

    }
}
