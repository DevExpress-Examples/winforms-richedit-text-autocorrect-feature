Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
#Region "#usings"
Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraRichEdit.Services
#End Region ' #usings

Namespace Expander
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
			richEditControl1.CreateNewDocument()
			richEditControl1.Document.AppendText("This sample expander uses Mary Morken's 2007 list of abbreviations for medical transcriptions, containing 13420 entries. " & ControlChars.CrLf & "The list has been downloaded from http://www.mtdaily.com/abbvs.txt. To indicate the beginning of the list, the line containing the 'START' string is added.")
'#Region "#autocorrectservice"
			Dim svc As IAutoCorrectService = richEditControl1.GetService(Of IAutoCorrectService)()
			If svc IsNot Nothing Then
				svc.SetReplaceTable(LoadAbbrevs("abbvs.txt"))
			End If
'#End Region ' #autocorrectservice
		End Sub

#Region "#setreplacetable"
		Private Function LoadAbbrevs(ByVal path As String) As AutoCorrectReplaceInfoCollection
			Dim coll As New AutoCorrectReplaceInfoCollection()
			Dim aLine As String = ""

			Dim acrInfoIm As New AutoCorrectReplaceInfo(":-)", CreateImageFromResx("smile.png"))
			coll.Add(acrInfoIm)

			If File.Exists(path) Then
				Dim sr As New StreamReader(path)
				Do While Not(sr.EndOfStream)
					aLine = sr.ReadLine()
					If aLine <> "START" Then
						Continue Do
					End If

					Do While Not(sr.EndOfStream)
						aLine = sr.ReadLine()
						aLine = aLine.Trim()
						Dim words() As String = aLine.Split("="c)
						If words.Length = 2 Then
							Dim acrInfo As New AutoCorrectReplaceInfo(words(0), words(1))
							coll.Add(acrInfo)
						End If
					Loop
				Loop
				sr.Close()
			End If
			Return coll
		End Function
#End Region ' #setreplacetable

		Private Function CreateImageFromResx(ByVal name As String) As Image
			Dim im As Image
			Dim [assembly] As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
			Using stream As Stream = [assembly].GetManifestResourceStream("Expander.Images." & name)
				im = Image.FromStream(stream)
			End Using
			Return im
		End Function
	End Class
End Namespace