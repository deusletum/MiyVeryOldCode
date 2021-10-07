# ----------------------------------------------------------------------
# Name      : newscripter.pl
#
# Company   : oration
# 

#
# Summary   : Create newsafeos.txt unattended file based on template
#             from safeos.txt, using settings from setup.scr
#
# Usage     : mungeini.pl
#
# History   : 12/12/2002 - myocom - Created
# ----------------------------------------------------------------------

use strict;

my $inifilename = 'c:/delete/safeos.txt'; # avoid that whole backslash problem
my $scrfilename = 'c:/delete/setup.scr';
my $newinifile  = 'c:/delete/newsafeos.txt';

my (%ini, %scr);

open(SCRFILE, $scrfilename) || die "Couldn't open $scrfilename for reading: $!\n";
while (<SCRFILE>) {
    next unless /^Params/i;
    chomp;
    my $data = (split('='))[1];

    my ($lvalue, $rvalue, $section) = split('}',$data);

    if ($rvalue =~ /^%(.+)%$/) {
        $rvalue = $ENV{$1};
    }

    $scr{$section} .= "$lvalue=$rvalue\n";
}
close SCRFILE;

open (INIFILE, $inifilename)    || die "Couldn't open $inifilename for reading: $!\n";
open (OUTFILE, ">$newinifile")  || die "Couldn't open $newinifile for writing: $!\n";
while (<INIFILE>) {
    print OUTFILE;
    chomp;
    if (exists $scr{$_}) { 
        print OUTFILE $scr{$_};
    }
}
close OUTFILE;
close INIFILE;