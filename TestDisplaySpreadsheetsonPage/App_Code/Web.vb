Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://localhost:59305/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Web
    Inherits System.Web.Services.WebService

    Public Class D3Data
        Public theDate As String
        Public theCount As String
        Public ID As Integer

        Public Sub New(ByVal dateValue As String, ByVal countValue As String, ByVal IDValue As Integer)
            theDate = dateValue
            theCount = countValue
            ID = IDValue
        End Sub


    End Class



    <WebMethod()>
    Public Function getData() As String

        Dim connectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
        Dim cnn As New SqlConnection(connectionString)
        cnn.Open()

        Dim cmd1 As New SqlCommand("select cast(thedate as date) as theDate, 1 as theCount, ID from PDisplaySpreadsheets order by cast(thedate as date)", cnn)

        Dim Reader As SqlDataReader = cmd1.ExecuteReader()

        Dim Data As List(Of D3Data) = New List(Of D3Data)

        Do While Reader.Read()

            Dim theDate As Date = Reader.Item("theDate").ToString()
            Dim theCount As Integer = Reader.Item("theCount").ToString()
            Dim ID As Integer = Reader.Item("ID")
            Dim theValue As D3Data = New D3Data(theDate, theCount, ID)
            Data.Add(theValue)

        Loop

        ' pass result of query to text value of asp lable control, which can then be accessed via javascript

        Dim js As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim JsonText = js.Serialize(Data)
        Return JsonText

        Reader.Close()
        cnn.Close()

    End Function

End Class