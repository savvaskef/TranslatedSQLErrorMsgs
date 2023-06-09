using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using Oracle.DataAccess.Client;

namespace dberrors4dll
{
    public enum WhichDatabase { ORACLEdatabase, SQLdatabase }

   public class dllProgram
    {
        public static bool SHOWMSG = true;
        public static string language = "greek";
        
       //private static void Main(string[] args)
       // {
       //         //TEST sqlERRORS
       //     List<string> sqlerrorsMessages = new List<string> 
       //     { 
       //         "The INSERT statement conflicted with the CHECK constraint 5nums"+(char)34+". The conflict occurred in database "+(char)34+"errorHandling"+(char)34+", table "+(char)34+"dbo.nonulls"+(char)34+", column 'zip'.\r\nThe statement has been terminated.",
       //         "The INSERT statement conflicted with the CHECK constraint 5nums"+(char)34+". The conflict occurred in database "+(char)34+"errorHandling"+(char)34+", table "+(char)34+"dbo.nonulls"+(char)34+".\r\nThe statement has been terminated.",
       //         "Cannot insert duplicate key row in object 'dbo.master' with unique index 'uniqIDX'.\r\nThe statement has been terminated.",
       //         "The INSERT statement conflicted with the FOREIGN KEY constraint masterdetails"+(char)34+". The conflict occurred in database "+(char)34+"errorHandling"+(char)34+", table "+(char)34+"dbo.master"+(char)34+", column 'masterPK'.\r\nThe statement has been terminated.",
       //         "Violation of PRIMARY KEY constraint 'PK_nonulls'. Cannot insert duplicate key in object 'dbo.nonulls'.\r\nThe statement has been terminated."
       //          };

       //     //TEST oracleERRORS
       //     List<string> ORACLEerrorsMessages = new List<string> 
       //     { 
       //         "ORA-01400: δεν μπορεί να γίνει εισαγωγή NULL στο (FQM_NET_LARISSA"+(char)34+"."+(char)34+"NONULLS"+(char)34+"."+(char)34+"NOTNULL"+(char)34+")",
       //         "ORA-02290: ο περιορισμός ελέγχου (FQM_NET_LARISSA.NONULLS_CHK1) παραβιάσθηκε",
       //         "ORA-00001: παραβιάσθηκε περιορισμός μοναδικότητας (FQM_NET_LARISSA.MASTER_UK1)",
       //         "ORA-00001: παραβιάσθηκε περιορισμός μοναδικότητας (FQM_NET_LARISSA.NOSEQ_PK)",
       //         "ORA-02291: ο περιορισμός ακεραιότητας (FQM_NET_LARISSA.DETAILS_MASTER_FK1) παραβιάσθηκε - δεν βρέθηκε κλειδί γονέα"
       //                 };
       //     string rslt="";
       //     Dictionary<string, string> dict = new Dictionary<string, string>();
       //     foreach (string sqlr in sqlerrorsMessages) {

       //         string CNstr = @"Data Source=development2003\sql2005;Initial Catalog=errorHandling;User ID=sa;Password=pass;MultipleActiveResultSets=True";
       //         rslt = treatErrors(sqlr, CNstr , WhichDatabase.SQLdatabase, out dict);
       //         dict.Clear(); 
       //     }
       //     foreach (string rclr in ORACLEerrorsMessages)
       //     {  string CNstr= @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.0.15)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME = MIK3)));User Id=FQM_NET_LARISSA;Password=pass;";
       //         rslt = treatErrors(rclr,  CNstr, WhichDatabase.ORACLEdatabase, out dict);
       //      dict.Clear(); 
       //     }
            
       //     //end main
       // }


