use XML::Parser;
use URI::Escape;
# use strict;


my $CurrentOwner;
my $CurrentLogFile;
my $CurrentSubArea;
my $StartArea = 1;


if ((@ARGV[0] ne "bvt") && (@ARGV[0] ne "smoke") && (@ARGV[0] ne "funcs")) {
	print "Invalid Results type - please make either 'bvt', 'smoke' or 'funcs' \n";
	exit 1;
}

if (@ARGV[0] eq "funcs") {
   @ARGV[0] = "Functional Regression";
}

print "\n\t *** Compiling Results for @ARGV[0] run against build @ARGV[1] *** \n\n";

CreateXMLHead(@ARGV[0],@ARGV[1]);

# Open the XML Paraser and setup default handlers
$p1 = new XML::Parser();
$p1 -> setHandlers(Start => \&project_start, End => \&xend);

$p1 -> parsefile('\\\\smdata\\data\\infra\\xml\\build\\testareas_MOM10SP1.xml');
#$p1 -> parsefile('testareas_MOM10SP1.xml');

my $CurrentArea = "";


CloseXML();

# XML Parser Handler sub-routines
sub project_start
{

   my ($p, $el, %atts) = @_;
   # Find each SubArea
   if ($el eq 'Area')         
   {
      # Pull out the name of the area
      $p -> setHandlers(Char => \&AreaText);
   }
   if ($el eq 'SubArea')         
   {
      # Pull out the name of the subarea and then finds its owner
      $p -> setHandlers(Char => \&text);
      $CurrentOwner = $atts{Owner};
      $CurrentLogFile = $atts{logfile};
   }
}

sub xend
{
   my ($p, $el) = @_;
   $p -> setHandlers(Char => 0);
}

sub text
{
   my ($p, $text) = @_;
   $CurrentSubArea = $text;
   print " $text  (owned by $CurrentOwner) with logfile = $CurrentLogFile\n";
   ParseLogFile();
}

sub AreaText
{
   my ($p, $text) = @_;
   $CurrentArea = $text;
   print " In Area: $CurrentArea \n";
   $p -> setHandlers(Char => 0);
   if ($StartArea) 
   {
      ToXML("<AreaResults>$CurrentArea");
      $StartArea=0;
   } else 
   {  
      ToXML("</AreaResults><AreaResults>$CurrentArea");
   } 

}


#Generic sub-routines - not part of those attached to XML Handles for the XML Parser

sub CreateXMLHead
{
    my ($Type, $Build) = @_;
    open(XMLResultsFile, "> TestResults.xml") or die "Can't open output XML file $!";
    my $HeaderLine = '<TestResults build = "' . $Build .'">' . $Type;
    #ToXML('<TestResults build = "$Build">$Type');
    ToXML($HeaderLine);
}

sub CloseXML
{
   if (!$StartArea) 
   {
      ToXML("</AreaResults>");
   }

    ToXML('</TestResults>');
    close (LOGFILE);
}

sub ParseLogFile
{
  $CurrentLogFile = $CurrentLogFile . ".log";
   open (CURRENTLOG, "$CurrentLogFile") or return;

   @pass=();
   @fail=();
   @abort=();
   @unknown=();

   #print " --- Open Log File $CurrentLogFile --- \n";
   while (<CURRENTLOG>) {
        $output .= $_;
	next unless m!Set=(\d+), Level=(\d+), Var=(\d+) (.*):(.*?)(\w+)!;
	my ($Set, $Level, $Var, $Result) = ($1, $2, $3, $6);

	$Desc = $4;
	$Desc =~ s!\"!\'!mg;
	$Desc =~ s!\&!&amp;!mg;
	$Desc =~ s!\<!&lt;!mg;

	$ResultLine = $Set . "." . $Level . "." . $Var . "    Desc: " . $Desc;
	#print "\t$ResultLine \n";
	if ($Result eq 'VAR_PASS')         
   	{	
		#print "This variation PASSED!\n";
		@pass=(@pass,$ResultLine);
	}
	elsif ($Result eq 'VAR_FAIL')         
   	{	
		#print "This variation FAILED!\n";
		@fail=(@fail,$ResultLine);
	}
	elsif ($Result eq 'VAR_ABORT')         
  	{	
		#print "This variation ABORTED!\n";
		@abort=(@abort,$ResultLine);
	}
	else
	{
		print "Unknown Result!\n";
		@unknown=(@unknown,$ResultLine);
	}

   }
   close (CURRENTLOG);
   LogResultsToXML();
}

sub LogResultsToXML
{
   $NumberPassed = @pass;
   $NumberFailed = @fail;
   $NumberAborted = @abort;
   $NumberUnknown = @unknown;

    $Node = 'pass=' .'"' . $NumberPassed .'" ';
    $Node = $Node . 'fail=' .'"' . $NumberFailed .'" ';
    $Node = $Node . 'abort=' .'"' . $NumberAborted .'" ';
    ToXML("<SubAreaResults $Node>");
    ToXML($CurrentSubArea);

# Remember to put in warning about Unknowns!!!!

print "\t----------------------------------------------------------------- \n";
print "\tThe number of tests passing is $NumberPassed \n";
print "\tThe number of tests failing is $NumberFailed  \n";

if ($NumberAborted != 0)
{
	print "\t** Number of test aborting is $NumberAborted \n";
}

if ($NumberUnknown != 0)
{
	print "\t** Have a $NumberUnknown Unparsable results - please investigate! \n";
}
print "\t----------------------------------------------------------------- \n\n";

# Put in check for No variation case - get out of here at this point
if (($NumberPassed + $NumberAborted + $NumberFailed) != 0)
{
#   print "We do have variations to list \n";
    ToXML('<Variations>');

    if ($NumberPassed != 0)
    {
        ToXML('<VarPass>');
#        print "\n List of Passed variations: \n";

       foreach $currentvar (@pass)
       {
#	printf "PASSED on -> " . $currentvar . "\n";
	XMLVariation($currentvar);
       }
       ToXML('</VarPass>');
   }


    if ($NumberFailed != 0)
   {
        ToXML('<VarFail>');
#      print "\n List of FAILING variations: \n";
      foreach $currentvar (@fail)
      {
#	printf "FAILED on -> " . $currentvar . "\n";
	XMLVariation($currentvar);

      }
        ToXML('</VarFail>');
   }

   if ($NumberAborted != 0)
   {
        ToXML('<VarAbort>');
#      print "\n List of ABORTING variations: \n";
      foreach $currentvar (@abort)
      {
#	printf "ABORTED on -> " . $currentvar . "\n";
	XMLVariation($currentvar);

      }
        ToXML('</VarAbort>');

   }
   ToXML( '</Variations>');

} #List variations to XML

    ToXML('</SubAreaResults>');
}

sub XMLVariation
{
     my ($VarDesc) = @_;
     $_=$VarDesc;
     m!(\d+).(\d+).(\d+).*?Desc:(.*)!;
     my ($Set, $Level, $Var, $Desc) = ($1, $2, $3, $4);

     # Escape the high-level characters to make testruns less sad
     $Desc = uri_escape($Desc,"\x00-\x1f\x7f-\xff");

     $ResultLine = $Set . "." . $Level . "." . $Var . "    Desc: " . $Desc;
     ToXML("<Variation triplet=\"$Set.$Level.$Var\"  desc=\"$Desc\" />");
}

sub ToXML
{
      my ($XMLLine) = @_;
      $old_fh = select(XMLResultsFile);
       print "$XMLLine\n";
       select($old_fh);
}


