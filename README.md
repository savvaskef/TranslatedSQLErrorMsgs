# TranslatedSQLErrorMsgs
dll to translate error messages for both Oracle and MS-SQL.(includes calls to system tables)
This is a library(.dll)gg that translates errors made by sql statements in greek.<br>

the procedure you are seeking is :<br>
  public static string treatErrors(string errorMessage, string connectionString, WhichDatabase db, out Dictionary<string, string> MyDictionary)
     <br>
the errorMessagage is the original message, the connectionString is helping to connect to the database and is used to look up system tables that 
contain information included in the message. MyDictionary will pass you extra information abou the error and db specifies the version of SQL Used.<br>
Currently supported are Oracle and SQL as defined in the type(WhichDatabase) of db argument which can take the values of :<br>
public enum WhichDatabase { ORACLEdatabase, SQLdatabase }.

<br>