        public static string treatORACLError(string msg, string oConnStr, out Dictionary<string, string> dict)
        {

            //constants go here
            List<errorlist> errors = new List<errorlist> 
            { 
             new errorlist { errorid=1  ,   errorShortDesc = "nullentry", errorVerifyifTextIncluded =  " δεν μπορεί να γίνει εισαγωγή NULL" },
             new errorlist { errorid=8  ,   errorShortDesc = "xpressionConstraint", errorVerifyifTextIncluded =  "ο περιορισμός ελέγχου" },
             new errorlist { errorid=9  ,   errorShortDesc = "uniqIDX", errorVerifyifTextIncluded =  "παραβιάσθηκε περιορισμός μοναδικότητας" },
             new errorlist { errorid=10  ,   errorShortDesc = "fkeyNoMatch", errorVerifyifTextIncluded =  " παραβιάσθηκε - δεν βρέθηκε κλειδί γονέα" },
             };

            //IDictionary<int, errorvars> errorvariables = new Dictionary<int, errorvars> 
            //{ 
            // {1,new errorvars(){ errorvarsID=1  ,   errorvarsStartsWith = "nullentry ", errorvarsEndsWith =  "nnot insert the value NULL into column"="",errorvarsVarName =""}},
            // {2,new errorvars(){ errorvarsID=1  ,   errorvarsStartsWith = "nullentry ", errorvarsEndsWith =  "nnot insert the value NULL into column"="",errorvarsVarName =""}}
            //};

            List<errorvars> errorsvariables = new List<errorvars> 
            { 
            new errorvars { errorvarsID=1  ,   errorvarsStartsWith = "ORA-01400: δεν μπορεί να γίνει εισαγωγή NULL στο ("+(char)34, errorvarsEndsWith = ( char)34+"."+(char)34,errorvarsVarName="databasename",errorvarsFK=1 },
            new errorvars { errorvarsID=2  ,   errorvarsStartsWith = ( char)34+"."+(char)34, errorvarsEndsWith =( char)34+"."+(char)34  ,errorvarsVarName="tablename",errorvarsFK=1 },
            new errorvars { errorvarsID=21  ,   errorvarsStartsWith = ( char)34+"."+(char)34, errorvarsEndsWith =( char)34+")",errorvarsVarName="fieldname",errorvarsFK=1 },

            new errorvars { errorvarsID=3  ,   errorvarsStartsWith = "ORA-02290: ο περιορισμός ελέγχου (", errorvarsEndsWith = ".",errorvarsVarName="databasename",errorvarsFK=8},
            new errorvars { errorvarsID=4  ,   errorvarsStartsWith = ".", errorvarsEndsWith =  ") παραβιάσθηκε",errorvarsVarName="constraintname",errorvarsFK=8 },
 

            new errorvars { errorvarsID=7  ,   errorvarsStartsWith = "ORA-00001: παραβιάσθηκε περιορισμός μοναδικότητας (", errorvarsEndsWith = ".",errorvarsVarName="databasename",errorvarsFK=9 },
            new errorvars { errorvarsID=8  ,   errorvarsStartsWith = ".", errorvarsEndsWith =  ")",errorvarsVarName="constraintname",errorvarsFK=9 },
 
            new errorvars { errorvarsID=10  ,   errorvarsStartsWith = "ORA-02291: ο περιορισμός ακεραιότητας (", errorvarsEndsWith =  ".",errorvarsVarName="databasename",errorvarsFK=10 },
            new errorvars { errorvarsID=11  ,   errorvarsStartsWith = ".", errorvarsEndsWith = ") παραβιάσθηκε - δεν βρέθηκε κλειδί γονέα",errorvarsVarName="constraintname",errorvarsFK=10 },
 

                };

            List<errorLngMsg> errorMessages = new List<errorLngMsg> 
           {new errorLngMsg { errlngID=23, shortdesc = "nullentry", language =  "greek",msg="Το πεδίο tablename.fieldname δεν επιτρέπεται να είναι κενό." },
            new errorLngMsg { errlngID=24, shortdesc = "xpressionConstraint", language =  "greek",msg="Ο περιορισμός constraintname για to πεδίο fieldname δεν επαληθεύεται." },
            new errorLngMsg { errlngID=25, shortdesc = "uniqIDX", language =  "greek",msg="O περιορισμός constraintname δεν επιτρέπει διπλότυπα στο πεδίο tablename.fieldname." },
            new errorLngMsg { errlngID=26, shortdesc = "fkeyNoMatch", language =  "greek",msg="O περιορισμός constraintname επιτρέπει μόνο τιμές του πεδίου tablename.fieldname στο πεδίο ftabname.ffldname." },
            new errorLngMsg { errlngID=27, shortdesc = "primarykey", language =  "greek",msg="O περιορισμός constraintname δεν επιτρέπει διπλότυπα στο πεδίο tablename.fieldname." },
            new errorLngMsg { errlngID=28, shortdesc = "nullentry ", language =  "english",msg="the field fieldname cannot be empty." },
            new errorLngMsg { errlngID=29, shortdesc = "xpressionConstraint", language =  "english",msg="Constraint constraintname for field fieldname does not evaluate." },
            new errorLngMsg { errlngID=30, shortdesc = "uniqIDX", language =  "english",msg="Constraint constraintname does not allow dublicates on the field tablename.fieldname." },
            new errorLngMsg { errlngID=31, shortdesc = "fkeyNoMatch", language =  "english",msg="Consraint constraintname allows only values of table tablename.fieldname on the field ftabname.ffldname." },
            new errorLngMsg { errlngID=32, shortdesc = "primarykey", language =  "english",msg="Constraint 'constraintname' does not allow dublicates on the field 'tablename'.'fieldname'." }

             };

            //  string oConnStr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.0.15)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME = MIK3)));User Id=FQM_NET_LARISSA;Password=fffsfff;";
            //  oConnStr = @"Data Source=LASITHI.MIK3.GR;User ID=FQM_NET_LARISSA;Password=pass;";


            //first Pass wraps around message information
            Dictionary<string, string> dictr = new Dictionary<string, string>();
            if (SHOWMSG) { Console.WriteLine(msg); }
            string logmessage = msg;
            var errListkey = 0;
            string outmsg = "";
            foreach (var e in errors)
            {
                string etext = e.errorVerifyifTextIncluded;
                if (msg.IndexOf(etext) > 0)
                {
                    errListkey = e.errorid;
                    string errShortdesc = e.errorShortDesc;
                    try
                    {
                        outmsg = errorMessages.Where(desc => desc.shortdesc == errShortdesc && desc.language == language).First().msg;

                    }
                    catch (Exception ex)
                    {
                        string a = ex.InnerException.Message;
                        dict = dictr;
                        return "NotTranslated";
                    }
                    break;
                }
            }



            ////firstPass :deduct variables from message 
            int txtstart;
            int txtfin;

            foreach (var varnames in errorsvariables.Where(rplc => rplc.errorvarsFK == errListkey))
            {
                txtstart = msg.IndexOf(varnames.errorvarsStartsWith) + varnames.errorvarsStartsWith.Length;
                //truncate the msg to allow for endswith content to occur more than once
                msg = msg.Substring(txtstart, msg.Length - txtstart);
                txtstart = 0;
                txtfin = msg.IndexOf(varnames.errorvarsEndsWith);
                string rplcStr = msg.Substring(txtstart, txtfin - txtstart);
                outmsg = outmsg.Replace(varnames.errorvarsVarName, rplcStr);
                dictr.Add(varnames.errorvarsVarName, rplcStr);
            }

            ////SecondPass :deduct fieldnames from variables via SQL calls on systm views
            switch (errListkey)
            {
                case 9:
                case 11:
                    var oConn = new OracleConnection(oConnStr);
                    oConn.Open();

                    string sql1 = @"SELECT Constraint_name, Table_name, Column_name  
                          from User_cons_columns
                          where Constraint_name= '" + dictr["constraintname"] + "'";

                    OracleDataAdapter dadapter = new OracleDataAdapter();
                    dadapter.SelectCommand = new OracleCommand(sql1, oConn);
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    dictr.Add("fieldname", dset.Tables[0].Rows[0]["Column_name"].ToString());
                    outmsg = outmsg.Replace("fieldname", dset.Tables[0].Rows[0]["Column_name"].ToString());
                    dictr.Add("tablename", dset.Tables[0].Rows[0]["Table_name"].ToString());
                    outmsg = outmsg.Replace("tablename", dset.Tables[0].Rows[0]["Table_name"].ToString());
                    oConn.Close();
                    break;
                case 10:

                    var oConn2 = new OracleConnection(oConnStr);
                    oConn2.Open();

                    string sql2 = @"SELECT Constraint_name, Table_name, Column_name  
                          from User_cons_columns
                          where Constraint_name= '" + dictr["constraintname"] + "'";

                    OracleDataAdapter dadapter2 = new OracleDataAdapter();
                    dadapter2.SelectCommand = new OracleCommand(sql2, oConn2);
                    DataSet dset2 = new DataSet();
                    dadapter2.Fill(dset2);
                    dictr.Add("ffldname", dset2.Tables[0].Rows[0]["Column_name"].ToString());
                    outmsg = outmsg.Replace("ffldname", dset2.Tables[0].Rows[0]["Column_name"].ToString());
                    dictr.Add("ftabname", dset2.Tables[0].Rows[0]["Table_name"].ToString());
                    outmsg = outmsg.Replace("ftabname", dset2.Tables[0].Rows[0]["Table_name"].ToString());


                    sql2 = @"select r_constraint_name,table_name from user_constraints 
                    where Constraint_name= '" + dictr["constraintname"] + "'";
                    dadapter2.SelectCommand = new OracleCommand(sql2, oConn2);
                    dset2 = new DataSet();
                    dadapter2.Fill(dset2);
                    dictr.Add("fieldname", dset2.Tables[0].Rows[0]["r_constraint_name"].ToString());
                    outmsg = outmsg.Replace("fieldname", dset2.Tables[0].Rows[0]["r_constraint_name"].ToString());

                    sql2 = @"select r_constraint_name,table_name from user_constraints 
                      
                    where Constraint_name= '" + dictr["fieldname"] + "'";
                    dadapter2.SelectCommand = new OracleCommand(sql2, oConn2);
                    dset2 = new DataSet();
                    dadapter2.Fill(dset2);
                    dictr.Add("tablename", dset2.Tables[0].Rows[0]["table_name"].ToString());
                    outmsg = outmsg.Replace("tablename", dset2.Tables[0].Rows[0]["table_name"].ToString());



                    oConn2.Close();
                    break;


                case 8:
                    oConn = new OracleConnection(oConnStr);
                    oConn.Open();

                    sql1 = @"SELECT Constraint_name, Table_name, Column_name  
                              from User_cons_columns
                              where Constraint_name= '" + dictr["constraintname"] + "'";

                    dadapter = new OracleDataAdapter();
                    dadapter.SelectCommand = new OracleCommand(sql1, oConn);
                    dset = new DataSet();
                    dadapter.Fill(dset);
                    string conqa = "";
                    foreach (DataRow r in dset.Tables[0].Rows)
                    {
                        conqa += r["column_name"].ToString() + ",";
                    }
                    conqa = conqa.Substring(0, conqa.Length - 1);
                    dictr.Add("fieldname", conqa);
                    outmsg = outmsg.Replace("fieldname", conqa);
                    dictr.Add("tablename", dset.Tables[0].Rows[0]["Table_name"].ToString());
                    outmsg = outmsg.Replace("tablename", dset.Tables[0].Rows[0]["Table_name"].ToString());


                    sql1 = @"SELECT SEARCH_CONDITION from user_CONSTRAINTS  where Constraint_name= '" + dictr["constraintname"] + "'";

                    dadapter = new OracleDataAdapter();
                    dadapter.SelectCommand = new OracleCommand(sql1, oConn);
                    dset = new DataSet();
                    dadapter.Fill(dset);
                    dictr.Add("condition", dset.Tables[0].Rows[0]["SEARCH_CONDITION"].ToString());
                    outmsg = outmsg.Replace("condition", dset.Tables[0].Rows[0]["SEARCH_CONDITION"].ToString());

                    oConn.Close();
                    break;



            }

            dict = dictr;
            if (SHOWMSG) { Console.WriteLine(outmsg); }
            return outmsg;


        }

