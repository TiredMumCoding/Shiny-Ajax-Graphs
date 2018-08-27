
Imports System.Data
Imports System.Data.SqlClient

Partial Class Refesh
    Inherits System.Web.UI.Page

    Public Class D3Data
        Public theDate As String
        Public theCount As String


        Public Sub New(ByVal dateValue As String, ByVal countValue As String)
            theDate = dateValue
            theCount = countValue
        End Sub

    End Class

    Public Function getData() As String
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
        Dim cnn As New SqlConnection(connectionString)
        cnn.Open()
        Dim cmd1 As New SqlCommand("  select cast(thedate as date) as theDate, count(thefile) as theCount from PDisplaySpreadsheets group by cast(thedate as date)", cnn)

        Dim Reader As SqlDataReader = cmd1.ExecuteReader()

        Dim Data As List(Of D3Data) = New List(Of D3Data)

        Do While Reader.Read()

            Dim theDate As Date = Reader.Item("theDate").ToString()
            Dim theCount As Integer = Reader.Item("theCount").ToString()
            Dim theValue As D3Data = New D3Data(theDate, theCount)
            Data.Add(theValue)

        Loop

        ' pass result of query to text value of asp lable control, which can then be accessed via javascript

        Dim jsonText As String = New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Data)
        Return jsonText

        Reader.Close()
        cnn.Close()

    End Function

End Class
