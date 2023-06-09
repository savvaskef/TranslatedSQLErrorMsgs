using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dberrors4dll;

namespace ErrorCaller
{
    class Program
    {
        private static void Main(string[] args)
        {
            //TEST sqlERRORS
            List<string> sqlerrorsMessages = new List<string> 
            { 
                "The INSERT statement conflicted with the CHECK constraint 5nums"+(char)34+". The conflict occurred in database "+(char)34+"errorHandling"+(char)34+", table "+(char)34+"dbo.nonulls"+(char)34+", column 'zip'.\r\nThe statement has been terminated.",
                "The INSERT statement conflicted with the CHECK constraint 5nums"+(char)34+". The conflict occurred in database "+(char)34+"errorHandling"+(char)34+", table "+(char)34+"dbo.nonulls"+(char)34+".\r\nThe statement has been terminated.",
                "Cannot insert duplicate key row in object 'dbo.master' with unique index 'uniqIDX'.\r\nThe statement has been terminated.",
                "The INSERT statement conflicted with the FOREIGN KEY constraint masterdetails"+(char)34+". The conflict occurred in database "+(char)34+"errorHandling"+(char)34+", table "+(char)34+"dbo.master"+(char)34+", column 'masterPK'.\r\nThe statement has been terminated.",
                "Violation of PRIMARY KEY constraint 'PK_nonulls'. Cannot insert duplicate key in object 'dbo.nonulls'.\r\nThe statement has been terminated."
                 };

            //TEST oracleERRORS
            List<string> ORACLEerrorsMessages = new List<string> 
            { 
                "ORA-01400: δεν μπορεί να γίνει εισαγωγή NULL στο (FQM_NET_LARISSA"+(char)34+"."+(char)34+"NONULLS"+(char)34+"."+(char)34+"NOTNULL"+(char)34+")",
                "ORA-02290: ο περιορισμός ελέγχου (FQM_NET_LARISSA.NONULLS_CHK1) παραβιάσθηκε",
                "ORA-00001: παραβιάσθηκε περιορισμός μοναδικότητας (FQM_NET_LARISSA.MASTER_UK1)",
                "ORA-00001: παραβιάσθηκε περιορισμός μοναδικότητας (FQM_NET_LARISSA.NOSEQ_PK)",
                "ORA-02291: ο περιορισμός ακεραιότητας (FQM_NET_LARISSA.DETAILS_MASTER_FK1) παραβιάσθηκε - δεν βρέθηκε κλειδί γονέα"
                        };
            string rslt = "";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string sqlr in sqlerrorsMessages)
            {

                string CNstr = @"Data Source=development2003\sql2005;Initial Catalog=errorHandling;User ID=sa;Password=pass;MultipleActiveResultSets=True";
                rslt = dberrors4dll.dllProgram.treatErrors(sqlr, CNstr, WhichDatabase.SQLdatabase, out dict);
                dict.Clear();
            }
            foreach (string rclr in ORACLEerrorsMessages)
            {
                string CNstr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.0.15)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME = MIK3)));User Id=FQM_NET_LARISSA;Password=pass;";
                rslt = dberrors4dll.dllProgram.treatErrors(rclr, CNstr, WhichDatabase.ORACLEdatabase, out dict);
                dict.Clear();
            }

            //end main
        }
    }
}