        public static string treatSQLError(string msg, string oConnStr, out Dictionary<string, string> dict)
        {

            //constants go here
            List<errorlist> errors = new List<errorlist> 
            { 
             new errorlist { errorid=10  ,   errorShortDesc = "fkeyNoMatch", errorVerifyifTextIncluded =  "INSERT statement conflicted with the FOREIGN KEY constraint" },
             new errorlist { errorid=1  ,   errorShortDesc = "nullentry", errorVerifyifTextIncluded =  "nnot insert the value NULL into column" },
             new errorlist { errorid=12  ,   errorShortDesc = "xpressionSingle", errorVerifyifTextIncluded =  (char)34+", column '" },
             new errorlist { errorid=8  ,   errorShortDesc = "xpressionConstraint", errorVerifyifTextIncluded =  "INSERT statement conflicted with the CHECK constraint" },
             new errorlist { errorid=9  ,   errorShortDesc = "uniqIDX", errorVerifyifTextIncluded =  "' with unique index '" },
             new errorlist { errorid=11 ,   errorShortDesc = "primarykey", errorVerifyifTextIncluded =  "olation of PRIMARY KEY constraint " }
            };

            //IDictionary<int, errorvars> errorvariables = new Dictionary<int, errorvars> 
            //{ 
            // {1,new errorvars(){ errorvarsID=1  ,   errorvarsStartsWith = "nullentry ", errorvarsEndsWith =  "nnot insert the value NULL into column"="",errorvarsVarName =""}},
            // {2,new errorvars(){ errorvarsID=1  ,   errorvarsStartsWith = "nullentry ", errorvarsEndsWith =  "nnot insert the value NULL into column"="",errorvarsVarName =""}}
            //};

            List<errorvars> errorsvariables = new List<errorvars> 
            { 
            new errorvars { errorvarsID=1  ,   errorvarsStartsWith = "Cannot insert the value NULL into column '", errorvarsEndsWith =  "', table '",errorvarsVarName="fieldname",errorvarsFK=1 },
            new errorvars { errorvarsID=2  ,   errorvarsStartsWith = "', table '", errorvarsEndsWith =  "'; column does not allow nulls. INSERT fails.",errorvarsVarName="tablename",errorvarsFK=1 },

            new errorvars { errorvarsID=3  ,   errorvarsStartsWith = "The INSERT statement conflicted with the CHECK constraint "+(char)34, errorvarsEndsWith =  (char)34+". The conflict occurred in database "+(char)34,errorvarsVarName="constraintname",errorvarsFK=8},
            new errorvars { errorvarsID=4  ,   errorvarsStartsWith = (char)34+". The conflict occurred in database "+(char)34, errorvarsEndsWith =  (char)34+", table "+(char)34,errorvarsVarName="database",errorvarsFK=8 },
            new errorvars { errorvarsID=5  ,   errorvarsStartsWith = (char)34+", table "+(char)34, errorvarsEndsWith = (char)34+".",errorvarsVarName="tablename",errorvarsFK=8 },
   //         new errorvars { errorvarsID=6  ,   errorvarsStartsWith = (char)34+", column '", errorvarsEndsWith =  "'.",errorvarsVarName="fieldname",errorvarsFK=8 },
            
            new errorvars { errorvarsID=16  ,   errorvarsStartsWith = "The INSERT statement conflicted with the CHECK constraint "+(char)34, errorvarsEndsWith =  (char)34+". The conflict occurred in database "+(char)34,errorvarsVarName="constraintname",errorvarsFK=12},
            new errorvars { errorvarsID=17  ,   errorvarsStartsWith = (char)34+". The conflict occurred in database "+(char)34, errorvarsEndsWith =  (char)34+", table "+(char)34,errorvarsVarName="database",errorvarsFK=12 },
            new errorvars { errorvarsID=18  ,   errorvarsStartsWith = (char)34+", table "+(char)34, errorvarsEndsWith =  (char)34+", column '" ,errorvarsVarName="tablename",errorvarsFK=12 },
            new errorvars { errorvarsID=19  ,   errorvarsStartsWith = (char)34+", column '", errorvarsEndsWith =  "'.",errorvarsVarName="fieldname",errorvarsFK=12 },

            new errorvars { errorvarsID=7  ,   errorvarsStartsWith = "Cannot insert duplicate key row in object '", errorvarsEndsWith =  "' with unique index '",errorvarsVarName="tablename",errorvarsFK=9 },
            new errorvars { errorvarsID=8  ,   errorvarsStartsWith = "' with unique index '", errorvarsEndsWith =  "'.",errorvarsVarName="constraintname",errorvarsFK=9 },

            new errorvars { errorvarsID=9  ,   errorvarsStartsWith = "The INSERT statement conflicted with the FOREIGN KEY constraint "+(char)34, errorvarsEndsWith =  (char)34+". The conflict occurred in database",errorvarsVarName="constraintname",errorvarsFK=10},
            new errorvars { errorvarsID=10  ,   errorvarsStartsWith = (char)34+". The conflict occurred in database "+(char)34, errorvarsEndsWith =  (char)34+", table "+(char)34,errorvarsVarName="database",errorvarsFK=10 },
            new errorvars { errorvarsID=11  ,   errorvarsStartsWith = (char)34+", table "+(char)34, errorvarsEndsWith =  (char)34+", column '",errorvarsVarName="tablename",errorvarsFK=10 },
            new errorvars { errorvarsID=12  ,   errorvarsStartsWith = (char)34+", column '", errorvarsEndsWith =  "'.",errorvarsVarName="fieldname",errorvarsFK=10 },

            new errorvars { errorvarsID=13  ,   errorvarsStartsWith = "Violation of PRIMARY KEY constraint '", errorvarsEndsWith =  "'. Cannot insert duplicate key in object",errorvarsVarName="constraintname",errorvarsFK=11 },
            new errorvars { errorvarsID=14  ,   errorvarsStartsWith = "Cannot insert duplicate key in object '", errorvarsEndsWith =  "'.",errorvarsVarName="tablename",errorvarsFK=11 }
              };

            List<errorLngMsg> errorMessages = new List<errorLngMsg> 
            {  
            new errorLngMsg { errlngID=23, shortdesc = "nullentry", language =  "greek",msg="Το πεδίο tablename.fieldname δεν επιτρέπεται να είναι κενό." },
            new errorLngMsg { errlngID=24, shortdesc = "xpressionSingle", language =  "greek",msg="Ο περιορισμός constraintname για to πεδίο fieldname δεν επαληθεύεται." },
            new errorLngMsg { errlngID=25, shortdesc = "uniqIDX", language =  "greek",msg="O περιορισμός constraintname δεν επιτρέπει διπλότυπα στο πεδίο tablename.fieldname." },
            new errorLngMsg { errlngID=26, shortdesc = "fkeyNoMatch", language =  "greek",msg="O περιορισμός constraintname επιτρέπει μόνο τιμές του πεδίου tablename.fieldname στο πεδίο ftabname.ffldname." },
            new errorLngMsg { errlngID=27, shortdesc = "primarykey", language =  "greek",msg="O περιορισμός constraintname δεν επιτρέπει διπλότυπα στο πεδίο tablename.fieldname." },
            new errorLngMsg { errlngID=28, shortdesc = "xpressionConstraint", language =  "greek",msg="Ο περιορισμός constraintname για στo πίνακα tablename δεν επαληθεύεται." },
            
            new errorLngMsg { errlngID=29, shortdesc = "nullentry ", language =  "english",msg="the field fieldname cannot be empty." },
            new errorLngMsg { errlngID=30, shortdesc = "xpressionSingle", language =  "english",msg="Constraint constraintname for field fieldname does not evaluate." },
            new errorLngMsg { errlngID=31, shortdesc = "uniqIDX", language =  "english",msg="Constraint constraintname does not allow dublicates on the field tablename.fieldname." },
            new errorLngMsg { errlngID=32, shortdesc = "fkeyNoMatch", language =  "english",msg="Consraint constraintname allows only values of table tablename.fieldname on the field ftabname.ffldname." },
            new errorLngMsg { errlngID=33, shortdesc = "primarykey", language =  "english",msg="Constraint 'constraintname' does not allow dublicates on the field 'tablename'.'fieldname'." },
            new errorLngMsg { errlngID=34, shortdesc = "xpressionConstraint", language =  "english",msg="Constraint constraintname for table tablename does not evaluate correctly." }

             };

            //first Pass wraps around message information
            Dictionary<string, string> dictr = new Dictionary<string, string>();
            if (SHOWMSG) { Console.WriteLine(msg); }
            string logmessage = msg;
            var errListkey = 0;
            string outmsg = "";
            foreach (var e in errors)
            {
                string etext = e.errorVerifyifTextIncluded;
                if (msg.IndexOf(etext) > 0)
                {
                    errListkey = e.errorid;
                    string errShortdesc = e.errorShortDesc;
                    try
                    {
                        outmsg = errorMessages.Where(desc => desc.shortdesc == errShortdesc && desc.language == language).First().msg;

                    }
                    catch (Exception ex)
                    {
                        string a = ex.InnerException.Message;
                        dict = dictr;
                        return "NotTranslated";
                    }
                    break;
                }
            }



            ////firstPass :deduct variables from message 
            int txtstart;
            int txtfin;

            foreach (var varnames in errorsvariables.Where(rplc => rplc.errorvarsFK == errListkey))
            {
                txtstart = msg.IndexOf(varnames.errorvarsStartsWith) + varnames.errorvarsStartsWith.Length;
                //truncate the msg to allow for endswith content to occur more than once
                msg = msg.Substring(txtstart, msg.Length - txtstart);
                txtstart = 0;
                txtfin = msg.IndexOf(varnames.errorvarsEndsWith);
                string rplcStr = msg.Substring(txtstart, txtfin - txtstart);
                outmsg = outmsg.Replace(varnames.errorvarsVarName, rplcStr);
                dictr.Add(varnames.errorvarsVarName, rplcStr);
            }

            ////SecondPass :deduct fieldnames from variables via SQL calls on systm views
            switch (errListkey)
            {
                case 9:
                case 11:

                    SqlConnection conn1 = new SqlConnection(oConnStr);
                    conn1.Open();
                    string sql1 = @"select i.object_id as objectID,i.name as indexname,c.name as fieldname,t.name as tablename
                                    FROM sys.indexes AS i
                                    INNER JOIN sys.index_columns AS ic 
                                        ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                                    INNER JOIN sys.columns AS c 
                                        ON ic.object_id = c.object_id AND c.column_id = ic.column_id
                                    INNER JOIN sys.tables as t
	                                    ON i.object_id = t.object_id 
                                    where i.name= '" + dictr["constraintname"] + "'";
                    SqlDataAdapter dadapter = new SqlDataAdapter();
                    dadapter.SelectCommand = new SqlCommand(sql1, conn1);
                    DataSet dset = new DataSet();
                    dadapter.Fill(dset);
                    dictr.Add("fieldname", dset.Tables[0].Rows[0]["fieldname"].ToString());
                    outmsg = outmsg.Replace("fieldname", dset.Tables[0].Rows[0]["fieldname"].ToString());
                    conn1.Close();
                    break;
                case 10:

                    SqlConnection conn2 = new SqlConnection(oConnStr);
                    conn2.Open();
                    string sql2 = @"SELECT f.parent_object_id as ObjectID,
                                    f.name AS foreign_key_name
                                   ,OBJECT_NAME(f.parent_object_id) AS table_name
                                   ,COL_NAME(fc.parent_object_id, fc.parent_column_id) AS constraint_column_name
                                   ,OBJECT_NAME (f.referenced_object_id) AS referenced_object
                                   ,COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS referenced_column_name
                                   ,is_disabled
                                   ,delete_referential_action_desc
                                   ,update_referential_action_desc
                                    FROM sys.foreign_keys AS f
                                    INNER JOIN sys.foreign_key_columns AS fc 
                                    ON f.object_id = fc.constraint_object_id  
                                    where f.name= '" + dictr["constraintname"] + "'";
                    SqlDataAdapter dadapter2 = new SqlDataAdapter();
                    dadapter2.SelectCommand = new SqlCommand(sql2, conn2);
                    DataSet dset2 = new DataSet();
                    dadapter2.Fill(dset2);
                    dictr.Add("ffldname", dset2.Tables[0].Rows[0]["constraint_column_name"].ToString());
                    outmsg = outmsg.Replace("ffldname", dset2.Tables[0].Rows[0]["constraint_column_name"].ToString());
                    dictr.Add("ftabname", dset2.Tables[0].Rows[0]["table_name"].ToString());
                    outmsg = outmsg.Replace("ftabname", dset2.Tables[0].Rows[0]["table_name"].ToString());
                    conn2.Close();
                    break;

                case 8:
                case 12:
                    SqlConnection conn3 = new SqlConnection(oConnStr);
                    conn3.Open();
                    string sql3 = @"	 select * from sys.check_constraints where name= '" + dictr["constraintname"] + "'";
                    SqlDataAdapter dadapter3 = new SqlDataAdapter();
                    dadapter3.SelectCommand = new SqlCommand(sql3, conn3);
                    DataSet dset3 = new DataSet();
                    dadapter3.Fill(dset3);
                    dictr.Add("condition", dset3.Tables[0].Rows[0]["definition"].ToString());
                    outmsg = outmsg.Replace("condition", dset3.Tables[0].Rows[0]["definition"].ToString());

                    conn3.Close();
                    break;


            }

            dict = dictr;
            if (SHOWMSG) { Console.WriteLine(outmsg); }
            return outmsg;


        }

        public static string treatErrors(string errorMessage, string connectionString, WhichDatabase db, out Dictionary<string, string> MyDictionary)
        {
            Dictionary<string, string> dictr = new Dictionary<string, string>();
     
            string errs = "";
            switch (db)
            {
                case WhichDatabase.ORACLEdatabase:
                    errs = treatORACLError(errorMessage, connectionString, out dictr);
                    break;

                case WhichDatabase.SQLdatabase:
                    errs = treatSQLError(errorMessage, connectionString, out dictr);
                    break; 
            }
            MyDictionary = dictr;
            return errs;
        }


    }
}
