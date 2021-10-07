// ----------------------------------------------------------------------
// Name      : VSPerf_Maddog
//
// Company   : oration
//

//
// Summary   : Start VSPerf Maddog PRI 1 tests
//
// Usage     : See usage Function
//
// History   : 1/27/2005 - Dean Gjedde - Created
// ----------------------------------------------------------------------
using System;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using MadDogObjects;

namespace VSPerf_Maddog
{
	/// <summary>
	/// Main Class
	/// </summary>
	class Start
	{
        #region Method Main
		/// <summary>
		/// Start VSPerf Tests
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            string Newbuild = "";
            string sku = "pro";
            string Goodbuild = "50204.00";
            int RowCountPro = 13;
            int RowCountVSTS = 12;
            int RowCount = RowCountPro;
            string Beta2VBL = "BETA2";
            string Lab22DevVBL = "LAB22DEV";
            string VBLBranch = Lab22DevVBL;
            Product NewProduct = new Product();
            bool PrivateRun = false;

            if(args.Length < 1)
            {
                Usage();
            }
            foreach(string arg in args)
            {
                string[] aSplit = arg.Split(":".ToCharArray());
                switch(aSplit[0].ToUpper())
                {
                    case "/?":
                        Usage();
                        break;
                    case "":
                        Usage();
                        break;
                    case "/NEWBUILD":
                        if(aSplit[1] == "")
                        {
                            Usage();
                        }
                        Newbuild = aSplit[1];
                        break;
                    case "/SKU":
                        sku = aSplit[1].ToUpper();
                        switch(sku)
                        {
                            case "":
                                Usage();
                                break;
                            case "PRO":
                                break;
                            case "VSTS":
                                RowCount = RowCountVSTS;
                                break;
                            default:
                                Usage();
                                break;
                        }
                        break;
                    case "/VBL":
                        switch(aSplit[1].ToUpper())
                        {
                            case "":
                                Usage();
                                break;
                            case "LAB22DEV":
                                break;
                            case "BETA2":
                                VBLBranch = Beta2VBL;
                                break;
                            default:
                                Usage();
                                break;
                        }
                        break;
                    case "/PRIVATE":
                        PrivateRun = true;
                        RowCount = RowCountVSTS;
                        break;
                }
            }
            Regex Reg = new Regex(Goodbuild);
  
            try
            {
                MadDogObjects.Utilities.Security.SetDB("mdsql2", "whidbey");
                MadDogObjects.Utilities.Security.AppName = "VSPerf Maddog Automation App";
                MadDogObjects.Utilities.Security.AppOwner = "a-deagje";

                Console.WriteLine("VSPerf_Maddog.exe - VSPerf Maddog Automation Application - v 1.0");
                Console.WriteLine("");

                //Get the last builds runs, clone them, and change build info
                Console.WriteLine("Cloning runs");
                QueryObject RunQ = new QueryObject(QueryConstants.BaseObjectTypes.Run);
                RunQ.QueryStartGroup();
                RunQ.QueryAdd(Tables.RunTable.TITLEFIELD,QueryConstants.CONTAINS,"perfds",QueryConstants.AND_OPERATOR);
                RunQ.QueryAdd(Tables.RunTable.TITLEFIELD,QueryConstants.CONTAINS,Goodbuild,QueryConstants.AND_OPERATOR);
                RunQ.QueryAdd(Tables.RunTable.TITLEFIELD,QueryConstants.CONTAINS,sku,QueryConstants.AND_OPERATOR);
                if(PrivateRun)
                {
                    RunQ.QueryAdd(Tables.RunTable.TITLEFIELD,QueryConstants.CONTAINS,"private",QueryConstants.AND_OPERATOR);
                }
                else
                {
                    RunQ.QueryAdd(Tables.RunTable.TITLEFIELD,QueryConstants.DOESNOTCONTAIN,"private",QueryConstants.AND_OPERATOR);
                }
                RunQ.QueryAdd(Tables.RunTable.TITLEFIELD,QueryConstants.CONTAINS,VBLBranch,QueryConstants.AND_OPERATOR);
                RunQ.QueryEndGroup();
                Collection RunCol = RunQ.GetCollection();
                if(RunCol.Count != RowCount)
                {
                    Console.WriteLine("There are not 13 runs in runs collection");
                    Environment.Exit(2);
                }
                foreach(Run r in RunCol)
                {
                    //Create a new product
                    Console.WriteLine("Creating new Product");
                    Product DefaultProduct = r.Product;
                    NewProduct = DefaultProduct.Clone();
                    NewProduct.Name = "VsPerf " + VBLBranch + " Layouts Ret " + sku + " " + Newbuild;
                    NewProduct.Location = "\\\\cpvsbuild\\drops\\Whidbey\\" + VBLBranch
                        + "\\layouts\\x86ret\\" + Newbuild + "\\enu\\vs\\" + sku + "\\cd\\NetSetup";
                    NewProduct.Version = Newbuild;
                    NewProduct.VBL = new VBL(VBLBranch);
                    NewProduct.Milestone = DefaultProduct.Milestone;
                    NewProduct.BuildType = DefaultProduct.BuildType;
                    NewProduct.ProductType = DefaultProduct.ProductType; //new ProductType(11); Visual Basic 7.0
                    NewProduct.Package = DefaultProduct.Package; //new Package(PackageID); VSPerf 8.0 Lab22DevRet Pro Setup-\\cpvsbuild
                    NewProduct.Platform = DefaultProduct.Platform;
                    NewProduct.Save();
                    Console.WriteLine("New Product created, Product ID: " + NewProduct.ID);
                    break;
                }

                foreach(Run r in RunCol)
                {
                    Run tempRun = r.Clone();
                    tempRun.Title = Reg.Replace(tempRun.Title,Newbuild);
                    tempRun.Product = NewProduct;
                    tempRun.Save();
                    Console.WriteLine("Created Run: " + tempRun.Title);
                    Run.RunHelpers.StartRun(tempRun);
                    Console.WriteLine("Starting Run: " + tempRun.Title);
                }
            }
            catch(MaddogException Exp)
            {
                Console.WriteLine("There was an MaddogException");
                Console.WriteLine(Exp.ToString());
            }
            catch(Exception Exp)
            {
                Console.WriteLine("There was an Exception");
                Console.WriteLine(Exp.ToString());
            }

            //Everything worked
            Console.WriteLine("Operation Complete");
            Environment.Exit(0);
		}
        #endregion

        #region Method Usage
        /// <summary>
        /// Displays Usage
        /// </summary>
        static void Usage()
        {
            Console.WriteLine("VSPerf_Maddog.exe takes a lab22dev build number and clones the runs for that build for a new build.");
            Console.WriteLine("/NEWBUILD:<buildnumber> [/SKU:<productsku - PRO|VSTS>] [/VBL:<vbl - BETA2|LAB22DEV] [/PRIVATE]");
            Console.WriteLine("Example: VSPerf_Maddog.exe /NEWBUILD:50118.00");
            Environment.Exit(1);
        }
        #endregion
	}
}
