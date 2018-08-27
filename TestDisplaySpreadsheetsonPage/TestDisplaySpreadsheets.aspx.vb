Imports System.Collections.Generic  'For "List" type
Imports System.IO    'GetFiles()
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization.JavaScriptSerializer



Partial Class TestDisplaySpreadsheets
    Inherits System.Web.UI.Page

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

    Dim fullPath As String = "C:\Users\iansh\Documents\Head-First-JavaScript-Programming-master\TestDisplaySheets\"

    Public Sub pageload(sender As Object, e As EventArgs) Handles Me.Load
        Dim filePath() As String = Directory.GetFiles("C:\Users\iansh\Documents\Head-First-JavaScript-Programming-master\TestDisplaySheets")
        Dim files As List(Of ListItem) = New List(Of ListItem)
        For Each file As String In filePath
            files.Add(New ListItem(Path.GetFileName(file)))
        Next

        Repeater.DataSource = files
        Repeater.DataBind()

        'get data back from sql for json string
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

        jsonLabel.Text = New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Data)

        Reader.Close()
        cnn.Close()
    End Sub

    'The Content-Disposition header basically allows you to specify that the response you're sending back is to be treated as 
    'an attachment rather than content which results In the download dialog
    'weblog.west-wind.com/posts/2007/May/21/Downloading-a-File-with-a-Save-As-Dialog-in-ASPNET

    Public Sub downloadFile(sender As Object, e As EventArgs)
        Dim Link As New LinkButton
        Link = sender
        Dim filePath As String = Link.CommandArgument
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filePath)
        Response.TransmitFile(fullPath & filePath)


        Dim connectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
        Dim cnn As New SqlConnection(connectionString)
        cnn.Open()
        Dim cmd As New SqlCommand("Insert into PDisplaySpreadsheets values (@file, 'Cheryl', @date)", cnn)

        cmd.Parameters.Add("@date", SqlDbType.DateTime)
        cmd.Parameters("@date").Value = Now

        cmd.Parameters.Add("@file", SqlDbType.VarChar)
        cmd.Parameters("@file").Value = filePath

        cmd.ExecuteNonQuery()

        cnn.Close()
        Response.End()

    End Sub



End Class

